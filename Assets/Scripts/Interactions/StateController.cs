using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public bool hasControl;

    public bool isGrounded;
    public bool isWalled;
    public bool isObstructed;

    public Jump jump;
    public Move move;  // devi lasciarmi public canCrouch e crouchKey. Poi da sotto con i commenti dovresti capire come voglio gestirlo

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
                // move.canCrouch = true;

                if (jump.jumpKey)
                {
                    move.canMove = false;
                    //move.canCrouch = false;
                }

                /* if (move.crouchKey)
                 * {
                 *    jump.canJump = false;
                 * }                
                */

                if (isObstructed) // && move.crouchKey
                {
                    // move.crouchKey = true;
                }
            }

            if (!isGrounded)
            {                
                jump.canJump = false;
                // move.canCrouch = false;

                if (!isWalled)
                {
                    move.canMove = true;
                }
            }
        } 
        
    }
    
}
