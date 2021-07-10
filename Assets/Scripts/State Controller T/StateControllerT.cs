using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class StateControllerT : MonoBehaviour
    {
        public bool hasControl;

        public bool isGrounded;
        public bool isWalled;
        public bool isObstructed;

        public Move move;

        new public CameraController camera;

        // Start is called before the first frame update
        void Start()
        {
            hasControl = true;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (hasControl)
            {
                if (isWalled)
                {
                    move.canMove = false;
                }
                else
                {
                    move.canMove = true;
                }
            }

        }
    }
}
