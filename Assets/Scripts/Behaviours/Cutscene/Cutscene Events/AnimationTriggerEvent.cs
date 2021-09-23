using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationTriggerEvent : MonoBehaviour, CutsceneEvent
{
    [Header("General Settings")]
    [Tooltip("only needed if this is the last object of the cutscene")]
    [SerializeField] private GameObject originalCutsceneController;
    [SerializeField] private GameObject nextEvent;
    [SerializeField] private int nextEventDelay;
    
    [Header("Animation Event")]
    [SerializeField] Animator targetAnimator;

    [Header("Animation Event Settings")]
    [SerializeField] private string[] animationVariableNames;

    [Tooltip("type a number with a dot if Float, true or false (Case insensitive) for bool or type trigger for triggers")]
    [SerializeField] private string[] animationVariableValues;

    private CutsceneEvent nextEventCutsceneEvent;

    public void Execute()
    {
        GetEvent();
        if (animationVariableNames.Length != animationVariableValues.Length)
        {
            throw new System.IndexOutOfRangeException("animation event arrays are of different size");
        }
        else
        {
            for (int i = 0; i < animationVariableValues.Length; i++)
            {
                string currVariable = animationVariableValues[i];
                if (currVariable.Contains(".")) //float timeline
                {
                    float varValue = parseFloat(currVariable);
                    targetAnimator.SetFloat(animationVariableNames[i], varValue);
                }
                else if (currVariable.ToLower() == "true" || currVariable.ToLower() == "false") //bool timeline
                {
                    bool varValue = parseBool(currVariable);
                    targetAnimator.SetBool(animationVariableNames[i],varValue);
                }
                else if (currVariable.ToLower() == "trigger")   //trigger timeline
                {
                    targetAnimator.SetTrigger(animationVariableNames[i]);
                }
                else    //int timeline
                {
                    int varValue = parseInt(currVariable);
                    targetAnimator.SetInteger(animationVariableNames[i], varValue);
                }
            }
        }
        DoNextEvent();
    }

    private int parseInt(string currVariable)
    {
        try
        {
            return int.Parse(currVariable);
        }
        catch (FormatException e)
        {
            throw new FormatException("variable value " + currVariable + " is unparsable\n" + e);
        }
    }

    private bool parseBool(string currVariable)
    {
        try
        {
            return bool.Parse(currVariable);
        }
        catch (FormatException e)
        {
            throw new FormatException("variable value " + currVariable + " is unparsable\n" + e);
        }
    }

    private float parseFloat(string currVariable)
    {
        try
        {
            return float.Parse(currVariable);
        }
        catch (FormatException e)
        {
            throw new FormatException("variable value " + currVariable + " is unparsable\n" + e);
        }
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

    private async void delayNextEvent()
    {
        await Task.Delay(nextEventDelay);
        nextEventCutsceneEvent.Execute();
    }

    private void GetEvent()
    {
        if (nextEvent != null)
        {
            nextEvent.TryGetComponent(out nextEventCutsceneEvent);
        }
    }
}
