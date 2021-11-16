using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;


    [Header("Photon Settings")]
    PhotonView PV;                                              //Setting up photon view 

    GameObject InventoryItem;                                   //Creating a variable for the inventory Item (to send instructions to) 

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        PV = this.gameObject.GetComponent<PhotonView>();        //Get the photonview on the player

        if (PV.IsMine)                                          //Only run this script on the owning player who triggered the event
        {
            if (item)
            {
                inventory.AddItem(item.item, 1);                    //Photon 
                // PhotonNetwork.Destroy(other.gameObject);        
                //Destroy(other.gameObject);
                InventoryItem = other.gameObject;
                PhotonView photonView = PhotonView.Get(this);       //Get PhotonView on this gameobject
                photonView.RPC("DestroyObject", RpcTarget.All);   //Send an RPC call to everyone 
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Clear the player's inventory when they quit
        inventory.Container.Clear();
    }

    [PunRPC]
    void DestroyObject()                                      //Making sure the object is destroyed on everyones copy
    {
        //Destroy(gameObject);
        Debug.Log("Destroy : " + InventoryItem.name);
        InventoryItem.gameObject.SetActive(false);
    }

}
