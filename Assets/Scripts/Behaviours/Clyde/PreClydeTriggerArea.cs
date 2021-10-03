using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class PreClydeTriggerArea : MonoBehaviour
    {
        private GameObject trajectPred;
        private Throw getThrow;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                bool ant = false;
                bool tyr = false;

                ant = collision.name.ToUpper().Contains("ANT");
                tyr = collision.name.ToUpper().Contains("TYR");

                collision.GetComponent<StateControllerInterface>().DisablePush();

                if (ant && !tyr)
                {
                    collision.GetComponent<StateController>().DisableDrop();
                }
                else if (!ant && tyr)
                {
                    collision.GetComponent<StateControllerT>().DisablePickup();

                    trajectPred = GameObject.Find("TrajectoryPrediction");
                    getThrow = trajectPred.GetComponentInChildren<Throw>();

                    getThrow.RemoveInputs();
                }
                else if (ant && tyr)
                {
                    trajectPred = GameObject.Find("TrajectoryPredictionTA");
                    getThrow = trajectPred.GetComponentInChildren<Throw>();

                    getThrow.RemoveInputs();
                }                

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                bool ant = false;
                bool tyr = false;

                collision.GetComponent<StateControllerInterface>().EnablePush();

                ant = collision.name.ToUpper().Contains("ANT");
                tyr = collision.name.ToUpper().Contains("TYR");

                if (ant && !tyr)
                {
                    collision.GetComponent<StateController>().EnableDrop();
                }
                else if (!ant && tyr)
                {
                    collision.GetComponent<StateControllerT>().EnablePickup();

                    trajectPred = GameObject.Find("TrajectoryPrediction");
                    getThrow = trajectPred.GetComponentInChildren<Throw>();

                    getThrow.SetInputs();
                }
                else if (ant && tyr)
                {
                    trajectPred = GameObject.Find("TrajectoryPredictionTA");
                    getThrow = trajectPred.GetComponentInChildren<Throw>();

                    getThrow.SetInputs();
                }                

            }
        }
    }
}

