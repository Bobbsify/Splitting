using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    public GameObject targetDoor;
    public KeyCode inputButton;

    public void Activate()
    {
        if (Input.GetKeyDown(inputButton)) { 
            IDoor door = targetDoor.GetComponent<IDoor>();
            door.ToggleDoor();
        }
    }
}
