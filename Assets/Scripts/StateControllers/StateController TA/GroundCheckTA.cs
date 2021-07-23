using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class GroundCheckTA : MonoBehaviour
    {
        public StateControllerTA stateControllerTA;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Carryable" || collision.gameObject.tag == "Platform")
            {
                stateControllerTA.isGrounded = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Carryable" || collision.gameObject.tag == "Platform")
            {
                stateControllerTA.isGrounded = false;
            }
        }

    }
}
