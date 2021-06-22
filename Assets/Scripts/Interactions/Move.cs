using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //speed of the object that's moving
    public float speed;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.position = new Vector2(transform.position.x + (Time.deltaTime * speed * horizontalInput), transform.position.y); //transform.Translate?
        if (horizontalInput != 0) {
            if (horizontalInput < 0)
            {
                horizontalInput = -1;
            }
            else
            {
                horizontalInput = 1;
            }
            transform.localScale = new Vector3(-horizontalInput, transform.localScale.y,1);
        }
    }
}
