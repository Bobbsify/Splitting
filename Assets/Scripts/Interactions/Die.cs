using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Building:

 Main Menu: 0
 First Cutscene : 1
 Level 1 : 2
 Second Cutscene : 3
 Level 5.1 : 4
 Level 5 checkpoint 1 : 5;
 Level 5.2 : 6
 Third Cutscene: 7
 Loading Screen: 8


*/

namespace Splitting { 
    public class Die : MonoBehaviour
    {
        public int sceneCheckpoint = 0;
        public LastCheckpointInfo checkpoint;

        public void ReloadLevel()
        {
            sceneCheckpoint = checkpoint.levelCheckpoint;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneCheckpoint);
        }
    }
}