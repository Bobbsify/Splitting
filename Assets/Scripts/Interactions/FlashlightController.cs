using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Splitting { 
    public class FlashlightController : MonoBehaviour
    {
        public float rotationAmount = 1.0f;

        [HideInInspector] public bool canUseFlashlight = true;
         public bool lightsAre = true;

        private List<Light2D> flashlights = new List<Light2D>();
        private float angle;

        private KeyCode rotateClockwise;
        private KeyCode rotateCounterClockwise;
        private KeyCode toggleFlashlight;

        private float flipped;

        private void OnEnable()
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

            flipped = getTyr(transform).localScale.x;
            angle = 180; //boh
        }

        private void Update()
        {
            if (flashlights[0].enabled != lightsAre)
            {
                SetLightsToState(lightsAre);
            }
            if (canUseFlashlight) { 
                if (Input.GetKeyUp(toggleFlashlight))
                {
                    ToggleLights();
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
                    flipPositionCheck();
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }
            }
        }

        public void SetLightsToState(bool state)
        {
            foreach (Light2D light in flashlights)
            {
                light.enabled = state;
            }
            lightsAre = state;
        }

        private void ToggleLights()
        {
            lightsAre = !lightsAre;
        }

        private void flipPositionCheck()
        {
            Transform player = getTyr(transform);
            if (positiveOrNegative(flipped) != positiveOrNegative(player.localScale.x))
            {
                flipped = player.localScale.x;

                float posOrNeg = positiveOrNegative(angle);

                angle = (180 - Mathf.Abs(angle)) * posOrNeg;
                InvertX();
            }
        }

        //Returns tyr or tyrant if it is within 10 transforms
        private Transform getTyr(Transform originalTransform)
        {
            Transform result = originalTransform;
            int exitCond = 0;
            while (!result.name.ToUpper().Contains("TYR"))
            {
                result = result.parent;

                if (exitCond == 10) break;

                exitCond++;
            }
            return result;
        }

        //Returns -1 if target is negative, +1 if positive
        private float positiveOrNegative(float value)
        {
            return value >= 0 ? 1 : -1;
        }

        private void InvertX()
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }

    }
}
