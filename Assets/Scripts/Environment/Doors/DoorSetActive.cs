using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour, IDoor
{

    public bool isOpen = false; //Stato iniziale della porta: False = Chiusa, 
    private Animator anim;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void CloseDoor()
    {
        //gameObject.SetActive(true);
        anim.SetBool("isOpen", false);
    }

    public void OpenDoor()
    {
        //gameObject.SetActive(false);
        anim.SetBool("isOpen", true);
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
