using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

/* slow to fast to slow non funziona la metterò a posto se servira :^)*/

public class Patrol : MonoBehaviour
{
    [Header("Path Properties")]
    [SerializeField] private Vector3[] path;
    [SerializeField] private float[] speeds;
    [SerializeField] private PatrolTypes patrolType;

    [Header("Movement Properties")]
    [SerializeField] public float speed;
    [Header("Slow to fast to Slow non funziona, la metterò a posto se servirà :^)")]
    [SerializeField] private MovementTypes movementType;

    private int nextPoint = 0;
    private Vector2 startPoint;

    private float distanceX;
    private float distanceY;
    private float totalDistance;

    private int whereToX;
    private int whereToY;

    private int travelMode = 1; //Going forward or backwards

    [Header("Determines whether this platform carries any objects colliding on top of it with itself")]
    [SerializeField] private bool elevator = false;
    [SerializeField] private int expectedCarriedUnits = 5;

    [Header("Events upon point arrival")]
    [SerializeField] private bool doOnPathCompletionInstead;
    [SerializeField] private UnityEvent checkpointEvents;

    private void OnEnable()
    {
        startPoint = transform.position;
        StartCalculations();
    }

    // Update is called once per frame
    void Update()
    {
        TravelToNextPoint();
        Disable();
    }

    private void TravelToNextPoint()
    {
        if (movementType == MovementTypes.Teleport) {
            transform.position = path[nextPoint];
        }
        else {
            CalculateMovement();
        }
    }

    private void CalculateMovement()
    {
        if (whereToX == 1 && whereToY == 1) //Moving Top Right
        {
            if (transform.position.y <= path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x <= path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == 1 && whereToY == -1) //Moving Top Left
        {
            if (transform.position.y >= path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x <= path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == -1 && whereToY == 1) //Moving Bottom Right
        {
            if (transform.position.y <= path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x >= path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == -1 && whereToY == -1) //Moving Bottom Left
        {
            if (transform.position.y >= path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x >= path[nextPoint].x)
            {
                AdvanceX();
            }
        }
    }

    private void AdvanceX()
    {
        Vector2 position = transform.position;
        if (movementType == MovementTypes.FixedSpeed)
        {
            position.x += (speed * Mathf.Min(distanceX / distanceY, 1) * Time.deltaTime) * whereToX;
        }
        else //MovementTypes.SlowToFastToSlow
        {
            position.x += (Speed(true) * Time.deltaTime) * whereToX;
        }
        transform.position = position;
    }

    private void AdvanceY()
    {
        //Se la distanza X è maggiore della distanzaY la velocità y deve diminuire altrimenti deve aumentare
        Vector2 position = transform.position;
        if (movementType == MovementTypes.FixedSpeed)
        {
            position.y += speed * Mathf.Min(distanceY/distanceX,1) * Time.deltaTime * whereToY;
        }
        else //MovementTypes.SlowToFastToSlow
        {
            position.y += (Speed(false) * distanceY / distanceX * Time.deltaTime) * whereToY;
        }
        transform.position = position;
    }

    private float Speed(bool x)
    {
        if (x)
        {
            /*if (isInRange(transform.position.x, startPoint.x, distanceX / 4) || isInRange(transform.position.x, path[nextPoint].x, -(distanceX / 4)))
            {
                Debug.Log("SpeedX = " + (speed * Mathf.Max(((distanceX - Mathf.Abs(transform.position.x - path[nextPoint].x)) / distanceX), 0.1f)));              */
            float currentDistance = Mathf.Abs(transform.position.x - path[nextPoint].x);
            return speed * Mathf.Max(((distanceX - currentDistance) / (distanceX / 2)),0.1f);
            /*}
                return speed;*/
        }
        else
        {
            /*   if (isInRange(transform.position.y, startPoint.y, distanceY / 4) || isInRange(transform.position.y, path[nextPoint].y, -(distanceY / 4)))
               {
            Debug.Log("SpeedY = " + (speed * Mathf.Max(((distanceY - Mathf.Abs(transform.position.y - path[nextPoint].y)) / distanceY), 0.1f)));
            */
            float currentDistance = Mathf.Abs(transform.position.y - path[nextPoint].y);
            return speed * Mathf.Max(((distanceY - currentDistance) / (distanceY / 2)), 0.1f);
            /*    }
               return speed;*/
        }
    }

    private bool isInRange(float numberToCompare,float rangeStart, float range)
    {
        return numberToCompare>=rangeStart && numberToCompare<=rangeStart+range;
    }
    
    private void StartCalculations()
    {
        distanceY = Mathf.Abs(transform.position.y - path[nextPoint].y);
        distanceX = Mathf.Abs(transform.position.x - path[nextPoint].x);
        totalDistance = Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow(distanceX, 2));

        whereToX = transform.position.x < path[nextPoint].x ? 1 : -1; //Left or Right
        whereToY = transform.position.y < path[nextPoint].y ? 1 : -1; //Up or down
        
        if (elevator)
        {
            Collider2D col = FetchElevatorCollider();
            Collider2D[] contacts = new Collider2D[expectedCarriedUnits];
            col.GetContacts(contacts);
            Debug.Log(contacts);
            if (contacts != null) { 
                attachObjectsToThis(contacts);
            }
        }
    }

    private void Disable()
    {
        switch (whereToX)
        {
            case 1:
                if (transform.position.x >= path[nextPoint].x)
                {
                    switch (whereToY)
                    {
                        case 1:
                            if (transform.position.y >= path[nextPoint].y)
                            {
                                HandleNextPoint();
                            }
                            break;
                        case -1:
                            if (transform.position.y <= path[nextPoint].y)
                            {
                                HandleNextPoint();
                            }
                            break;
                        default:
                            Debug.Log("Unknown Error, whereToY is undefined");
                            break;
                    }
                }
                break;
            case -1:
                if (transform.position.x <= path[nextPoint].x)
                {
                    switch (whereToY)
                    {
                        case 1:
                            if (transform.position.y >= path[nextPoint].y)
                            {
                                HandleNextPoint();
                            }
                            break;
                        case -1:
                            if (transform.position.y <= path[nextPoint].y)
                            {
                                HandleNextPoint();
                            }
                            break;
                        default:
                            Debug.Log("Unknown Error, whereToY is undefined");
                            break;
                    }
                }
                break;
            default:
                Debug.Log("Unknown Error, whereToX is undefined");
                break;
        }
    }

    private void HandleNextPoint()
    {
        if (nextPoint + travelMode >= path.Length || nextPoint + travelMode < 0) //If the next point would go out of bounds
        {
            if (patrolType == PatrolTypes.ContinuousNeverRepeat || patrolType == PatrolTypes.NextPointOnAwakeNeverRepeat)
            {
                path = new Vector3[0];
                nextPoint = 0;
                this.enabled = false; //Disable to not get errors in the console
            }
            else if (patrolType == PatrolTypes.ContinuousSwing || patrolType == PatrolTypes.NextPointOnAwakeSwing)
            {
                travelMode = -travelMode;
                nextPoint += travelMode;
                speed = speeds[nextPoint];
            }
            else
            {
                nextPoint = 0;
            }
            if (doOnPathCompletionInstead)
            {
                checkpointEvents.Invoke();
            }
        }
        else
        {
            nextPoint += (travelMode);
            speed = speeds[nextPoint];
        }

        removeAttachedObjects();

        //Execute checkpoint events
        if (!doOnPathCompletionInstead)
        {
            checkpointEvents.Invoke();
        }

        if (patrolType == PatrolTypes.NextPointOnAwake || patrolType == PatrolTypes.NextPointOnAwakeNeverRepeat || patrolType == PatrolTypes.NextPointOnAwakeSwing)
        {
            this.enabled = false;
        }
        else
        {
            StartCalculations();
        }
    }

    private Collider2D FetchElevatorCollider()
    {
        Collider2D result;

        result = GetComponent<Collider2D>();

        if(result == null || result.isTrigger) //No collider was found or it's wrong
        {
            Collider2D[] cols = GetComponentsInChildren<Collider2D>(); //search in children
            foreach (Collider2D col in cols)
            {
                if (!col.isTrigger && result == null)
                {
                    result = col;
                }
                else if (!col.isTrigger && result != null)
                {
                    throw new Exception("Multiple valid colliders were found!! First fetched collider: " + result + "\nOther: " + col);
                }
            }
        }

        return result;
    }

    private void attachObjectsToThis(Collider2D[] cols)
    {
        if (cols != null) { 
            foreach (Collider2D col in cols)
            {
                if (col != null) { 
                    if (col.tag == "Player" || col.tag == "Carryable" || col.name.ToLower().Contains("tyr") || col.name.ToLower().Contains("ant"))
                    {
                        col.gameObject.transform.parent = transform;
                    }
                }
            }
        }
    }

    private void removeAttachedObjects()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child);
            if (child.tag == "Player" || child.tag == "Carryable" || child.name.ToLower() == "tyr" || child.name.ToLower() == "ant")
            {
                child.gameObject.transform.parent = null;
            }
        }
    }

}

enum PatrolTypes
{
    Continuous,
    NextPointOnAwake,
    ContinuousSwing,
    NextPointOnAwakeSwing,
    ContinuousNeverRepeat,
    NextPointOnAwakeNeverRepeat,
}

enum MovementTypes
{
    FixedSpeed,
    SlowToFastToSlow,
    Teleport,
}