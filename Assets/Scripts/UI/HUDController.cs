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
         
        // ant
        Move antMove;
        Jump antJump;
        Carry antCarry;

        // tyr
        public GameObject tyrTrajectPred;
        public Throw tyrThrow;

        // tyrant
        public GameObject tyrantTrajectPred;
        public Throw tyrantThrow;

        [SerializeField] private int currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
            currentPlayer = ControlWhoIsPlayer();            

            // ant
            antMove = players[0].GetComponent<Move>();
            antJump = players[0].GetComponent<Jump>();
            antCarry = players[0].GetComponent<Carry>();

            // tyr
            tyrTrajectPred = players[1].transform.Find("TrajectoryPrediction").gameObject;
            tyrThrow = tyrTrajectPred.GetComponent<Throw>();

            // tyrant
            tyrantTrajectPred = players[2].transform.Find("TrajectoryPredictionTA").gameObject;
            tyrantThrow = tyrantTrajectPred.GetComponent<Throw>();

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
            
            if (players[currentPlayer].tag != "Player")
            {
                for (int i = 0; i < currentAbilities.Length; i++)
                {
                    currentAbilities[i] = null;
                }

                currentPlayer = ControlWhoIsPlayer();

                GetCurrentAbilities();
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
