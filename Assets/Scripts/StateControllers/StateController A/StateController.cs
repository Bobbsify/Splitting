using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


        private KeyCode jumpButton;
        private Jump antJump;
        private Move antMove;
        private Carry antCarry;

        private AutobotUnity antAutobotUnity;
        private AutobotUnity tyrAutobotUnity;

        private Animator animator;

        new private CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            jumpButton = new InputSettings().JumpButton;

            // Get Move script  
            antMove = gameObject.GetComponent<Move>();                     

            // Get Jump script            
            antJump = gameObject.GetComponent<Jump>();
                        
            // Get Carry script                        
            antCarry = gameObject.GetComponent<Carry>();                       

            // Get Animator                        
            animator = gameObject.GetComponent<Animator>();
                        
            // Get Ant AutobotUnity script        
            antAutobotUnity = gameObject.GetComponent<AutobotUnity>();          
            
            // Get Tyr AutobotUnity script
            try
            {
                tyrAutobotUnity = GameObject.Find("Tyr").GetComponent<AutobotUnity>();
            }
            catch
            {
                throw new Exception("Tyr AutobotUnity script not found");
            }

            // Get CameraController script
            try
            {
                camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }
            catch
            {
                throw new Exception("Camera not found");
            }
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
            if (antJump.isLanded && !AnimatorIsPlaying("AntJump5"))
            {
                antJump.isLanded = false;
            }
            
            if (isGrounded)
            {
                if (antJump.wasJumping && !animator.IsInTransition(0))
                {
                    antJump.isLanded = true;
                    antJump.wasJumping = false;

                    ScreenShake(shake, lenght);
                }
            }

            if (hasControl)
            {
                antJump.enabled = true;
                antCarry.enabled = true;
                antMove.enabled = true;

                if (isWalled)
                {
                     antMove.canMove = false;
                }
                else
                {
                     antMove.canMove = true;
                }

                if (isGrounded)
                {                   

                    if (AnimatorIsPlaying("AntJump5") || AnimatorIsPlaying("AntButtonPress"))
                    {
                        antMove.canMove = false;
                        antJump.canJump = false;
                        antMove.canCrouch = false;
                    }                    
                    else if (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd"))
                    {
                        antJump.canJump = false;
                        antMove.canCrouch = false;
                        antMove.canMove = false;
                    }
                    else 
                    {
                        if (antCarry.isCarrying)
                        {
                            antJump.canJump = true;
                            antJump.superJump = false;
                            antJump.jumpDivider = antJump.jumpMultiplier;
                            antMove.canCrouch = false;

                            antAutobotUnity.canConnect = false;
                            tyrAutobotUnity.canConnect = false;
                        }
                        else
                        {
                            antJump.canJump = true;
                            antJump.jumpDivider = 1.0f;                            
                            antMove.canCrouch = true;

                            antAutobotUnity.canConnect = true;
                            tyrAutobotUnity.canConnect = true;
                        }
                       
                    }

                    if (Input.GetKey(jumpButton))
                    {
                         antMove.canMove = false;
                    }

                    if (antMove.isCrouched)
                    {
                         antJump.canJump = false;
                         antJump.superJump = false;
                    }


                    if (isObstructed && antMove.isCrouched)
                    {
                         antMove.isObstructed = true;
                    }
                    else
                    {
                         antMove.isObstructed = false;
                    }

                    if (isObstructed || antCarry.isCarrying)
                    {
                        antCarry.canCarry = false;
                    }
                    else
                    {
                        antCarry.canCarry = true;
                    }
                }

                if (!isGrounded)
                {
                    antJump.canJump = false;
                    antMove.canCrouch = false;
                    antCarry.canCarry = false;

                    if (!isWalled)
                    {
                         antMove.canMove = true;
                    }
                }               
                
            }
            else
            {                
                antMove.enabled = false;
                antJump.canJump = false;

                if (isGrounded && AnimatorIsPlaying("AntIdle"))
                {
                    antJump.enabled = false;
                }
                else
                {
                    antJump.enabled = true;
                }
            }
            
            if (gameObject.tag != "Player" )
            {
                hasControl = false;
            }
            else if (!antAutobotUnity.connectionPrep && gameObject.tag == "Player")
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
