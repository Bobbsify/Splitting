using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class CameraTransitionController : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField] private Camera gameCamera;
        [SerializeField] private float targetSize = 30;
        [SerializeField] private float zoomSpeed = 1;

        [Header("Offset")]
        [SerializeField] private Vector3 targetOffset;
        [SerializeField] private Vector3 offsetSpeeds = new Vector3(1, 1, 1);

        [HideInInspector] public bool doZoom = false;
        [HideInInspector] public bool doOffset = false;

        private float modifier = 1;
        private bool reachedX = false, reachedY = false, reachedZ = false;
        private bool tyr;
        private bool ant;

        private void Awake()
        {
            if (gameCamera == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent(out gameCamera);
                if (gameCamera == null)
                {
                    throw new System.Exception("Unable to find camera, assign MainCamera tag to camera or attach the camera object to the script");
                }
            }
        }

        private void Start()
        {
            if (gameCamera == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent(out gameCamera);
                if (gameCamera == null)
                {
                    throw new System.Exception("Unable to find camera, assign MainCamera tag to camera or attach the camera object to the script");
                }
            }
        }

        private void Update()
        {
                if (doZoom)
                {
                if (ReachedZoomCheck())
                {
                    doZoom = false;
                }
                else
                { 
                    Zoom();
                }
                if (modifier == 1)
                    {
                        if (gameCamera.orthographicSize >= targetSize) doZoom = false;
                    }
                    else
                    {
                        if (gameCamera.orthographicSize <= targetSize) doZoom = false;
                    }
                }
                if (doOffset)
                {
                    if (ReachedOffsetCheck())
                    {
                        doOffset = false;
                        reachedX = false;
                        reachedY = false;
                        reachedZ = false;
                    }
                    else
                    { 
                        Offset();
                    }
                }
        }

        public void StopAll()
        {
            StopZoomOut();
            StopOffset();
        }

        public void StopZoomOut()
        {
            doZoom = false;
        }

        public void StopOffset()
        {
            doOffset = false;
        }

        public void StartAll()
        {
            StartZoomOut();
            StartOffset();
        }

        public void StartOffset()
        {
            doOffset = true;
        }

        public void StartZoomOut()
        {
            modifier = targetSize > gameCamera.orthographicSize ? 1 : -1;
            doZoom = true;
        }

        private void Zoom()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            tyr = player.name.ToLower().Contains("tyr");
            ant = player.name.ToLower().Contains("ant");
            float zoomAmount = modifier * zoomSpeed * Time.deltaTime;
            CameraController ctrl = gameCamera.GetComponent<CameraController>();
            if (tyr && ant)
            {
                ctrl.tyrCameraSize = ctrl.tyrCameraSize + zoomAmount;
                ctrl.antCameraSize = ctrl.antCameraSize + zoomAmount;
            }
            else if (tyr)
            {
                ctrl.tyrCameraSize = ctrl.tyrCameraSize + zoomAmount;
            }
            else
            {
                ctrl.antCameraSize = ctrl.antCameraSize + zoomAmount;
            }
        }

        private void Offset()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            tyr = player.name.ToLower().Contains("tyr");
            ant = player.name.ToLower().Contains("ant");
            CameraController cameraCtrl;
            gameCamera.gameObject.TryGetComponent(out cameraCtrl);

            float xMod, yMod, zMod;
            if (ant && tyr)
            {
                xMod = cameraCtrl.tyrCameraOffset.x > targetOffset.x ? -1 : 1;
                yMod = cameraCtrl.tyrCameraOffset.y > targetOffset.y ? -1 : 1;
                zMod = cameraCtrl.tyrCameraOffset.z > targetOffset.z ? -1 : 1;

                if (!reachedX)
                {
                    if (xMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.x += cameraCtrl.tyrCameraOffset.x >= targetOffset.x ? cameraCtrl.tyrCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        cameraCtrl.antCameraOffset.x = cameraCtrl.tyrCameraOffset.x;
                        if (cameraCtrl.tyrCameraOffset.x >= targetOffset.x || cameraCtrl.antCameraOffset.x >= targetOffset.x) reachedX = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.x += cameraCtrl.tyrCameraOffset.x <= targetOffset.x ? cameraCtrl.tyrCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        cameraCtrl.antCameraOffset.x = cameraCtrl.tyrCameraOffset.x;
                        if (cameraCtrl.tyrCameraOffset.x <= targetOffset.x || cameraCtrl.antCameraOffset.x <= targetOffset.x) reachedX = true;
                    }
                }
                if (!reachedY)
                {
                    if (yMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.y += cameraCtrl.tyrCameraOffset.y >= targetOffset.y ? cameraCtrl.tyrCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        cameraCtrl.antCameraOffset.y = cameraCtrl.tyrCameraOffset.y;
                        if (cameraCtrl.tyrCameraOffset.y >= targetOffset.y || cameraCtrl.antCameraOffset.y >= targetOffset.y) reachedY = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.y += cameraCtrl.tyrCameraOffset.y <= targetOffset.y ? cameraCtrl.tyrCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        cameraCtrl.antCameraOffset.y = cameraCtrl.tyrCameraOffset.y;
                        if (cameraCtrl.tyrCameraOffset.y <= targetOffset.y || cameraCtrl.antCameraOffset.y <= targetOffset.y) reachedY = true;
                    }
                }
                if (!reachedZ)
                {
                    if (zMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.z += cameraCtrl.tyrCameraOffset.z >= targetOffset.z ? cameraCtrl.tyrCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        cameraCtrl.antCameraOffset.z = cameraCtrl.tyrCameraOffset.z;
                        if (cameraCtrl.tyrCameraOffset.z >= targetOffset.z || cameraCtrl.antCameraOffset.z >= targetOffset.z) reachedZ = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.z += cameraCtrl.tyrCameraOffset.z <= targetOffset.z ? cameraCtrl.tyrCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        cameraCtrl.antCameraOffset.z = cameraCtrl.tyrCameraOffset.z;
                        if (cameraCtrl.tyrCameraOffset.z <= targetOffset.z || cameraCtrl.antCameraOffset.z <= targetOffset.z) reachedZ = true;
                    }
                }
                if (reachedX && reachedY && reachedZ)
                {
                    doOffset = false;
                    reachedZ = false; reachedX = false; reachedY = false;
                }
            }
            else if (tyr)
            {
                xMod = cameraCtrl.tyrCameraOffset.x > targetOffset.x ? -1 : 1;
                yMod = cameraCtrl.tyrCameraOffset.y > targetOffset.y ? -1 : 1;
                zMod = cameraCtrl.tyrCameraOffset.z > targetOffset.z ? -1 : 1;

                if (!reachedX)
                {
                    if (xMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.x += cameraCtrl.tyrCameraOffset.x >= targetOffset.x ? cameraCtrl.tyrCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.x >= targetOffset.x) reachedX = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.x += cameraCtrl.tyrCameraOffset.x <= targetOffset.x ? cameraCtrl.tyrCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.x <= targetOffset.x) reachedX = true;
                    }
                }
                if (!reachedY)
                {
                    if (yMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.y += cameraCtrl.tyrCameraOffset.y >= targetOffset.y ? cameraCtrl.tyrCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.y >= targetOffset.y) reachedY = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.y += cameraCtrl.tyrCameraOffset.y <= targetOffset.y ? cameraCtrl.tyrCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.y <= targetOffset.y) reachedY = true;
                    }
                }
                if (!reachedZ)
                {
                    if (zMod == 1)
                    {
                        cameraCtrl.tyrCameraOffset.z += cameraCtrl.tyrCameraOffset.z >= targetOffset.z ? cameraCtrl.tyrCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.z >= targetOffset.z) reachedZ = true;
                    }
                    else
                    {
                        cameraCtrl.tyrCameraOffset.z += cameraCtrl.tyrCameraOffset.z <= targetOffset.z ? cameraCtrl.tyrCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        if (cameraCtrl.tyrCameraOffset.z <= targetOffset.z) reachedZ = true;
                    }
                }
                if (reachedX && reachedY && reachedZ)
                {
                    doOffset = false;
                    reachedZ = false; reachedX = false; reachedY = false;
                }
            }
            else if( ant )
            { 
                xMod = cameraCtrl.antCameraOffset.x > targetOffset.x ? -1 : 1;
                yMod = cameraCtrl.antCameraOffset.y > targetOffset.y ? -1 : 1;
                zMod = cameraCtrl.antCameraOffset.z > targetOffset.z ? -1 : 1;

                if (!reachedX)
                { 
                    if (xMod == 1)
                    {
                        cameraCtrl.antCameraOffset.x += cameraCtrl.antCameraOffset.x >= targetOffset.x ? cameraCtrl.antCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.x >= targetOffset.x) reachedX = true;
                    }
                    else
                    {
                        cameraCtrl.antCameraOffset.x += cameraCtrl.antCameraOffset.x <= targetOffset.x ? cameraCtrl.antCameraOffset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.x <= targetOffset.x) reachedX = true;
                    }
                }

                if (!reachedY)
                { 
                    if (yMod == 1)
                    {
                        cameraCtrl.antCameraOffset.y += cameraCtrl.antCameraOffset.y >= targetOffset.y ? cameraCtrl.antCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.y >= targetOffset.y) reachedY = true;
                    }
                    else
                    {
                        cameraCtrl.antCameraOffset.y += cameraCtrl.antCameraOffset.y <= targetOffset.y ? cameraCtrl.antCameraOffset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.y <= targetOffset.y) reachedY = true;
                    }
                }

                if (!reachedZ)
                { 
                    if (zMod == 1)
                    {
                        cameraCtrl.antCameraOffset.z += cameraCtrl.antCameraOffset.z >= targetOffset.z ? cameraCtrl.antCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.z >= targetOffset.z) reachedZ = true;
                    }
                    else
                    {
                        cameraCtrl.antCameraOffset.z += cameraCtrl.antCameraOffset.z <= targetOffset.z ? cameraCtrl.antCameraOffset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                        if (cameraCtrl.antCameraOffset.z <= targetOffset.z) reachedZ = true;
                    }
                }

                if (reachedX && reachedY && reachedZ)
                {
                    doOffset = false;
                    reachedZ = false; reachedX = false; reachedY = false;
                }
            }
        }

        private bool ReachedOffsetCheck()
        {
            CameraController ctrl;
            if (gameCamera.TryGetComponent(out ctrl))
            {
                return CheckIfVectorsNear(ctrl.tyrCameraOffset, targetOffset, 0.5f);
            }
            return false;
        }

        private bool ReachedZoomCheck()
        {
            CameraController ctrl;
            if (gameCamera.TryGetComponent(out ctrl))
            {
                return isInRange(gameCamera.orthographicSize, targetSize, 0.5f);
            }
            return false;
        }

        private bool CheckIfVectorsNear(Vector3 vec1, Vector3 vec2, float range)
        {
            return isInRange(vec1.x, vec2.x, range) && isInRange(vec1.y, vec2.y, range) && isInRange(vec1.z, vec2.z, range);
        }

        private bool isInRange(float value1, float value2,float range)
        {
            return value1 >= value2 - range && value1 <= value2 + range;
        }
    }
}
