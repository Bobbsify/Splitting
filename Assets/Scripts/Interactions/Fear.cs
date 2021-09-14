using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Fear : MonoBehaviour
    {
        private StateController antStateController;
        private StateControllerTA tyrantStateController;

        public FlashlightController flashlightController;
        public Collider2D lightAreaCol;

        [SerializeField] private float elapsedInDark;
        [SerializeField] private float timerInDark = 1.5f;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.TryGetComponent<FlashlightController>(out flashlightController);
            gameObject.TryGetComponent<Collider2D>(out lightAreaCol);
        }

        // Update is called once per frame
        void Update()
        {
            ControlLightStatus();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            elapsedInDark = 0.0f;

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
            elapsedInDark += Time.deltaTime;

            bool ant = false;
            bool tyr = false;

            ant = collision.name.ToUpper().Contains("ANT");
            tyr = collision.name.ToUpper().Contains("TYR");

            if (elapsedInDark >= timerInDark)
            {
                if (ant && !tyr)
                {
                    antStateController.isIlluminated = false;
                }
                else if (ant && tyr)
                {
                    tyrantStateController.isIlluminated = false;
                }

                elapsedInDark = 0.0f;
            }
           
        }
        
        void ControlLightStatus()
        {
            if (flashlightController != null && lightAreaCol != null)
            {
                if (flashlightController.lightsAre)
                {
                    lightAreaCol.enabled = true;
                }
                else
                {
                    lightAreaCol.enabled = false;
                }
            }
        }
    }
}
