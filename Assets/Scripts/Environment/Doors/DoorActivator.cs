using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class DoorActivator : MonoBehaviour
    {
        public GameObject[] targetDoors;
        public KeyCode inputButton;
        public ActivationTypes activationType = ActivationTypes.PressWhenInArea;

        [SerializeField] private bool pressOnlyOnce = false;

        [SerializeField] private GameObject scoringPlatform;
        [SerializeField] private int targetScore;

        private bool playerIsInZone = false;

        private GameObject player;
        private StateController antStateController;
        private StateControllerTA tyrantStateController;
        private int currentPlayer;

        void Awake()
        {
            currentPlayer = ControlWhoIsPlayer();
        }

        public void Update()
        {
            currentPlayer = ControlWhoIsPlayer();

            switch (activationType)
            {
                case ActivationTypes.PressWhenInArea:
                    if (Input.GetKeyDown(inputButton) && playerIsInZone && (currentPlayer == 0 || currentPlayer == 2))
                    {
                        UpdateStateController();

                        foreach (GameObject d in targetDoors)
                        {
                            IDoor door = d.GetComponent<IDoor>();
                            door.ToggleDoor();
                        }
                        Animator anim = GetComponent<Animator>();
                        anim.SetBool("lit", !anim.GetBool("lit"));
                        if (pressOnlyOnce) turnOff();
                    }
                    break;
                case ActivationTypes.Score:
                    if (scoringPlatform.GetComponent<Platform>().score >= targetScore)
                    {
                        foreach (GameObject d in targetDoors)
                        {
                            IDoor door = d.GetComponent<IDoor>();
                            door.OpenDoor();
                        }
                        if (pressOnlyOnce) turnOff();
                    }
                    else
                    {
                        if (currentPlayer == 0)
                        {
                            antStateController = player.GetComponent<StateController>();

                            if (antStateController != null)
                            {
                                antStateController.isPressingButton = false;
                            }
                        }
                        else if (currentPlayer == 2)
                        {
                            tyrantStateController = player.GetComponent<StateControllerTA>();

                            if (tyrantStateController != null)
                            {
                                tyrantStateController.isPressingButton = false;
                            }
                        }

                        foreach (GameObject d in targetDoors)
                        {
                            IDoor door = d.GetComponent<IDoor>();
                            door.CloseDoor();
                        }
                        if (pressOnlyOnce) turnOff();
                    }
                    break;
                default:
                    throw new Exception("Unknown Activation type for " + gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.name.Contains("Check")) //Praticamente un chiodo che regge un palazzo
            {
                if (col.gameObject.tag == "Player")
                {
                    playerIsInZone = true;
                }
                else
                {
                    playerIsInZone = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (!col.name.Contains("Check")) //Praticamente un chiodo che regge un palazzo
            {
                if (col.gameObject.tag == "Player")
                {
                    playerIsInZone = false;
                }
            }
        }

        private void turnOff()
        {
            this.enabled = false;
        }


        public enum ActivationTypes
        {
            PressWhenInArea,
            Score
        }

        private GameObject findPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }

        int ControlWhoIsPlayer()
        {
            int x = 0;

            bool ant = false;
            bool tyr = false;

            player = GameObject.FindGameObjectWithTag("Player");

            ant = player.name.ToUpper().Contains("ANT");
            tyr = player.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {
                x = 0; // ant
            }
            else if (tyr && !ant)
            {
                x = 1; // tyr
            }
            else if (ant && tyr)
            {
                x = 2; // tyrant
            }

            return x;
        }

        void UpdateStateController()
        {
            if (currentPlayer == 0)
            {
                antStateController = player.GetComponent<StateController>();

                if (antStateController != null)
                {
                    antStateController.isPressingButton = true;
                }
            }
            else if (currentPlayer == 2)
            {
                tyrantStateController = player.GetComponent<StateControllerTA>();

                if (tyrantStateController != null)
                {
                    tyrantStateController.isPressingButton = true;
                }
            }
        }
    }
}
