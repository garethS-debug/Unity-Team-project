using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetWorkCameraFollow : MonoBehaviour
{
    [Header("states")]
    public eCamState camMode = eCamState.far;
    public enum eCamState { far, nearL, nearR };                                                                        //Enum sates for camera positions




    [Header("Player Ref")]
    public GameObject Targetplayer;




    //FIX THIS 
    [Header("Far Mode")]
    [Tooltip("If this is set to [0,0,0], the relative position of the camera " +
          "to the player in the scene will be used.")]
    public Vector3 relativePosFar = Vector3.zero;
    [Tooltip("The rotation about the x axis of the camera in Far mode.")]
    [Range(-180, 180)]
    public float xRotationFar = 60;
    [Range(-180, 180)]
    public float yRotationFar = -90;

    [Header("Camera Settings")]
    [Tooltip("[0..1] At 0, the camera will never move, at 1, the camera will " +
             "follow immediately with no lag.")]
    [Range(0, 1)]
    public float cameraEasing = 0.25f;
    Vector3 pDesired;



    [Header("Inscribed – Near Mode")]
    public Vector3 relativePosNear = new Vector3(0, 2.5f, -2);
    [Tooltip("Determines how far the camera will lead the player in Near mode.")]
    public float relativePosNearLRShift = 1;
    [Tooltip("The rotation about the x axis of the camera in Near mode.")]
    public float xRotationNear = 25;


    [Header("Photon Settings")]
    PhotonView PV;
    public GameObject[] players;

    // FIX THIS
    public GameObject mainChar;


    private void Awake()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
       
        foreach (GameObject player in players)
        {
            if (player.gameObject.GetComponent<PhotonView>().IsMine)
            {
                Targetplayer = player ;
                break;
            }
        }


    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
      
        Quaternion rotDesired = Quaternion.identity;

        switch (camMode)
        {
            case eCamState.far:
                pDesired = Targetplayer.transform.position + relativePosFar;
                rotDesired = Quaternion.Euler(xRotationFar, yRotationFar, 0);

                break;
            case eCamState.nearL:


                break;
            case eCamState.nearR:

                // Desired position should be relative to playerInstance facing and position
                //THIS NEEDS FIXING

                Vector3 pRelative = relativePosNear;

                //FIX NEEDED HERE -------> DEPENDING ON POSITION THE X NEEDS TO BE EITHER + or - 

                pRelative.x += (camMode == eCamState.nearL) ? -relativePosNearLRShift : relativePosNearLRShift; // if statement 


                pDesired = mainChar.transform.TransformPoint(pRelative);
                rotDesired = Quaternion.Euler(xRotationNear, ThirdPersonCoverSystem.Instance.inCover * -90, 0);


                break;

        }

        Vector3 pInterp = (1 - cameraEasing) * transform.position + cameraEasing * pDesired;
        transform.position = pInterp;

        Quaternion rotInterp = Quaternion.Slerp(transform.rotation, rotDesired, cameraEasing);
        transform.rotation = rotInterp;

    }

}
