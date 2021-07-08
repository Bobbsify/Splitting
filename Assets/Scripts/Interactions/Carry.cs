using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Carry : MonoBehaviour
    {
        public bool canCarry;

        public bool dropKey;

        public bool isFixing;
        [SerializeField] private float fixingRange = 20.0f;
        public bool isCarrying;

        [SerializeField] private float elapsedFix = 0.0f;

        //[SerializeField] private float takeDistance = 5.0f;

        public GameObject carryedObj;
        public GameObject lastObj;

        private Collider2D col;

        //private RaycastHit2D takeCheck;
        private Collider2D carryedCol;
        private Rigidbody2D carryedRig;

        [SerializeField] private float horizontalForce = 750.0f;
        [SerializeField] private float verticalForce = 250.0f;

        private Animator animator;
        
        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();

            col = gameObject.GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            dropKey = Input.GetKey(KeyCode.P);
            
            /*
            Vector2 editedTransform = new Vector2(transform.position.x, transform.position.y + col.bounds.size.y / 2);

            takeCheck = Physics2D.Raycast(editedTransform, Vector2.up, takeDistance);

            if (takeCheck.collider != null && takeCheck.collider.tag == "Carryable")
            {
                Debug.Log("boh");
                carryedRig = takeCheck.collider.gameObject.GetComponent<Rigidbody2D>();
                takeCheck.collider.gameObject.transform.parent = transform;
            }
            */
            

            
            if (canCarry)
            {
                if (carryedObj != null)
                {                                        
                    isFixing = true;
                    lastObj = null;
                }
            }
            else
            {
                if (dropKey && isCarrying)
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

            if (isFixing)
            {             
                lastObj = carryedObj;
                carryedCol =carryedObj.GetComponent<Collider2D>();
                carryedRig = carryedObj.GetComponent<Rigidbody2D>();

                if (carryedRig != null)
                {
                    carryedRig.isKinematic = true;
                }

                carryedObj.transform.SetParent(transform);
                carryedObj.transform.position = new Vector2(carryedObj.transform.position.x, transform.position.y + col.bounds.size.y);

                if (carryedObj.transform.position.x != transform.position.x)
                {
                    elapsedFix += Time.deltaTime;
                    
                }
            }

            /*
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
                carryedObj.transform.position = new Vector2(transform.position.x, transform.position.y + 4);               
            }         

            CallAnimator(isCarrying, preparingCarry);
            */
            
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

        /*
        private void OnDrawGizmosSelected()
        {
            col = gameObject.GetComponent<Collider2D>();
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y + col.bounds.size.y), Vector2.right);
        }
        */
    }
    
}
