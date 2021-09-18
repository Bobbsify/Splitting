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

        private GameObject target;
        private Vector3 raystart;

        private bool arePatrolsDisabled = false;
        private bool doApproach = false;
        private bool bite = false;
        [SerializeField] private float speed = 10;


        private void Awake()
        {
             raystart = transform.Find("bone_1").position;
        }

        private void Update()
        {
            if (doApproach)
            {
                DisablePatrols();
                Jump();
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
        }

        private void DisablePatrols()
        {
            if (!arePatrolsDisabled)
            { 
                foreach (Patrol patrol in GetComponents<Patrol>())
                {
                    patrol.enabled = false;
                }
                GetComponent<Animator>().SetTrigger("Jump");
            }
            arePatrolsDisabled = true;
        }

        private void Jump()
        {
            if (!bite)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime)); //Jump up

                RaycastHit2D[] stuffBitten = Physics2D.RaycastAll(raystart,Vector2.down); // get all things he bites
                foreach (RaycastHit2D hit in stuffBitten)
                {
                    if (hit.collider.tag == "Player") //if he bites the player apply bite
                    {
                        bite = true;
                        GetComponent<Animator>().SetTrigger("Bite");
                        break;
                    }
                }
            }
        }

        private void Bite()
        {
            if (bite)
            { 
                target.GetComponent<Animator>().SetTrigger("Death");
            }
        }
    }
}
