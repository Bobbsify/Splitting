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

        private bool tyr;
        private float modifier = 1;
        private bool reachedX = false, reachedY = false, reachedZ = false;

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

        private void Update()
        {
            if (doZoom)
            {
                Zoom();
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
                Offset();
            }
        }

        public void StopAll()
        {
            StopZoomOut();
            StopOffset();
        }

        public void StopZoomOut()
        {
            tyr = GameObject.FindGameObjectWithTag("Player").name.ToLower().Contains("tyr") ? true : false;
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
            float zoomAmount = modifier * zoomSpeed * Time.deltaTime;
            if (tyr)
            {
                gameCamera.GetComponent<CameraController>().tyrCameraSize = gameCamera.orthographicSize + zoomAmount;
            }
            else
            {
                gameCamera.GetComponent<CameraController>().antCameraSize = gameCamera.orthographicSize + zoomAmount;
            }
        }

        private void Offset()
        {
            CameraController cameraCtrl;
            gameCamera.gameObject.TryGetComponent(out cameraCtrl);

            float xMod, yMod, zMod;
            xMod = cameraCtrl.offset.x > targetOffset.x ? -1 : 1;
            yMod = cameraCtrl.offset.y > targetOffset.y ? -1 : 1;
            zMod = cameraCtrl.offset.z > targetOffset.z ? -1 : 1;

            if (!reachedX)
            { 
                if (xMod == 1)
                {
                    cameraCtrl.offset.x += cameraCtrl.offset.x >= targetOffset.x ? cameraCtrl.offset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                    if (cameraCtrl.offset.x >= targetOffset.x) reachedX = true;
                }
                else
                {
                cameraCtrl.offset.x += cameraCtrl.offset.x <= targetOffset.x ? cameraCtrl.offset.x : xMod * offsetSpeeds.x * Time.deltaTime;
                if (cameraCtrl.offset.x <= targetOffset.x) reachedX = true;
                }
            }
            if (!reachedY)
            { 
                if (yMod == 1)
                {
                    cameraCtrl.offset.y += cameraCtrl.offset.y >= targetOffset.y ? cameraCtrl.offset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                    if (cameraCtrl.offset.y >= targetOffset.y) reachedY = true;
                }
                else
                {
                    cameraCtrl.offset.y += cameraCtrl.offset.y <= targetOffset.y ? cameraCtrl.offset.y : yMod * offsetSpeeds.y * Time.deltaTime;
                    if (cameraCtrl.offset.y <= targetOffset.y) reachedY = true;
                }
            }
            if (!reachedZ)
            { 
                if (zMod == 1)
                {
                    cameraCtrl.offset.z += cameraCtrl.offset.z >= targetOffset.z ? cameraCtrl.offset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                    if (cameraCtrl.offset.z >= targetOffset.z) reachedZ = true;
                }
                else
                {
                    cameraCtrl.offset.z += cameraCtrl.offset.z >= targetOffset.z ? cameraCtrl.offset.z : zMod * offsetSpeeds.z * Time.deltaTime;
                    if (cameraCtrl.offset.z <= targetOffset.z) reachedZ = true;
                }
            }
            if (reachedX && reachedY && reachedZ)
            {
                doOffset = false;
                reachedZ = false; reachedX = false; reachedY = false;
            }
        }
    }
}
