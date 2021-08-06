using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class CeilingCheckTA : MonoBehaviour
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
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateControllerTA.isObstructed = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                stateControllerTA.isObstructed = false;
            }
        }
    }
}
