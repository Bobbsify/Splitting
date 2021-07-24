using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Jump jump;
        public KeyCode jumpButton;
        public Move move;
        public Pickup pickup;

        private GameObject trajectPred;
        private Throw getThrow;

        public GameObject ant;
        public AutobotUnity antUnity;
        public GameObject tyr;
        public AutobotUnity tyrUnity;

        private Animator animator;

        new public CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            hasControl = true;

            animator = gameObject.GetComponent<Animator>();

            trajectPred = GameObject.Find("TrajectoryPredictionTA");
            getThrow = trajectPred.GetComponentInChildren<Throw>();

            ant = gameObject.transform.Find("Ant").gameObject;
            tyr = gameObject.transform.Find("Tyr").gameObject;

            antUnity = ant.GetComponent<AutobotUnity>();
            tyrUnity = tyr.GetComponent<AutobotUnity>();

            jumpButton = new InputSettings().JumpButton;
        }

        // Update is called once per frame
        void Update()
        {
            if (jump.isLanded && !AnimatorIsPlaying("Tyrant Jump 4"))
            {
                jump.isLanded = false;
            }

            if (hasControl)
            {
                jump.enabled = true;
                move.enabled = true;

                move.canCrouch = false;

                if (isWalled || Input.GetKey(jumpButton))
                {
                    move.canMove = false;
                }
                else
                {
                    move.canMove = true;
                }

                if (isGrounded)
                {
                    pickup.enabled = true;                    

                    if (jump.wasJumping && !animator.IsInTransition(0))
                    {
                        jump.isLanded = true;
                        jump.wasJumping = false;

                        ScreenShake(shake, lenght);
                    }

                    if (AnimatorIsPlaying("Tyrant Jump 4") || getThrow.throwing || AnimatorIsPlaying("TyrantUnity") || AnimatorIsPlaying("TyrantUnlink"))
                    {
                        move.enabled = false;
                        jump.canJump = false;                        
                    }
                    else if (getThrow.rbToThrow)
                    {
                        move.enabled = true;
                        jump.canJump = false;
                    }
                    else
                    {
                        move.enabled = true;
                        jump.jumpDivider = jump.jumpMultiplier;
                        jump.canJump = true;
                    }

                }
                else
                {
                    pickup.enabled = false;
                }

            }
            else
            {
                jump.enabled = false;
                pickup.enabled = false;
                move.enabled = false;
            }

            if (gameObject.tag != "Player")
            {
                hasControl = false;
            }
            else if (gameObject.tag == "Player")
            {
                hasControl = true;

                antUnity.readyForConnection = false;
                tyrUnity.readyForConnection = false;
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
