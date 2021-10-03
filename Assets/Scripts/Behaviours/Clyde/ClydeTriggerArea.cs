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

        private GameObject tyrantTrajectPred;
        private Throw tyrantGetThrow;

        private SwitchCharacter switchCharacter;
        private UnlinkTyrAnt unlinkTyrAnt;

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
                    unlinkTyrAnt = collision.GetComponent<UnlinkTyrAnt>();
                    unlinkTyrAnt.RemoveInputs();

                    tyrantTrajectPred = GameObject.Find("TrajectoryPredictionTA");
                    tyrantGetThrow = tyrantTrajectPred.GetComponentInChildren<Throw>();

                    tyrantGetThrow.RemoveInputs();

                    if (!tyrantGetThrow.rbToThrow)
                    {
                        collision.GetComponent<StateControllerInterface>().DisableControl();

                        clydePatrol.enabled = false;
                        clydeController.startApproach = true;
                    }
                }
                else
                {
                    switchCharacter = collision.GetComponent<SwitchCharacter>();
                    switchCharacter.RemoveInputs();

                    collision.GetComponent<StateControllerInterface>().DisableControl();

                    clydePatrol.enabled = false;
                    clydeController.startApproach = true;
                }               
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                bool ant = false;
                bool tyr = false;

                ant = collision.name.ToUpper().Contains("ANT");
                tyr = collision.name.ToUpper().Contains("TYR");

                if (ant && tyr)
                {
                    unlinkTyrAnt = collision.GetComponent<UnlinkTyrAnt>();
                    unlinkTyrAnt.SetInputs();

                    tyrantTrajectPred = GameObject.Find("TrajectoryPredictionTA");
                    tyrantGetThrow = tyrantTrajectPred.GetComponentInChildren<Throw>();

                    tyrantGetThrow.SetInputs();                   

                    if (!tyrantGetThrow.rbToThrow)
                    {
                        collision.GetComponent<StateControllerInterface>().EnableControl();

                        clydePatrol.enabled = true;
                        clydeController.startApproach = false;
                    }
                    else
                    {
                        tyrantGetThrow.ReleaseBox();
                    }
                }
                else
                {
                    switchCharacter = collision.GetComponent<SwitchCharacter>();
                    switchCharacter.SetInputs();

                    collision.GetComponent<StateControllerInterface>().EnableControl();

                    clydePatrol.enabled = true;
                    clydeController.startApproach = false;
                }
                
            }
        }
    }
}
