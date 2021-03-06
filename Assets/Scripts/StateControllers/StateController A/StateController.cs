using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class StateController : MonoBehaviour, StateControllerInterface
    {
        public bool hasControl;

        public bool forcedStop;
        public bool stopJump;
        public bool stopDrop;
        private bool stopPush;

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
        private Jump antJump;
        private Move antMove;
        private Carry antCarry;
        private Extend antExtend;

        private AutobotUnity antAutobotUnity;
        private AutobotUnity tyrAutobotUnity;

        private SwitchCharacter switchCharacter;

        private Animator animator;

        private Rigidbody2D antRigidBody;

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

            // Get Extend script
            antExtend = gameObject.GetComponent<Extend>();
                                    
            // Get SwitchCharacter script
            switchCharacter = gameObject.GetComponent<SwitchCharacter>();

            // Get Ant AutobotUnity script        
            antAutobotUnity = gameObject.GetComponent<AutobotUnity>();

            // Get Tyr AutobotUnity script
            try
            {
                tyrAutobotUnity = switchCharacter.targetEntity.GetComponent<AutobotUnity>();
            }
            catch
            {
                throw new Exception("Tyr AutobotUnity script not found");
            }

            // Get Animator                        
            animator = gameObject.GetComponent<Animator>();

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
                antRigidBody = gameObject.GetComponent<Rigidbody2D>();
            }
            catch
            {
                throw new Exception("RigidBody not found");
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {            
            ControlIfHasControl(); // Se il Player sta giocando nei panni di Ant gli vengono assegnati i controlli, mentre se si switcha a Tyr o muore gli vengono tolti 

            CheckIfHasFallen(); // Controlla se Ant ? atterrato dopo un salto/caduta
            ResetLandBoolAfterHasFallen(); // Controlla se l'animazione di atterraggio di Ant ? terminata e resetta la booleana per una prossima caduta
            ResetFallTime();

            AntIsNotAfraid(); // Controlla se Ant ? spaventato o no          

            ModifySpeedWhenJump();            

            ResetVerticalSpeedWhenPushing();           

            //ControlWhenForceDrop();
            CallAnimator(isNotScared, isGrounded, forcedStop);

            if (hasControl)
            {
                ControlWhenCanMove();

                ControlWhenCanCrouch();
                ControlIfMustRemainCrouched();
                ControlWhenCanExtend();

                ControlWhenCanJump();
                ControlIfCanSuperJump();

                ControlWhenCanCarry();
                ControlWhenCanDrop();                
            }
            else // Questo ? ci? che avviene quando hasControl == false
            {                
                antMove.enabled = false; // Disabilito l'intera script Move di Ant
                antExtend.enabled = false; // Disabilito l'intera script Extend di Ant
                                
                antJump.canJump = false; // Ant non pu? saltare
                ControlIfDisableJump();

                antCarry.canDrop = false;
                if (isNotScared)
                {
                    antCarry.canCarry = true;
                }
                else
                {
                    antCarry.canCarry = false;
                }                
            }

        }       

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        void AntIsNotAfraid()
        {
            if (isIlluminated || !isGrounded)
            {
                elapsedInDark = 0.0f;
                isNotScared = true;
            }
            else
            {
                elapsedInDark += Time.deltaTime;

                if (elapsedInDark >= timerInDark)
                {
                    isNotScared = false;
                }                
            }
        }       
        

        void ControlIfHasControl()
        {
            if (gameObject.tag != "Player" || AnimatorIsPlaying("AntDeath") || !isNotScared || forcedStop)
            {
                hasControl = false;
            }
            else if (!antAutobotUnity.connectionPrep && gameObject.tag == "Player" && !AnimatorIsPlaying("AntDeath") && isNotScared && !forcedStop) // Viene controllato che connectionPrep sia false perch? altrimenti il giocatore potrebbe muovere Ant
            {                                                                                                         // mentre sta avvenendo l'unione in Tyrant
                hasControl = true;
            }
        }

        private void CallAnimator(bool isNotScared, bool isGrounded, bool hasControl)
        {
            if (animator != null)
            {
                animator.SetBool("isNotScared", isNotScared);
                animator.SetBool("isGrounded", isGrounded);
                animator.SetBool("hasControl", hasControl);
            }
        }       

        void CheckIfHasFallen()
        {
            if (isGrounded)
            {
                if (antJump.wasJumping && !animator.IsInTransition(0)) // Controlla se Ant ? a terra dopo un salto/caduta
                {
                    
                    antJump.isLanded = true; // Conferma l'atterraggio
                    antJump.wasJumping = false;

                    if (antJump.bigFall) // Se Ant ? atterrato dopo una caduta lunga pi? di un tot verr? considerata bigFall ed avverr? uno ScreenShake
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
            if (antJump.isLanded && (AnimatorIsPlaying("AntIdle") || AnimatorIsPlaying("AntWalking") || AnimatorIsPlaying("AntCarrying") || AnimatorIsPlaying("AntCarryingIdle")))
            {
                antJump.isLanded = false;
            }
        }    
              

        void ControlWhenCanMove()
        {            
            antMove.enabled = true;

            if (isWalled || (isPushing && (Input.GetKey(jumpButton) || stopPush)) || (antJump.chargeJump && !antMove.isCrouched) || AnimatorIsPlaying("AntLift1") || AnimatorIsPlaying("AntLift2") || AnimatorIsPlaying("AntLift3") || AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress")) //  || (isPushing && Input.GetKey(jumpButton))
            {
                
                antMove.canMove = false;
            }
            else
            {
                
                antMove.canMove = true;
            }

        }

        void ControlWhenCanJump()
        {
            antJump.enabled = true;

            if (!stopJump && isGrounded && !antExtend.isExtended && !antMove.isCrouched && (!AnimatorIsPlaying("AntCarryingAdjust") && !AnimatorIsPlaying("AntCarryingEnd") && !AnimatorIsPlaying("AntButtonPress")) && !isObstructed)
            {
                antJump.canJump = true;
            }
            else
            {
                antJump.canJump = false;
            }
        }

        void ControlWhenCanCrouch()
        {
            if (!isGrounded || antCarry.isCarrying || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress")) || antExtend.isExtended)
            {
                antMove.canCrouch = false;
            }
            else
            {
                antMove.canCrouch = true;
            }
        }

        void ControlWhenCanExtend()
        {
            antExtend.enabled = true;

            if (!isGrounded || antCarry.isCarrying || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress")) || antMove.isCrouched || isObstructed)
            {
                antExtend.canExtend = false;
            }
            else
            {
                antExtend.canExtend = true;
            }
        }

        void ControlIfCanSuperJump()
        {
            if (antCarry.isCarrying || antMove.isCrouched || antExtend.isExtended)
            {
                antJump.jumpDivider = antJump.jumpMultiplier;
                antJump.superJump = false;
            }
            else
            {
                antJump.jumpDivider = 1.0f;
            }
        }

        void ControlWhenCanCarry()
        {
            antCarry.enabled = true;

            if (stopDrop || Input.GetKey(jumpButton) || antCarry.isCarrying || !isGrounded || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress")) || antMove.isCrouched || antExtend.isExtended)
            {
                antCarry.canCarry = false;
            }
            else
            {
                antCarry.canCarry = true;
            }
        }

        void ControlWhenCanDrop()
        {
            if (stopDrop || Input.GetKey(jumpButton) || !antCarry.isCarrying || !isGrounded || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntButtonPress")) || isObstructed || antMove.isCrouched || antExtend.isExtended)
            {
                antCarry.canDrop = false;
            }
            else
            {
                antCarry.canDrop = true;
            }
        }

        void ControlIfDisableJump()
        {
            if (isGrounded && AnimatorIsPlaying("AntIdle") || AnimatorIsPlaying("AntCarryingIdle"))
            {
                antJump.enabled = false;
            }
            else
            {
                antJump.enabled = true;
            }
        }
        
        void ControlIfMustRemainCrouched()
        {
            if (isObstructed && antMove.isCrouched) // Se Ant ? obstructed mentre ? abbassatto continuer? ad esserlo finch? non sar? pi? obstructed 
            {
                antMove.isObstructed = true;
            }
            else
            {
                antMove.isObstructed = false;
            }
        }

        void ModifySpeedWhenJump()
        {
            if (!isGrounded)
            {
                antMove.speedModifier = speedInAir;
            }
            else
            {
                antMove.speedModifier = 1.0f;
            }
        }

        void ResetFallTime()
        {
            if (isGrounded)
            {
                antJump.elapsedFall = 0.0f;
            }
        }       

        void ResetVerticalSpeedWhenPushing()
        {
            if (isGrounded && isPushing)
            {                
                antJump.velocityY = 0.0f;
            }
            else
            {
                antJump.velocityY = antRigidBody.velocity.y;
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

        public void DisableDrop()
        {
            stopDrop = true;
        }

        public void EnableDrop()
        {
            stopDrop = false;
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
