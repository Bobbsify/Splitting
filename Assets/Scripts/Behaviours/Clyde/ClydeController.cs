using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class ClydeController : MonoBehaviour
    {
        [SerializeField] private float readyToShotPosition = 0.0f;
        [SerializeField] private float approachSpeed = 1.0f;

        [SerializeField] private float changeDirDx = 326.0f;
        [SerializeField] private float changeDirSx = 403.0f;

        private bool checkDir;
        private bool previousDirection;
        private bool direction;
        private bool controlIfUpdateDir;

        public GameObject waterGunProj;
        private Transform projSpawner;

        [SerializeField] private Vector3 offset = new Vector3(-3, 0, 0);

        private WaterGunController waterGunController;

        private Animator animator;

        public bool startApproach;
        private bool boom;
        public bool startShot;

        // Start is called before the first frame update
        void Start()
        {
            projSpawner = transform.Find("bone_1/bone_2/bone_3");

            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            ClydeApproach();

            ClydeWaterGun();

            UpdateAnimation();

            CallAnimator(direction, startShot, previousDirection);

        }

        public void ClydeApproach()
        {
            if (startApproach)
            {
                if (transform.position.x > readyToShotPosition)
                {
                    if (direction && !checkDir)
                    {
                        previousDirection = true;
                    }
                    else if (!direction && !checkDir)
                    {
                        previousDirection = false;
                    }

                    checkDir = true;
                    direction = false;
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed * -1), transform.position.y);

                    if (transform.position.x <= readyToShotPosition)
                    {
                        startApproach = false;
                        startShot = true;
                    }
                }
                else if (transform.position.x < readyToShotPosition)
                {
                    if (direction && !checkDir)
                    {
                        previousDirection = true;
                    }
                    else if (!direction && !checkDir)
                    {
                        previousDirection = false;
                    }

                    checkDir = true;
                    direction = true;
                    transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed), transform.position.y);

                    if (transform.position.x >= readyToShotPosition)
                    {
                        startApproach = false;
                        startShot = true;
                    }
                }
            }
        }

        public void ClydeWaterGun()
        {
            if (startShot)
            {
                if (!boom)
                {
                    boom = true;
                    GameObject water = Instantiate(waterGunProj);

                    water.transform.parent = projSpawner;

                    water.transform.localPosition = projSpawner.transform.localPosition + offset;

                    Quaternion rotation = water.transform.rotation;
                    Vector3 rotationEulers = new Vector3(0, 0, (projSpawner.eulerAngles.z + 90));
                    rotation.eulerAngles = rotationEulers;
                    water.transform.rotation = rotation;

                    waterGunController = water.GetComponentInChildren<WaterGunController>();
                }

                if (waterGunController.collisionCheck)
                {
                    direction = previousDirection;
                    checkDir = false;

                    waterGunController.collisionCheck = false;
                    startShot = false;
                    boom = false;
                }
            }
            
        }

        private void CallAnimator(bool direction, bool startShot, bool previoudDir)
        {
            if (animator != null)
            {
                animator.SetBool("direction", direction);
                animator.SetBool("startShot", startShot);
                animator.SetBool("previousDirection", previoudDir);
            }
        }

        private void UpdateAnimation()
        {
            if (transform.position.x > changeDirSx && controlIfUpdateDir)
            {
                direction = false;
                controlIfUpdateDir = false;
            }
            else if (transform.position.x < changeDirDx && !controlIfUpdateDir)
            {
                direction = true;
                controlIfUpdateDir = true;
            }
        }


    }
}
