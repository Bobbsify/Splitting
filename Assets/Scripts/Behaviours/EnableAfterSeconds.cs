using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterSeconds : MonoBehaviour
{
    [SerializeField] private List<GameObject> objsToEnable;
    [SerializeField] private float delayInSeconds = 2.0f;

    private void Awake()
    {
        StartCoroutine(doEnabling());
    }

    private IEnumerator doEnabling()
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        foreach (GameObject obj in objsToEnable)
        {
            obj.SetActive(true);
        }
    }

    public void startAction()
    {
        StartCoroutine(doEnabling());
    }
}
