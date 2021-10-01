using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Jump : MonoBehaviour
    {
        private Animator animator;

        public bool canJump;
        public bool superJump;
        public bool isLanded;

        public bool wasJumping;

        [SerializeField] private float jumpForce = 1000.0f;
        public float jumpMultiplier = 2.0f;
        public float jumpDivider = 1.0f;

        private KeyCode jumpButton;

        [SerializeField] private float timerChargeJump = 1.0f;
        public bool chargeJump;

        [SerializeField] private float timerJump = 2.0f;
        [SerializeField] private float elapsedKeyDown;

        [SerializeField] private float timerFall = 2.0f;
        public float elapsedFall;

        [SerializeField] private float timerStartFall = 1.0f;
        private bool startFall;

        public bool bigFall;

        public float velocityY;

        new private Rigidbody2D rigidbody2D;

        // Start is called before the first frame update
        void Awake()
        {
            animator = gameObject.GetComponent<Animator>();

            rigidbody2D = GetComponent<Rigidbody2D>();

            jumpButton = new InputSettings().JumpButton;
        }

        // Update is called once per frame
        void Update()
        {
            if (bigFall)
            {
                Debug.Log("bigFall is " + bigFall);
            }
            

            if (canJump && !wasJumping)
            {
                if (Input.GetKey(jumpButton))
                {
                    elapsedKeyDown += Time.deltaTime;
                }

                if (elapsedKeyDown >= timerChargeJump)
                {
                    chargeJump = true;
                }
                else
                {
                    chargeJump = false;
                }


                if (elapsedKeyDown >= timerJump)
                {
                    superJump = true;
                }
                else
                {
                    superJump = false;
                }

                if (Input.GetKeyUp(jumpButton) && !superJump)
                {
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce));                  

                    elapsedKeyDown = 0.0f;
                }
                else if (Input.GetKeyUp(jumpButton) && superJump)
                {                    
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce * (jumpMultiplier / jumpDivider)));                    

                    elapsedKeyDown = 0.0f;
                }
            }

            
            if (!canJump)
            {
                elapsedKeyDown = 0.0f;
            }
            else
            {
                elapsedFall = 0.0f;
            }
            
                        

            if (Mathf.Abs(velocityY) > 4 && Mathf.Abs(velocityY) < 100)
            {          
                wasJumping = true;
            }

            if (velocityY < 0)
            {
                elapsedFall += Time.deltaTime;
            }

            if (elapsedFall >= timerStartFall)
            {
                startFall = true;
                wasJumping = true;
            }
            else
            {
                startFall = false;
            }

            if (elapsedFall > timerFall)
            {
                bigFall = true;
            }
            else
            {
                bigFall = false;
            }            

            CallAnimator(Input.GetKey(jumpButton), velocityY, isLanded, startFall);
            
        }

        private void CallAnimator(bool isPreparing, float verticalSpeed, bool isLanded, bool startingFall)
        {
            if (animator != null)
            {
                animator.SetBool("jumpPrep", isPreparing);
                animator.SetFloat("velocityY", verticalSpeed);
                animator.SetBool("land", isLanded);
                animator.SetBool("startFall", startingFall);
            }
        }       

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }


    }
}
