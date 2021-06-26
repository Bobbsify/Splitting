using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //speed of the object that's moving
    [SerializeField] private float speed;
    private float horizontalInput;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) { 
            horizontalInput = Input.GetAxis("Horizontal");
            transform.position = new Vector2(transform.position.x + (Time.deltaTime * speed * horizontalInput), transform.position.y); //transform.Translate?
            CallAnimator(Time.deltaTime * speed * horizontalInput);
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

    //Updates animator velocity
    private void CallAnimator(float speed)
    {
        if (gameObject.GetComponent<Animator>() != null) //is null falsey?
        { 
            gameObject.GetComponent<Animator>().SetFloat("velocityX", Mathf.Abs(speed));
        }
    }
}
