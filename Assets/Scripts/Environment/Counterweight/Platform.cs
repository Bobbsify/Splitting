using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private BoxCollider2D entityEntryDetection;
    private Vector3 size;
    private float initialColliderSize; //These regard the
    private float initialColliderOffset; //Y of both objects
    private float colliderModifier = 0; //Collidersize modifier based on entities who have entered and exited the collider
    public int score = 0;

    private void Awake()
    {
        entityEntryDetection = GetComponent<BoxCollider2D>();
        initialColliderSize = entityEntryDetection.size.y;
        initialColliderOffset = entityEntryDetection.offset.y;
    }

    private void Update()
    {
        entityEntryDetection.size = new Vector3(entityEntryDetection.size.x, initialColliderSize + colliderModifier);
        entityEntryDetection.offset = new Vector3(entityEntryDetection.offset.x, initialColliderOffset + colliderModifier / 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Carryable")
        {
            score += 2;
            colliderModifier += collision.bounds.size.y;
        }
        else if (collision.tag == "Player")
        {
            score += 1;
            colliderModifier += collision.bounds.size.y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Carryable")
        {
            score -= 2;
            colliderModifier -= collision.bounds.size.y;
        }
        else if (collision.tag == "Player")
        {
            score -= 1;
            colliderModifier -= collision.bounds.size.y;
        }
    }

}
