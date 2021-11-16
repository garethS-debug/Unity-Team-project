using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;


    [Header("Photon Settings")]
    PhotonView PV;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        PV = this.gameObject.GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            if (item)
            {
                inventory.AddItem(item.item, 1);                //Photon 
                Destroy(other.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Clear the player's inventory when they quit
        inventory.Container.Clear();
    }
}
