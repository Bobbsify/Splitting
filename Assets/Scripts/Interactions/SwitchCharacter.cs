using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    [SerializeField] private KeyCode swapButton = KeyCode.Q; //Defaults to Q
    [SerializeField] private GameObject targetEntity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEntity != null)
        {
            if (Input.GetKeyUp(swapButton))
            {
                turnThisOff();
                turnOtherOn();
            }
        }
    }

    private void turnThisOff()
    {
        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        //TODO sort layer
        gameObject.tag = "Untagged";
    }

    private void turnOtherOn()
    {
        MonoBehaviour[] scripts = targetEntity.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        //TODO sort layer
        targetEntity.tag = "Player";

    }
}
