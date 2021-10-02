using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class ClydeTriggerArea : MonoBehaviour
    {
        public GameObject clyde;
        private ClydeController clydeController;
        private Patrol clydePatrol;

        private GameObject trajectPred;
        private Throw getThrow;

        // Start is called before the first frame update
        void Start()
        {
            clyde = GameObject.FindGameObjectWithTag("Clyde");

            if (clyde != null)
            {
                clydeController = clyde.GetComponent<ClydeController>();
                clydePatrol = clyde.GetComponent<Patrol>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                bool ant = false;
                bool tyr = false;

                ant = collision.name.ToUpper().Contains("ANT");
                tyr = collision.name.ToUpper().Contains("TYR");

                if (ant && tyr)
                {
                    trajectPred = GameObject.Find("TrajectoryPredictionTA");
                    getThrow = trajectPred.GetComponentInChildren<Throw>();

                    if (!getThrow.rbToThrow)
                    {
                        collision.GetComponent<StateControllerInterface>().DisableControl();
                    }
                }
                else
                {
                    collision.GetComponent<StateControllerInterface>().DisableControl();
                }

                clydePatrol.enabled = false;
                clydeController.startApproach = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<StateControllerInterface>().EnableControl();

                clydePatrol.enabled = true;
                clydeController.startApproach = false;
            }
        }
    }
}
