using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviourPunCallbacks
{

    public LevelInfoSO levelInfo;


    public SceneReference levelToLoad;


    /// <summary>
    /// FOR testing purposes
    /// </summary>
    public void OnClick_AssignPoints()
    {
        levelInfo.pointsGainedInLevel += 10;

    }

    public void OnClick_LeaveLevel()
    {
        PhotonNetwork.LeaveRoom(); // leave current match


        //Set up the existing room that we entered with 
        //Add a kick button and a 'private' / 'public' match options 

    }

    public override void OnLeftRoom()
    {

        PhotonNetwork.LoadLevel(levelToLoad);
        base.OnLeftRoom();

    }
}