using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    public GameObject[] targetDoors;
    public KeyCode inputButton;

    private bool playerIsInZone = false;

    public void Update()
    {
        if (Input.GetKeyDown(inputButton) && playerIsInZone)
        {
            foreach (GameObject d in targetDoors)
            {
                IDoor door = d.GetComponent<IDoor>();
                door.ToggleDoor();
            }
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
}
