using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menubuttoncontroller;
    [SerializeField] Animator animator;
    [SerializeField] int thisindex;

    // Update is called once per frame
    void Update()
    {
        if (menubuttoncontroller.index == thisindex)
        {
            animator.SetBool("selected button", true);
            if(Input.GetAxis ("Submit") ==1)
            {
                animator.SetBool("pressed button", true);
            }
            else
            {
                animator.SetBool("selected button", false);
            }
        }
    }
}
