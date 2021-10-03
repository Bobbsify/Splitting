using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class GroundCheckTA : MonoBehaviour
    {
        private StateControllerTA stateControllerTA;

        // Start is called before the first frame update
        void Start()
        {
            stateControllerTA = gameObject.GetComponentInParent<StateControllerTA>();
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

            /*
            if (collision.gameObject.tag == "Carryable")
            {
                stateControllerTA.isPushing = true;
            }
            */
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Carryable" || collision.gameObject.tag == "Platform")
            {
                stateControllerTA.isGrounded = false;
            }

            /*
            if (collision.gameObject.tag == "Carryable")
            {
                stateControllerTA.isPushing = false;
            }
            */
        }

    }
}
