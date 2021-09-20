using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animation variables for this are:
 * 
 * FadeIn --> Trigger
 * 
 * FadeOut --> Trigger
 *
 */

public class ConveyorBeltPlatformController : MonoBehaviour
{
    private Animator anim;

    [HideInInspector] public bool move = false;
    [HideInInspector] public bool reachedX = false, reachedY = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            throw new System.Exception("could not find animator for platform " + this);
        }
    }

    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
        move = true;
    }

    public void FadeOut()
    {
        move = false;
        anim.SetTrigger("FadeOut");
    }

    public void DestroyPlatform()
    {
        Destroy(this);
    }
}
