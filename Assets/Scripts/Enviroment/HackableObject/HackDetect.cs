using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Splitting
{
    public class HackDetect : MonoBehaviour
    {
        private GameObject hacker;

        private bool onHack;        
        private bool hackerIsHacking;

        public bool hacked;        

        private Hacking hacking;       

        private Animator animator;

        [Header("Other Actions")]
        [SerializeField] UnityEvent subsequentEvents;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (hacked)
            {
                WhoIsTheHacker();

                if (hacker != null)
                {
                    animator = hacker.GetComponent<Animator>();
                }

                if (animator != null)
                {
                    hackerIsHacking = true;
                }
            }                      

            if (hackerIsHacking && onHack && AnimatorIsPlaying("Tyr hacking2"))
            {
                ExecuteHackingAction();                
            }

            CallAnimator(hackerIsHacking);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "HackArea")
            {
                onHack = true;

                WhoIsTheHacker();

                if (hacker != null)
                {
                    hacking.hackableObj.Add(gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "HackArea")
            {
                onHack = false;

                WhoIsTheHacker();

                if (hacker != null)
                {
                    hacking.hackableObj.Remove(gameObject);
                }

                gameObject.transform.Find("InteractionOff").gameObject.SetActive(false);
                gameObject.transform.Find("InteractionOn").gameObject.SetActive(false);
            }
        }

        void WhoIsTheHacker()
        {
            bool ant = false;
            bool tyr = false;

            hacker = GameObject.FindGameObjectWithTag("Player");

            ant = hacker.name.ToUpper().Contains("ANT");
            tyr = hacker.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {
                hacking = null;
            }
            else
            {
                hacking = hacker.GetComponentInChildren<Hacking>();
            }

        }

        void ExecuteHackingAction()
        {
            hackerIsHacking = false;
            hacked = false;

            DoEvents();

            this.enabled = false;
        }

        private void CallAnimator(bool isHacking)
        {
            if (animator != null)
            {
                animator.SetBool("isHacking", isHacking);
            }
        }

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        private void DoEvents()
        {
            subsequentEvents.Invoke();
        }
    }
}
