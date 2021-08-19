using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class HackDetect : MonoBehaviour
    {
        public GameObject hacker;

        public bool onHack;

        public Hacking hacking;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
    }
}
