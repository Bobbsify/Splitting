using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    public Directions direction;
    public bool active;
    public Vector3 destination;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            TravelTo(destination);
        }
    }

    //Makes the platform travel
    private void TravelTo(Vector3 destination)
    {
        Vector3 position = transform.position;
        if (direction == Directions.Up) 
        {
            position.y += speed * Time.deltaTime; // Add Y
            if (position.y >= destination.y)
            {
                active = false;
            }
        }
        else if (direction == Directions.Right)
        {
            position.x += speed * Time.deltaTime; // Add X
            if (position.x >= destination.x)
            {
                active = false;
            }
        }
        else if (direction == Directions.Left)
        {
            position.x -= speed * Time.deltaTime; // Remove X
            if (position.x <= destination.x)
            {
                active = false;
            }
        }
        else
        {
            position.y -= speed * Time.deltaTime; // Remove Y
            if (position.y <= destination.y) {
                active = false;
            }
        }
        transform.position = position;
    }
}

public enum Directions
{
    Up,
    Down,
    Right,
    Left
}
