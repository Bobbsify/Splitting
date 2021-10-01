using Splitting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    [SerializeField] private GameObject firstEvent;
    [SerializeField] private bool removePlayerControl = true;

    [HideInInspector] public GameObject player;
    private bool isInCutscene = false;

    public void startCutscene()
    {
        if (removePlayerControl) { 
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<StateControllerInterface>().DisableControl();
        }
        isInCutscene = true;
        firstEvent.GetComponent<CutsceneEvent>().Execute();
    }

    public void turnOffCutscene()
    {
        isInCutscene = false;
        if (removePlayerControl)
        {
            player.GetComponent<StateControllerInterface>().EnableControl();
        }
    }
}
