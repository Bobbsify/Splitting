using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Rigidbody2D rbToThrow;
    public GameObject entityThrowing;
    public KeyCode throwButton = KeyCode.E;

    //control min and max of heightModifier
    [SerializeField] private float maxThrowDistance = 15f;
    [SerializeField] private float minThrowDistance = 1f;
    //control min and max of distanceModifier
    [SerializeField] private float maxHeightDistance = 15f;
    [SerializeField] private float minHeightDistance = 1f;
    //Throw base power
    [SerializeField] private float power = 5f;

    private float distanceModifier = 1f;
    private float heightModifier = 1f;

    private float horizontalInput;
    private float verticalInput;

    private float throwPower; //factors distanceModifier, power and direction

    private bool throwing = false;

    LineRenderer lr;

    Vector2 startPos;
    Vector2 endPos;
    Vector2 _velocity;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.position = rbToThrow.transform.position;
        //When key gets pressed
        if (Input.GetKeyDown(throwButton))
        {
            startPos = rbToThrow.transform.position;
            throwing = true;
        }

        if (throwing)
        {
            endPos = startPos;
            controlVelocity();
            _velocity = (endPos - startPos);

            Vector2[] trajectory = Plot(rbToThrow, transform.position, _velocity, calculateSteps());

            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            lr.SetPositions(positions);
        }

        if (Input.GetKeyUp(throwButton))
        {
            rbToThrow.velocity = _velocity;
            lr.positionCount = 0;
            throwing = false;
        }
    }

    private Vector2[] Plot(Rigidbody2D rigibody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigibody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigibody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    private void controlVelocity()
    {
        heightModifier = Mathf.Min(Mathf.Max(minHeightDistance, heightModifier + verticalInput), maxHeightDistance);
        distanceModifier = Mathf.Min(Mathf.Max(minThrowDistance, distanceModifier + horizontalInput), maxThrowDistance);

        throwPower = (power + distanceModifier) * -entityThrowing.transform.localScale.x;
        endPos.x += throwPower;
        endPos.y += power + heightModifier;
    }

    private int calculateSteps()
    {
        int heightInfluence = (int)(heightModifier); // 75%
        int distanceInfluence = (int)(distanceModifier * 25 / 100); // 25%
        return (200 + distanceInfluence + heightInfluence);
    }
}
