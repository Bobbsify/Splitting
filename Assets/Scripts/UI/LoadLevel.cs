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
     Level 5.2 checkpoint 1 : 7
     Level 5.2 checkpoint 2 : 8
     Third Cutscene: 9
     Loading Screen: 10


    */

    public enum InGameScenes
    {
        MainMenu = 0,
        FirstCutscene = 1,
        Tutorial = 2,
        SecondCutscene = 3,
        Level5part1 = 4,
        Level5part1checkpoint1 = 5,
        Level5part2 = 6,
        Level5part2checkpoint1 = 7,
        Level5part2checkpoint2 = 8,
        ThirdCutscene = 9,
        LoadingScreen = 10
    }
}