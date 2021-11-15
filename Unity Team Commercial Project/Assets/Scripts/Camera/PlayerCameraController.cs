using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{

    // public bool 
    public Camera _cam;
    [HideInInspector] public NetworkedPlayerController _netControll;


    [Header("Camera Positioning")]
   // private Vector3 originalCamPOS;
 //   public float CameraFlipAmount;
    public bool CameraOverShoulder, CameraFrontFacing;

    //public Vector3 OverShoulderPOS;
   // public Vector3 FrontFacingPOS; // startingPOS

    public float lerpSpeed = 0.5f;



    public enum CameraPosition { FrontFacing, OverShoulder };
    [SerializeField] public CameraPosition myDirection;

    [Header("Camera Movement")]
    //Vector3 currentPOS;
    //Vector3 newPOS;

   public  CinemachineFreeLook cinemachineFreeLook;
     public GameObject parent;
    public CinemachineVirtualCamera vcam;



    // Start is called before the first frame update
    void Awake()
    {
        _cam = this.gameObject.GetComponent<Camera>();
       // _netControll = this.gameObject.GetComponent<NetworkedPlayerController>();

       
      //  originalCamPOS = _cam.transform.localPosition;

        CameraFrontFacing = true;
        CameraOverShoulder = false;


        //   FrontFacingPOS = _cam.transform.localPosition; //testing

       // print("Player ROT : " + this.transform.eulerAngles.y);

       // newPOS = FrontFacingPOS;


        myDirection = CameraPosition.FrontFacing;




    

  //      vcam.LookAt = parent.transform;
      //  vcam.Follow = parent.transform;
//

    }

    public void Start()
    {

        cinemachineFreeLook.m_Follow = parent.transform;
        cinemachineFreeLook.m_LookAt = parent.transform;

        
    }
    // Update is called once per frame
    void Update()
    {

        //rotating player
        //   currentPOS = _cam.transform.localPosition;
        //   _cam.transform.localPosition = Vector3.Lerp(currentPOS, newPOS, lerpSpeed);   //TESTd



        //rotating camera over time
        //  currentPOS = _cam.transform.localPosition;
        //  _cam.transform.localPosition = Vector3.Lerp(currentPOS, newPOS, lerpSpeed);   //TESTd


    }


    public void SetCamOverShoulder(float newRot)
    {

        //Camera Rotation

        //Player Rotation
        //_netControll.NewRotation(newRot);

        CameraOverShoulder = true;
        CameraFrontFacing = false;

    //    print("Setting over the shoulder");
        // _cam.transform.localPosition = OverShoulderPOS;
        // _cam.transform.LookAt(this.gameObject.transform);
      //  newPOS = OverShoulderPOS;
        _cam.transform.LookAt(this.gameObject.transform);

        //Player 
      //  _netControll.MovementInversion = 1;
        myDirection = CameraPosition.OverShoulder;


        //Controlls


    }

    public void SetCamFrontFacing(float newRot)
    {
        //Camera Rotation


     //   print("Setting front Facing");

        CameraFrontFacing = true;
        CameraOverShoulder = false;

        // _cam.transform.localPosition = FrontFacingPOS;
        // _cam.transform.LookAt(this.gameObject.transform);
      //  newPOS = FrontFacingPOS;
        _cam.transform.LookAt(this.gameObject.transform);

        //Player Rotation
      //  _netControll.MovementInversion = 1;

        myDirection = CameraPosition.FrontFacing;

        //Controlls

    }









}
