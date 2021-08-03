using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{

    public class CameraController : MonoBehaviour
    {
        private float xTo;
        private float yTo;

        [Header("Shaking Settings")]
        public float shakeLenght = 0.0f;
        public float shakeMagnitude = 0.0f;
        public float shakeRemain = 0.0f;

        private GameObject player;

        [SerializeField] private Vector3 offset = new Vector3(0, 8, 0);

        // Start is called before the first frame update
        void Awake()
        {
            player = findPlayer();
            xTo = transform.position.x;
            yTo = transform.position.y;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                shakeMagnitude = 4.0f;
                shakeRemain = 4.0f;
                shakeLenght = 12.0f;
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (player != null)
            {
                if (player.tag != "Player")
                {
                    player = findPlayer();
                }

                xTo = player.transform.position.x;
                yTo = player.transform.position.y;

                // Update position
                transform.position = new Vector3(transform.position.x + ((xTo - transform.position.x)), transform.position.y + ((yTo - transform.position.y)), transform.position.z) + offset;

                // Screen shake 
                if (shakeLenght > 0)
                {
                    transform.position = new Vector3(transform.position.x + Random.Range(-shakeRemain, shakeRemain), transform.position.y + Random.Range(-shakeRemain, shakeRemain), transform.position.z);
                    shakeRemain = Mathf.Max(0, shakeRemain - ((1 / shakeLenght) * shakeMagnitude * Time.deltaTime));
                }
            }
            

        }

        private GameObject findPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
    }
}
