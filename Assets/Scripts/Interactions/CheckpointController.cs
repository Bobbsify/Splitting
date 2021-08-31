using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires Trigger Collider
namespace Splitting { 
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private List<GameObject> objectsToSave;

        private void Awake()
        {
            if (gameManager == null) { 
                GameObject.Find("GameManager").TryGetComponent(out gameManager); //Cerca di prendere in automatico il game Manager
                if (gameManager == null)
                {
                    Debug.LogError("Could not find Object with name 'Game Manager' in current scene and no gameManager has been provided to "+this);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") {
                foreach (GameObject obj in objectsToSave)
                {
                    GameManager.instance.checkpointSavedObjects.Add(obj);
                }
                this.enabled = false; //Checkpoints may only trigger once
            }
        }
    }
}
