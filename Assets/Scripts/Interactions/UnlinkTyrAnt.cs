using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{ 
    public class UnlinkTyrAnt : MonoBehaviour
    {
        [SerializeField] private GameObject tyrPrefab;
        [SerializeField] private GameObject antPrefab;

        [HideInInspector] public List<string> antScripts;
        [HideInInspector] public List<string> tyrScripts;
    
        private Animator animator;
        private KeyCode unlinkKey;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unlinkKey = new InputSettings().SwitchCharacterButton;
        }

        private void Update()
        {
            if (Input.GetKey(unlinkKey))
            {
                Disconnect();
            }   
        }

        private void Disconnect() //Start Disconnection animation
        {
            animator.SetTrigger("unlink");
        }

        public void Unlink()    //Animation should fire this event
        {
            Destroy(gameObject); //Destroy This

            GameObject tyr = Instantiate(tyrPrefab); //Create Ant
            GameObject ant = Instantiate(antPrefab); //Create Tyr

            tyr.transform.position = transform.position;
            ant.transform.position = transform.position;

            ant.GetComponent<SwitchCharacter>().targetEntity = tyr;
            tyr.GetComponent<SwitchCharacter>().targetEntity = ant;
        }
    }
}
