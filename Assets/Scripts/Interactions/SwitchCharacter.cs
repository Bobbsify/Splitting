using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
    public class SwitchCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject targetEntity;
        [SerializeField] private List<string> NamesOfScriptsToDisable;
        [SerializeField] private List<string> NamesOfScriptsToEnable;
        private KeyCode swapButton; //Defaults to Q

        // Start is called before the first frame update
        void Awake()
        {
            swapButton = new InputSettings().SwitchCharacterButton;
        }

        // Update is called once per frame
        void Update()
        {
            if (targetEntity != null)
            {
                if (Input.GetKeyUp(swapButton))
                {
                    turnThisOff();
                    turnOtherOn();
                }
            }
        }

        private void turnThisOff()
        {
            MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if(NamesOfScriptsToDisable.Contains(script.name))
                    script.enabled = false;
            }
            gameObject.layer = 8;
            StopMovementX();
            gameObject.tag = "Untagged";
        }

        private void turnOtherOn()
        {
            MonoBehaviour[] scripts = targetEntity.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (NamesOfScriptsToEnable.Contains(script.name))
                    script.enabled = true;
            }
            targetEntity.layer = 9;
            targetEntity.tag = "Player";

        }

        private void StopMovementX()
        {
            GetComponent<Animator>().SetFloat("velocityX", 0f);
        }
    }
}