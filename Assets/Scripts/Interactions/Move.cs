using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Move : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] private float speed;
        private float horizontalInput;
        private float verticalInput;
        private Vector3 initialScale;

        //For state controller
        public bool canCrouch;
        [HideInInspector] public bool isCrouched;
        [HideInInspector] public bool canMove;
        [HideInInspector] public bool isObstructed;


        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            initialScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isCrouched = (verticalInput < 0 || isObstructed) && canCrouch;

            if (canMove)
            {
                transform.position = new Vector2(transform.position.x + (Time.deltaTime * speed / (isCrouched ? 2 : 1) * horizontalInput), transform.position.y); //halves speed if is crouchings                
            }
            //Invertscale
            if (horizontalInput != 0)
            {
                if (horizontalInput < 0)
                {
                    horizontalInput = 1;
                }
                else
                {
                    horizontalInput = -1;
                }
                transform.localScale = new Vector3(initialScale.x * horizontalInput, initialScale.y);
            }
            if (canCrouch || canMove)
            { 
                CallAnimator(Time.deltaTime * speed / (isCrouched ? 2 : 1) * horizontalInput, isCrouched);
            }
        }

        //Updates animator velocity
        private void CallAnimator(float speed, bool crouched)
        {
            if (animator != null) //is null falsey?
            {
                animator.SetFloat("velocityX", Mathf.Abs(speed));
                animator.SetBool("isCrouched", crouched);
            }
        }       
    }
}
