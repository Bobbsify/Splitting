using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{
    public class CameraUpdatePosition : MonoBehaviour
    {
        new private CameraController camera;

        private BoxCollider2D cameraBoundsCollider;

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

            cameraBoundsCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool x = false;
            bool y = false;

            x = gameObject.name.ToUpper().Contains("X");
            y = gameObject.name.ToUpper().Contains("Y");

            if (collision.gameObject.tag == "MainCamera")
            {
                if (x)
                {
                    camera.checkColX = true;
                    camera.boundsX = true;                    
                }

                if (y)
                {
                    camera.checkColY = true;
                    camera.boundsY = true;                    
                }
                
            }                     
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            bool x = false;
            bool y = false;

            x = gameObject.name.ToUpper().Contains("X");
            y = gameObject.name.ToUpper().Contains("Y");

            if (collision.gameObject.tag == "MainCamera")
            {
                if (x)
                {
                    camera.checkColX = false;
                    camera.boundsX = false;
                }
                
                if (y)
                {
                    camera.checkColY = false;
                    camera.boundsY = false;
                }

            }
        }

    }
}
