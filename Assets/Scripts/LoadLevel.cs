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

    private void Start()
    {
        loadLevel(transition);
    }

    private void Awake()
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            gameScenes.Add(SceneUtility.GetScenePathByBuildIndex(i));

        }
    }

    private void loadLevel(bool goToTransition) {
        SceneManager.LoadScene(gameScenes.ToArray()[(int)sceneToLoad]);
        //TODO implement transition
    }
}

public enum InGameScenes
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2
}