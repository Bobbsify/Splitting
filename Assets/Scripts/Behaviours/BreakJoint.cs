using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakJoint : MonoBehaviour
{
    [SerializeField] private Joint2D target;

    public void Break()
    {
        target.breakForce = 0;
        Debug.Log(target.breakForce);
    }
}
