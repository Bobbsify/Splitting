using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Should be enhanced with death transition & other stuff..
namespace Splitting { 
    public class Die : MonoBehaviour
    {
        [SerializeField] private LastCheckpointInfo lastCheckpointInfo;

        private void OnLevelWasLoaded(int level)
        {
            foreach (GameObject savedObject in lastCheckpointInfo.savedObjects)
            {
                GameObject temp = GameObject.Find(savedObject.name);
                temp = savedObject;
            }
        }

        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}