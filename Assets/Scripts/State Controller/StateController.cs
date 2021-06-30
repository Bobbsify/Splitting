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

        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        public Jump jump;
        public Move move;

        private Animator animator;

        new public CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            hasControl = true;

            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
            if (jump.isLanded && !AnimatorIsPlaying("AntDeath"))
            {
                jump.isLanded = false;
            }       
                        
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
                    if (jump.wasJumping && !animator.IsInTransition(0))
                    {
                        jump.isLanded = true;
                        jump.wasJumping = false;

                        ScreenShake(shake, lenght);                        
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

        private void ScreenShake(float shake, float lenght)
        {
            if (shake > camera.shakeRemain)
            {
                camera.shakeMagnitude = shake;
                camera.shakeRemain = shake;
                camera.shakeLenght = lenght;
            }
        }

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

    }
}
