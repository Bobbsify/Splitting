using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectAlterationEvent : MonoBehaviour, CutsceneEvent
{
    [Header("General Settings")]
    [SerializeField] private GameObject nextEvent;
    [SerializeField] private int nextEventDelay;
    
    [SerializeField] private UnityEvent alterations;

    private CutsceneEvent nextEventCutsceneEvent;

    public void Execute()
    {
        nextEvent.TryGetComponent(out nextEventCutsceneEvent);
        alterations.Invoke();
        if (nextEventDelay > 0)
        {
            delayNextEvent();
        }
        else
        {
            nextEventCutsceneEvent.Execute();
        }
    }

    private async void delayNextEvent()
    {
        await Task.Delay(nextEventDelay);
        nextEventCutsceneEvent.Execute();
    }
}
