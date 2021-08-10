using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopController : MonoBehaviour
{

    private AudioSource audioToLoop;

    [Header("Settings")]
    [SerializeField] private float loopDelayInSeconds;

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
        if(!audioToLoop.isPlaying)
        {
            StartCoroutine(AudioLoopCorutine());
        }
    }

    IEnumerator AudioLoopCorutine()
    {
        yield return new WaitForSeconds(loopDelayInSeconds); // Wait

        audioToLoop.Play();

        StopCoroutine(AudioLoopCorutine()); //Stop couroutine when ended;
    }
}
