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
        public bool isNotScared;
        public bool isIlluminated;

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
        }

        // Update is called once per frame
        void LateUpdate()
        {            
            ControlIfHasControl(); // Se il Player sta giocando nei panni di Ant gli vengono assegnati i controlli, mentre se si switcha a Tyr o muore gli vengono tolti 

            CheckIfHasFallen(); // Controlla se Ant è atterrato dopo un salto/caduta
            ResetLandBoolAfterHasFallen(); // Controlla se l'animazione di atterraggio di Ant è terminata e resetta la booleana per una prossima caduta

            AntIsNotAfraid(); // Controlla se Ant è spaventato o no          

            ModifySpeedWhenJump();

            //ControlWhenForceDrop();
            CallAnimator(isNotScared);

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
            else // Questo è ciò che avviene quando hasControl == false
            {                
                antMove.enabled = false; // Disabilito l'intera script Move di Ant
                antExtend.enabled = false; // Disabilito l'intera script Extend di Ant
                                
                antJump.canJump = false; // Ant non può saltare
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

        void AntIsNotAfraid()
        {
            if (isIlluminated || !isGrounded)
            {
                isNotScared = true;
            }
            else
            {
                isNotScared = false;
            }
        }

        /*
        void PrepareToBeScared() // da aggiustare
        {
            if (!isNotScared)
            {
                hasControl = false;                
            }
        }
        */
        

        void ControlIfHasControl()
        {
            if (gameObject.tag != "Player" || AnimatorIsPlaying("AntDeath") || !isNotScared)
            {
                hasControl = false;
            }
            else if (!antAutobotUnity.connectionPrep && gameObject.tag == "Player" && !AnimatorIsPlaying("AntDeath") && isNotScared) // Viene controllato che connectionPrep sia false perché altrimenti il giocatore potrebbe muovere Ant
            {                                                                                                         // mentre sta avvenendo l'unione in Tyrant
                hasControl = true;
            }
        }

        private void CallAnimator(bool isNotScared)
        {
            if (animator != null)
            {
                animator.SetBool("isNotScared", isNotScared);
            }
        }       

        void CheckIfHasFallen()
        {
            if (isGrounded)
            {
                if (antJump.wasJumping && !animator.IsInTransition(0)) // Controlla se Ant è a terra dopo un salto/caduta
                {
                    
                    antJump.isLanded = true; // Conferma l'atterraggio
                    antJump.wasJumping = false;

                    if (antJump.bigFall) // Se Ant è atterrato dopo una caduta lunga più di un tot verrà considerata bigFall ed avverrà uno ScreenShake
                    {
                        ScreenShake(shake, lenght);
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

            if (isWalled || (Input.GetKey(jumpButton) && !antMove.isCrouched) || antExtend.isExtended || AnimatorIsPlaying("AntLift3") || AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress"))
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

            if (isGrounded && !antExtend.isExtended && !antMove.isCrouched && (!AnimatorIsPlaying("AntCarryingAdjust") && !AnimatorIsPlaying("AntCarryingEnd") && !AnimatorIsPlaying("AntButtonPress")) && !isObstructed)
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

            if (Input.GetKey(jumpButton) || antCarry.isCarrying || !isGrounded || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntCarryingEnd") || AnimatorIsPlaying("AntButtonPress")) || antMove.isCrouched || antExtend.isExtended)
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
            if (Input.GetKey(jumpButton) || !antCarry.isCarrying || !isGrounded || (AnimatorIsPlaying("AntCarryingAdjust") || AnimatorIsPlaying("AntButtonPress")) || isObstructed || antMove.isCrouched || antExtend.isExtended)
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
            if (isObstructed && antMove.isCrouched) // Se Ant è obstructed mentre è abbassatto continuerà ad esserlo finché non sarà più obstructed 
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
    }
}
