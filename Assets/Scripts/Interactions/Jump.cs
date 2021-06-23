using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    public bool canJump;

    public float vSpeed = 2000.0f;
    public bool jumpForce;

    public float timerJump = 2.0f;
    public float elapsed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        jumpForce = Input.GetKey(KeyCode.Space);

        if (jumpForce && canJump)
        {
            elapsed += Time.deltaTime;             
        }

        if (Input.GetKeyUp(KeyCode.Space) && elapsed < timerJump)
        {
            transform.Translate(Vector3.up * Time.deltaTime * vSpeed);
            canJump = false;
            elapsed = 0.0F;

        } else if (Input.GetKeyUp(KeyCode.Space) && elapsed >= timerJump)
        {
            transform.Translate(Vector3.up * Time.deltaTime * vSpeed * 2);
            canJump = false;
            elapsed = 0.0F;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }

        Debug.Log("reset jump");
    }
}
