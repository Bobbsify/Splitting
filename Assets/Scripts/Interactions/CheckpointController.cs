using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires Trigger Collider
namespace Splitting { 
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private LastCheckpointInfo lastCheckpointInfo;
        [SerializeField] private List<GameObject> objectsToSave;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            lastCheckpointInfo.savedObjects = objectsToSave;
            this.enabled = false; //Checkpoints may only trigger once
        }
    }
}
