using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
    public class FlashlightController : MonoBehaviour
    {
        public float rotationAmount = 1.0f;

        private float angle;

        private KeyCode rotateClockwise;
        private KeyCode rotateCounterClockwise;

        private void Start()
        {
            rotateClockwise = new InputSettings().TorchAngleUpButton;
            rotateCounterClockwise = new InputSettings().TorchAngleDownButton;

            angle = transform.localRotation.z;
        }

        private void Update()
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
