using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
    public class Chase : MonoBehaviour
    {
        [Header("This requires a Patrol attached to the object")]
        [SerializeField] private float maxSpeed = 25.0f;
        [SerializeField] private float minSpeed = 1.0f;
        [SerializeField] private float IdealDistance = 20.0f;
        [SerializeField] private float DistanceUnit = 10.0f;
        [SerializeField] private float amountOfSpeedPerDistanceUnit = 1.0f;

        private Patrol attachedPatrol;

        private float baseSpeed;

        private void OnEnable()
        {
            if (TryGetComponent(out attachedPatrol))    //get attachedPatrol
            {
                throw new System.Exception("no Patrol found attached to object "+ this + "\n Please attach and configure a patrol");
            }
            baseSpeed = attachedPatrol.speed;
        }

        private void Update()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            attachedPatrol.speed = calculateSpeed(player);
        }

        private float calculateSpeed(GameObject target)
        {
            float result = baseSpeed;

            float distanceX = Mathf.Pow(target.transform.position.x - transform.position.x,2); //count distance on X Axis
            float distanceY = Mathf.Pow(target.transform.position.y - transform.position.y,2); //count distance on Y Axis

            float totalDistance = Mathf.Sqrt(distanceX + distanceY);

            result = Mathf.Max(Mathf.Min(maxSpeed,getSpeedDiff(totalDistance)),minSpeed);

            return result;
        }

        private float getSpeedDiff(float totalDist)
        {
            float result = ((totalDist - IdealDistance) > 0 ? 1 : -1); //if the distance is more than expected add speed else subtract

            result = Mathf.Abs(totalDist - IdealDistance) * result // total number of distance units to measure
                / DistanceUnit  // total number of distance units in distance to measure
                * amountOfSpeedPerDistanceUnit; // speed increases per distance unit difference

            return result;
        }
    }
}
