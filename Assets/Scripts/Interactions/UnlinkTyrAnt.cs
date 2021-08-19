using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Splitting
{ 
    public class UnlinkTyrAnt : MonoBehaviour
    {
        [SerializeField] private GameObject tyrPrefab;
        [SerializeField] private GameObject antPrefab;

        [HideInInspector] public List<string> antScripts;
        [HideInInspector] public List<string> tyrScripts;
    
        private Animator animator;
        private KeyCode unlinkKey;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unlinkKey = new InputSettings().SwitchCharacterButton;
        }

        private void Update()
        {
            if (Input.GetKey(unlinkKey))
            {
                Disconnect();
            }   
        }

        private void Disconnect() //Start Disconnection animation
        {
            animator.SetTrigger("unlink");
        }

        public void Unlink()    //Animation should fire this event
        {
            Destroy(gameObject); //Destroy This
            
            GameObject ant = Instantiate(antPrefab); //Create Ant
            GameObject tyr = Instantiate(tyrPrefab); //Create Tyr

            ant.transform.position = transform.position;
            Vector2 lowerTyr = new Vector2(ant.transform.position.x,ant.transform.position.y - 1); //1 --> height difference
            tyr.transform.position = lowerTyr;

            ant.GetComponent<SwitchCharacter>().targetEntity = tyr;
            tyr.GetComponent<SwitchCharacter>().targetEntity = ant;
            
            //Toggle flashlight correctly
            tyr.transform.Find("bone_1/bone_14/Flashlight").GetComponent<FlashlightController>().SetLightsToState(transform.Find("bone_1/bone_2/bone_3/bone_13/Flashlight").GetComponent<FlashlightController>().lightsAre);

            SetupTyr(tyr);
            SetupAnt(ant);
 
        }

        private void SetupAnt(GameObject ant)
        {
            ant.GetComponent<AutobotUnity>().enabled = true;
            ant.GetComponent<AutobotUnity>().GetStateControllers();
            ant.GetComponent<SwitchCharacter>().enabled = true;
            ant.transform.localScale = ant.transform.localScale.z * transform.localScale;
        }

        private void SetupTyr(GameObject tyr)
        {
            tyr.GetComponent<AutobotUnity>().enabled = true;
            tyr.GetComponent<AutobotUnity>().GetStateControllers();
            tyr.transform.localScale = new Vector3(tyr.transform.localScale.x * transform.localScale.x, tyr.transform.localScale.y, 1);
        }
    }
}
