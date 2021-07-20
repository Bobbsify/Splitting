using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extend : MonoBehaviour
{
    private Animator animator;

    [HideInInspector] public bool canExtend = true;
    private float verticalInput;

    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        if (canExtend) { 
            if (verticalInput > 0)
            {
                animator.SetBool("isExtending", true);
            }
            else
            {
                animator.SetBool("isExtending", false);
            }
        }
    }
}
