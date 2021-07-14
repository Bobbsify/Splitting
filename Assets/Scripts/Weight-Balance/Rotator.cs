using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms = new GameObject[2];
    [SerializeField] private float maxAngle;
    private GameObject leftPlatform;
    private GameObject rightPlatform;
    private Vector3 rotation;

    private void Awake()
    {
        leftPlatform = platforms[0].transform.position.x < platforms[1].transform.position.x ? platforms[0] : platforms[1];
        rightPlatform = leftPlatform == platforms[0] ? platforms[1] : platforms[0];
        rotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        Platform leftPlatformScript = leftPlatform.GetComponent<Platform>();
        Platform rightPlatformScript = rightPlatform.GetComponent<Platform>();
        //Check errors
        if (leftPlatformScript == null)
        {
            throw new Exception("Can't seem to find component Platform in " + leftPlatform);
        }
        if (rightPlatformScript == null)
        {
            throw new Exception("Can't seem to find component Platform in " + rightPlatform);
        }
        //Calculate Movement
        if (rightPlatformScript.score < leftPlatformScript.score)
        {
            LeanLeft();
        }
        else if (rightPlatformScript.score > leftPlatformScript.score)
        {
            LeanRight();
        }
        else //Tie
        {
            Normalize();
        }
        Debug.Log(rotation);
        Quaternion rot = new Quaternion();
        rot.eulerAngles = rotation;
        transform.localRotation = rot;
    }

    private void LeanLeft()
    {
        if (rotation.z <= maxAngle)
        {
            rotation.z++;
        }
    }

    private void LeanRight()
    {
        if (rotation.z >= -maxAngle)
        {
            rotation.z--;
        }
    }

    private void Normalize()
    {
        if (rotation.z < 0)
        {
            rotation.z++;
        }
        else if (rotation.z > 0)
        {
            rotation.z--;
        }
    }
    
}
