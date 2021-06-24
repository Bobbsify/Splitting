using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    public bool canJump;

    public float jumpForce = 1000.0f;
    public bool jumping;

    public float timerJump = 2.0f;
    public float elapsed;

    new private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        jumping = Input.GetKey(KeyCode.Space);


        if ( canJump)
        {
            if (jumping)
            {
                elapsed += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space) && elapsed < timerJump)
            {
                rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                canJump = false;
                elapsed = 0.0f;

            }
            else if (Input.GetKeyUp(KeyCode.Space) && elapsed >= timerJump)
            {
                rigidbody2D.AddForce(new Vector2(0f, jumpForce * 2));
                canJump = false;
                elapsed = 0.0f;
            }

        }        

    }
}
