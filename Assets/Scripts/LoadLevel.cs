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
    private GameObject sceneLoadingInfo;

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
        sceneLoadingInfo = new GameObject();
        sceneLoadingInfo.AddComponent<SceneToLoad>();
    }

    private IEnumerator loadLevel(bool goToTransition) {
        yield return new WaitForSeconds(3.0f);
        if (goToTransition)
        {
            sceneLoadingInfo.GetComponent<SceneToLoad>().value = (int)sceneToLoad;
            DontDestroyOnLoad(sceneLoadingInfo);
        }
        else
        {
            SceneManager.LoadScene(gameScenes.ToArray()[(int)sceneToLoad]);
        }
        //TODO implement transition
    }
}

/*
1: Main Menu
2: Livello 0
3: Livello 1
4: Livello 2
5: Livello 3
6: Loading Screen
*/

public enum InGameScenes
{
    MainMenu = 0,
    Level0 = 1,
    Level1 = 2,
    Level2 = 3,
    Level3 = 4,
    LoadingScreen = 5
}

public class SceneToLoad : MonoBehaviour
{
    public int value;
}