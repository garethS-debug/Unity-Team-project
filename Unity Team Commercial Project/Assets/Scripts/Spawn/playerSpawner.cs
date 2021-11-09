using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerSpawner : MonoBehaviour


{
    public GameObject[] playerPrefabs;         //stored player prefabs
    public Transform[] spawnPoints;             //

  

    private void Start()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];

     
       Debug.Log("Player Prefab = " + playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]]);
       Debug.Log("Player ID = " + ((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]));

            //if ((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] > 0)
            //{
         
            //}


        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        //There is no 'limit on the array when selecting a character
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }
}
