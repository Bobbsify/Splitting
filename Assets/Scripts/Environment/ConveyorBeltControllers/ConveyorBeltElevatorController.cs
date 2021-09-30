using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class ConveyorBeltElevatorController : MonoBehaviour
{
    [Header("Platform Generation Settings")]
    [SerializeField] private GameObject platformSpawned;
    [SerializeField] private Transform platformGenerationCoordinates;
    [Tooltip("In seconds")]
    [SerializeField] private float platformSpawnDelay = 5;
    [SerializeField] private bool stopGeneration;

    [Header("Platform Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform destinationCoordinates;

    [Header("Generated Platforms (Do not touch)")]
    [SerializeField] private List<GameObject> generatedPlatforms;

    private void Update()
    {
        if (generatedPlatforms.Count > 0) {
            try
            {
                foreach (GameObject platform in generatedPlatforms)
                {
                    try
                    {
                        if (platform.GetComponent<ConveyorBeltPlatformController>().move)
                        {
                            MoveY(platform);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("platform does not exist\n" + e);
                        generatedPlatforms.Remove(platform);
                    }
                }
            }
            catch (InvalidOperationException) { /*Suppress*/  }
        }
    }

    public void startGeneration()
    {
        StartCoroutine(generatePlatforms());
    }

    private void MoveY(GameObject platform)
    {
        if (platform.transform.position.y <= destinationCoordinates.position.y && movementSpeed > 0) //Platform going up check
        {
            platform.transform.position = new Vector2(platform.transform.position.x, platform.transform.position.y + movementSpeed * Time.deltaTime); //move Y   
        }
        else if (platform.transform.position.y >= destinationCoordinates.position.y && movementSpeed < 0) //Platform going down check
        {
            platform.transform.position = new Vector2(platform.transform.position.x, platform.transform.position.y + movementSpeed * Time.deltaTime); //move Y   
        }
        else
        {
            ConveyorBeltPlatformController ctrl;
            platform.TryGetComponent(out ctrl);
            ctrl.FadeOut(); //Fade in is done as soon as the platform is generated
            generatedPlatforms.Remove(platform);
        }
    }
    
    public void generateAPlatform()
    {
        GameObject platform = Instantiate(platformSpawned);
        platform.transform.parent = this.transform;
        platform.transform.position = platformGenerationCoordinates.position;
        generatedPlatforms.Add(platform);
    }

    private IEnumerator generatePlatforms()
    {
        generateAPlatform();
        yield return new WaitForSeconds(platformSpawnDelay);
        if (!stopGeneration)
        {
            StartCoroutine(generatePlatforms());
        }
    }
}
