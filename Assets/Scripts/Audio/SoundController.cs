using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Splitting { 
    public class SoundController : MonoBehaviour
    {
        public AudioMixer mixer;

        [SerializeField] private SoundTypes soundType;
        [SerializeField] bool alwaysPlaying = false;
        [SerializeField] bool directional = true;
        [SerializeField] private float maxVolume = 1.0f;
        [SerializeField] private float minVolume = 0.0f;

        private AudioSource audioToPlay;
        private Collider2D objCollider; //Player collider

        private float maxY = 1;
        private float maxX = 1;
        private bool doFade = false;
        private float fadeOutSpeed = 0.1f;
        private float fadeMod = 1;

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
                    audioToPlay.volume = Mathf.Max(ProcessVolume(objCollider.transform.position.x, objCollider.transform.position.y),minVolume);
                    audioToPlay.panStereo = DetectStereoPan(objCollider.transform.position.x);
            }
            if (doFade)
            {
                objCollider = null;
                audioToPlay.volume += fadeOutSpeed * fadeMod * Time.deltaTime;
                if (fadeMod < 0 ? audioToPlay.volume <= minVolume : audioToPlay.volume >= maxVolume)
                {
                    doFade = false;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            { 
                objCollider = collision;
            }
            if (!directional && !alwaysPlaying) //Se il suono non ? direzionale, una volta uscito dall'area il volume deve partire;
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
            if (!directional && !alwaysPlaying) //Se il suono non ? direzionale, una volta uscito dall'area il volume deve azzerarsi;
            {
                audioToPlay.volume = 0;
            }
        }

        //Detects how far away an object should be sterepoanned
        private float DetectStereoPan(float incomingX)
        {
            float currX = transform.position.x;

            int direction = (currX < incomingX) ? -1 : 1; //comes from left or right
            float distanceX = Mathf.Abs(currX - incomingX); //detect distance X

            return (distanceX / maxX * direction);
        }

        private float ProcessVolume(float incomingX, float incomingY)
        {
            Vector2 curr = transform.position;

            float distanceX = Mathf.Abs(curr.x - incomingX); //detect distance X
            float distanceY = Mathf.Abs(curr.y - incomingY); //detect distance Y
            
            return maxVolume - Mathf.Sqrt(Mathf.Pow(distanceX / maxX, 2) + Mathf.Pow(distanceY / maxY, 2));
        }

        public void fadeOut()
        {
            fadeMod = -1;
            doFade = true;
        }
        public void fadeIn()
        {
            fadeMod = 1;
            doFade = true;
        }

        private void OnDrawGizmos()
        {
            if (objCollider != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(objCollider.transform.position, transform.position);
            }
        }

        public void setFadeoutSpeed(float speed)
        {
            fadeOutSpeed = speed;
        }

        private enum SoundTypes
        {
            music,
            soundEffect
        }
    }


}