using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFire_Trigger : MonoBehaviour
{

    public LobbyManager lobbyManager;


    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            //Open UI

            CharacterID charID = other.gameObject.GetComponent<CharacterID>();


            if (charID.isHumanCharater == true)
            {
                lobbyManager.lobbyUI.gameObject.SetActive(true);
            }
            else
            {
                lobbyManager.lobbyUI.gameObject.SetActive(false);
            }
        }

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Close UI
            lobbyManager.lobbyUI.gameObject.SetActive(false);
        }
    }


}
