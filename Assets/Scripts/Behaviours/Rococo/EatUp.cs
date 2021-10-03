using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animations triggered:
 * 
 * Object destroy set to animation variable of type TRIGGER with name Break
 * 
 * Rococo jumping set to animation variable of type TRIGGER Jump
 * 
 * Rococo biting set to animation variable of type TRIGGER Bite
 *
 */
namespace Splitting { 
    public class EatUp : MonoBehaviour
    {
        public Vector3 currentTransform;

        [SerializeField] private GameObject rococoJumpAttackPrefab;
        [SerializeField] private GameObject rococoIdleAttackPrefab;
        [SerializeField] private float skipToBiteOffset = 5.0f;
        
        private Transform originalRococo;

        private Vector3 jumpRaystart;
        private Vector3 biteRangeCenter;
        private Vector3 rococoSize;

        private void Awake()
        {
            originalRococo = transform.parent;
            rococoSize = new Vector3(27.0f,5.6f,0);
        }

        private void Update()
        {
            currentTransform = transform.parent.position;
            Transform bone1 = originalRococo.Find("bone_1");
            jumpRaystart = bone1.Find("bone_2").position;
            biteRangeCenter = new Vector2 (bone1.position.x, bone1.Find("bone_2/bone_6").position.y);

            RaycastHit2D[] jumpCollisions = Physics2D.RaycastAll(jumpRaystart, Vector2.up * 200);
            RaycastHit2D[] biteCollisions = Physics2D.BoxCastAll(biteRangeCenter,new Vector2(5.2f,3.2f),0,new Vector2(0,0));
            GameObject activatedRococo;

            foreach (RaycastHit2D collision in jumpCollisions)
            {
                if (collision.collider.tag == "Player")
                {
                    activatedRococo = Instantiate(rococoJumpAttackPrefab);
                    activatedRococo.transform.position = new Vector2(collision.transform.position.x - rococoSize.x - (collision.collider.bounds.size.x / 2) , transform.parent.Find("FollowMe").position.y);
                    GameObject.FindGameObjectWithTag("Player").tag = "Untagged"; //Stop player from moving
                }
            }

            foreach (RaycastHit2D collision in biteCollisions)
            {
                if (collision.collider.tag == "Player")
                {
                    activatedRococo = Instantiate(rococoIdleAttackPrefab);
                    activatedRococo.transform.position = collision.transform.position - rococoSize;
                    GameObject.FindGameObjectWithTag("Player").tag = "Untagged"; //Stop player from moving
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(jumpRaystart, Vector2.up * 200);
            Gizmos.DrawCube(biteRangeCenter, new Vector2(5.2f, 3.2f));
        }
    }
}
