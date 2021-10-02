using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Splitting {

    public class ConstantSpeed : MonoBehaviour
    {
        public bool isEnabled = false;

        [SerializeField] private float speed;
        [SerializeField] private Direction direction;
        private int speedDirectionMultiplier = 1;

        [SerializeField] private UnityEvent wakingEvents;
        [SerializeField] private UnityEvent disabilngEvents;

        private void Awake()
        {

            if (isEnabled)
            {
                Enable();
            }
            else
            {
                Disable();
            }

            if (direction == Direction.Left)
            {
                speedDirectionMultiplier = -1;
            }
            else
            {
                speedDirectionMultiplier = 1;
            }

        }

        public void Enable()
        {
            isEnabled = true;
            wakingEvents.Invoke();
            foreach (Animator anim in GetComponentsInChildren<Animator>())
            {
                anim.enabled = true;
            }
        }

        public void Disable()
        {
            isEnabled = false;
            disabilngEvents.Invoke();
            foreach (Animator anim in GetComponentsInChildren<Animator>())
            {
                anim.enabled = false;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (isEnabled) { 
                Vector3 pos = collision.collider.transform.position;
                Vector2 newVec = new Vector3(pos.x + speed * speedDirectionMultiplier * Time.deltaTime, pos.y, pos.z);
                collision.collider.transform.position = newVec;
                if (collision.collider.tag == "Player")
                {
                    collision.collider.GetComponent<StateControllerInterface>().DisableJump();
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (isEnabled)
            {
                if (collision.collider.tag == "Player")
                {
                    collision.collider.GetComponent<StateControllerInterface>().EnableJump();
                }
            }
        }
    }

    enum Direction
    {
        Left,
        Right
    }
}
