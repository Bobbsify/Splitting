using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Splitting { 
    public class ForceConnectEvent : MonoBehaviour, CutsceneEvent
    {
        [Header("General Settings")]
        [SerializeField] private CutsceneController originalCutsceneController;
        [SerializeField] private GameObject nextEvent;
        [SerializeField] private int nextEventDelay;

        [Header("When called forces player to connect into TyrAnt")]

        private CutsceneEvent nextEventCutsceneEvent;

        private GameObject target;

        private void Awake()
        {
            if (originalCutsceneController == null)
            {
                throw new System.MissingFieldException(this + " is missing original cutscene controller");
            }
        }

        public void Execute()
        {
            target = originalCutsceneController.GetComponent<CutsceneController>().player;
            nextEvent.TryGetComponent(out nextEventCutsceneEvent);

            if (!(target.name.ToLower().Contains("ant") && target.name.ToLower().Contains("tyr")))
            {
                if (target.TryGetComponent(out SwitchCharacter sw))
                { 
                    GameObject tyrant = sw.Connect();
                    originalCutsceneController.GetComponent<CutsceneController>().player = tyrant;
                    if (originalCutsceneController.TryGetComponent(out CutsceneController ctrl))
                    {
                        if (ctrl.removePlayerControl)
                        {
                            tyrant.GetComponent<StateControllerTA>().DisableControl();
                        }
                    }
                }
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
}