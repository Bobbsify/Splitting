using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorActivator : MonoBehaviour
{
    public GameObject[] targetDoors;
    public KeyCode inputButton;
    public ActivationTypes activationType = ActivationTypes.PressWhenInArea;

    [SerializeField] private GameObject scoringPlatform;
    [SerializeField] private int targetScore;

    private bool playerIsInZone = false;

    public void Update()
    {
        switch (activationType)
        {
            case ActivationTypes.PressWhenInArea:
                if (Input.GetKeyDown(inputButton) && playerIsInZone)
                {
                    foreach (GameObject d in targetDoors)
                    {
                        IDoor door = d.GetComponent<IDoor>();
                        door.ToggleDoor();
                        Animator anim = GetComponent<Animator>();
                        anim.SetBool("lit",!anim.GetBool("lit"));
                    }
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
                }
                else
                {
                    foreach (GameObject d in targetDoors)
                    {
                        IDoor door = d.GetComponent<IDoor>();
                        door.CloseDoor();
                    }
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


    public enum ActivationTypes
    {
        PressWhenInArea,
        Score
    }
}
