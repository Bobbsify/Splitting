using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{

    public int index;
    [SerializeField] bool keydown;
    [SerializeField] int maxindex;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keydown)
            {
                if (Input.GetAxis ("Vertical") < 0)
                {
                    if (index < maxindex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis ("Vertical") >0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxindex;
                    }
                }
                keydown = false;
            }
        }
    }
}
