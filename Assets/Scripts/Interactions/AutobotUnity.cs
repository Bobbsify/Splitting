using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class AutobotUnity : MonoBehaviour
    {
        private KeyCode connectButton;
        public bool canConnect;
        public bool connectable;
        public bool connectionPrep;         
        public bool readyForConnection;
        [SerializeField] private float connectDistance = 5.0f;
        [SerializeField] private float approachSpeed;

        // ant components
        private StateController antStateController;
        private Carry antCarry;

        // tyr components
        private StateControllerT tyrStateController;
        private GameObject tyrTrajectPred;
        private Throw tyrGetThrow;
        
        private SwitchCharacter switchCharacter;
        private Animator animator;

        private Rigidbody2D targetRig;
        private BoxCollider2D boxCol;
        private CapsuleCollider2D capsuleCol;
        private RaycastHit2D connectCheck;
        private int layerMask;

        private GameObject ant;

        private Vector3 initialScale;

        // Start is called before the first frame update
        void Awake()
        {
            connectButton = new InputSettings().SwitchCharacterButton;

            boxCol = gameObject.GetComponent<BoxCollider2D>();
            capsuleCol = gameObject.GetComponent<CapsuleCollider2D>();

            animator = gameObject.GetComponent<Animator>();

            switchCharacter = gameObject.GetComponent<SwitchCharacter>();

            try
            {
                if (switchCharacter.targetEntity != null)
                {
                    GetStateControllers();
                }                
            }
            catch
            {
                throw new Exception("Could not get state controllers");
            }            

            initialScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            ControlIfCanConnet();

            if (gameObject.layer == 8)
            {
                layerMask = LayerMask.GetMask("Gameplay-Middle");
            }
            else if (gameObject.layer == 9)
            {
                layerMask = LayerMask.GetMask("Gameplay-Back");
            }

            // La gizmo che si vede una volta avviato il gioco non è la reale connect distance necessaria per l'unione tra ant e tyr
            connectDistance = Mathf.Sqrt(Mathf.Pow(boxCol.bounds.extents.y, 2.0f) + Mathf.Pow(boxCol.bounds.extents.x, 2.0f)) * 2;

            Vector2 editedTransform = new Vector2(boxCol.bounds.center.x - ((capsuleCol.bounds.extents.x) * -gameObject.transform.localScale.x), boxCol.bounds.center.y + (boxCol.bounds.extents.y));

            if (canConnect)
            {
                connectCheck = Physics2D.Raycast(editedTransform, Vector2.down + (Vector2.right * -gameObject.transform.localScale.x), connectDistance, layerMask);
            }

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
                    antStateController.hasControl = false;
                }
                else if (gameObject.layer == 8)
                {
                    tyrStateController.hasControl = false;
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
            Gizmos.DrawRay(new Vector2(boxCol.bounds.center.x - ((capsuleCol.bounds.extents.x ) * - gameObject.transform.localScale.x), boxCol.bounds.center.y + (boxCol.bounds.extents.y)), (Vector2.down + (Vector2.right * -gameObject.transform.localScale.x)) * connectDistance);
        }

        private void CallAnimator(float speed)
        {
            if (animator != null) //is null falsey?
            {
                animator.SetFloat("velocityX", Mathf.Abs(speed));                
            }
        }

        public void GetStateControllers()
        {
            if (switchCharacter.targetEntity.name.ToUpper().Contains("ANT"))
            {
                antStateController = switchCharacter.targetEntity.GetComponent<StateController>();
                antCarry = switchCharacter.targetEntity.GetComponent<Carry>();

                tyrStateController = gameObject.GetComponent<StateControllerT>();
                tyrTrajectPred = transform.Find("TrajectoryPrediction").gameObject;
                tyrGetThrow = tyrTrajectPred.GetComponentInChildren<Throw>();
                                
                ant = switchCharacter.targetEntity;
            }
            else
            {
                antStateController = gameObject.GetComponent<StateController>();
                antCarry = gameObject.GetComponent<Carry>();

                tyrStateController = switchCharacter.targetEntity.GetComponent<StateControllerT>();
                tyrTrajectPred = GameObject.Find("TrajectoryPrediction");
                tyrGetThrow = tyrTrajectPred.GetComponentInChildren<Throw>();

                ant = gameObject;
            }
        }

        void ControlIfCanConnet()
        {
            if (!antStateController.isNotScared || antCarry.isCarrying || tyrGetThrow.rbToThrow)
            {
                canConnect = false;
            }
            else
            {
                canConnect = true;
            }
        }
    }
}