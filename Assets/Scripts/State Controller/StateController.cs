using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class StateController : MonoBehaviour
    {
        public bool hasControl;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;

        public Jump jump;
        public Move move;

        // Start is called before the first frame update
        void Start()
        {
            hasControl = true;
        }

        // Update is called once per frame
        void LateUpdate()
        {          

            if (hasControl)
            {         
                
                if (isWalled)
                {
                     move.canMove = false;
                }
                else
                {
                     move.canMove = true;
                }

                if (isGrounded)
                {
                    if (jump.wasJumping)
                    {
                        jump.isLanded = true;
                        jump.wasJumping = false;
                    }                    

                    jump.canJump = true;
                    move.canCrouch = true;

                    if (jump.jumpKeyDown)
                    {
                         move.canMove = false;
                    }

                    if (move.isCrouched)
                    {
                         jump.canJump = false;
                    }


                    if (isObstructed && move.isCrouched)
                    {
                         move.isObstructed = true;
                    }
                    else
                    {
                         move.isObstructed = false;
                    }
                }

                if (!isGrounded)
                {
                    jump.canJump = false;
                    move.canCrouch = false;

                    if (!isWalled)
                    {
                         move.canMove = true;
                    }
                }
                
            }

        }

    }
}
