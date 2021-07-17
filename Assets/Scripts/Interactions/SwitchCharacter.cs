using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
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
        

        void Awake()
        {
            swapButton = new InputSettings().SwitchCharacterButton;
            try
            {
                tyrAnt = transform.Find(tyrantName).gameObject;
            }
            catch
            {
                //throw new MissingFieldException(gameObject.name+" is missing TyrAnt");
            }
            unionCheck = GetComponent<AutobotUnity>();
        }
        

        void Update()
        {
            if (targetEntity != null)
            {
                if (unionCheck != null)
                {
                    if (Input.GetKeyUp(swapButton) && !unionCheck.readyForConnection)
                    {
                        TurnThisOff();
                        TurnOtherOn();
                    }
                    if (unionCheck.readyForConnection) //todo change to correct variable
                    {
                        Connect();
                    }
                }
                else
                {
                    if (Input.GetKeyUp(swapButton))
                    {
                        TurnThisOff();
                        TurnOtherOn();
                    }
                }
            }
        }

        private void Connect()
        {
            targetEntity.SetActive(false); //Disable Other
            gameObject.SetActive(false); //Disable This
            tyrAnt.transform.parent = null; //Remove Object from parent
            targetEntity.transform.parent = tyrAnt.transform; // Set Ant&Tyr as children of TyrAnt
            transform.parent = tyrAnt.transform;
            tyrAnt.SetActive(true);
            tyrAnt.GetComponent<Animator>().SetTrigger("Link");
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