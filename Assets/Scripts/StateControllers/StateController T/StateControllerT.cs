using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class StateControllerT : MonoBehaviour, StateControllerInterface
    {
        public bool hasControl;
        public bool forcedStop;
        public bool stopJump;
        public bool stopPickup;
        private bool stopPush;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;

        public bool isPushing;

        [SerializeField] private float speedInAir = 0.5f;

        private Move tyrMove;
        private Jump tyrJump;
        private Pickup tyrPickup;
        private Hacking tyrHacking;
        private FlashlightController tyrFlashlight;

        private AutobotUnity tyrAutobotUnity;
        private AutobotUnity antAutobotUnity;

        private SwitchCharacter switchCharacter;

        private GameObject trajectPred;
        private Throw getThrow;

        new private CameraController camera;
        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        private Rigidbody2D tyrRigidBody;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            // Get Move script  
            tyrMove = gameObject.GetComponent<Move>();

            // Get Jump script            
            tyrJump = gameObject.GetComponent<Jump>();

            // Get Pickup script
            tyrPickup = gameObject.GetComponent<Pickup>();

            // Get Hacking script
            tyrHacking = gameObject.GetComponentInChildren<Hacking>();

            // Get FlashlightController script
            tyrFlashlight = gameObject.GetComponentInChildren<FlashlightController>();

            // Get SwitchCharacter script
            switchCharacter = gameObject.GetComponent<SwitchCharacter>();

            // Get Tyr AutobotUnity script        
            tyrAutobotUnity = gameObject.GetComponent<AutobotUnity>();                     

            // Get Ant AutobotUnity script
            try
            {
                antAutobotUnity = switchCharacter.targetEntity.GetComponent<AutobotUnity>();
            }
            catch
            {
                throw new Exception("Ant AutobotUnity script not found");
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

            // Get RigidBody
            try
            {
                tyrRigidBody = gameObject.GetComponent<Rigidbody2D>();
            }
            catch
            {
                throw new Exception("RigidBody not found");
            }

            // Get animator
            animator = gameObject.GetComponent<Animator>();

            trajectPred = transform.Find("TrajectoryPrediction").gameObject;
            getThrow = trajectPred.GetComponentInChildren<Throw>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            ControlIfHasControl();

            CheckIfHasFallen();
            ResetLandBoolAfterHasFallen();

            ResetFallTime();
            
            ModifySpeedWhenFall();

            ResetVerticalSpeedWhenPushing();

            CallAnimator(forcedStop);

            if (hasControl)
            {
                ControlWhenCanMove();

                ControlWhenCanPickup();
                ControlWhenCanThrow();

                ControlWhenCanHack();

                tyrFlashlight.canUseFlashlight = true;

                tyrJump.enabled = true;
                tyrJump.canJump = false;

            }
            else
            {
                tyrMove.enabled = false;
                tyrPickup.enabled = false;
                getThrow.enabled = false;
                tyrHacking.enabled = false;

                tyrFlashlight.canUseFlashlight = false;

                ControlIfDisableJump();
            }           
        }        

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        private void CallAnimator(bool hasControl)
        {
            if (animator != null)
            {                
                animator.SetBool("hasControl", hasControl);
            }
        }

        void CheckIfHasFallen()
        {
            if (isGrounded)
            {
                if (tyrJump.wasJumping && !animator.IsInTransition(0)) // Controlla se Tyr è a terra dopo un salto/caduta
                {

                    tyrJump.isLanded = true; // Conferma l'atterraggio
                    tyrJump.wasJumping = false;                    

                    if (tyrJump.bigFall) // Se Tyr è atterrato dopo una caduta lunga più di un tot verrà considerata bigFall ed avverrà uno ScreenShake
                    {
                        camera.shakeMagnitude = shake;
                        camera.shakeRemain = shake;
                        camera.shakeLenght = lenght;

                        camera.exeShake = true;
                    }
                }
            }
        }

        void ControlIfHasControl()
        {
            if (gameObject.tag != "Player" || AnimatorIsPlaying("Tyr death") || forcedStop)
            {
                hasControl = false;
            }
            else if (!tyrAutobotUnity.connectionPrep && gameObject.tag == "Player" && !AnimatorIsPlaying("tyr death") && !forcedStop) // Viene controllato che connectionPrep sia false perché altrimenti il giocatore potrebbe muovere Ant
            {                                                                                                         // mentre sta avvenendo l'unione in Tyrant
                hasControl = true;
            }
        }

        void ResetLandBoolAfterHasFallen()
        {
            if (tyrJump.isLanded && (AnimatorIsPlaying("Tyr idle") || AnimatorIsPlaying("Tyr Walking")))
            {
                tyrJump.isLanded = false;                
            }
        }

        void ControlIfDisableJump()
        {
            if (isGrounded && AnimatorIsPlaying("Tyr idle"))
            {
                tyrJump.enabled = false;
            }
            else
            {
                tyrJump.enabled = true;
            }
        }

        void ControlWhenCanMove()
        {
            if (getThrow.rbToThrow)
            {
                tyrMove.enabled = false;
            }
            else
            {
                tyrMove.enabled = true;
            }            

            if (isWalled || (isPushing && stopPush) || AnimatorIsPlaying("Tyr hacking") || AnimatorIsPlaying("Tyr hacking2") || getThrow.rbToThrow || AnimatorIsPlaying("Tyr throw1") || AnimatorIsPlaying("Tyr throw4"))
            {
                tyrMove.canMove = false;
            }
            else
            {
                tyrMove.canMove = true;
            }
        }

        void ControlWhenCanPickup()
        {
            if (stopPickup || !isGrounded || AnimatorIsPlaying("Tyr hacking") || AnimatorIsPlaying("Tyr hacking2") || getThrow.rbToThrow || AnimatorIsPlaying("Tyr throw1") || AnimatorIsPlaying("Tyr throw4"))
            {
                tyrPickup.enabled = false;
            }
            else
            {
                tyrPickup.enabled = true;
            }
        }

        void ControlWhenCanThrow()
        {
            if (!isGrounded || AnimatorIsPlaying("Tyr hacking") || AnimatorIsPlaying("Tyr hacking2"))
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
            tyrHacking.enabled = true;

            if (!isGrounded || getThrow.rbToThrow || AnimatorIsPlaying("Tyr throw1") || AnimatorIsPlaying("Tyr throw4"))
            {
                tyrHacking.canHack = false;
            }
            else
            {
                tyrHacking.canHack = true;
            }
        }

        void ModifySpeedWhenFall()
        {
            if (!isGrounded)
            {
                tyrMove.speedModifier = speedInAir;
            }
            else
            {
                tyrMove.speedModifier = 1.0f;
            }
        }

        void ResetFallTime()
        {
            if (isGrounded)
            {
                tyrJump.elapsedFall = 0.0f;
            }
        }

        void ResetVerticalSpeedWhenPushing()
        {
            if (isGrounded && isPushing)
            {
                tyrJump.velocityY = 0.0f;
            }
            else
            {
                tyrJump.velocityY = tyrRigidBody.velocity.y;
            }
        }

        public void DisableControl()
        {
            forcedStop = true;
        }

        public void EnableControl()
        {
            forcedStop = false;
        }

        public void DisableJump()
        {
            stopJump = true;
        }

        public void EnableJump()
        {
            stopJump = false;
        }

        public void DisablePickup()
        {
            stopPickup = true;
        }

        public void EnablePickup()
        {
            stopPickup = false;
        }

        public void DisablePush()
        {
            stopPush = true;
        }

        public void EnablePush()
        {
            stopPush = false;
        }
    }
}
