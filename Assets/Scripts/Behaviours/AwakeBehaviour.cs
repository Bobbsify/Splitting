using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeBehaviour : MonoBehaviour
{
    [Header("Awaking Settings")]
    [SerializeField] private Turn actionType;
    [SerializeField] private ActivationTypes activationType;
    [SerializeField] private KeyCode buttonToPress; //Not compulsory
    [SerializeField] private MonoBehaviour[] scriptsToLoad;

    private bool isPlayerHere = false;
    private Collider2D objCollider;

    private void Start()
    {
        switch (activationType) {
            case ActivationTypes.onEnter:
                objCollider = gameObject.GetComponent<Collider2D>();
                break;
            case ActivationTypes.onClick:

                break;
            case ActivationTypes.enterAndClick:
                objCollider = gameObject.GetComponent<Collider2D>();
                break;
            default:
                Debug.Log("Error, unknown activation Type for " + gameObject);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (activationType)
        {
            case ActivationTypes.onEnter:
                if (isPlayerHere) {
                    awakeScripts();
                }
                break;
            case ActivationTypes.onClick:
                if (Input.GetKey(buttonToPress))
                {
                    awakeScripts();
                }
                break;
            case ActivationTypes.enterAndClick:
                if (isPlayerHere && Input.GetKey(buttonToPress))
                {
                    awakeScripts();
                }
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

    private void OnTriggerExit2D(Collider2D col)
    {
        if (activationType == ActivationTypes.onEnter || activationType == ActivationTypes.enterAndClick && col.gameObject.tag == "Player")
        {
            isPlayerHere = false;
        }
    }

    private void awakeScripts()
    {
        bool action = actionType == Turn.on;
        foreach (MonoBehaviour script in scriptsToLoad)
        {
            script.enabled = actionType == Turn.toggle ? !script.enabled : action;
        }
    }
}

enum ActivationTypes
{
    onEnter,
    onClick,
    enterAndClick
}

enum Turn
{
    on,
    off,
    toggle
}
