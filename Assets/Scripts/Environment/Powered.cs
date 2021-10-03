using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powered : MonoBehaviour
{
    [SerializeField] private bool absorbBattery = false;

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
        if (!absorbBattery)
        {
            if (collision.TryGetComponent(out pow))
            {
                SetPower(true);
            }
        }
        else
        {
            if (collision.transform.parent != null)
            {
                if (collision.TryGetComponent(out pow) && !collision.transform.parent.name.ToLower().Contains("bone_"))
                {
                    Destroy(collision.gameObject); //remove object
                    SetPower(true);
                }
            }
            else
            {
                if (collision.TryGetComponent(out pow))
                {
                    Destroy(collision.gameObject); //remove object
                    SetPower(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!absorbBattery) { 
            Power pow;
            if (collision.TryGetComponent(out pow))
            {
                SetPower(false);
            }
        }
    }
}
