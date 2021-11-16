using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BidgePuzzle_Lever : MonoBehaviour
{
    [Header("Spawn Piece")]
    public GameObject missingBridePiece;

    [Header("Spawn Points")]
    public GameObject SpawnPoint;

    bool SpawnedPiece;

    // Start is called before the first frame update
    void Start()
    {
        SpawnedPiece = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerEnter(Collider other)
    {


        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //Open UI

            NetworkedPlayerController controller = other.gameObject.GetComponent<NetworkedPlayerController>();

            if (controller.PermormingAction == true && SpawnedPiece == false)
            {
                print("Performing Action");
                //SEND CALL FOR ACTION - suvscription 
                //Spawn Bridge Piece
                SpawnedPiece = true;
                PhotonNetwork.Instantiate(missingBridePiece.name, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
            }
         
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Close UI
        }
    }
}
