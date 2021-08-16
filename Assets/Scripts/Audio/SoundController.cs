using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Splitting { 
    public class SoundController : MonoBehaviour
    {
        [SerializeField] bool alwaysPlaying = false;
        [SerializeField] bool directional = true;
        [SerializeField] private SoundTypes soundType;
        
        public AudioMixer mixer;

        private AudioSource audioToPlay;
        private Collider2D objCollider; //Player collider

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
                maxY = col.bounds.extents.y * 2; //Total Height
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
            if (!directional && !alwaysPlaying) //Se il suono non è direzionale, una volta uscito dall'area il volume deve partire;
            {
                audioToPlay.volume = 1;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            { 
                objCollider = null;
            }
            if (!directional && !alwaysPlaying) //Se il suono non è direzionale, una volta uscito dall'area il volume deve azzerarsi;
            {
                audioToPlay.volume = 0;
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
            Vector2 curr = transform.position;

            float distanceX = Mathf.Abs(curr.x - incomingX); //detect distance X
            float distanceY = Mathf.Abs(curr.y - incomingY); //detect distance Y
            
            return 1 - Mathf.Sqrt(Mathf.Pow(distanceX / maxX, 2) + Mathf.Pow(distanceY / maxY, 2));
        }

        private void OnDrawGizmos()
        {
            if (objCollider != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(objCollider.transform.position, transform.position);
            }
        }

        private enum SoundTypes
        {
            music,
            soundEffect
        }
    }


}