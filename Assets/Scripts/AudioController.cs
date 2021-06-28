

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioIn;
    private Collider2D objCollider;
    private float maxY;
    private float maxX;

    private void Start()
    {
        objCollider = gameObject.GetComponent<Collider2D>();
        maxX = objCollider.bounds.size.x / 2;
        maxY = objCollider.bounds.size.y / 2;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.tag == "Audio")
        {
            collided.GetComponent<AudioSource>().volume = ProcessVolume(collided.transform.position.x, collided.transform.position.y);
            collided.GetComponent<AudioSource>().panStereo = DetectStereoPan(collided.transform.position.x);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Audio")
        {
            collision.gameObject.GetComponent<AudioSource>().volume = 0;
        }
    }

    //Detects how far away an object should be sterepoanned
    private float DetectStereoPan(float incomingX)
    {
        float currX = transform.position.x;

        int direction = (currX > incomingX) ? -1 : 1; //comes from left or right
        float distanceX = Mathf.Abs(currX - incomingX); //detect distance X

        float maxX = objCollider.bounds.size.x/2;

        return (distanceX / maxX * direction);
    }

    private float ProcessVolume(float incomingX,float incomingY)
    {
        float currX = transform.position.x;
        float currY = transform.position.y;

        float distanceX = Mathf.Abs(currX - incomingX); //detect distance X
        float distanceY = Mathf.Abs(currY - incomingY); //detect distance X


        return 1-Mathf.Sqrt(Mathf.Pow(distanceX / maxX,2) + Mathf.Pow(distanceY / maxY,2));

    }
}
