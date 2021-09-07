using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class CeilingCheck : MonoBehaviour
    {
        private StateController stateController;
        private Carry carry;

        // Start is called before the first frame update
        void Start()
        {           
            stateController = gameObject.GetComponentInParent<StateController>();

            carry = gameObject.GetComponentInParent<Carry>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Carryable")
            {
                stateController.isObstructed = true;
            }
            
            if (collision.gameObject.tag == "Carryable" && !carry.isCarrying && !carry.wasCarrying)
            {
                carry.carryedObj = collision.gameObject;                
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Carryable")
            {
                stateController.isObstructed = false;
            }
            
            if (collision.gameObject.tag == "Carryable")
            {
                carry.carryedObj = null;                
            }
            
        }
    }
}
