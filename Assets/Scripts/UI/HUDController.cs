using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class HUDController : MonoBehaviour
    {
        public GameObject[] players;

        // Ant stuff
        public GameObject[] antAbilities = new GameObject[4];

        private KeyCode jumpButton;
        Move move;

        private int currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
            currentPlayer = ControlWhoIsPlayer();

            jumpButton = new InputSettings().JumpButton;

            move = players[0].GetComponent<Move>();

            for (int i = 0; i < antAbilities.Length; i++)
            {
                antAbilities[i] = players[0].transform.GetChild(2 + i).gameObject;
            }


        }

        // Update is called once per frame
        void Update()
        {            

            if (players[currentPlayer].tag != "Player")
            {
                currentPlayer = ControlWhoIsPlayer();
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
