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

        private Transform rococoTransform;
        private Transform originalRococo;
        
        private void Awake()
        {
            originalRococo = transform.parent;
            rococoTransform = originalRococo.Find("bone_1");
        }

        private void Update()
        {
            currentTransform = transform.parent.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Vector3 playerPos = collision.transform.position;
                if (playerPos.y >= rococoTransform.position.y - skipToBiteOffset && playerPos.y <= rococoTransform.position.y + skipToBiteOffset)
                {
                    GameObject activatedRococo = Instantiate(rococoIdleAttackPrefab);
                    activatedRococo.transform.position = transform.parent.position;
                    GameObject.FindGameObjectWithTag("Player").tag = "Untagged"; //Stop player from moving
                }
                else
                {
                    GameObject activatedRococo = Instantiate(rococoJumpAttackPrefab);
                    activatedRococo.transform.position = transform.parent.position;
                    GameObject.FindGameObjectWithTag("Player").tag = "Untagged"; //Stop player from moving
                }
            }
        }
    }
}
