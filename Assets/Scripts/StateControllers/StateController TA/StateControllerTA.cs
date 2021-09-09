using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class StateControllerTA : MonoBehaviour
    {
        public bool hasControl;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;
        public bool isNotScared;
        public bool isIlluminated;

        [SerializeField] private float speedInAir = 0.5f;

        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        private KeyCode jumpButton;
        private Jump tyrantJump;        
        private Move tyrantMove;
        private Extend tyrantExtend;
        private Pickup tyrantPickup;

        private GameObject trajectPred;
        private Throw getThrow;
        private Hacking tyrantHacking;
        private FlashlightController tyrantFlashlight;

        private Animator animator;

        new private CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            // Get Move script  
            tyrantMove = gameObject.GetComponent<Move>();

            // Get Extend script
            tyrantExtend = gameObject.GetComponent<Extend>();

            // Get Jump script            
            tyrantJump = gameObject.GetComponent<Jump>();

            // Get Pickup script
            tyrantPickup = gameObject.GetComponent<Pickup>();

            // Get Hacking script
            tyrantHacking = gameObject.GetComponentInChildren<Hacking>();

            // Get FlashlightController script
            tyrantFlashlight = gameObject.GetComponentInChildren<FlashlightController>();

            animator = gameObject.GetComponent<Animator>();

            trajectPred = GameObject.Find("TrajectoryPredictionTA");
            getThrow = trajectPred.GetComponentInChildren<Throw>();

            // Get CameraController script
            try
            {
                camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }
            catch
            {
                throw new Exception("Camera not found");
            }

            jumpButton = new InputSettings().JumpButton;
        }

        // Update is called once per frame
        void Update()
        {
            ControlIfHasControl();

            CheckIfHasFallen();
            ResetLandBoolAfterHasFallen();

            TyrantIsNotAfraid();

            ModifySpeedWhenJump();

            CallAnimator(isNotScared, getThrow.throwing);

            if (hasControl)
            {
                ControlWhenCanMove();
                ControlWhenCanExtend();

                ControlWhenCanJump();

                ControlWhenCanPickup();
                ControlWhenCanThrow();

                ControlWhenCanHack();

                tyrantJump.jumpDivider = tyrantJump.jumpMultiplier;
                tyrantFlashlight.canUseFlashlight = true;
                tyrantMove.canCrouch = false;

            }
            else
            {
                tyrantMove.enabled = false;
                tyrantExtend.enabled = false;

                tyrantPickup.enabled = false;
                getThrow.enabled = false;

                tyrantHacking.enabled = false;

                tyrantFlashlight.canUseFlashlight = false;

                tyrantJump.canJump = false;
                ControlIfDisableJump();
            }

            /*
            if (tyrantJump.isLanded && !AnimatorIsPlaying("Tyrant Jump 4"))
            {
                tyrantJump.isLanded = false;
            }

            TyrantIsNotAfraid();

            if (!isNotScared)
            {
                hasControl = false;
            }

            if (isGrounded)
            {
                if (tyrantJump.wasJumping && !animator.IsInTransition(0))
                {
                    tyrantJump.isLanded = true;
                    tyrantJump.wasJumping = false;

                    if (tyrantJump.bigFall)
                    {
                        ScreenShake(shake, lenght);
                    }
                }
            }

            if (hasControl)
            {

                tyrantJump.enabled = true;
                tyrantMove.enabled = true;
                tyrantHacking.enabled = true;

                tyrantFlashlight.canUseFlashlight = true;
                tyrantMove.canCrouch = false;

                if (isWalled || Input.GetKey(jumpButton))
                {
                    tyrantMove.canMove = false;
                }
                else
                {
                    tyrantMove.canMove = true;
                }

                if (isGrounded)
                {
                    tyrantPickup.enabled = true;                                   

                    if (getThrow.throwing || AnimatorIsPlaying("TyrantUnity") || AnimatorIsPlaying("TyrantUnlink"))
                    {
                        tyrantMove.enabled = false;
                        tyrantJump.canJump = false;                        
                    }
                    else if (getThrow.rbToThrow)
                    {
                        tyrantMove.enabled = true;
                        tyrantJump.canJump = false;
                    }
                    else
                    {
                        tyrantMove.enabled = true;
                        tyrantJump.jumpDivider = tyrantJump.jumpMultiplier;
                        tyrantJump.canJump = true;
                    }

                }
                else
                {
                    tyrantPickup.enabled = false;
                }

            }
            else
            {                
                tyrantPickup.enabled = false;
                tyrantMove.enabled = false;
                tyrantHacking.enabled = false;

                tyrantJump.canJump = false;

                if (isGrounded && AnimatorIsPlaying("Tyrant idle"))
                {
                    tyrantJump.enabled = false;
                }
                else
                {
                    tyrantJump.enabled = true;
                }

                if (gameObject.tag == "Player")
                {
                    tyrantFlashlight.canUseFlashlight = true;
                }
                else
                {
                    tyrantFlashlight.canUseFlashlight = false;
                }
                
            }

            if (gameObject.tag != "Player")
            {
                hasControl = false;
            }
            else if (gameObject.tag == "Player")
            {
                hasControl = true;
            }

            CallAnimator(isNotScared);

            */
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

        void TyrantIsNotAfraid()
        {
            if (tyrantFlashlight.lightsAre || isIlluminated || isGrounded)
            {              
                isNotScared = true;
            }
            else if ((!isIlluminated || !tyrantFlashlight.lightsAre) && isGrounded)
            {
                isNotScared = false;
            }
        }

        private void CallAnimator(bool isNotScared, bool throwingMode)
        {
            if (animator != null)
            {
                animator.SetBool("isNotScared", isNotScared);
                animator.SetBool("isThrowing", throwingMode);
            }
        }

       void ControlIfHasControl()
       {
            if (gameObject.tag != "Player" || AnimatorIsPlaying("TyrantDeath") || !isNotScared || AnimatorIsPlaying("TyrantUnity") || AnimatorIsPlaying("TyrantUnlink"))
            {
                hasControl = false;
            }
            else if (gameObject.tag == "Player" && !AnimatorIsPlaying("TyrantDeath") && isNotScared && !AnimatorIsPlaying("TyrantUnity") && !AnimatorIsPlaying("TyrantUnlink"))
            {
                hasControl = true;
            }
       }

        void CheckIfHasFallen()
        {
            if (isGrounded)
            {
                if (tyrantJump.wasJumping && !animator.IsInTransition(0)) 
                {

                    tyrantJump.isLanded = true; 
                    tyrantJump.wasJumping = false;

                    if (tyrantJump.bigFall) 
                    {
                        ScreenShake(shake, lenght);
                    }
                }
            }
        }

        void ResetLandBoolAfterHasFallen()
        {
            if (tyrantJump.isLanded && (AnimatorIsPlaying("Tyrant idle") || AnimatorIsPlaying("Tyrant walking") || AnimatorIsPlaying("Tyrant grab walking") || AnimatorIsPlaying("Tyrant grab2")))
            {
                tyrantJump.isLanded = false;
            }
        }

        void ModifySpeedWhenJump()
        {
            if (!isGrounded)
            {
                tyrantMove.speedModifier = speedInAir;
            }
            else
            {
                tyrantMove.speedModifier = 1.0f;
            }
        }

        void ControlIfDisableJump()
        {
            if (isGrounded && AnimatorIsPlaying("Tyrant idle") || AnimatorIsPlaying("Tyrant grab2"))
            {
                tyrantJump.enabled = false;
            }
            else
            {
                tyrantJump.enabled = true;
            }
        }

        void ControlWhenCanMove()
        {            
            if (getThrow.throwing)
            {                
                tyrantMove.enabled = false;
            }
            else
            {
                tyrantMove.enabled = true;
            }              
            
            if (isWalled || (Input.GetKey(jumpButton) && !getThrow.rbToThrow) || tyrantExtend.isExtended || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || getThrow.throwing) //  bisogna controllare anche le animazioni di hacking e lift che ancora non ci sono
            {
                tyrantMove.canMove = false;
            }
            else
            {
                tyrantMove.canMove = true;
            }
        }

        void ControlWhenCanJump()
        {
            tyrantJump.enabled = true;

            if (isGrounded && !tyrantExtend.isExtended && !getThrow.rbToThrow && !isObstructed && (!AnimatorIsPlaying("Tyrant grab3") && !AnimatorIsPlaying("Tyrant grab"))) // manca il controllo sulla discesa da extend e sull'hacking
            {
                tyrantJump.canJump = true;
            }
            else
            {
                tyrantJump.canJump = false;
            }
        }

        void ControlWhenCanExtend()
        {
            tyrantExtend.enabled = true;

            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || isObstructed) // manca il controllo sull'hacking
            {
                tyrantExtend.canExtend = false;
            }
            else
            {
                tyrantExtend.canExtend = true;
            }
        }

        void ControlWhenCanPickup()
        {
            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || tyrantExtend.isExtended) // manca il controllo sull'hacking e sul lift in discesa
            {
                tyrantPickup.enabled = false;
            }
            else
            {
                tyrantPickup.enabled = true;
            }
        }

        void ControlWhenCanThrow()
        {
            if (!isGrounded) // Manca il controllo sull'hacking
            {
                getThrow.enabled = false;
            }
            else
            {
                getThrow.enabled = true;
            }
        }

        void ControlWhenCanHack()
        {
            tyrantHacking.enabled = true;

            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || tyrantExtend.isExtended) // manca il controllo sulla discesa dal lift 
            {
                tyrantHacking.canHack = false;
            }
            else
            {
                tyrantHacking.canHack = true;
            }
        }       

    }
}
