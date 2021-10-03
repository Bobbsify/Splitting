using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableAfterSeconds : MonoBehaviour
{
    [SerializeField] private List<GameObject> objsToEnable;
    [SerializeField] private float delayInSeconds = 2.0f;

    [SerializeField] private UnityEvent actions;

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

    private IEnumerator doActions()
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        actions.Invoke();
    }

    public void startActions()
    {
        StartCoroutine(doActions());
    }

    public void startEnabilng()
    {
        StartCoroutine(doEnabling());
    }

    public void doAll()
    {
        startEnabilng();
        startActions();
    }
}
