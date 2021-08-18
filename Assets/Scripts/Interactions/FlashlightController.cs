using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Splitting { 
    public class FlashlightController : MonoBehaviour
    {
        public float rotationAmount = 1.0f;
        
        [HideInInspector] public bool canUseFlashlight;

        private List<Light2D> flashlights = new List<Light2D>();
        private float angle;
        private bool lightsAre = true;

        private KeyCode rotateClockwise;
        private KeyCode rotateCounterClockwise;
        private KeyCode toggleFlashlight;

        private void Start()
        {
            rotateClockwise = new InputSettings().TorchAngleUpButton;
            rotateCounterClockwise = new InputSettings().TorchAngleDownButton;
            toggleFlashlight = new InputSettings().FlashlightButton;

            foreach (Transform child in transform)
            {
                Light2D light = child.GetComponent<Light2D>();
                if (light != null)
                {
                    flashlights.Add(light);
                }
            }

            angle = transform.localRotation.z;
        }

        private void Update()
        {
            if (canUseFlashlight) { 
                if (Input.GetKeyUp(toggleFlashlight))
                {
                    foreach (Light2D light in flashlights)
                    {
                        light.enabled = !light.enabled;
                    }
                    lightsAre = flashlights[0].enabled;
                }
                if (lightsAre == true) //ON?
                { 

                    if (Input.GetKey(rotateClockwise))
                    {
                        angle += rotationAmount * Time.deltaTime;
                    }
                    else if (Input.GetKey(rotateCounterClockwise))
                    {
                        angle -= rotationAmount * Time.deltaTime;
                    }
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }
            }
        }
    }
}
