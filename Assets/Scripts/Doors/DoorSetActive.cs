using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{

    private bool isOpen = false;

    public void CloseDoor()
    {
        gameObject.SetActive(false);
    }

    public void OpenDoor()
    {
        gameObject.SetActive(true);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
