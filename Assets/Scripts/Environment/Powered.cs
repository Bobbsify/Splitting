using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powered : MonoBehaviour
{
    [SerializeField] private UnityEvent turnOnEvents;
    [SerializeField] private UnityEvent turnOffEvents;

    private void SetPower(bool pow)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Power pow;
        if (collision.TryGetComponent(out pow))
        {
            SetPower(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Power pow;
        if (collision.TryGetComponent(out pow))
        {
            SetPower(false);
        }
    }
}
