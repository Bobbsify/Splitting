using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Splitting {

    public class ConstantSpeed : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Direction direction;
        private int speedDirectionMultiplier = 1;

        [SerializeField] private UnityEvent wakingEvents;
        [SerializeField] private UnityEvent disabilngEvents;

        private void Awake()
        {
            if (direction == Direction.Left)
            {
                speedDirectionMultiplier = -1;
            }
            else
            {
                speedDirectionMultiplier = 1;
            }
            wakingEvents.Invoke();
        }

        private void OnDisable()
        {
            disabilngEvents.Invoke();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            collision.collider.attachedRigidbody.AddForce(new Vector2(speed * speedDirectionMultiplier * Time.deltaTime, 0));
        }
    }

    enum Direction
    {
        Left,
        Right
    }
}
