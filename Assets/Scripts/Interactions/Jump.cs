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
        public bool jumpKeyDown;

        [SerializeField] private float timerJump = 2.0f;
        [SerializeField] private float elapsedKeyDown;       

        private float velocityY;

        new private Rigidbody2D rigidbody2D;

        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();

            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

            jumpKeyDown = Input.GetKey(KeyCode.Space);


            if (canJump && !isLanded && !wasJumping)
            {
                if (jumpKeyDown)
                {
                    elapsedKeyDown += Time.deltaTime;
                }

                if (elapsedKeyDown >= timerJump)
                {
                    superJump = true;
                }
                else
                {
                    superJump = false;
                }

                if (Input.GetKeyUp(KeyCode.Space) && !superJump)
                {
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce));                  

                    elapsedKeyDown = 0.0f;
                }
                else if (Input.GetKeyUp(KeyCode.Space) && superJump)
                {                    
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce * (jumpMultiplier / jumpDivider)));                    

                    elapsedKeyDown = 0.0f;
                }
            }

            if (!canJump)
            {
                elapsedKeyDown = 0.0f;
            }

            velocityY = rigidbody2D.velocity.y;

            if (Mathf.Abs(velocityY) > 4 && Mathf.Abs(velocityY) < 100)
            {          
                wasJumping = true;
            }
            

            CallAnimator(jumpKeyDown, velocityY, isLanded);
            
        }

        private void CallAnimator(bool isPreparing, float verticalSpeed, bool isLanded)
        {
            if (animator != null)
            {
                animator.SetBool("jumpPrep", isPreparing);
                animator.SetFloat("velocityY", verticalSpeed);
                animator.SetBool("land", isLanded);
            }
        }

       
    }
}
