using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public KeyCode swapButton;
    public GameObject targetEntity;

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
        gameObject.tag = "Untagged";
    }

    private void turnOtherOn()
    {
        MonoBehaviour[] scripts = targetEntity.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        targetEntity.tag = "Player";

    }
}
