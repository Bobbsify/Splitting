using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float xTo;
        [SerializeField] private float yTo;
        [SerializeField] private bool blockYTo;

        [Header("Shaking Settings")]
        public float shakeLenght = 1.0f;
        public float shakeMagnitude = 1.0f;
        public float shakeRemain = 1.0f;

        private float boundsOffsetX = 0.5f;
        private float boundsOffsetY = 1.5f;

        public float antCameraSize;
        public Vector3 antCameraOffset = new Vector3(0, 8, 0);
        public float tyrCameraSize;
        public Vector3 tyrCameraOffset = new Vector3(0, 8, 0);

        public bool boundsX;
        public bool checkColX;
        private bool pushRight;
        private bool checkPushRight;
        private bool pushLeft;
        private bool checkPushLeft;

        public bool boundsY;
        public bool checkColY;
        private bool pushUp;
        private bool checkPushUp;
        private bool pushDown;
        private bool checkPushDown;

        public bool exeShake;

        private KeyCode swapButton;
        private GameObject cameraBounds;        

        private List<ContactPoint2D> contactsX = new List<ContactPoint2D>();
        private List<ContactPoint2D> contactsY = new List<ContactPoint2D>();

        private GameObject player;

        private Camera cameraComponent;
        private BoxCollider2D cameraCollider;
        public BoxCollider2D offsetCollider;

        [SerializeField] public Vector3 offset = new Vector3(0, 8, 0);        

        public bool updateCamOffset;

        // Start is called before the first frame update
        void Awake()
        {
            player = findPlayer();

            swapButton = new InputSettings().SwitchCharacterButton;

            cameraComponent = gameObject.GetComponent<Camera>();
            cameraCollider = gameObject.GetComponent<BoxCollider2D>();
            offsetCollider = GameObject.Find("OffsetCol").GetComponent<BoxCollider2D>();
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

                UpdateCameraSize();

                ControlBoundsXContacts();                             
                ControlBoundsYContacts();

                CheckCameraBoundsX();
                CheckCameraBoundsY();
                CheckCameraBoundsXY();                                          

                if (boundsX)
                {
                    PushOutFromBoundsX();
                }
                else
                {
                    pushRight = false;
                    pushLeft = false;
                }

                if (boundsY)
                {
                    PushOutFromBoundsY();
                }
                else
                {
                    pushUp = false;
                    pushDown = false;
                }
                
                           
                if (contactsX.Count == 0 && contactsY.Count == 0 && !pushRight && !pushLeft)
                {                    
                    xTo = player.transform.position.x;                    
                }                
                
                if (contactsX.Count == 0 && contactsY.Count == 0 && !pushUp && !pushDown && !blockYTo)
                {                    
                    yTo = player.transform.position.y;                   
                }              
                                
            }
            else //Object could be destroyed
            {
                player = findPlayer();
            }

            if (exeShake)
            {
                ScreenShake();
            }
            else
            {               
                if (!pushRight && !pushLeft && !pushUp && !pushDown)
                {
                    transform.position = new Vector3(transform.position.x + ((xTo - transform.position.x)), transform.position.y + ((yTo - transform.position.y)), transform.position.z) + offset;
                }
            }
                                             
           
        }

        public void ScreenShake()
        {
            if (shakeLenght > 0)
            {                
                transform.position = new Vector3((transform.position.x + (xTo - transform.position.x)) + Random.Range(-shakeRemain, shakeRemain), (transform.position.y + (yTo - transform.position.y)) + Random.Range(-shakeRemain, shakeRemain), transform.position.z) + offset;
                shakeRemain = Mathf.Max(0, shakeRemain - ((1 / shakeLenght) * shakeMagnitude * Time.deltaTime));                               

                if (shakeRemain == 0.0f)
                {
                    exeShake = false;
                }
            }          

        }

        private GameObject findPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }

        void FixCameraColliderSize()
        {
            cameraCollider.size = new Vector2((((cameraComponent.orthographicSize * 2) * 16) / 9), cameraComponent.orthographicSize * 2);
        }

        void UpdateCameraSize()
        {
            bool ant = false;
            bool tyr = false;

            ant = player.name.ToUpper().Contains("ANT");
            tyr = player.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {
                cameraComponent.orthographicSize = antCameraSize;
                offset = antCameraOffset;
            }
            else if (!ant && tyr)
            {
                cameraComponent.orthographicSize = tyrCameraSize;
                offset = tyrCameraOffset;
            }
            else if (ant && tyr)
            {
                cameraComponent.orthographicSize = antCameraSize;
                offset = tyrCameraOffset;
            }
        }

        void DisableCameraBounds()
        {            

            if (Input.GetKeyUp(swapButton))
            {
                cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");

                cameraBounds.transform.Find("XBounds").gameObject.SetActive(false);
                cameraBounds.transform.Find("YBounds").gameObject.SetActive(false);

                cameraBounds.transform.Find("XBounds").gameObject.SetActive(true);
                cameraBounds.transform.Find("YBounds").gameObject.SetActive(true);
            }
            
        }       

        void ControlBoundsXContacts()
        {
            if (boundsX && (checkColX))
            {                
                checkColX = false;
                cameraCollider.GetContacts(contactsX);                

                var copyContactsX = new List<ContactPoint2D>(contactsX);

                for (int i = 0; i < copyContactsX.Count; i++)
                {
                    if (copyContactsX.ToArray()[i].collider.name.ToUpper().Contains("Y"))
                    {                        
                        contactsX.Remove(copyContactsX.ToArray()[i]);
                    }                    
                }

                copyContactsX.Clear();              

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

                var copyContactsY = new List<ContactPoint2D>(contactsY);

                for (int i = 0; i < copyContactsY.Count; i++)
                {
                    if (copyContactsY.ToArray()[i].collider.name.ToUpper().Contains("X"))
                    {                        
                        contactsY.Remove(copyContactsY.ToArray()[i]);
                    }

                }

                copyContactsY.Clear();              

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


        
        void PushOutFromBoundsX()
        {
            if (contactsX.Count > 0)
            {
                if ((contactsX.ToArray()[0].point.x <= (contactsX.ToArray()[0].collider.bounds.center.x + contactsX.ToArray()[0].collider.bounds.extents.x - boundsOffsetX)) && (contactsX.ToArray()[0].point.x >= (contactsX.ToArray()[0].collider.bounds.center.x - contactsX.ToArray()[0].collider.bounds.extents.x + boundsOffsetX)))
                {                    
                    if (contactsX.ToArray()[0].point.x < cameraCollider.bounds.center.x)
                    {
                        pushRight = true;
                        checkPushRight = false;
                    }
                    else
                    {
                        pushLeft = true;
                        checkPushLeft = false;
                    }
                }
                else
                {
                    pushRight = false;
                    pushLeft = false;
                }
            }
            else
            {
                cameraCollider.GetContacts(contactsX);

                var copyContactsX = new List<ContactPoint2D>(contactsX);

                for (int i = 0; i < copyContactsX.Count; i++)
                {
                    if (copyContactsX.ToArray()[i].collider.name.ToUpper().Contains("Y"))
                    {
                        contactsX.Remove(copyContactsX.ToArray()[i]);
                    }
                }

                copyContactsX.Clear();
            }          

            if (pushRight && !checkPushRight)
            {
                float distanceToPush = ((contactsX.ToArray()[0].collider.bounds.center.x) + contactsX.ToArray()[0].collider.bounds.extents.x) - (contactsX.ToArray()[0].point.x);                              

                transform.position  = new Vector3(transform.position.x + (distanceToPush * 2), transform.position.y, transform.position.z);
                xTo = transform.position.x;

                checkPushRight = true;

                contactsX.Clear();
            }
            else if (pushLeft && !checkPushLeft)
            {
                float distanceToPush = (contactsX.ToArray()[0].point.x) - ((contactsX.ToArray()[0].collider.bounds.center.x) - contactsX.ToArray()[0].collider.bounds.extents.x);

                transform.position = new Vector3(transform.position.x - (distanceToPush * 2), transform.position.y, transform.position.z);
                xTo = transform.position.x;

                checkPushLeft = true;

                contactsX.Clear();
            }
            
        }
        
        void PushOutFromBoundsY()
        {
            if (contactsY.Count > 0)
            {               

                if ((contactsY.ToArray()[0].point.y <= (contactsY.ToArray()[0].collider.bounds.center.y + contactsY.ToArray()[0].collider.bounds.extents.y - boundsOffsetY)) && (contactsY.ToArray()[0].point.y >= (contactsY.ToArray()[0].collider.bounds.center.y - contactsY.ToArray()[0].collider.bounds.extents.y + boundsOffsetY)))
                {                 

                    if (contactsY.ToArray()[0].point.y < cameraCollider.bounds.center.y)
                    {
                        pushUp = true;
                        checkPushUp = false;                        
                    }
                    else
                    {                        
                        pushDown = true;
                        checkPushDown = false;
                    }
                }
                else
                {
                    pushUp = false;
                    pushDown = false;
                }
            }
            else
            {
                cameraCollider.GetContacts(contactsY);

                var copyContactsY = new List<ContactPoint2D>(contactsY);

                for (int i = 0; i < copyContactsY.Count; i++)
                {
                    if (copyContactsY.ToArray()[i].collider.name.ToUpper().Contains("X"))
                    {
                        contactsY.Remove(copyContactsY.ToArray()[i]);
                    }

                }

                copyContactsY.Clear();
            }

            if (pushUp && !checkPushUp)
            {
                float distanceToPush = ((contactsY.ToArray()[0].collider.bounds.center.y) + contactsY.ToArray()[0].collider.bounds.extents.y) - (contactsY.ToArray()[0].point.y);

                transform.position = new Vector3(transform.position.x, transform.position.y + (distanceToPush), transform.position.z);
                yTo = transform.position.y;              

                checkPushUp = true;              

                contactsY.Clear();
            }
            else if (pushDown && !checkPushDown)
            {              
                
                float distanceToPush = (contactsY.ToArray()[0].point.y) - ((contactsY.ToArray()[0].collider.bounds.center.y) - contactsY.ToArray()[0].collider.bounds.extents.y);

                transform.position = new Vector3(transform.position.x, transform.position.y - (distanceToPush), transform.position.z);
                yTo = transform.position.y;

                checkPushDown = true;

                contactsY.Clear();                               
            }
        }
        
        public void DisableYTo()
        {
            blockYTo = true;
        }

        public void EnableYTo()
        {
            blockYTo = false;
        }
          
        public void EnableExeShake()
        {
            exeShake = true;
        }

    }
}
