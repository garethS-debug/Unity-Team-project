using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]


public class NetWorkedPlayerManager : MonoBehaviour
{

    public GameObject[] playerPrefabs;         //stored player prefabs
    //public Transform[] spawnPoints;             //

    public List<GameObject> spawnPoints = new List<GameObject>();



    PhotonView PV;

    private void Awake()
    {
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("spawnPoint"))
        {

            spawnPoints.Add(fooObj);
        }


        PV = GetComponent<PhotonView>();
        
    }


    // Start is called before the first frame update
    void Start()
    {

       
       


        Debug.Log("Player Prefab = " + playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]]);
        Debug.Log("Player ID = " + ((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]));

        //if ((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] > 0)
        //{

        //}


      

        if (PV.IsMine) // if owned by local players
        {
            if (spawnPoints.Count > 0)
            {
                CreateController();
            }
   
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateController()
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomNumber].transform;
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        Debug.Log("Cretated Player Controller " + playerToSpawn.name);

        Debug.Log("Im located on " + this.gameObject);
        //  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);

        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }
}
