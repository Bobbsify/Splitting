using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseposition = Input.mousePosition;
        {
            Debug.Log(mouseposition.x);
            Debug.Log(mouseposition.y);
        }
    }
}
