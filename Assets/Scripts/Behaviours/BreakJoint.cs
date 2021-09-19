using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakJoint : MonoBehaviour
{
    [SerializeField] private Joint2D[] targets;

    public void Break()
    {
        foreach (Joint2D joint in targets)
        {
            joint.breakForce = 0;
        }
    }
}
