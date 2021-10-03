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
        public LevelLoadingInfo lvlInfoObj;

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

        public void loadLevel(bool goToTransition) {
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
            lastCheckpoint.levelCheckpoint = 0;
        }
    }

    /*
    Building:

     Main Menu: 0
     First Cutscene : 1
     Level 1 : 2
     Second Cutscene : 3
     Level 5.1 : 4
     Level 5 checkpoint 1 : 5
     Level 5.2 : 6
     ToBeContinued: 7
     Loading Screen: 8


    */

    public enum InGameScenes
    {
        MainMenu = 0,
        FirstCutscene = 1,
        Tutorial = 2,
        SecondCutscene = 3,
        Level5Part1 = 4,
        Level5Part1Checkpoint1 = 5,
        Level5Part2 = 6,
        ToBeContinued = 7,
        LoadingScreen = 8
    }
}