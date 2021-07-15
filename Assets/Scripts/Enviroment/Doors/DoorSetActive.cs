using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{

    public bool isOpen = false; //Stato iniziale della porta: False = Chiusa, 

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
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
        isOpen = !isOpen;
    }
}
