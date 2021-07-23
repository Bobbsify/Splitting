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
            animator.SetTrigger("unlink");
        }

        public void Unlink()    //Animation should fire this event
        {
            //Set correct TyrAnt
            gameObject.transform.localScale = new Vector2(Mathf.Abs(gameObject.transform.localScale.x),gameObject.transform.localScale.y);

            //Remove Ant & Tyr Children
            ant.transform.parent = null;
            ant.tag = "Player";

            tyr.transform.parent = null;
            tyr.tag = "Untagged";
            tyr.transform.localScale = new Vector2(1.13f,1.13f);

            //Disable TyrAnt
            gameObject.SetActive(false);
            gameObject.tag = "Untagged";

            //Attach TyrAnt to Ant
            transform.parent = ant.transform;

            //Enable Ant & Tyr
            ant.SetActive(true);
            tyr.SetActive(true);
        }
    }
}
