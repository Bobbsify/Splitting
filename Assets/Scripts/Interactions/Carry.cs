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

        private BoxCollider2D antBoxCol;     
        private GameObject bone;
        private GameObject headBone;        

        public GameObject carryedObj;
        public GameObject lastObj;

        private BoxCollider2D carryedObjCol;
        private Rigidbody2D carryedObjRig;

        [SerializeField] private float horizontalForce = 750.0f;
        [SerializeField] private float verticalForce = 100.0f;

        private Animator animator;
        
        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();

            antBoxCol = gameObject.GetComponent<BoxCollider2D>();

            dropButton = new InputSettings().ReleaseItemButton;

            bone = gameObject.transform.GetChild(0).gameObject;

            if (bone != null)
            {
                headBone = bone.transform.GetChild(3).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {                         
                       
            if (canCarry)
            {
                if (carryedObj != null )
                {
                    carryedObjCol = carryedObj.GetComponent<BoxCollider2D>();
                    carryedObjRig = carryedObj.GetComponent<Rigidbody2D>();

                    if (ControlCarryableObjPos(carryedObjCol, antBoxCol))
                    {
                        isFixing = true;
                        lastObj = carryedObj;
                    }
                    else
                    {
                        carryedObjCol = null;
                        carryedObjRig = null;
                    }
                }
            }
            else
            {
                if (Input.GetKeyUp(dropButton) && isCarrying)
                {                  

                    wasCarrying = true;
                    isFixed = false;
                    
                    carryedObjRig.simulated = true;                    
                    
                    lastObj.transform.SetParent(null);                                                              
                }
            }


            if (isFixing && !isFixed)
            {

                if (carryedObjRig != null)
                {
                    carryedObjRig.simulated = false;
                }

                carryedObj.transform.SetParent(headBone.transform);
                carryedObj.transform.position = new Vector2(headBone.transform.position.x, (antBoxCol.bounds.center.y + antBoxCol.bounds.extents.y + carryedObjCol.bounds.extents.y));                

                isFixed = true;
            }
            
            if (AnimatorIsPlaying("AntCarryingIdle") || AnimatorIsPlaying("AntCarrying"))
            {
                isFixing = false;                
            }

            if (headBone.transform.childCount > 0)
            {
                isCarrying = true;
            }
            else
            {
                isCarrying = false;               

                if (AnimatorIsPlaying("AntIdle") || AnimatorIsPlaying("AntWalking"))
                {
                    wasCarrying = false;

                    lastObj = null;
                    carryedObjCol = null;
                    carryedObjRig = null;
                }
            }

            if (AnimatorIsPlaying("AntCarryingEnd"))
            {
                if (transform.localScale.x > 0)
                {
                    carryedObjRig.AddForce(new Vector2(-horizontalForce, verticalForce));
                }
                else
                {
                    carryedObjRig.AddForce(new Vector2(horizontalForce, verticalForce));
                }
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
