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
        loadLevel(transition);
    }

    private void Awake()
    {
        Debug.Log("Start compiling game scenes\ntotal number of scenes: " + SceneManager.sceneCountInBuildSettings);

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            gameScenes.Add(SceneUtility.GetScenePathByBuildIndex(i));

        }
        foreach (string name in gameScenes)
        {
            Debug.Log(name);
        }
    }

    private void loadLevel(bool goToTransition) {
        SceneManager.LoadScene(gameScenes.ToArray()[(int)sceneToLoad]);
    }
}

public enum InGameScenes
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2
}