using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Fear : MonoBehaviour
    {
        private StateController antStateController;
        private StateControllerTA tyrantStateController;

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
            bool ant = false;
            bool tyr = false;          

            ant = collision.name.ToUpper().Contains("ANT");
            tyr = collision.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {
                antStateController = collision.GetComponent<StateController>();

                antStateController.isIlluminated = true;
            }
            else if (ant && tyr)
            {
                tyrantStateController = collision.GetComponent<StateControllerTA>();

                tyrantStateController.isIlluminated = true;
            }           
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            bool ant = false;
            bool tyr = false;

            ant = collision.name.ToUpper().Contains("ANT");
            tyr = collision.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {               
                antStateController.isIlluminated = false;
            }
            else if (ant && tyr)
            {              
                tyrantStateController.isIlluminated = false;
            }
        }        
    }
}
