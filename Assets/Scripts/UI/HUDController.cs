using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class HUDController : MonoBehaviour
    {
        private GameObject player;
                
        private GameObject[] abilities = new GameObject[3];
        private GameObject[] currentAbilities = new GameObject[5];
         
        // ant
        private Move antMove;
        private Jump antJump;
        private Carry antCarry;        

        // tyr
        private GameObject tyrTrajectPred;
        private Throw tyrThrow;
        
        // tyrant
        private GameObject tyrantTrajectPred;
        private Throw tyrantThrow;

        private AutobotUnity autobotUnity;

        [SerializeField] private int currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
            
            currentPlayer = ControlWhoIsPlayer();            

            GetComponentsFromCurrentPlayer();

            // Get abilities
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = gameObject.transform.GetChild(i).gameObject;
            }

            // Get current abilities
            GetCurrentAbilities();
                        
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
            {
                for (int i = 0; i < currentAbilities.Length; i++)
                {
                    currentAbilities[i] = null;
                }

                currentPlayer = ControlWhoIsPlayer();

                GetComponentsFromCurrentPlayer();

                GetCurrentAbilities();

            }
            else
            {
                if (player.tag != "Player")
                {
                    for (int i = 0; i < currentAbilities.Length; i++)
                    {
                        currentAbilities[i] = null;
                    }

                    currentPlayer = ControlWhoIsPlayer();

                    GetComponentsFromCurrentPlayer();

                    GetCurrentAbilities();
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
                if (tyrThrow.throwing == true)
                {
                    currentAbilities[0].transform.Find("AbilityFrame1").gameObject.SetActive(false);
                    currentAbilities[0].transform.Find("Icon_Launch").gameObject.SetActive(false);

                    currentAbilities[0].transform.Find("AbilityOnFrame1").gameObject.SetActive(true);
                    currentAbilities[0].transform.Find("OnIcon_Launch").gameObject.SetActive(true);
                }
                else
                {
                    currentAbilities[0].transform.Find("AbilityFrame1").gameObject.SetActive(true);
                    currentAbilities[0].transform.Find("Icon_Launch").gameObject.SetActive(true);

                    currentAbilities[0].transform.Find("AbilityOnFrame1").gameObject.SetActive(false);
                    currentAbilities[0].transform.Find("OnIcon_Launch").gameObject.SetActive(false);
                }

            }

            // Tyrant
            if (currentPlayer == 2)
            {
                if (tyrantThrow.rbToThrow == true && tyrantThrow.throwing == false)
                {
                    currentAbilities[1].transform.Find("AbilityFrame4").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("Icon_Carry").gameObject.SetActive(false);

                    currentAbilities[1].transform.Find("AbilityOnFrame4").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("OnIcon_Carry").gameObject.SetActive(true);
                }
                else if (tyrantThrow.throwing == true && tyrantThrow.rbToThrow == true)
                {
                    currentAbilities[1].transform.Find("AbilityFrame4").gameObject.SetActive(true);
                    currentAbilities[1].transform.Find("Icon_Carry").gameObject.SetActive(true);

                    currentAbilities[1].transform.Find("AbilityOnFrame4").gameObject.SetActive(false);
                    currentAbilities[1].transform.Find("OnIcon_Carry").gameObject.SetActive(false);

                    currentAbilities[2].transform.Find("AbilityFrame1").gameObject.SetActive(false);
                    currentAbilities[2].transform.Find("Icon_Launch").gameObject.SetActive(false);

                    currentAbilities[2].transform.Find("AbilityOnFrame1").gameObject.SetActive(true);
                    currentAbilities[2].transform.Find("OnIcon_Launch").gameObject.SetActive(true);
                }
                else if (tyrantThrow.throwing == false)
                {                 
                    currentAbilities[2].transform.Find("AbilityFrame1").gameObject.SetActive(true);
                    currentAbilities[2].transform.Find("Icon_Launch").gameObject.SetActive(true);

                    currentAbilities[2].transform.Find("AbilityOnFrame1").gameObject.SetActive(false);
                    currentAbilities[2].transform.Find("OnIcon_Launch").gameObject.SetActive(false);
                }
            }
            
        }

        int ControlWhoIsPlayer()
        {
            int x = 0;

            bool ant = false;
            bool tyr = false;
            
            player = GameObject.FindGameObjectWithTag("Player");

            ant = player.name.ToUpper().Contains("ANT");
            tyr = player.name.ToUpper().Contains("TYR");

            if (ant && !tyr)
            {
                x = 0; // ant
            }
            else if (tyr && !ant)
            {
                x = 1; // tyr
            }
            else if (ant && tyr)
            {                
                x = 2; // tyrant
            }

            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);

                if (i == x)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            return x;
        }

        void GetComponentsFromCurrentPlayer()
        {
            
            if (currentPlayer == 0) // ant
            {
                antMove = player.GetComponent<Move>();
                antJump = player.GetComponent<Jump>();
                antCarry = player.GetComponent<Carry>();

                autobotUnity = player.GetComponent<AutobotUnity>();
            }
            else if (currentPlayer == 1) // tyr
            {
                tyrTrajectPred = player.transform.Find("TrajectoryPrediction").gameObject;
                tyrThrow = tyrTrajectPred.GetComponent<Throw>();

                autobotUnity = player.GetComponent<AutobotUnity>();
            }
            else if (currentPlayer == 2) // tyrant
            {
                tyrantTrajectPred = player.transform.Find("TrajectoryPredictionTA").gameObject;
                tyrantThrow = tyrantTrajectPred.GetComponent<Throw>();
            }          
        }
        
        public void GetCurrentAbilities()
        {
            if (currentPlayer == 0)
            {
                for (int i = 0; i < (currentAbilities.Length - 1); i++)
                {
                    currentAbilities[i] = abilities[currentPlayer].transform.Find("Ability" + (i + 1)).gameObject;
                }
            }
            else if (currentPlayer == 1)
            {
                for (int i = 0; i < (currentAbilities.Length - 2); i++)
                {
                    currentAbilities[i] = abilities[currentPlayer].transform.Find("Ability" + (i + 1)).gameObject;
                }
            }
            else if (currentPlayer == 2)
            {
                for (int i = 0; i < (currentAbilities.Length); i++)
                {
                    currentAbilities[i] = abilities[currentPlayer].transform.Find("Ability" + (i + 1)).gameObject;
                }
            }

        }
    }    
}
