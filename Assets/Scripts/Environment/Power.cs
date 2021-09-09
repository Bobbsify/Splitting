using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Powered pow;
        if (collision.gameObject.TryGetComponent(out pow))
        {
            Debug.Log("Entering");
            pow.SetPower(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Powered pow;
        if (collision.gameObject.TryGetComponent(out pow))
        {
            Debug.Log("Exiting");
            pow.SetPower(false);
        }

    }
}
