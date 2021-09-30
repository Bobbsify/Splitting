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

        public bool isPushing;        

        [SerializeField] private float elapsedInDark;
        [SerializeField] private float timerInDark = 0.5f;

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

        private Rigidbody2D tyrantRigidBody;

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

            // Get RigidBody
            try
            {
                tyrantRigidBody = gameObject.GetComponent<Rigidbody2D>();
            }
            catch
            {
                throw new Exception("RigidBody not found");
            }

            jumpButton = new InputSettings().JumpButton;
        }

        // Update is called once per frame
        void Update()
        {
            ControlIfHasControl();

            CheckIfHasFallen();
            ResetLandBoolAfterHasFallen();
            ResetFallTime();

            TyrantIsNotAfraid();

            ModifySpeedWhenJump();

            ResetVerticalSpeedWhenPushing();            

            CallAnimator(isNotScared, getThrow.throwing, isGrounded, getThrow.rbToThrow);

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

                tyrantFlashlight.canUseFlashlight = true;

                tyrantJump.canJump = false;
                ControlIfDisableJump();
            }
            
        }       

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        void TyrantIsNotAfraid()
        {
            if (tyrantFlashlight.lightsAre || isIlluminated || !isGrounded)
            {                
                elapsedInDark = 0.0f;
                isNotScared = true;
            }
            else if ((!isIlluminated || !tyrantFlashlight.lightsAre) && isGrounded)
            {
                elapsedInDark += Time.deltaTime;
                
                if (elapsedInDark >= timerInDark)
                {
                    isNotScared = false;
                }                
            }
        }

        private void CallAnimator(bool isNotScared, bool throwingMode, bool isGrounded, bool isCarrying)
        {
            if (animator != null)
            {
                animator.SetBool("isNotScared", isNotScared);
                animator.SetBool("isThrowing", throwingMode);
                animator.SetBool("isGrounded", isGrounded);
                animator.SetBool("isCarrying", isCarrying);                
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
                        camera.shakeMagnitude = shake;
                        camera.shakeRemain = shake;
                        camera.shakeLenght = lenght;

                        camera.exeShake = true;
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
            
            if (isWalled || (isPushing && Input.GetKey(jumpButton)) || (tyrantJump.chargeJump && !getThrow.rbToThrow) || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || getThrow.throwing || AnimatorIsPlaying("TyrantHacking") || AnimatorIsPlaying("TyrantLift1") || AnimatorIsPlaying("TyrantLift2") || AnimatorIsPlaying("TyrantLift3") || AnimatorIsPlaying("TyrantButtonPress"))
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

            if (isGrounded && !tyrantExtend.isExtended && !getThrow.rbToThrow && !isObstructed && (!AnimatorIsPlaying("Tyrant grab3") && !AnimatorIsPlaying("Tyrant grab")) && !AnimatorIsPlaying("TyrantLift3") && !AnimatorIsPlaying("TyrantHacking"))
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

            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || isObstructed || AnimatorIsPlaying("TyrantHacking"))
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
            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || tyrantExtend.isExtended || AnimatorIsPlaying("TyrantHacking") || AnimatorIsPlaying("TyrantLift3"))
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
            if (!isGrounded || AnimatorIsPlaying("TyrantHacking"))
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

            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyrant grab3") || AnimatorIsPlaying("Tyrant grab") || tyrantExtend.isExtended || AnimatorIsPlaying("TyrantLift3"))
            {
                tyrantHacking.canHack = false;
            }
            else
            {
                tyrantHacking.canHack = true;
            }
        }

        void ResetFallTime()
        {
            if (isGrounded)
            {
                tyrantJump.elapsedFall = 0.0f;
            }
        }        

        void ResetVerticalSpeedWhenPushing()
        {
            if (isGrounded && isPushing)
            {
                tyrantJump.velocityY = 0.0f;
            }
            else
            {
                tyrantJump.velocityY = tyrantRigidBody.velocity.y;
            }
        }        

    }
}
