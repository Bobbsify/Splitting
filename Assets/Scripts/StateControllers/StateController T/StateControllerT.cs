using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class StateControllerT : MonoBehaviour
    {
        public bool hasControl;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;

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

            // Get Tyr AutobotUnity script        
            tyrAutobotUnity = gameObject.GetComponent<AutobotUnity>();

            // Get SwitchCharacter script
            switchCharacter = gameObject.GetComponent<SwitchCharacter>();

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

            // Get animator
            animator = gameObject.GetComponent<Animator>();

            trajectPred = GameObject.Find("TrajectoryPrediction");
            getThrow = trajectPred.GetComponentInChildren<Throw>();
        }

        // Update is called once per frame
        void LateUpdate()
        {

            if (tyrJump.isLanded && !AnimatorIsPlaying("Tyr falling2"))
            {
                tyrJump.isLanded = false;
            }

            if (isGrounded)
            {
                if (tyrJump.wasJumping && !animator.IsInTransition(0))
                {
                    tyrJump.isLanded = true;
                    tyrJump.wasJumping = false;

                    if (tyrJump.bigFall)
                    {
                        ScreenShake(shake, lenght);
                    }                    
                }
            }

            if (gameObject.tag != "Player")
            {
                hasControl = false;
                
            }
            else if (!tyrAutobotUnity.connectionPrep && gameObject.tag == "Player")
            {
                hasControl = true;
            }

            if (hasControl)
            {
                tyrJump.enabled = true;
                tyrMove.enabled = true;
                tyrHacking.enabled = true;

                tyrFlashlight.canUseFlashlight = true;

                tyrJump.canJump = false;

                if (isWalled)
                {
                    tyrMove.canMove = false;
                }
                else
                {
                    tyrMove.canMove = true;
                }

                if (getThrow.rbToThrow || AnimatorIsPlaying("Tyr throw1") || AnimatorIsPlaying("Tyr throw4") || AnimatorIsPlaying("Tyr hacking") || AnimatorIsPlaying("Tyr hacking2") || !isGrounded)
                {
                    tyrMove.enabled = false;
                    tyrPickup.enabled = false;

                    tyrHacking.canHack = false;

                    antAutobotUnity.canConnect = false;
                    tyrAutobotUnity.canConnect = false;
                }
                else
                {
                    tyrMove.enabled = true;
                    tyrPickup.enabled = true;

                    tyrHacking.canHack = true;

                    antAutobotUnity.canConnect = true;
                    tyrAutobotUnity.canConnect = true;
                }

            }
            else
            {                
                tyrPickup.enabled = false;
                tyrMove.enabled = false;
                tyrHacking.enabled = false;

                tyrFlashlight.canUseFlashlight = false;

                if (isGrounded && AnimatorIsPlaying("Tyr idle"))
                {
                    tyrJump.enabled = false;
                }
                else
                {
                    tyrJump.enabled = true;
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
