using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float loopDelayInSeconds;

    [Header("Randomization")]
    [SerializeField] private bool randomizeDelay;
    [SerializeField] private float randomizationLessenRange = -1;
    [SerializeField] private float randomizationIncreaseRange = 1;

    private AudioSource audioToLoop;
    private bool coroutineStarted = false;


    void OnEnable()
    {
        audioToLoop = GetComponent<AudioSource>();
        if (audioToLoop.loop)
        {
            audioToLoop.loop = false; //Il loop è gestito dalla script
        }
    }

    void Update()
    {
        if(!audioToLoop.isPlaying && !coroutineStarted)
        {
            StartCoroutine(AudioLoopCorutine());
            coroutineStarted = true;
        }
    }

    IEnumerator AudioLoopCorutine()
    {
        float loopDelay = Mathf.Max(loopDelayInSeconds + (randomizeDelay ? Random.Range(randomizationLessenRange, randomizationIncreaseRange) : 0),0);

        yield return new WaitForSeconds(loopDelay); // Wait

        audioToLoop.Play();

        StopCoroutine(AudioLoopCorutine()); //Stop couroutine when ended;
        coroutineStarted = false;
    }
}
