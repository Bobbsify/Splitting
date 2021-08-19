using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting {
    public class Hacking : MonoBehaviour
    {
        //public GameObject[] hackableObj = new GameObject[3];

        public List<GameObject> hackableObj = new List<GameObject>();

        public int hackableObjIndex = 0;

        private KeyCode switchForHackUpButton;
        private KeyCode switchForHackDownButton;

        // Start is called before the first frame update
        void Start()
        {
            switchForHackUpButton = new InputSettings().SwitchForHackUpButton;
            switchForHackDownButton = new InputSettings().SwitchForHackDownButton;
        }

        // Update is called once per frame
        void Update()
        {
            if (hackableObj.Count > 0)
            {
                if (hackableObjIndex > (hackableObj.Count - 1))
                {
                    hackableObjIndex = 0;
                }

                hackableObjIndex = ManageHackableTarget(hackableObjIndex);

                SwitchHackInteractionBalloon();
            }          
        }      
                
        int ManageHackableTarget(int x)
        {                   

            if (Input.GetKeyUp(switchForHackUpButton))
            {
                if (x == (hackableObj.Count - 1))
                {
                    x = 0;
                }
                else
                {
                    x++;
                }
            }

            if (Input.GetKeyUp(switchForHackDownButton))
            {
                if (x == 0)
                {
                    x = hackableObj.Count - 1;
                }
                else
                {
                    x--;
                }
            }         

            return x;
        }

        void SwitchHackInteractionBalloon()
        {
            for (int i = 0; i < hackableObj.Count; i++)
            {
                if (i == hackableObjIndex)
                {
                    hackableObj[i].transform.Find("InteractionOn").gameObject.SetActive(true);
                }
                else
                {
                    hackableObj[i].transform.Find("InteractionOn").gameObject.SetActive(false);
                    hackableObj[i].transform.Find("InteractionOff").gameObject.SetActive(true);
                }
            }
        }
        
    }
}
