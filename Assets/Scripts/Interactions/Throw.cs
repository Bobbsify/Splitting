using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class Throw : MonoBehaviour
    {
        public Rigidbody2D rbToThrow;
        public GameObject entityThrowing;
        private KeyCode throwButton;
        private KeyCode undoThrowButton;

        //control min and max of heightModifier
        [SerializeField] private float maxPower = 15f;
        [SerializeField] private float minPower = -15f;
        private float power = 0f;
        //control min and max of distanceModifier
        [SerializeField] private float maxAngle = 90f;
        [SerializeField] private float minAngle = -90f;
        private float angle = 0f;

        private float horizontalInput;
        private float verticalInput;

        public bool throwing = false;

        LineRenderer lr;

        Vector2 startPos;
        Vector2 endPos;
        Vector2 _velocity;

        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
            throwButton = new InputSettings().ThrowButton;
            undoThrowButton = new InputSettings().UndoThrowButton;
        }

        private void Update()
        {
            if (rbToThrow != null)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");

                transform.position = rbToThrow.transform.position;

                if (Input.GetKeyUp(undoThrowButton)) //drop
                {
                    rbToThrow.velocity = new Vector2(entityThrowing.GetComponent<Collider2D>().bounds.size.x*2, 0) * -entityThrowing.transform.localScale;
                    resetLr();
                }

                if (Input.GetKeyDown(throwButton))
                {
                    throwing = true;
                }

                if (throwing
                    && (horizontalInput != 0 || verticalInput != 0)) //Optimize to change position only when it changes
                {
                    endPos = startPos;
                    calculateVelocity();
                    _velocity = (endPos - startPos);
                        Vector2[] trajectory = Plot(rbToThrow, transform.position, _velocity);

                        lr.positionCount = trajectory.Length;

                        //Sets positions from plot into linerenderer
                        Vector3[] positions = new Vector3[trajectory.Length];
                        for (int i = 0; i < positions.Length; i++)
                        {
                            positions[i] = trajectory[i];
                        }
                        lr.SetPositions(positions);
                }

                if (Input.GetKeyUp(throwButton))
                {
                    entityThrowing.GetComponent<Animator>().SetBool("pickUp", false);
                    rbToThrow.velocity = _velocity;
                    resetLr();
                }
            }
        }

        private Vector2[] Plot(Rigidbody2D rigibody, Vector2 pos, Vector2 velocity) //Plots course until point touches ground
        {
            List<Vector2> results = new List<Vector2>();

            float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
            Vector2 gravityAccel = Physics2D.gravity * rigibody.gravityScale * timestep * timestep;

            float drag = 1f - timestep * rigibody.drag;
            Vector2 moveStep = velocity * timestep;

            for (int i = 0; true; i++)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results.Add(pos);
                if (Physics2D.BoxCast(pos,new Vector2(0.1f,0.1f),0,Vector2.down).collider.tag == "Ground")
                {
                    break;
                } 
            }

            return results.ToArray();
        }

        private void calculateVelocity()
        {
            defineAngle();
            defineStrength();
        }

        private void defineAngle()
        {
            //horizontalInput
            angle = Mathf.Min(Mathf.Max(angle + horizontalInput, minAngle),maxAngle);

            //flip when angle is less than zero
            Vector3 originalScale = entityThrowing.transform.localScale;
            originalScale.x = Mathf.Abs(originalScale.x);
            if (angle > 0 && originalScale.x > 0) //If front and flipped
            {
                originalScale.x = -Mathf.Abs(originalScale.x); //Flip back
            }
            else {
                originalScale.x = Mathf.Abs(originalScale.x); // Stay
            }
            entityThrowing.transform.localScale = originalScale;

            endPos.x += angle;
        }

        private void defineStrength()
        {
            //verticalInput
            power = Mathf.Min(Mathf.Max(power + verticalInput, minPower), maxPower);
            endPos.y = (maxAngle - Mathf.Abs(angle)) / maxAngle + power;
        }

        //Resets Linerenderer so it may be used once more
        private void resetLr()
        {
            lr.positionCount = 0;
            throwing = false;
            if (rbToThrow.gameObject != null && rbToThrow.gameObject.tag == "Carryable")
            {
                rbToThrow.gameObject.transform.parent = null;
                rbToThrow.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
            rbToThrow = null; //remove object
        }
    }

}
