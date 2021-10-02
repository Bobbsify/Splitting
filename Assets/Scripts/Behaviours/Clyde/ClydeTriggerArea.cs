using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeTriggerArea : MonoBehaviour
{
    private GameObject clyde;
    private ClydeController clydeController;
    private Patrol clydePatrol;

    // Start is called before the first frame update
    void Start()
    {
        clyde = GameObject.FindGameObjectWithTag("Clyde");

        if (clyde != null)
        {
            clydeController = clyde.GetComponent<ClydeController>();
            clydePatrol = clyde.GetComponent<Patrol>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<StateControllerInterface>().DisableControl();

            clydePatrol.enabled = false;
            clydeController.startApproach = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<StateControllerInterface>().EnableControl();

            clydePatrol.enabled = true;
            clydeController.startApproach = false;
        }
    }
}
