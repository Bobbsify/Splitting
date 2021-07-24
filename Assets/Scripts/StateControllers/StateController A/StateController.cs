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
        public KeyCode jumpButton;
        public Move move;
        public Carry carry;

        public AutobotUnity autobotUnityA;
        public AutobotUnity autobotUnityT;

        private Animator animator;

        new public CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            jumpButton = new InputSettings().JumpButton;

            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
            if (jump.isLanded && !AnimatorIsPlaying("AntJump5"))
            {
                jump.isLanded = false;
            }
            
            if (isGrounded)
            {
                if (jump.wasJumping && !animator.IsInTransition(0))
                {
                    jump.isLanded = true;
                    jump.wasJumping = false;

                    ScreenShake(shake, lenght);
                }
            }

            if (hasControl)
            {
                jump.enabled = true;
                carry.enabled = true;
                move.enabled = true;

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

                    if (AnimatorIsPlaying("AntJump5") || AnimatorIsPlaying("AntButtonPress"))
                    {
                        move.canMove = false;
                        jump.canJump = false;
                        move.canCrouch = false;
                    }                    
                    else if (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd"))
                    {
                        jump.canJump = false;
                        move.canCrouch = false;
                        move.canMove = false;
                    }
                    else 
                    {
                        if (carry.isCarrying)
                        {
                            jump.canJump = true;
                            jump.superJump = false;
                            jump.jumpDivider = jump.jumpMultiplier;
                            move.canCrouch = false;

                            autobotUnityA.canConnect = false;
                            autobotUnityT.canConnect = false;
                        }
                        else
                        {
                            jump.canJump = true;
                            jump.jumpDivider = 1.0f;                            
                            move.canCrouch = true;

                            autobotUnityA.canConnect = true;
                            autobotUnityT.canConnect = true;
                        }
                       
                    }

                    if (Input.GetKey(jumpButton))
                    {
                         move.canMove = false;
                    }

                    if (move.isCrouched)
                    {
                         jump.canJump = false;
                         jump.superJump = false;
                    }


                    if (isObstructed && move.isCrouched)
                    {
                         move.isObstructed = true;
                    }
                    else
                    {
                         move.isObstructed = false;
                    }

                    if (isObstructed || carry.isCarrying)
                    {
                        carry.canCarry = false;
                    }
                    else
                    {
                        carry.canCarry = true;
                    }
                }

                if (!isGrounded)
                {
                    jump.canJump = false;
                    move.canCrouch = false;
                    carry.canCarry = false;

                    if (!isWalled)
                    {
                         move.canMove = true;
                    }
                }               
                
            }
            else
            {                
                move.enabled = false;
                jump.canJump = false;

                if (isGrounded && AnimatorIsPlaying("AntIdle"))
                {
                    jump.enabled = false;
                }
                else
                {
                    jump.enabled = true;
                }
            }
            
            if (gameObject.tag != "Player" )
            {
                hasControl = false;
            }
            else if (!autobotUnityA.connectionPrep && gameObject.tag == "Player")
            {
                hasControl = true;
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
