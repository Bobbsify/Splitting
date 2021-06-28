using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHovering : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.x <80 && Input.mousePosition.x > -80)
        {
            if(Input.mousePosition.y > -22.2 && Input.mousePosition.y < 7.8)
            {
                animator.SetBool("hovering", true);
            }
        }
    }
}
