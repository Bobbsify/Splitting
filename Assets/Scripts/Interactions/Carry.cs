using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Carry : MonoBehaviour
    {
        public bool canCarry;

        public bool carryKey;
        public bool preparingCarry;
        public bool isCarrying;

        public GameObject carryedObj;
        public GameObject lastObj;

        private Collider2D carryedCol;
        private Rigidbody2D carryedRig;

        [SerializeField] private float horizontalForce = 750.0f;
        [SerializeField] private float verticalForce = 250.0f;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();       
        }

        // Update is called once per frame
        void Update()
        {
            carryKey = Input.GetKey(KeyCode.P);           

            if (canCarry)
            {
                if (carryedObj != null && carryKey && !isCarrying)
                {                                        
                    preparingCarry = true;
                    lastObj = null;
                }
            }
            else
            {
                if (carryKey && isCarrying)
                {                            
                    carryedRig.isKinematic = false;

                    lastObj.transform.SetParent(null);

                    if (transform.localScale.x > 0)
                    {
                        carryedRig.AddForce(new Vector2(-horizontalForce, verticalForce));
                    }
                    else
                    {
                        carryedRig.AddForce(new Vector2(horizontalForce, verticalForce));
                    }
                    isCarrying = false;
                }                
            }

            if (preparingCarry && AnimatorIsPlaying("AntIdle 0"))
            {
                isCarrying = true;
            }

            if (isCarrying && preparingCarry)
            {
                lastObj = carryedObj;
                preparingCarry = false;

                carryedRig = carryedObj.GetComponent<Rigidbody2D>();
                carryedCol = carryedObj.GetComponent<Collider2D>();

                if (carryedRig != null)
                {
                    carryedRig.isKinematic = true;
                }
                carryedObj.transform.SetParent(transform);
                carryedObj.transform.position = new Vector2(transform.position.x, transform.position.y + 10);               
            }         

            CallAnimator(isCarrying, preparingCarry);
        }

        private void CallAnimator(bool isCarrying, bool preparingCarry)
        {
            if (animator != null)
            {
                animator.SetBool("isCarrying", isCarrying);
                animator.SetBool("preparingCarry", preparingCarry);
            }
        }

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

    }
    
}
