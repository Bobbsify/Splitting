using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private InGameScenes sceneToLoad;
    [SerializeField] private bool transition;
    private List<string> gameScenes = new List<string>();

    private bool playerIsHere;

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            gameScenes.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }
    }

    private void Update()
    {
        if (playerIsHere && Input.GetKeyDown(KeyCode.E))
        { 
            loadLevel(transition);
        }
    }

    private void loadLevel(bool transition) {
        Debug.Log("Loading Scene: " + gameScenes[(int)sceneToLoad]);
        //SceneManager.LoadScene(gameScenes);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsHere = false;
        }
    }
}

public enum InGameScenes
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2
}