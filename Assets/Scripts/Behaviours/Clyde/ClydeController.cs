using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeController : MonoBehaviour
{
    [SerializeField] private float readyToShotPosition = 0.0f;
    [SerializeField] private float approachSpeed = 1.0f;

    public CapsuleCollider2D waterGunColl;
    [SerializeField] private float waterGunSpeed = 1.0f;

    private WaterGunController waterGunController;

    [SerializeField] private Vector2 realSize = new Vector2(0, 0);

    public bool startApproach;
    public bool startShot;

    // Start is called before the first frame update
    void Start()
    {
        waterGunColl = gameObject.GetComponentInChildren<CapsuleCollider2D>();
        waterGunController = gameObject.GetComponentInChildren<WaterGunController>();
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
                    realSize = waterGunColl.size;
                    startApproach = false;
                    startShot = true;
                }
            }
            else if (transform.position.x < readyToShotPosition)
            {
                transform.position = new Vector2(transform.position.x + (Time.deltaTime * approachSpeed), transform.position.y);

                if (transform.position.x >= readyToShotPosition)
                {
                    realSize = waterGunColl.size;
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

            waterGunColl.size = new Vector2(waterGunColl.size.x + (waterGunSpeed * Time.deltaTime), waterGunColl.size.y);
           
            if (waterGunController.collisionCheck)
            {
                waterGunColl.size = realSize;
                waterGunController.collisionCheck = false;
                startShot = false;
            }
        }
    }

    
}
