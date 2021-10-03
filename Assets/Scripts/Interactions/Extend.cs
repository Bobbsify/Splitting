using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Extend : MonoBehaviour
    {
        private Animator animator;

        public bool isExtended;
        [HideInInspector] public bool canExtend = true;
        private float verticalInput;


        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            verticalInput = Input.GetAxis("Vertical");
            if (canExtend)
            {
                if (verticalInput > 0)
                {
                    isExtended = true;
                    animator.SetBool("isExtending", true);
                }
                else
                {
                    isExtended = false;
                    animator.SetBool("isExtending", false);
                }
            }
        }
    }
}
