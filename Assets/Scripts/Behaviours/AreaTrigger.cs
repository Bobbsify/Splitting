using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent enteringEvents;
    [SerializeField] private UnityEvent stayingEvents;
    [SerializeField] private UnityEvent exitingEvents;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enteringEvents.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       stayingEvents.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        exitingEvents.Invoke();
    }
}
