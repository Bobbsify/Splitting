using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2 Azioni:
 *
 * Crea un collider nella direzione in cui è orientata l'entità lungo (X) dove x è settabile dal programmatore
 * - Se questo collider colpisce il collider di un oggetto con il tag "Carryable" raccoglilo ed assegnalo al Rigidbody del throw
 * - Sposta l'oggetto in cima all'entità corrente
 *
 */

namespace Splitting { 
    public class Pickup : MonoBehaviour
    {

        [Header("Throw Settings")]
        public Throw throwScript;

        private KeyCode pickupButton;
        public float grabDistance = 1.0f;

        private Collider2D col;
        private RaycastHit2D grabCheck;

        public GameObject trajectoryPrediction;

        private Animator animator;
        
        void Awake()
        {
            col = gameObject.GetComponent<CapsuleCollider2D>();
            pickupButton = new InputSettings().PickupButton;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(pickupButton))
            {
                //Check if there is a grabbable object
                BoxCollider2D wallCheckCollider = transform.Find("Wall Check").GetComponent<BoxCollider2D>();
                Vector2 editedTransform = new Vector2(wallCheckCollider.transform.position.x + (Mathf.Abs(wallCheckCollider.offset.x) + wallCheckCollider.size.x) * -transform.localScale.x, transform.position.y);
                grabCheck = Physics2D.Raycast(editedTransform, Vector2.right * -transform.localScale.x);
                if (grabCheck.collider != null && grabCheck.collider.tag == "Carryable")
                {
                    animator.SetBool("pickUp", true);
                }
            }
        }

        public void PickUp()
        {
            Rigidbody2D objRigidbody = grabCheck.collider.gameObject.GetComponent<Rigidbody2D>();
            grabCheck.collider.gameObject.transform.parent = transform;
            grabCheck.collider.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + col.bounds.size.y * 2);
            objRigidbody.isKinematic = true;
            throwScript.rbToThrow = objRigidbody;
        }

        public void Throw()
        {
            trajectoryPrediction.GetComponent<Throw>().ThrowEntity();
        }

        private void OnDrawGizmosSelected()
        {
            col = gameObject.GetComponent<CapsuleCollider2D>();
            BoxCollider2D wallCheckCollider = transform.Find("Wall Check").GetComponent<BoxCollider2D>();
            Gizmos.color = Color.red;
            //Gizmos.DrawRay(new Vector2((wallCheckCollider.transform.position.x + wallCheckCollider.offset.x - wallCheckCollider.size.x), transform.position.y), Vector2.right * -transform.localScale.x);
            Gizmos.DrawRay(new Vector2(wallCheckCollider.transform.position.x + (Mathf.Abs(wallCheckCollider.offset.x) + wallCheckCollider.size.x ) * -transform.localScale.x, transform.position.y), Vector2.right * -transform.localScale.x);
        }
    }
}
