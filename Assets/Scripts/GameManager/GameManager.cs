using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> checkpointSavedObjects = new List<GameObject>();

    public GameObject[] conditions;

    public bool[] flag;
    private bool flagControl;
    private bool flagReset;

    private void Awake()
    {
        Debug.Log(instance);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            LoadInstance();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        flag = new bool[conditions.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (flagControl)
        {
            var i = 0;
            foreach (GameObject condition in conditions)
            {
                if (condition.activeSelf)
                {
                    flag[i] = true;
                }
                else
                {
                    flag[i] = false;
                }
                i = i + 1;
            }
            flagControl = false;
        }       

        if (flagReset)
        {
            for (int i = 0; i < flag.Length; i++)
            {
                flag[i] = false;
            }
            flagReset = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        flagControl = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flagReset = true;
    }

    private void LoadInstance() //Loads instance into the preferred variables
    {
        Debug.Log("Loading Instance " + instance);
        this.checkpointSavedObjects = instance.checkpointSavedObjects;
        LoadCheckpoint();
    }

    private void LoadCheckpoint()
    {
        foreach(GameObject obj in checkpointSavedObjects)
        {
            Debug.Log("Sub: " + obj.name);
            GameObject currSceneObj = GameObject.Find(obj.name);
            Debug.Log(currSceneObj);
            currSceneObj = obj;
        }
    }
}
