using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires Trigger Collider
namespace Splitting { 
    public class CheckpointController : MonoBehaviour
    {
        public LastCheckpointInfo lastCheckpoint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") {
                lastCheckpoint.levelCheckpoint++;
                this.enabled = false; //Checkpoints may only trigger once
            }
        }
    }
}
