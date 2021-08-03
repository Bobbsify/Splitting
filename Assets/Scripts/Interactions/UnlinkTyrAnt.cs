using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{ 
    public class UnlinkTyrAnt : MonoBehaviour
    {
        [SerializeField] private GameObject tyrPrefab;
        [SerializeField] private GameObject antPrefab;
    
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

            Instantiate(tyrPrefab); //Create Ant
            Instantiate(antPrefab); //Create Tyr
        }
    }
}
