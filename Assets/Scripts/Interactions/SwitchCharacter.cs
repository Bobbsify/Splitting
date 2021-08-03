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
        [SerializeField] private GameObject tyrant;
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
            unionCheck = GetComponent<AutobotUnity>();
            if (unionCheck == null)
            {
                unionCheck = targetEntity.GetComponent<AutobotUnity>();
            }
        }

        private void Connect()
        {
            Destroy(targetEntity); //Destroy Other
            Destroy(gameObject); //Destroy This

            Instantiate(tyrant);
            tyrant.transform.position = transform.position;
        }

        private void TurnThisOff()
        {
            foreach (string script in ScriptsToDisable) //Get Scripts and disable them one by one
            {
                StopMovementX();
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
            GetComponent<Animator>().SetFloat("velocityX", 0.0f);
        }
    }
}