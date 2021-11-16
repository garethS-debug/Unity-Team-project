using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager Instance;

    private void Awake()
    {
        if (Instance)                                   //If room manager already in scene
        {
            Destroy(gameObject);                        //If yes destroy
            return;
        }

        DontDestroyOnLoad(gameObject);                      //I am the only 1


        Instance = this;
    }


    public void Start()
    {


        CreatePlayer();
    }

    //private void OnEnable()
    //{
    //    base.OnEnable();
    //    SceneManager.sceneLoaded += OnSceneLoaded;

    //}

 

    //private void OnDisable()
    //{
    //    base.OnDisable();
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode LoadSceneMode)
    {
        // if (scene.buildIndex == 1) // if we are in the game scene
        // {
     GameObject networkedPlayer =   PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false;
      //  }
    }

    public void CreatePlayer()
    {
        print("Creating Player : ".Color("Green"));

        GameObject networkedPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
       // networkedPlayer.gameObject.GetComponent<NetworkedPlayerController>().isInLobby = false;
    }


}


