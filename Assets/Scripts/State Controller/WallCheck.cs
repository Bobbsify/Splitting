using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{

    public class WallCheck : MonoBehaviour
    {
        public StateController stateController;

        public Carry carry;       

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CallAnimator(carry.isCarrying);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateController.isWalled = true;
            }

            if (collision.gameObject.tag == "Carryable" && !carry.carryKey)
            {
                carry.carryedObj = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateController.isWalled = false;
            }

            if (collision.gameObject.tag == "Carryable" && !carry.carryKey)
            {
                carry.carryedObj = null;
            }
        }

        private void CallAnimator(bool isCarrying)
        {
            if (animator != null)
            {
                animator.SetBool("carry", isCarrying);
            }
        }
    }
}
