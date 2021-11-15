using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gate : MonoBehaviour
{
    private MeshRenderer gateRend;
    private static Gate instance;
    private GameObject player;
    public InventoryObject inventory;
    private Animator anim;

    private void Awake()
    {
        instance = this;
        gateRend = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inventory.twoKeysCollected)
        {
            ChangeColor();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(inventory.twoKeysCollected && other.gameObject.tag == "Player")
        {
            anim.SetTrigger("OpenDoor");
        }
    }

    public void pauseAnimationEvent()
    {
        anim.enabled = false;
    }

    public void ChangeColor()
    {
        gateRend.material.color = Color.yellow;
    }

}
