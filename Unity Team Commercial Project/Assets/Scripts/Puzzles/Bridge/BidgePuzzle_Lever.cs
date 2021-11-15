using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidgePuzzle_Lever : MonoBehaviour
{

    public GameObject missingBridePiece;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            //Open UI

            NetworkedPlayerController controller = other.gameObject.GetComponent<NetworkedPlayerController>();
            if (controller.PermormingAction == true )
            {
                //SEND CALL FOR ACTION - suvscription 
                //Spawn Bridge Piece
            }
            else
            {

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
