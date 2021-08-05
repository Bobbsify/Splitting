using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class CeilingCheckT : MonoBehaviour
    {
        private StateControllerT stateController;

        // Start is called before the first frame update
        void Start()
        {
            stateController = gameObject.GetComponentInParent<StateControllerT>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateController.isObstructed = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateController.isObstructed = false;
            }
        }
    }
}
