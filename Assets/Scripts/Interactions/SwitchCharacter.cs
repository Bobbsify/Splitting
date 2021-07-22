using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class SwitchCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject targetEntity;
        [SerializeField] private List<string> ScriptsToDisable;
        [SerializeField] private List<string> ScriptsToEnable;
        private KeyCode swapButton; //Defaults to Q

        [Header("Tyrant")]
        [SerializeField] private string tyrantName;
        private GameObject tyrAnt;
        private AutobotUnity unionCheck;


        private void Awake()
        {
            gatherInfo();
        }

        private void OnEnable()
        {
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
                    throw new Exception("Unknown State Exception: unionCheck should be assigned but is instead " + unionCheck);
                }
            }
        }

        private void gatherInfo()
        {
            swapButton = new InputSettings().SwitchCharacterButton;
            try
            {
                tyrAnt = transform.Find(tyrantName).gameObject;
            }
            catch
            {
                try
                {
                    tyrAnt = targetEntity.transform.Find(tyrantName).gameObject;
                }
                catch
                {
                    throw new Exception("Neither Entity has a TyrAnt attached OR the specified TyrAnt name is incorrect! Please attach a tyrant to either entity and assign its correct name in the \"SwitchCharacter\" module");
                }
            }
            unionCheck = GetComponent<AutobotUnity>();
            if (unionCheck == null)
            {
                unionCheck = targetEntity.GetComponent<AutobotUnity>();
            }
        }

        private void Connect()
        {
            targetEntity.SetActive(false); //Disable Other
            targetEntity.tag = "Untagged";
            
            gameObject.SetActive(false); //Disable This
            gameObject.tag = "Untagged";

            tyrAnt.transform.parent = null; //Remove Object from parent
            
            targetEntity.transform.parent = tyrAnt.transform; // Set Ant&Tyr as children of TyrAnt
            transform.parent = tyrAnt.transform;
            
            tyrAnt.SetActive(true);
            tyrAnt.tag = "Player";
        }

        private void TurnThisOff()
        {
            foreach (string script in ScriptsToDisable) //Get Scripts and disable them one by one
            {
                (gameObject.GetComponent(script) as MonoBehaviour).enabled = false;
            }
            StopMovementX();
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
            GetComponent<Animator>().SetFloat("velocityX", 0f);
        }
    }
}