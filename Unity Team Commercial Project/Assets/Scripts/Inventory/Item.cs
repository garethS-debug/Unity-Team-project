using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Item : MonoBehaviour
{
    public ItemObject item;



    public void DestroyItem()
    {
         PhotonView photonView = PhotonView.Get(this);       //Get PhotonView on this gameobject
         photonView.RPC("DestroyObject", RpcTarget.All);    //Send an RPC call to everyone 
    }


    [PunRPC]
    void DestroyObject()                                      //Making sure the object is destroyed on everyones copy of the game
    {
        this.gameObject.SetActive(false);

    }

}
