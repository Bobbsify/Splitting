using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Splitting { 
    public class CameraEvent : MonoBehaviour, CutsceneEvent
    {
        [Header("General Settings")]
        [Tooltip("only needed if this is the last object of the cutscene")]
        [SerializeField] private GameObject originalCutsceneController;
        [SerializeField] private GameObject nextEvent;
        [SerializeField] private int nextEventDelay;

        [Header("Camera Event")]
        [SerializeField] private CameraTransitionController transition;
        
        private CutsceneEvent nextEventCutsceneEvent;

        public void Execute()
        {
            nextEvent.TryGetComponent(out nextEventCutsceneEvent);
            transition.StartZoomOut();
            transition.StartOffset();
            waitForCompletion();
        }

        private async void waitForCompletion()
        {
            while (transition.doOffset || transition.doZoom) { 
                await Task.Delay(1);
            }
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
                originalCutsceneController.GetComponent<CutsceneController>().isInCutscene = false;
            }
        }
    }
}