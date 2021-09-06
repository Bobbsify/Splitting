using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powered : MonoBehaviour
{
    [SerializeField] private UnityEvent turnOnEvents;
    [SerializeField] private UnityEvent turnOffEvents;

    public void SetPower(bool pow)
    {
        if (pow)
        {
            turnOnEvents.Invoke();
        }
        else
        {
            turnOffEvents.Invoke();
        }
    }
}
