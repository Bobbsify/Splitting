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
        [SerializeField] private string rococoJumpAttackName = "RococoJump";
        [SerializeField] private string rococoIdleAttackName = "RococoAttack";
        [SerializeField] private float skipToBiteOffset = 5.0f;

        private Transform rococoTransform;
        private Transform originalRococo;
        
        private void Awake()
        {
            originalRococo = transform.parent;
            rococoTransform = originalRococo.Find("bone_1");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Vector3 playerPos = collision.transform.position;
                if (playerPos.y >= rococoTransform.position.y - skipToBiteOffset && playerPos.y <= rococoTransform.position.y + skipToBiteOffset)
                {
                    Transform activatedRococo = originalRococo.Find(rococoIdleAttackName);
                    activatedRococo.transform.parent = null; //remove from this obj
                    activatedRococo.transform.position = originalRococo.position; // move to current rococo pos
                    activatedRococo.gameObject.SetActive(true); //activate obj
                    Destroy(originalRococo.gameObject);
                }
                else
                {
                    Transform activatedRococo = originalRococo.Find(rococoJumpAttackName);
                    activatedRococo.transform.parent = null; //remove from this obj
                    activatedRococo.transform.position = originalRococo.position; // move to current rococo pos
                    activatedRococo.gameObject.SetActive(true); //activate obj
                    Destroy(originalRococo.gameObject);
                }
            }
        }
    }
}
