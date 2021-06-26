using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public bool hasControl;

    public bool isGrounded;
    public bool isWalled;

    public Jump jump;
    public Move move;

    // Start is called before the first frame update
    void Start()
    {
        hasControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl)
        {

            if (isWalled)
            {
                move.canMove = false;            
            } else
            {
                move.canMove = true;
            }

            if (isGrounded)
            {
                jump.canJump = true;
            }

            if (jump.isJumping)
            {
                jump.canJump = false;
            }
        }        
        
    }
    
}
