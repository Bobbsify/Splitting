using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Carry : MonoBehaviour
    {
        public bool canCarry;

        public bool dropKey;

        public bool isFixed;
        public bool isFixing;
        
        public bool isCarrying;
        public bool wasCarrying;

        public GameObject carryedObj;
        public GameObject lastObj;
       
        public Rigidbody2D carryedRig;

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
            dropKey = Input.GetKeyDown(KeyCode.P);                    
                        
            if (canCarry)
            {
                if (carryedObj != null)
                {                                        
                    isFixing = true;
                    lastObj = carryedObj;
                }
            }
            else
            {
                if (dropKey && isCarrying)
                {                  

                    wasCarrying = true;
                    isFixed = false;
                    
                    carryedRig.simulated = true;                    
                    
                    lastObj.transform.SetParent(null);                    
                    
                    if (transform.localScale.x > 0)
                    {
                        carryedRig.AddForce(new Vector2(-horizontalForce, verticalForce));
                    }
                    else
                    {
                        carryedRig.AddForce(new Vector2(horizontalForce, verticalForce));
                    }                                     
                }
            }


            if (isFixing && !isFixed)
            {

                if (carryedRig != null)
                {
                    carryedRig.simulated = false;
                }

                carryedObj.transform.SetParent(transform);
                carryedObj.transform.position = new Vector2(carryedObj.transform.position.x, carryedObj.transform.position.y);

                isFixed = true;
            }
            
            if (AnimatorIsPlaying("AntCarryingIdle") || AnimatorIsPlaying("AntCarrying"))
            {
                isFixing = false;
                isCarrying = true;
            }
            else
            {
                isCarrying = false;
            }

            if (isCarrying)
            {
                lastObj.transform.position = new Vector2(transform.position.x, lastObj.transform.position.y);
            }

            if (wasCarrying && canCarry)
            {
                lastObj = null;
                carryedRig = null;
                wasCarrying = false;
            }
            

            CallAnimator(isCarrying, isFixing, wasCarrying);            
        }

        private void CallAnimator(bool isCarrying, bool preparingCarry, bool wasCarrying)
        {
            if (animator != null)
            {
                animator.SetBool("isCarrying", isCarrying);
                animator.SetBool("preparingCarry", preparingCarry);
                animator.SetBool("wasCarrying", wasCarrying);
            }
        }

        bool AnimatorIsPlaying(string stateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }       
    }
    
}
