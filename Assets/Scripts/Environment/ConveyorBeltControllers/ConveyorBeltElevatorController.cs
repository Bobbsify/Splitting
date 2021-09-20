using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConveyorBeltElevatorController : MonoBehaviour
{
    [Header("Platform Generation Settings")]
    [SerializeField] private GameObject platformSpawned;
    [SerializeField] private Vector3 platformGenerationCoordinates;

    [Header("Platform Movement Settings")]
    [SerializeField] private Vector2 movementDirectionSpeeds;
    [SerializeField] private Vector3 destinationCoordinates;

    [Header("Generated Platforms (Do not touch)")]
    [SerializeField] private List<GameObject> generatedPlatforms;

    public void generatePlatform()
    {
        GameObject platform = Instantiate(platformSpawned);
        platform.transform.position = platformGenerationCoordinates;
        generatedPlatforms.Add(platform);

        try
        {
            platform.GetComponent<ConveyorBeltPlatformController>().FadeIn();
        }
        catch (Exception e)
        {
            Debug.LogError("Platform does not have ConveyorBeltPlatformController \n" + e);
        }
    }

    public void Update()
    {
        foreach (GameObject platform in generatedPlatforms)
        {
            try
            {
                if (platform.GetComponent<ConveyorBeltPlatformController>().move)
                {
                    MoveX(platform);

                    MoveY(platform);

                }
            }
            catch (Exception e)
            {
                generatedPlatforms.Remove(platform);
            }
        }
    }

    private void MoveX(GameObject platform)
    {
        if (!(platform.transform.position.x >= destinationCoordinates.x && movementDirectionSpeeds.x > 0) || !(platform.transform.position.x <= destinationCoordinates.x && movementDirectionSpeeds.x < 0))
        {
            platform.transform.position = new Vector2(platform.transform.position.x + movementDirectionSpeeds.x, platform.transform.position.y); //move X
        }
        else
        {
            ConveyorBeltPlatformController ctrl;
            platform.TryGetComponent(out ctrl);
            if (!ctrl.reachedX)
            { 
                ctrl.reachedX = true;
                if (ctrl.reachedY)
                {
                    ctrl.FadeOut();
                }
            }
        }
    }

    private void MoveY(GameObject platform)
    {
        if (!(platform.transform.position.y >= destinationCoordinates.y && movementDirectionSpeeds.y > 0) || !(platform.transform.position.y <= destinationCoordinates.y && movementDirectionSpeeds.y < 0))
        {
            platform.transform.position = new Vector2(platform.transform.position.x, platform.transform.position.y + movementDirectionSpeeds.y); //move Y        
        }
        else
        {
            ConveyorBeltPlatformController ctrl;
            platform.TryGetComponent(out ctrl);
            ctrl.reachedY = true;
            if (!ctrl.reachedY)
            {
                if (ctrl.reachedX)
                { 
                    ctrl.FadeOut();
                }
            }
        }
    }
}
