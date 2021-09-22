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
        private RaycastHit2D[] grabChecks;
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
                grabChecks = Physics2D.RaycastAll(editedTransform, Vector2.right * -transform.localScale.x, 1f);
                foreach (RaycastHit2D check in grabChecks) { 
                    if (check.collider != null && check.collider.tag == "Carryable")
                    {
                        grabCheck = check;
                        animator.SetTrigger("pickUp");
                        break;
                    }
                }
            }
        }

        public void PickUp()
        {
            string TyrFingerBone = "bone_1/bone_5/bone_6/bone_18/bone_7/bone_17/bone_8/bone_9";
            string TyrantFingerBone = "bone_1/bone_2/bone_3/bone_14/bone_15/bone_16/bone_17/bone_18/bone_19/bone_20";
            Rigidbody2D objRigidbody = grabCheck.collider.gameObject.GetComponent<Rigidbody2D>();
            grabCheck.collider.gameObject.transform.parent = transform.Find(name.ToUpper().Contains("ANT") ? TyrantFingerBone : TyrFingerBone);
            grabCheck.collider.gameObject.transform.localPosition = fetchCorrectPosition(grabCheck.collider.gameObject);
            objRigidbody.isKinematic = true;
            grabCheck.collider.enabled = false;
            throwScript.rbToThrow = objRigidbody;
        }

        public void Throw()
        {
            trajectoryPrediction.GetComponent<Throw>().ThrowEntity();
            grabCheck.collider.enabled = true;
        }

        private Vector2 fetchCorrectPosition(GameObject box)
        {
            Vector2 result;

            try
            {
                Vector2 boxColliderSize = box.GetComponent<BoxCollider2D>().bounds.extents; // x --> width | y --> height
                                                                                            //BoxCollider2D playerBoxCollider = GetComponent<BoxCollider2D>();

                //Get top ( Y + 1/2 size + offset)
                //float top = playerBoxCollider.bounds.center.y + playerBoxCollider.bounds.extents.y;

                //offsetY is distance from top to bottom + boxColliderSize/2 
                //float offsetY = boxColliderSize.y;

                //result = new Vector2(playerBoxCollider.transform.position.x, top + offsetY); // center , top + box
                box.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                result = new Vector2(boxColliderSize.x/2,-boxColliderSize.y);
            }
            catch
            {
                Debug.LogWarning("Could not find collider, resorting to default formula");
                result = new Vector2(transform.position.x, transform.position.y + col.bounds.size.y * 2);
            }

            return result;
        }

        private void OnDrawGizmosSelected()
        {
            col = gameObject.GetComponent<CapsuleCollider2D>();
            BoxCollider2D wallCheckCollider = transform.Find("Wall Check").GetComponent<BoxCollider2D>();
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(wallCheckCollider.transform.position.x + (Mathf.Abs(wallCheckCollider.offset.x) + wallCheckCollider.size.x ) * -transform.localScale.x, transform.position.y), Vector2.right * -transform.localScale.x);
        }
    }
}
