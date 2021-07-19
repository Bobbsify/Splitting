using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillArea : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Animator anim = collision.transform.GetComponent<Animator>();
        try
        {
            anim.SetTrigger("death");
        }
        catch
        {
            Debug.LogWarning("entity has no animator attached");
        }
    }
}
