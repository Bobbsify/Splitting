using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Splitting { 
    public class AwakeBehaviour : MonoBehaviour
    {
        [Header("Awaking Settings")]
        [SerializeField] private Turn actionType;
        [SerializeField] private ActivationTypes activationType;
        [SerializeField] private MonoBehaviour[] scriptsToLoad;

        [Header("Scoring Settings")]
        [SerializeField] private Platform scoringPlatform;
        [SerializeField] private int targetScore;

        [Header("Other Actions")]
        [SerializeField] UnityEvent subsequentEvents;

        private Collider2D objCollider;
        private KeyCode buttonToPress;
        private bool isPlayerHere = false;

        private void Start()
        {
            buttonToPress = new InputSettings().InteractButton;

            switch (activationType) {
                case ActivationTypes.onEnter:
                    objCollider = gameObject.GetComponent<Collider2D>();
                    break;
                case ActivationTypes.onClick:
                    break;
                case ActivationTypes.score:
                    break;
                case ActivationTypes.onTurnOn:
                    break;
                case ActivationTypes.enterAndClick:
                    objCollider = gameObject.GetComponent<Collider2D>();
                    break;
                default:
                    throw new Exception("Error, unknown activation Type for " + gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (activationType)
            {
                case ActivationTypes.onEnter:
                    if (isPlayerHere) {
                        AwakeScripts();
                    }
                    break;

                case ActivationTypes.onClick:
                    if (Input.GetKey(buttonToPress))
                    {
                        AwakeScripts();
                    }
                    break;

                case ActivationTypes.enterAndClick:
                    if (isPlayerHere && Input.GetKey(buttonToPress))
                    {
                        AwakeScripts();
                    }
                    break;

                case ActivationTypes.score:
                    if (scoringPlatform.score >= targetScore)
                    {
                        AwakeScripts();
                    }
                    break;

                case ActivationTypes.onTurnOn:
                    AwakeScripts();
                    break;

                default:
                    Debug.Log("Error, unknown activation Type for " + gameObject);
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (activationType == ActivationTypes.onEnter || activationType == ActivationTypes.enterAndClick)
            {
                if (!col.name.Contains("Check"))
                { 
                    if (col.gameObject.tag == "Player")
                    {
                        isPlayerHere = true;
                    }
                    else
                    {
                        isPlayerHere = false;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (activationType == ActivationTypes.onEnter || activationType == ActivationTypes.enterAndClick && col.gameObject.tag == "Player")
            {
                isPlayerHere = false;
            }
        }

        private void AwakeScripts()
        {
            bool action = actionType == Turn.on;
            foreach (MonoBehaviour script in scriptsToLoad)
            {
                script.enabled = actionType == Turn.toggle ? !script.enabled : action;
            }
            DoEvents();
            this.enabled = false;
        }

        private void DoEvents()
        {
            subsequentEvents.Invoke();
        }
    }

    enum ActivationTypes
    {
        onEnter,
        onClick,
        enterAndClick,
        score,
        onTurnOn
    }

    enum Turn
    {
        on,
        off,
        toggle
    }
}