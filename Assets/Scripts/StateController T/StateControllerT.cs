using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class StateControllerT : MonoBehaviour
    {
        public bool hasControl;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;

        public Move move;
        public Jump jump;

        private GameObject trajectPred;
        private Throw getThrow;

        new public CameraController camera;
        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            hasControl = true;

            animator = gameObject.GetComponent<Animator>();

            trajectPred = GameObject.Find("TrajectoryPrediction");
            getThrow = trajectPred.GetComponentInChildren<Throw>();
        }

        // Update is called once per frame
        void LateUpdate()
        {

            if (jump.isLanded && !AnimatorIsPlaying("Tyr falling2"))
            {
                jump.isLanded = false;
            }

            if (hasControl)
            {
                jump.canJump = false;

                if (isWalled)
                {
                    move.canMove = false;
                }
                else
                {
                    move.canMove = true;
                }

                if (getThrow.rbToThrow)
                {
                    move.canMove = false;
                }
                else
                {
                    move.canMove = true;
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
