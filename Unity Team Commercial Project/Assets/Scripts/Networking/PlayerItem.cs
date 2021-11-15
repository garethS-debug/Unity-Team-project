using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playername;

    public Image backgroundImage;

    public Color highlightColor;

    public GameObject leftArrowButton;

    public GameObject rightArrowButton;

    Player player;  //premade photon type, that describes different players


    [Header("PlayerDetails")]
    public ExitGames.Client.Photon.Hashtable playerproperties = new ExitGames.Client.Photon.Hashtable();   //Custom property - Hashtable (a list with a name instead of #)
    public Image playerAvatar;
    public Sprite[] avatars;

    [Header("Host Details")]
    public GameObject KickButton;

    [Header("SO")]
    public PlayerSO playerSOData;

    private void Start()
    {
        // backgroundImage = GetComponent<Image>();


            playerproperties["playerAvatar"] = 0; //ensures it exists
            playerAvatar.sprite = avatars[0]; //player avatar will get changed on all player controllers

        if (playerSOData.AutoConnect == true)
        {
            playerproperties["playerAvatar"] = playerSOData.PlayerCharacterChoise;
            print("PLayer ID = " + (int)playerproperties["playerAvatar"]);
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
        }

        print("Start ID = " + (int)playerproperties["playerAvatar"]);

            PhotonNetwork.SetPlayerCustomProperties(playerproperties);
            KickButton.gameObject.SetActive(false);

        //Kick Buttong
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) //looping through the current room players, retrieving the curret players in the room. It comes inthe form of a KVP (dictionary)
            {
                if (player.Value != PhotonNetwork.LocalPlayer) //checking if this is the local player in question
                {
                  //  KickButton.gameObject.SetActive(true);
                }

            }



       
        }

        else if (!PhotonNetwork.IsMasterClient)
        {
          //  KickButton.gameObject.SetActive(true);
        }
    }

    public void Setplayerinfo(Player _player)
    {
        playername.text = _player.NickName;
        player = _player;       //stored the photon player
        
   

        UpdatePlayerItem(player); //when joining room update player

    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = highlightColor;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }



    //called every time property is modified 
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer) //we only want to modify the image for the player who modified their avatar
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    private void UpdatePlayerItem(Player _player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))                                        //checking if player has a custom avatar 
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];                //player avatar will get changed on all player controllers
            playerproperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];            //keep player avater
        }

        else
        {
            playerproperties["playerAvatar"] = 0; //ensures it exists

        }
    }





    public void OnclickArrowLeft()
    {
        if ((int)playerproperties["playerAvatar"] == 0)
        {
            playerproperties["playerAvatar"] = avatars.Length - 1;
            print("PLayer ID = " + (int)playerproperties["playerAvatar"]);

        }
        else
        {
            playerproperties["playerAvatar"] = (int)playerproperties["playerAvatar"] - 1;
            print("PLayer ID = " + (int)playerproperties["playerAvatar"]);
        }

        PhotonNetwork.SetPlayerCustomProperties(playerproperties);
    }

    public void OnclickArrowRight()
    {
        if ((int)playerproperties["playerAvatar"] == avatars.Length - 1)
        {
            playerproperties["playerAvatar"] = 0;
            print("PLayer ID = " + (int)playerproperties["playerAvatar"]);
        }
        else
        {
            playerproperties["playerAvatar"] = (int)playerproperties["playerAvatar"] + 1;
            print("PLayer ID = " + (int)playerproperties["playerAvatar"]);
        }

        PhotonNetwork.SetPlayerCustomProperties(playerproperties); // notify players custom peroperties has been changed

    }

    public void OnClick_Kick()
    {

      //  PhotonNetwork.CloseConnection(player);
        //Remove player item card
        //refresh for everyone
    }
}
