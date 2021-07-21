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
                    }
                }
                break;
            case ActivationTypes.Score:
                if (scoringPlatform.GetComponent<Platform>().score >= targetScore)
                {
                    foreach (GameObject d in targetDoors)
                    {
                        IDoor door = d.GetComponent<IDoor>();
                        door.CloseDoor();
                    }
                }
                else
                {
                    foreach (GameObject d in targetDoors)
                    {
                        IDoor door = d.GetComponent<IDoor>();
                        door.OpenDoor();
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
                Debug.Log("In - "+ col.name);
            }
            else
            {
                playerIsInZone = false;
                Debug.Log("Out" + col.name);
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
                Debug.Log("Out2" + col.name);
            }
        }
    }


    public enum ActivationTypes
    {
        PressWhenInArea,
        Score
    }
}
