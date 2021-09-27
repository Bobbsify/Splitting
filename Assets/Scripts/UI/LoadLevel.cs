using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Splitting
{ 
    public class LoadLevel : MonoBehaviour
    {
        public LastCheckpointInfo lastCheckpoint;

        [SerializeField] private InGameScenes sceneToLoad;
        [SerializeField] private bool transition;
        private List<string> gameScenes = new List<string>();
        private LevelLoadingInfo lvlInfoObj;

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
            lastCheckpoint.levelCheckpoint = 0;
            if (goToTransition)
            {
                lvlInfoObj.levelInfo.levelToLoad = (int)sceneToLoad;
                SceneManager.LoadScene(gameScenes.ToArray()[(int)InGameScenes.LoadingScreen]);
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
    2: Livello 5
    3: Loading Screen
    4: Win Scene
    5: Tutorial Scene
    */

    public enum InGameScenes
    {
        MainMenu = 0,
        Level5 = 1,
        LoadingScreen = 2,
        WinScene = 3,
        TutorialScene = 4
    }
}