using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class SwitchCharacter : MonoBehaviour
    {
        [SerializeField] public GameObject targetEntity;
        [SerializeField] private List<string> ScriptsToDisable;
        [SerializeField] private List<string> ScriptsToEnable;
        private KeyCode swapButton; //Defaults to Q

        [Header("Tyrant")]
        [SerializeField] private GameObject tyrantPrefab;
        private AutobotUnity unionCheck;

        
        private void OnEnable()
        {
            StopMovementX();
            gatherInfo();
        }

        void Update()
        {
            if (targetEntity != null)
            {
                if (unionCheck != null)
                {
                    if (Input.GetKeyUp(swapButton) && (!unionCheck.connectable && !unionCheck.readyForConnection))
                    {
                        TurnThisOff();
                        TurnOtherOn();
                    }
                    else if (unionCheck.readyForConnection)
                    {
                        Connect();
                    }
                }
                else
                {
                    gatherInfo();
                    if (unionCheck == null) { 
                        throw new Exception("Unknown State Exception: unionCheck should be assigned but is instead " + unionCheck+"\n");
                    }
                }
            }
        }

        private void gatherInfo()
        {
            swapButton = new InputSettings().SwitchCharacterButton;
            unionCheck = GetComponent<AutobotUnity>();
            if (unionCheck == null)
            {
                unionCheck = targetEntity.GetComponent<AutobotUnity>();
            }
        }

        public GameObject Connect()
        {
            bool isThisAnt = gameObject.name.ToUpper().Contains("ANT");

            Destroy(targetEntity); //Destroy Other
            Destroy(gameObject); //Destroy This

            GameObject tyrant = Instantiate(tyrantPrefab);
            tyrant.transform.position = transform.position;

            Transform tyrFlashlight = isThisAnt ? targetEntity.transform.Find("bone_1/bone_14/Flashlight") : transform.Find("bone_1/bone_14/Flashlight");
            Transform tyrantFlashlight = tyrant.transform.Find("bone_1/bone_2/bone_3/bone_13/Flashlight");

            //Turn flashlight correctly
            tyrantFlashlight.GetComponent<FlashlightController>().SetLightsToState(tyrFlashlight.GetComponent<FlashlightController>().lightsAre);
            
            tyrant.GetComponent<UnlinkTyrAnt>().antScripts = isThisAnt ? ScriptsToDisable : ScriptsToEnable;
            tyrant.GetComponent<UnlinkTyrAnt>().tyrScripts = isThisAnt ? ScriptsToEnable : ScriptsToDisable;

            return tyrant;
        }

        private void TurnThisOff()
        {
            foreach (string script in ScriptsToDisable) //Get Scripts and disable them one by one
            {
                (gameObject.GetComponent(script) as MonoBehaviour).enabled = false;
            }
            gameObject.tag = "Untagged";
        }

        private void TurnOtherOn()
        {
            foreach (string script in ScriptsToEnable) //Get Scripts and enable them one by one
            {
                (targetEntity.GetComponent(script) as MonoBehaviour).enabled = true;
            }
            targetEntity.tag = "Player";

        }

        private void StopMovementX()
        {
            if (targetEntity != null) { 
                targetEntity.GetComponent<Animator>().SetFloat("velocityX",0);
            }
        }
    }
}