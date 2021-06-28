using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //speed of the object that's moving
    private Animator animator;

    [SerializeField] private float speed;
    private float horizontalInput;
    private float verticalInput;

    [System.NonSerialized] public bool isCrouched;
    public bool canCrouch;
    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isCrouched = verticalInput < 0 && canCrouch;

        if (canMove)
        {
            transform.position = new Vector2(transform.position.x + (Time.deltaTime * speed/(isCrouched ? 2 : 1) * horizontalInput), transform.position.y); //halves speed if is crouchings
            CallAnimator(Time.deltaTime * speed/(isCrouched ? 2 : 1) * horizontalInput, isCrouched); 
        }
        //Invertscale
        if (horizontalInput != 0)
        {
            if (horizontalInput < 0)
            {
                horizontalInput = -1;
            }
            else
            {
                horizontalInput = 1;
            }
            transform.localScale = new Vector3(-horizontalInput, transform.localScale.y, 1);
        }
    }

    //Updates animator velocity
    private void CallAnimator(float speed,bool crouched)
    {
        if (animator != null) //is null falsey?
        {
            animator.SetFloat("velocityX", Mathf.Abs(speed));
            animator.SetBool("isCrouched", crouched);
        }
    }
}
