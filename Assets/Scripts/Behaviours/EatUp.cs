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
        [SerializeField] private Collider2D deathAreaTrigger;
        [SerializeField] private float skipToBiteOffset = 5.0f;

        private GameObject target;
        private Vector3 raystart;
        private Animator crocoAnim;

        private bool arePatrolsDisabled = false;
        private bool doApproach = false;
        private bool bite = false;
        [SerializeField] private float jumpSpeed = 10;


        private void Awake()
        {
            raystart = transform.parent.Find("bone_1").position;
            transform.parent.TryGetComponent(out crocoAnim);
        }

        private void Update()
        {
            if (doApproach)
            {
                Jump();
                Bite();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Object")
            {
                collision.GetComponent<Animator>().SetTrigger("Break"); // if collided object set it to broken
            }
            else if (collision.tag == "Player")
            {
                Approach(collision.gameObject);
            }
        }

        private void Approach(GameObject player)
        {
            target = player;
            doApproach = true;
            if (player.transform.position.y >= raystart.y - skipToBiteOffset && player.transform.position.y <= raystart.y + skipToBiteOffset)
            {
                bite = true;
            }
        }

        private void DisablePatrols()
        {
            if (!arePatrolsDisabled)
            { 
                foreach (Patrol patrol in GetComponents<Patrol>())
                {
                    patrol.enabled = false;
                }
                transform.parent.GetComponent<Animator>().SetTrigger("Jump");
            }
            arePatrolsDisabled = true;
        }

        private void Jump()
        {
            if (!bite)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (jumpSpeed * Time.deltaTime)); //Jump up

                RaycastHit2D[] stuffBitten = Physics2D.RaycastAll(raystart,Vector2.down); // get all things he bites
                foreach (RaycastHit2D hit in stuffBitten)
                {
                    if (hit.collider.name.ToLower().Contains("ant") || hit.collider.name.ToLower().Contains("tyr")) //if he bites the player apply bite
                    {
                        bite = true;
                        break;
                    }
                }
            }
        }

        private void Bite()
        {
            if (bite)
            {
                target.transform.parent = transform.parent.Find("bone_1");
                transform.parent.GetComponent<Animator>().SetTrigger("Bite");
                target.GetComponent<Animator>().SetTrigger("death");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raystart, Vector2.down);
        }
    }
}
