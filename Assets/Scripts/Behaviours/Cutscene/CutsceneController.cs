using Splitting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public bool isInCutscene = false;

    [SerializeField] private GameObject firstEvent;

    public void startCutscene()
    {
        firstEvent.GetComponent<CutsceneEvent>().Execute();
    }
}
