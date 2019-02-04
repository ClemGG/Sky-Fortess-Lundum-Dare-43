using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {


    public MovementSystem[] listOfPlayers;  //Rempli à la main dans l'inspector



    public static PlayerManager instance;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }






    // Use this for initialization
    void Start () {
        
        //listOfPlayers = FindObjectsOfType<MovementSystem>();
	}


    public void GainExpForPlayer(int ID)
    {
        for (int i = 0; i < listOfPlayers.Length; i++)
        {
            if(listOfPlayers[i].joueurID == ID)
            {
                listOfPlayers[i].currentWeapon.GainExp();
            }
        }
    }


    public void StopAllPlayersMovement()
    {
        for (int i = 0; i < listOfPlayers.Length; i++)
        {
            listOfPlayers[i].canMove = false;
            listOfPlayers[i].mouseClickInput = 0;
            listOfPlayers[i].mouseDownInput = false;
        }
    }
}
