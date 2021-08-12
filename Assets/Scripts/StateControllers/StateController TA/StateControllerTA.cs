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

        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        private Jump tyrantJump;
        private KeyCode jumpButton;
        private Move tyrantMove;
        private Pickup tyrantPickup;

        private GameObject trajectPred;
        private Throw getThrow;

        /*
        public GameObject ant;
        public AutobotUnity antUnity;
        public GameObject tyr;
        public AutobotUnity tyrUnity;
        */

        private Animator animator;

        new private CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            // Get Move script  
            tyrantMove = gameObject.GetComponent<Move>();

            // Get Jump script            
            tyrantJump = gameObject.GetComponent<Jump>();

            // Get Pickup script
            tyrantPickup = gameObject.GetComponent<Pickup>();

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

            /*
            ant = gameObject.transform.Find("Ant").gameObject;
            tyr = gameObject.transform.Find("Tyr").gameObject;
            */

            jumpButton = new InputSettings().JumpButton;
        }

        // Update is called once per frame
        void Update()
        {
            if (tyrantJump.isLanded && !AnimatorIsPlaying("Tyrant Jump 4"))
            {
                tyrantJump.isLanded = false;
            }

            if (hasControl)
            {

                tyrantJump.enabled = true;
                tyrantMove.enabled = true;

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

                    if (tyrantJump.wasJumping && !animator.IsInTransition(0))
                    {
                        tyrantJump.isLanded = true;
                        tyrantJump.wasJumping = false;

                        if (tyrantJump.bigFall)
                        {
                            ScreenShake(shake, lenght);
                        }                        
                    }

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
                tyrantJump.enabled = false;
                tyrantPickup.enabled = false;
                tyrantMove.enabled = false;
            }

            if (gameObject.tag != "Player")
            {
                hasControl = false;
            }
            else if (gameObject.tag == "Player")
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
