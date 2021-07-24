using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* slow to fast to slow non funziona la metterò a posto se servira :^)*/

public class Patrol : MonoBehaviour
{
    [Header("Path Properties")]
    [SerializeField] private Vector3[] path;
    [SerializeField] private PatrolTypes patrolType;

    [Header("Movement Properties")]
    [SerializeField] private float speed;
    [Header("Slow to fast to Slow non funziona, la metterò a posto se servirà :^)")]
    [SerializeField] private MovementTypes movementType;

    private int nextPoint = 0;
    private Vector2 startPoint;

    private float distanceX;
    private float distanceY;
    private float totalDistance;

    private int whereToX;
    private int whereToY;

    private int travelMode = 1;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        StartCalculations();
    }

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
            if (transform.position.y < path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x < path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == 1 && whereToY == -1) //Moving Top Left
        {
            if (transform.position.y > path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x < path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == -1 && whereToY == 1) //Moving Bottom Right
        {
            if (transform.position.y < path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x > path[nextPoint].x)
            {
                AdvanceX();
            }
        }
        if (whereToX == -1 && whereToY == -1) //Moving Bottom Left
        {
            if (transform.position.y > path[nextPoint].y)
            {
                AdvanceY();
            }
            if (transform.position.x > path[nextPoint].x)
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
            }
            else
            {
                nextPoint = 0;
            }
        }
        else
        {
            nextPoint += (travelMode);
        }
        if (patrolType == PatrolTypes.NextPointOnAwake || patrolType == PatrolTypes.NextPointOnAwakeNeverRepeat || patrolType == PatrolTypes.NextPointOnAwakeSwing)
        {
            this.enabled = false;
        }
        else
        {
            StartCalculations();
        }
        Debug.Log("Next Point: "+nextPoint);
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