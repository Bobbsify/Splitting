using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite; 

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.flag[1])
        {            
            transform.GetChild(0).gameObject.SetActive(true);

            if (sprite != null)
            {
                sprite.enabled = true;
            }

            if (anim != null)
            {
                anim.enabled = true;
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);

            if (sprite != null)
            {
                sprite.enabled = false;
            }

            if (anim != null)
            {
                anim.enabled = false;
            }
        }
        
    }
}
