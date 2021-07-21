using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class HUDController : MonoBehaviour
    {
        public GameObject[] players;              

        public GameObject[] abilities = new GameObject[3];
        public GameObject[] currentAbilities = new GameObject[5];
                
        Move antMove;
        Jump antJump;
        Carry antCarry;

        private GameObject trajectPred;
        private Throw getThrow;

        [SerializeField] private int currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
            currentPlayer = ControlWhoIsPlayer();            

            antMove = players[0].GetComponent<Move>();
            antJump = players[0].GetComponent<Jump>();
            antCarry = players[0].GetComponent<Carry>();

            trajectPred = GameObject.Find("TrajectoryPrediction");
            getThrow = trajectPred.GetComponentInChildren<Throw>();

            // Get abilities
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = gameObject.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < currentAbilities.Length; i++)
            {
                currentAbilities[i] = abilities[currentPlayer].transform.Find("Ability" + (i + 1)).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {            

            if (players[currentPlayer].tag != "Player")
            {
                for (int i = 0; i < currentAbilities.Length; i++)
                {
                    currentAbilities[i] = null;
                }

                currentPlayer = ControlWhoIsPlayer();

                for (int i = 0; i < currentAbilities.Length; i++)
                {
                    currentAbilities[i] = abilities[currentPlayer].transform.Find("Ability" + (i + 1)).gameObject;
                }
            }
            
            // Ant
            if (currentPlayer == 0)
            {          

                if (antMove.isCrouched)
                {
                    currentAbilities[1].transform.Find("AbilityFrame2").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("Icon_Crouch").gameObject.SetActive(false);

                    currentAbilities[1].transform.Find("AbilityOnFrame2").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("OnIcon_Crouch").gameObject.SetActive(true);
                }
                else
                {
                    currentAbilities[1].transform.Find("AbilityFrame2").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("Icon_Crouch").gameObject.SetActive(true);

                    currentAbilities[1].transform.Find("AbilityOnFrame2").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("OnIcon_Crouch").gameObject.SetActive(false);
                }

                if (antJump.superJump)
                {
                    currentAbilities[2].transform.Find("AbilityFrame3").gameObject.SetActive(false);
                    currentAbilities[2].transform.Find("Icon_Chargedjump").gameObject.SetActive(false);

                    currentAbilities[2].transform.Find("AbilityOnFrame3").gameObject.SetActive(true);
                    currentAbilities[2].transform.Find("OnIcon_Chargedjump").gameObject.SetActive(true);
                }
                else if (antJump.isLanded)
                {
                    currentAbilities[2].transform.Find("AbilityFrame3").gameObject.SetActive(true);
                    currentAbilities[2].transform.Find("Icon_Chargedjump").gameObject.SetActive(true);

                    currentAbilities[2].transform.Find("AbilityOnFrame3").gameObject.SetActive(false);
                    currentAbilities[2].transform.Find("OnIcon_Chargedjump").gameObject.SetActive(false);
                }

                if (antCarry.isCarrying)
                {
                    currentAbilities[3].transform.Find("AbilityFrame4").gameObject.SetActive(false);
                    currentAbilities[3].transform.Find("Icon_Carry").gameObject.SetActive(false);

                    currentAbilities[3].transform.Find("AbilityOnFrame4").gameObject.SetActive(true);
                    currentAbilities[3].transform.Find("OnIcon_Carry").gameObject.SetActive(true);
                }
                else
                {
                    currentAbilities[3].transform.Find("AbilityFrame4").gameObject.SetActive(true);
                    currentAbilities[3].transform.Find("Icon_Carry").gameObject.SetActive(true);

                    currentAbilities[3].transform.Find("AbilityOnFrame4").gameObject.SetActive(false);
                    currentAbilities[3].transform.Find("OnIcon_Carry").gameObject.SetActive(false);
                }
            }

            // Tyr
            if (currentPlayer == 1)
            {
                if (getThrow.rbToThrow)
                {
                    currentAbilities[1].transform.Find("AbilityFrame1").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("Icon_Launch").gameObject.SetActive(false);

                    currentAbilities[1].transform.Find("AbilityOnFrame1").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("OnIcon_Launch").gameObject.SetActive(true);
                }
                else
                {
                    currentAbilities[1].transform.Find("AbilityFrame1").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("Icon_Launch").gameObject.SetActive(true);

                    currentAbilities[1].transform.Find("AbilityOnFrame1").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("OnIcon_Launch").gameObject.SetActive(false);
                }

            }
            
        }

        int ControlWhoIsPlayer()
        {
            int x = 0;

            for (int i = 0; i < players.Length; i++)
            {

                if (players[i].tag == "Player")
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    x = i;
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            return x;
        }        
    }    
}
