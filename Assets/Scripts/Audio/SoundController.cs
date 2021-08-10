using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
    public class SoundController : MonoBehaviour
    {
        [SerializeField] bool alwaysPlaying = false;
        [SerializeField] bool directional = true;
        [SerializeField] private SoundTypes soundType;

        private AudioSource audioToPlay;
        private Collider2D objCollider; //Player collider

        private float maxVolume; // settings Master Volume - Sound Volume

        private float maxY = 1;
        private float maxX = 1;

        // Start is called before the first frame update
        void OnEnable()
        {
            audioToPlay = GetComponent<AudioSource>();
            if (!alwaysPlaying)
            {
                Collider2D col = GetComponent<Collider2D>();
                maxX = col.bounds.extents.x;
                maxY = col.bounds.extents.y;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (directional && objCollider != null) {
                    audioToPlay.volume = ProcessVolume(objCollider.transform.position.x, objCollider.transform.position.y);
                    audioToPlay.panStereo = DetectStereoPan(objCollider.transform.position.x);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            { 
                objCollider = collision;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            { 
                objCollider = null;
            }
        }

        //Detects how far away an object should be sterepoanned
        private float DetectStereoPan(float incomingX)
        {
            float currX = transform.position.x;

            int direction = (currX > incomingX) ? -1 : 1; //comes from left or right
            float distanceX = Mathf.Abs(currX - incomingX); //detect distance X

            return (distanceX / maxX * direction);
        }

        private float ProcessVolume(float incomingX, float incomingY)
        {
            float currX = transform.position.x;
            float currY = transform.position.y;

            float distanceX = Mathf.Abs(currX - incomingX); //detect distance X
            float distanceY = Mathf.Abs(currY - incomingY); //detect distance X


            return 1 - Mathf.Sqrt(Mathf.Pow(distanceX / maxX, 2) + Mathf.Pow(distanceY / maxY, 2));
        }

        private enum SoundTypes
        {
            music,
            soundEffect
        }
    }


}