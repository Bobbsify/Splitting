using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectAlterationEvent : MonoBehaviour, CutsceneEvent
{
    [Header("General Settings")]
    [Tooltip("only needed if this is the last object of the cutscene")]
    [SerializeField] private CutsceneController originalCutsceneController;
    [SerializeField] private GameObject nextEvent;
    [SerializeField] private int nextEventDelay;
    
    [SerializeField] private UnityEvent alterations;

    private CutsceneEvent nextEventCutsceneEvent;

    public void Execute()
    {
        GetEvent();
        alterations.Invoke();
        DoNextEvent();
    }

    private async void delayNextEvent()
    {
        await Task.Delay(nextEventDelay);
        nextEventCutsceneEvent.Execute();
    }

    private void DoNextEvent()
    {
        if (nextEvent != null)
        {
            if (nextEventDelay > 0)
            {
                delayNextEvent();
            }
            else
            {
                nextEventCutsceneEvent.Execute();
            }
        }
        else
        {
            originalCutsceneController.GetComponent<CutsceneController>().turnOffCutscene();
        }
    }

    private void GetEvent()
    {
        if (nextEvent != null)
        {
            nextEvent.TryGetComponent(out nextEventCutsceneEvent);
        }
    }
}
