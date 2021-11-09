using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks            //Photon callback = called automatically when a certain event happens
{

    public TMP_InputField userNameInput;
    public TMP_Text buttonText;



    [Header("SO")]
    public PlayerSO playerSOData;

    [Header("Autoconnect")]
    public GameObject AutoConnectUI;

    public void Awake()
    {
        if (playerSOData.AutoConnect == true)
        {
            AutoConnectUI.SetActive(true);
            userNameInput.text = playerSOData.PlayerName;
            PhotonNetwork.NickName = userNameInput.text;            //Setting players username to Pun's 'Nickname'
            buttonText.text = "Connecting....";
            PhotonNetwork.AutomaticallySyncScene = true;            //Client scene can be changed by master 
            PhotonNetwork.ConnectUsingSettings();                   //Connect to Photon server

        }

        else
        {
            AutoConnectUI.SetActive(false);
        }
    }

    public void OnClickConnect()
    {
        if (userNameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = userNameInput.text;            //Setting players username to Pun's 'Nickname'
            buttonText.text = "Connecting....";
            PhotonNetwork.AutomaticallySyncScene = true;            //Client scene can be changed by master 
            PhotonNetwork.ConnectUsingSettings();                   //Connect to Photon server
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");                            //Copy to lobby scene. 
    }

 

}
