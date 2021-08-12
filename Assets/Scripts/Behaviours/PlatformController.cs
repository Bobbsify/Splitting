using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  Steps
 *  
 *  1) Detect if a player is on top the object
 *  
 *  2) When a player has collided enable Down+Space
 *  
 *  3) When Down+Space is pressed enable collider trigger
 *  
 *  4) When y is less than transform y disable trigger
 *  
 *  5) Pray
 *  
 */

public class PlatformController : MonoBehaviour
{
    private GameObject entityFalling;
    private Collider2D entityBoxCollider;
    private Collider2D entityCapsuleCollider;

    private Collider2D platformCollider;

    private bool isFalling = false;
    private int entityFallingLayer;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        if (platformCollider == null) { throw new System.Exception("Could not find Collider for platform " + gameObject); }
    }

    private void Update()
    {
        if (entityFalling != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetAxis("Vertical") < 0)
            {
                StartFall();
            }
        }
        if (isFalling)
        {
            if (entityFalling.transform.position.y + entityBoxCollider.bounds.extents.y < platformCollider.transform.position.y)
            {
                isFalling = false;
                entityBoxCollider.isTrigger = false;
                entityCapsuleCollider.isTrigger = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            entityFalling = collision.gameObject;
        }
    }

    private void StartFall()
    {
        //Fall
        entityFallingLayer = entityFalling.layer;
        entityCapsuleCollider = entityFalling.GetComponent<CapsuleCollider2D>();
        entityBoxCollider = entityFalling.GetComponent<BoxCollider2D>();

        isFalling = true;

        //remove collision until character is below effector
        entityBoxCollider.isTrigger = true;
        entityCapsuleCollider.isTrigger = true;
    }

}
