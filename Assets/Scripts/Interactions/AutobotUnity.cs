using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class AutobotUnity : MonoBehaviour
    {
        private KeyCode connectButton;
        public bool connectable;
        private bool connectionPrep;         
        public bool readyForConnection;
        [SerializeField] private float connectDistance = 5.0f;
        [SerializeField] private float approachSpeed;

        public StateController stateController;
        public StateControllerT stateControllerT;
        private Animator animator;

        private Rigidbody2D targetRig;
        private BoxCollider2D boxCol;
        private CapsuleCollider2D capsuleCol;
        private RaycastHit2D connectCheck;
        private int layerMask;

        public GameObject ant;

        private Vector3 initialScale;

        // Start is called before the first frame update
        void Start()
        {
            connectButton = new InputSettings().SwitchCharacterButton;

            boxCol = gameObject.GetComponent<BoxCollider2D>();
            capsuleCol = gameObject.GetComponent<CapsuleCollider2D>();

            animator = gameObject.GetComponent<Animator>();

            initialScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.layer == 8)
            {
                layerMask = LayerMask.GetMask("Gameplay-Middle");
            }
            else if (gameObject.layer == 9)
            {
                layerMask = LayerMask.GetMask("Gameplay-Back");
            }

            connectDistance = Mathf.Sqrt(Mathf.Pow(boxCol.bounds.size.y + (capsuleCol.bounds.size.y / 2), 2.0f) + Mathf.Pow(boxCol.bounds.size.x, 2.0f));

            Vector2 editedTransform = new Vector2(boxCol.bounds.center.x - (boxCol.bounds.size.x / 2), boxCol.bounds.center.y + (boxCol.bounds.size.y / 2));
            connectCheck = Physics2D.Raycast(editedTransform, Vector2.down + Vector2.right, connectDistance, layerMask);

            if (connectCheck.collider != null)
            {
                targetRig = connectCheck.collider.gameObject.GetComponent<Rigidbody2D>();
                connectable = true;
            }
            else
            {
                connectable = false;
            }

            if (connectable && Input.GetKeyUp(connectButton))
            {
                if (gameObject.layer == 9)
                {
                    stateController.hasControl = false;
                }
                else if (gameObject.layer == 8)
                {
                    stateControllerT.hasControl = false;
                }                
                connectionPrep = true;
            }

            if (connectionPrep)
            {
                if (transform.position.x > targetRig.transform.position.x)
                {
                    transform.localScale = new Vector3(initialScale.x * 1, initialScale.y);
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed *-1), transform.position.y);                   

                    if (transform.position.x <= targetRig.transform.position.x)
                    {                       

                        if (ant.transform.localScale.x < 0)
                        {
                            ant.transform.localScale = new Vector3(ant.transform.localScale.x * -1, ant.transform.localScale.y);
                        }

                        connectionPrep = false;
                        readyForConnection = true;
                    }
                }
                else if (transform.position.x < targetRig.transform.position.x)
                {
                    transform.localScale = new Vector3(initialScale.x * -1, initialScale.y);
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed * 1), transform.position.y);                    

                    if (transform.position.x >= targetRig.transform.position.x)
                    {                       

                        if (ant.transform.localScale.x < 0)
                        {
                            ant.transform.localScale = new Vector3(ant.transform.localScale.x * -1, ant.transform.localScale.y);
                        }                      

                        connectionPrep = false;
                        readyForConnection = true;
                    }
                }

                
                if (connectionPrep)
                {
                    CallAnimator(Time.deltaTime * approachSpeed);
                }
                
                else
                {
                    CallAnimator(0);
                }
                
            }

        }

        private void OnDrawGizmosSelected()
        {
            boxCol = gameObject.GetComponent<BoxCollider2D>();
            capsuleCol = gameObject.GetComponent<CapsuleCollider2D>();

            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(boxCol.bounds.center.x - (boxCol.bounds.size.x / 2), boxCol.bounds.center.y + (boxCol.bounds.size.y / 2)), (Vector2.down + Vector2.right) * 5);
        }

        private void CallAnimator(float speed)
        {
            if (animator != null) //is null falsey?
            {
                animator.SetFloat("velocityX", Mathf.Abs(speed));                
            }
        }
    }
}