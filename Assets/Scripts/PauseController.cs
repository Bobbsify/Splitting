using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Splitting { 
public class PauseController : MonoBehaviour
    {
        [SerializeField]
        private GameObject GUI;
        [SerializeField]
        private Button Button;

        private KeyCode pauseButton;
        private bool pause = false;

        private void Start()
        {
            pauseButton = new InputSettings().PauseButton;
            Button.onClick.AddListener(Update);
        }

        private void Update()
        {
            if (Input.GetKeyUp(pauseButton))
            {
                Time.timeScale = pause ? 1 : 0;
                pause = !pause;
                GUI.SetActive(pause);
            }
        }
    }
}