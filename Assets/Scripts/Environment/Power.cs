using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        Powered pow;
        if (collision.gameObject.TryGetComponent(out pow))
        {
            pow.SetPower(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Powered pow;
        if (collision.gameObject.TryGetComponent(out pow))
        {
            pow.SetPower(false);
        }

    }
}
