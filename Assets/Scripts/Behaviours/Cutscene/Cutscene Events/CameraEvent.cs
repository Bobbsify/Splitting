using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Splitting { 
    public class CameraEvent : MonoBehaviour, CutsceneEvent
    {
        [Header("General Settings")]
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
            StartCoroutine(waitForCompletion());
        }

        private IEnumerator waitForCompletion()
        {
            while (transition.doOffset || transition.doZoom)
            {
                //wait
            }
            if (nextEventDelay > 0)
            {
                delayNextEvent();
            }
            else
            {
                nextEventCutsceneEvent.Execute();
            }
            yield return 0;
        }

        private async void delayNextEvent()
        {
            await Task.Delay(nextEventDelay);
            nextEventCutsceneEvent.Execute();
        }
    }
}