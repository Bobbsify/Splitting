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
        [SerializeField] private float connectDistance = 5.0f;

        public StateController stateController;

        private Rigidbody2D tyrRig;
        private BoxCollider2D boxCol;
        private CapsuleCollider2D capsuleCol;
        private RaycastHit2D connectCheck;
        private int layerMask;

        // Start is called before the first frame update
        void Start()
        {
            connectButton = new InputSettings().SwitchCharacterButton;

            boxCol = gameObject.GetComponent<BoxCollider2D>();
            capsuleCol = gameObject.GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            layerMask = LayerMask.GetMask("Gameplay-Back");

            Vector2 editedTransform = new Vector2(transform.position.x - (boxCol.bounds.size.x / 2), transform.position.y + (boxCol.bounds.size.y / 2) + (capsuleCol.bounds.size.y / 8));
            connectCheck = Physics2D.Raycast(editedTransform, Vector2.down + Vector2.right, connectDistance, layerMask);

            if (connectCheck.collider != null)
            {
                tyrRig = connectCheck.collider.gameObject.GetComponent<Rigidbody2D>();
                connectable = true;
            }
            else
            {
                connectable = false;
            }

            if (connectable && Input.GetKeyUp(connectButton))
            {
                stateController.hasControl = false;
                connectionPrep = true;
            }

            if (connectionPrep)
            {
                Debug.Log("pos1 : " + transform.position.x);
                Debug.Log("pos2 : " + tyrRig.transform.position.x);

                if (transform.position.x > tyrRig.transform.position.x)
                {
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * -1), transform.position.y);
                    Debug.Log("Boh");
                }
                else if (transform.position.x <= tyrRig.transform.position.x)
                {
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * 1), transform.position.y);
                    Debug.Log("Yes");
                }
            }


        }

        private void OnDrawGizmosSelected()
        {
            boxCol = gameObject.GetComponent<BoxCollider2D>();
            capsuleCol = gameObject.GetComponent<CapsuleCollider2D>();

            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(transform.position.x - (boxCol.bounds.size.x / 2), transform.position.y + (boxCol.bounds.size.y / 2) + (capsuleCol.bounds.size.y / 8)), (Vector2.down + Vector2.right) * 5);
        }
    }
}