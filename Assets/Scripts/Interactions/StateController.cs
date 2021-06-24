using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public bool isGrounded;

    public Jump jump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            jump.canJump = true;
        }

        if (jump.jumping)
        {
            isGrounded = false;
        }
        
    }
    
}
