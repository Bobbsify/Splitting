using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float xTo;
        [SerializeField] private float yTo;

        [Header("Shaking Settings")]
        public float shakeLenght = 0.0f;
        public float shakeMagnitude = 0.0f;
        public float shakeRemain = 0.0f;
                
        public bool boundsX;
        public bool checkColX;
        public bool boundsY;
        public bool checkColY;

        private KeyCode swapButton;
        private GameObject cameraBounds;

        [SerializeField] private float timerAbleBounds = 1.0f;
        [SerializeField] private float elapsedFromSwap;

        public List<ContactPoint2D> contactsX = new List<ContactPoint2D>();
        public List<ContactPoint2D> contactsY = new List<ContactPoint2D>();

        private GameObject player;

        private Camera cameraComponent;
        private BoxCollider2D cameraCollider;

        [SerializeField] public Vector3 offset = new Vector3(0, 8, 0);

        // Start is called before the first frame update
        void Awake()
        {
            player = findPlayer();

            swapButton = new InputSettings().SwitchCharacterButton;

            cameraComponent = gameObject.GetComponent<Camera>();
            cameraCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            FixCameraColliderSize();

            DisableCameraBounds();

            if (player != null)
            {
                if (player.tag != "Player") //Switching
                {
                    player = findPlayer();                                        
                }

                ControlBoundsXContacts();
                ControlBoundsYContacts();

                CheckCameraBoundsX();
                CheckCameraBoundsY();
                CheckCameraBoundsXY();

                                
                if (contactsX.Count == 0 && contactsY.Count == 0)
                {
                    xTo = player.transform.position.x;
                    yTo = player.transform.position.y;
                }              
                                

                // Screen shake 
                if (shakeLenght > 0)
                {
                    transform.position = new Vector3(transform.position.x + Random.Range(-shakeRemain, shakeRemain), transform.position.y + Random.Range(-shakeRemain, shakeRemain), transform.position.z);
                    shakeRemain = Mathf.Max(0, shakeRemain - ((1 / shakeLenght) * shakeMagnitude * Time.deltaTime));
                }
            }
            else //Object could be destroyed
            {
                player = findPlayer();
            }

            // Update position
            transform.position = new Vector3(transform.position.x + ((xTo - transform.position.x)), transform.position.y + ((yTo - transform.position.y)), transform.position.z) + offset;
        }

        private GameObject findPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }

        void FixCameraColliderSize()
        {
            cameraCollider.size = new Vector2((((cameraComponent.orthographicSize * 2) * 16) / 9), cameraComponent.orthographicSize * 2);
        }

        void DisableCameraBounds()
        {
            if (Input.GetKeyDown(swapButton))
            {
                cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");

                cameraBounds.transform.Find("XBounds").gameObject.SetActive(false);
                cameraBounds.transform.Find("YBounds").gameObject.SetActive(false);
            }

            if (Input.GetKeyUp(swapButton))
            {
                cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");

                cameraBounds.transform.Find("XBounds").gameObject.SetActive(true);
                cameraBounds.transform.Find("YBounds").gameObject.SetActive(true);
            }
            
        }       

        void ControlBoundsXContacts()
        {
            if (boundsX && checkColX)
            {
                checkColX = false;
                cameraCollider.GetContacts(contactsX);

                Debug.Log("Xcollision " + contactsX.Count);

                var copyContactsX = new List<ContactPoint2D>(contactsX);

                for (int i = 0; i < copyContactsX.Count; i++)
                {
                    if (copyContactsX.ToArray()[i].collider.name.ToUpper().Contains("Y"))
                    {
                        Debug.Log("Remove 1 from contactsY");
                        contactsX.Remove(copyContactsX.ToArray()[i]);
                    }
                }

                copyContactsX.Clear();

                Debug.Log("New Xcollision " + contactsX.Count);

            }
            else if (!boundsX)
            {
                contactsX.Clear();
            }
        }

        void ControlBoundsYContacts()
        {
            if (boundsY && checkColY)
            {
                checkColY = false;
                cameraCollider.GetContacts(contactsY);

                Debug.Log("Ycollision " + contactsY.Count);

                var copyContactsY = new List<ContactPoint2D>(contactsY);

                for (int i = 0; i < copyContactsY.Count; i++)
                {
                    if (copyContactsY.ToArray()[i].collider.name.ToUpper().Contains("X"))
                    {
                        Debug.Log("Remove 1 from contactsY");
                        contactsY.Remove(copyContactsY.ToArray()[i]);
                    }
                }

                copyContactsY.Clear();

                Debug.Log("New Ycollision " + contactsY.Count);

            }
            else if (!boundsY)
            {
                contactsY.Clear();
            }
        }

        

        void CheckCameraBoundsX()
        {
            if (boundsX && !boundsY && contactsX.Count > 0)
            {
                if (contactsX.ToArray()[0].point.x < xTo)
                {
                    if (player.transform.position.x > xTo)
                    {
                        xTo = player.transform.position.x;
                    }

                }
                else if (contactsX.ToArray()[0].point.x > xTo)
                {
                    if (player.transform.position.x < xTo)
                    {
                        xTo = player.transform.position.x;
                    }
                }

                yTo = player.transform.position.y;
            }
        }

        

        void CheckCameraBoundsY()
        {
            if (boundsY && !boundsX && contactsY.Count > 0)
            {
                if (contactsY.ToArray()[0].point.y < yTo)
                {
                    if (player.transform.position.y > yTo)
                    {
                        yTo = player.transform.position.y;
                    }
                }
                else if (contactsY.ToArray()[0].point.y > yTo)
                {
                    if (player.transform.position.y < yTo)
                    {
                        yTo = player.transform.position.y;
                    }
                }

                xTo = player.transform.position.x;
            }
        }

        

        void CheckCameraBoundsXY()
        {
            if (boundsX && boundsY)
            {
                if ((contactsY.ToArray()[0].point.y < yTo && contactsX.ToArray()[0].point.x < xTo)) // check 1
                {
                    if (player.transform.position.y > yTo && player.transform.position.x > xTo)
                    {
                        xTo = player.transform.position.x;
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y > yTo && player.transform.position.x < xTo)
                    {
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y < yTo && player.transform.position.x > xTo)
                    {
                        xTo = player.transform.position.x;
                    }
                }
                else if ((contactsY.ToArray()[0].point.y < yTo && contactsX.ToArray()[0].point.x > xTo)) // check 2
                {
                    if (player.transform.position.y > yTo && player.transform.position.x < xTo)
                    {
                        xTo = player.transform.position.x;
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y > yTo && player.transform.position.x > xTo)
                    {
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y < yTo && player.transform.position.x < xTo)
                    {
                        xTo = player.transform.position.x;
                    }
                }
                else if ((contactsY.ToArray()[0].point.y > yTo && contactsX.ToArray()[0].point.x > xTo)) // check 3
                {
                    if (player.transform.position.y < yTo && player.transform.position.x < xTo)
                    {
                        xTo = player.transform.position.x;
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y < yTo && player.transform.position.x > xTo)
                    {
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y > yTo && player.transform.position.x < xTo)
                    {
                        xTo = player.transform.position.x;
                    }
                }
                else if ((contactsY.ToArray()[0].point.y > yTo && contactsX.ToArray()[0].point.x < xTo)) // check 4
                {   
                    
                    if (player.transform.position.y < yTo && player.transform.position.x > xTo)
                    {                        
                        xTo = player.transform.position.x;
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y < yTo && player.transform.position.x < xTo)
                    {
                        yTo = player.transform.position.y;
                    }
                    else if (player.transform.position.y > yTo && player.transform.position.x > xTo)
                    {                        
                        xTo = player.transform.position.x;
                    }                   
                                        
                }
            }
        }
        

    }
}
