using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{

    public bool isOpen = false; //Stato iniziale della porta: False = Chiusa, 

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        isOpen = !isOpen;
    }
}
