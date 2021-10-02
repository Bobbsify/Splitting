using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeController : MonoBehaviour
{
    [SerializeField] private float readyToShotPosition = 0.0f;
    [SerializeField] private float approachSpeed = 1.0f;

    public GameObject waterGunProj;
    private Transform projSpawner;

    [SerializeField] private Vector3 offsetY = new Vector3(0, -1, 0); 

    private WaterGunController waterGunController;    

    public bool startApproach;
    private bool boom;
    public bool startShot;

    // Start is called before the first frame update
    void Start()
    { 
        projSpawner = transform.Find("bone_1/bone_2/bone_3");
    }

    // Update is called once per frame
    void Update()
    {
        ClydeApproach();

        ClydeWaterGun();
    }

    public void ClydeApproach()
    {
        if (startApproach)
        {
            if (transform.position.x > readyToShotPosition)
            {
                transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed * -1), transform.position.y);

                if (transform.position.x <= readyToShotPosition)
                {                    
                    startApproach = false;
                    startShot = true;
                }
            }
            else if (transform.position.x < readyToShotPosition)
            {
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
                GameObject water =  Instantiate(waterGunProj);

                water.transform.position = projSpawner.position + offsetY;                

                Quaternion rotation = water.transform.rotation;
                Vector3 rotationEulers = new Vector3(0, 0, (projSpawner.eulerAngles.z + 90));
                rotation.eulerAngles = rotationEulers;
                water.transform.rotation = rotation;

                waterGunController = water.GetComponentInChildren<WaterGunController>();
            }            
            
            if (waterGunController.collisionCheck)
            {                
                waterGunController.collisionCheck = false;
                startShot = false;
                boom = false;
            }
        }
    }

    
}
