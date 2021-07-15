using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : MonoBehaviour
{
    [Header("Area Condition Order:  0)Exit - 1)Enter - 2)Stay")]
    public GameObject[] areaCondition;

    private bool inArea;
    private bool outArea;

    [Header("\n")]

    [SerializeField] private bool onStayAfterTimer;
    [SerializeField] private float timerTriggerStay = 0.0f;

    [Header("\n")]

    [SerializeField] private bool offStayWhenExit;

    [Header("\n")]

    [SerializeField] private float elapsedFromEnter = 0.0f;
    [SerializeField] private bool resetElapsedWhenExit;

    [SerializeField] private bool offEnterWhenStay;
    [SerializeField] private float timerOffEnter = 0.0f;

    [Header("\n")]

    [SerializeField] private float elapsedFromExit = 0.0f;

    [SerializeField] private bool offExitAfterTimer;
    [SerializeField] private float timerOffExit = 0.0f;

    [Header("\n")]

    [SerializeField] private bool triggerExitOnlyOnce;
    private bool alreadyExit;

    [SerializeField] private bool triggerEnterOnlyOnce;
    private bool alreadyEnter;

    [SerializeField] private bool triggerStayOnlyOnce;
    private bool alreadyStay;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (inArea)
        {
            elapsedFromEnter += Time.deltaTime;
        }
        else
        {
            if (!offEnterWhenStay || !areaCondition[1].activeSelf)
            {
                elapsedFromEnter = 0;
            }
        }

        if (offEnterWhenStay)
        {
            if (elapsedFromEnter >= timerOffEnter)
            {
                areaCondition[1].SetActive(false);
            }
        }

        if (outArea && areaCondition[0].activeSelf && offExitAfterTimer)
        {
            elapsedFromExit += Time.deltaTime;            
        }
        else
        {
            elapsedFromExit = 0;
        }

        if (outArea && offStayWhenExit)
        {
            areaCondition[2].SetActive(false);  
        }
        
        if (offExitAfterTimer)
        {
            if (elapsedFromExit >= timerOffExit)
            {
                areaCondition[0].SetActive(false);                
            }
        }

        if (elapsedFromEnter >= timerTriggerStay && inArea && onStayAfterTimer && !alreadyStay)
        {
            areaCondition[2].SetActive(true);

            if (triggerStayOnlyOnce)
            {
                alreadyStay = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inArea = false;
        outArea = true;

        if (collision.gameObject.tag == "Player" && !alreadyExit)
        {
            areaCondition[0].SetActive(true);

            if (triggerExitOnlyOnce)
            {
                alreadyExit = true;
            }
        }

        if (resetElapsedWhenExit)
        {
            elapsedFromEnter = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inArea = true;
        outArea = false;

        if (collision.gameObject.tag == "Player" && !alreadyEnter)
        {
            areaCondition[1].SetActive(true);

            if (triggerEnterOnlyOnce)
            {
                alreadyEnter = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {     

        if (collision.gameObject.tag == "Player" && !onStayAfterTimer && !alreadyStay)
        {            
            areaCondition[2].SetActive(true);

            if (triggerStayOnlyOnce)
            {
                alreadyStay = true;
            }
        }       
    }
}
