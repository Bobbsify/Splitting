using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Carry : MonoBehaviour
    {
        public bool canCarry;
                
        public KeyCode dropButton;

        public bool isFixed;
        public bool isFixing;
        
        public bool isCarrying;
        public bool wasCarrying;

        private BoxCollider2D boxCol;
        private BoxCollider2D carryedObjCol;

        public GameObject carryedObj;
        public GameObject lastObj;
       
        public Rigidbody2D carryedRig;

        [SerializeField] private float horizontalForce = 750.0f;
        [SerializeField] private float verticalForce = 100.0f;

        private Animator animator;
        
        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();

            boxCol = gameObject.GetComponent<BoxCollider2D>();

            dropButton = new InputSettings().ReleaseItemButton;
        }

        // Update is called once per frame
        void Update()
        {                         
                       
            if (canCarry)
            {
                if (carryedObj != null )
                {
                    carryedObjCol = carryedObj.GetComponent<BoxCollider2D>();

                    if (ControlCarryableObjPos(carryedObjCol, boxCol))
                    {
                        isFixing = true;
                        lastObj = carryedObj;
                    }                    
                }
            }
            else
            {
                if (Input.GetKeyUp(dropButton) && isCarrying)
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
                carryedObj.transform.position = new Vector2(carryedObj.transform.position.x, (boxCol.bounds.center.y + boxCol.bounds.extents.y + carryedObjCol.bounds.extents.y));

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
                lastObj.transform.position = new Vector2(transform.position.x, (boxCol.bounds.center.y + boxCol.bounds.extents.y + carryedObjCol.bounds.extents.y));
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
        
        bool ControlCarryableObjPos(BoxCollider2D carryedObjCol, BoxCollider2D antCol)
        {
            bool isCarryable;

            if (((carryedObjCol.bounds.center.y - carryedObjCol.bounds.extents.y) > (antCol.bounds.center.y + antCol.bounds.extents.y)) && (carryedObjCol.bounds.center.x < (antCol.bounds.center.x + antCol.bounds.extents.x)) && (carryedObjCol.bounds.center.x > (antCol.bounds.center.x - antCol.bounds.extents.x)))
            {
                isCarryable = true;
            }
            else
            {
                isCarryable = false;
            }
            return isCarryable;
        }
    }
    
}
