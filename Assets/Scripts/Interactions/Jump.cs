using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Jump : MonoBehaviour
    {

        public bool canJump;

        public bool isJumping;
        public bool isFalling;

        [SerializeField] private float jumpForce = 1000.0f;
        public bool jumpKeyDown;

        [SerializeField] private float timerJump = 2.0f;
        [SerializeField] private float elapsed;

        private float velocityY;

        new private Rigidbody2D rigidbody2D;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

            jumpKeyDown = Input.GetKey(KeyCode.Space);


            if (canJump)
            {
                if (jumpKeyDown)
                {
                    elapsed += Time.deltaTime;
                }

                if (Input.GetKeyUp(KeyCode.Space) && elapsed < timerJump)
                {
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce));

                    isJumping = true;

                    elapsed = 0.0f;

                }
                else if (Input.GetKeyUp(KeyCode.Space) && elapsed >= timerJump)
                {
                    rigidbody2D.AddForce(new Vector2(0f, jumpForce * 2));

                    isJumping = true;

                    elapsed = 0.0f;
                }

            }

            velocityY = rigidbody2D.velocity.y;

            if (velocityY < 0)
            {
                isJumping = false;
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }




        }
    }
}
