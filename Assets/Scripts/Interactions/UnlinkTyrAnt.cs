using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{ 
    public class UnlinkTyrAnt : MonoBehaviour
    {
        [SerializeField] private string tyrName = "Tyr";
        private GameObject tyr;
        [SerializeField] private string antName = "Ant";
        private GameObject ant;
    
        private Animator animator;
        private KeyCode unlinkKey;

        private void Awake()
        {
            ant = transform.Find(antName).gameObject;
            tyr = transform.Find(tyrName).gameObject;
        
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
            animator.SetTrigger("Unlink");
        }

        public void Unlink()    //Animation should fire this event
        {
            //Remove Ant & Tyr Children
            ant.transform.parent = null;
            tyr.transform.parent = null;

            //Disable TyrAnt
            gameObject.SetActive(false);

            //Attach TyrAnt to Ant
            transform.parent = ant.transform;

            //Enable Ant & Tyr
            ant.SetActive(true);
            tyr.SetActive(true);
        }
    }
}
