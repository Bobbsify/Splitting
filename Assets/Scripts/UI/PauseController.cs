using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Splitting { 
public class PauseController : MonoBehaviour
    {
        [SerializeField]
        private GameObject GUI;

        private KeyCode pauseButton;
        private bool pause = false;

        private void Start()
        {
            pauseButton = new InputSettings().PauseButton;
        }

        private void Update()
        {
            if (Input.GetKeyUp(pauseButton))
            {
                pause = !pause;
                Time.timeScale = pause ? 0 : 1;
                ActivateGui();
            }
        }

        public void TaskOnClick()
        {
            pause = !pause;
            Time.timeScale = pause ? 0 : 1;
            ActivateGui();
        }

        private void ActivateGui()
        {
            GUI.SetActive(pause);
            foreach (Transform obj in GUI.transform)
            {
                if (obj.name.ToLower() == "main" || obj.name.ToLower() == "logo")
                {
                    obj.gameObject.SetActive(true);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}