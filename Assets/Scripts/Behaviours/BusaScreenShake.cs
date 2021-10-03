using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class BusaScreenShake : MonoBehaviour
    {
        new private CameraController camera;
        [SerializeField] private float shake = 1.0f;
        [SerializeField] private float lenght = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            // Get CameraController script
            try
            {
                camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }
            catch
            {
                throw new Exception("Camera not found");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CutsceneScreenShake()
        {
            camera.shakeMagnitude = shake;
            camera.shakeRemain = shake;
            camera.shakeLenght = lenght;

            camera.exeShake = true;
        }
    }
}
