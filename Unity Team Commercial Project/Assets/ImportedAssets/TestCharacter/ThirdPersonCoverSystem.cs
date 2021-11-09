using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCoverSystem : MonoBehaviour {


    //Singleton
    private static ThirdPersonCoverSystem _instance;
    public static ThirdPersonCoverSystem Instance{  get { return _instance; } }

    [Header("Cover Settings")]
    public float coverTriggerDist = 3f;


    //Bools
    [Header("Triggers")]
    [HideInInspector]  public bool shoulderL;
    public bool farL;
    public bool innerL;
    public bool innerR;
    public bool farR;
    [HideInInspector] public bool shoulderR;
    [HideInInspector] public bool footFront;
    public bool leaningAgainstWall;

    [Header("EdgeBools")]
    public bool farLEdge;
    public bool innerLEdge;
    public bool innerREdge;
    public bool farREdge;
    public bool farRInWall;
    public bool farLInWall;

    private bool inDoorway;


    [Header("Cover")]
    [Tooltip("The primary direction that the player is pushing into cover")]
    public bool isInCover;
    public int inCover = -1;
    public int pushingDirection = -1;
    public float pushingStartTime, pushingDuration;
    public float pushTimeToCover = 0.5f;
    float lastAnimMoveTime, animMoveTimeDelta;

    [Header("RayCast")]
    public float raycastDistance = 1.25f;                                                   //rayvast Distance                                       
    public float raycastOffset ;                                                       //RaycastOffset
    public GameObject footSensor;
    public float inWallRayDepth;
    // public Transform[] sensorTransforms;                                              
    //  bool[] edgeBools = new bool[4];
    // float[] distances;
    // public float camZoomWallEdgeDist = 1.25f;

    [Header("Sneaking")]
    // The direction a player will move to the right in cover
    Vector3[] inCoverMovementDirs = { Vector3.right, Vector3.back, Vector3.left, Vector3.forward }; // NOTE: This is just a 90 deg rotation from pushingDirs
    public float sneakSpeed = 1;
    public float sneak;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }




    public void Start ()
    {
        
    }



    void FixedUpdate()
    {

    
        if (innerL == true && innerR == true)
        {
            isInCover = true;  
         
        }

        else
        {
            isInCover = false;
         
        }


        int newPushingDirection = ReturnPrimaryPushingDirection();
        
        if (newPushingDirection != pushingDirection)
        {
            pushingStartTime = Time.time;
            pushingDirection = newPushingDirection;
            pushingDuration = 0;
        }

        // Check to see if cover needs to be exited
        if (inCover != -1 && pushingDirection != inCover)
        {
            ExitCover();
        }

        // If no pushingDirection, exit this method
        if (pushingDirection == -1)
        {
            return;
        }

    
        // If player has been pushing in the same direction as cover for long enough,
        //  enter cover
        pushingDuration = (Time.time - pushingStartTime);
       
        bool pushingLongEnough = pushingDuration > pushTimeToCover;
        
        if (inCover == -1 &&  pushingLongEnough )
        {
            if (isInCover == true)
            {
                // Enter cover!
                DoorwayCamCheck();
                EnterCover(pushingDirection);
            }
         
        }

        if (inCover != -1)
        {
            
                AlignWithCover();
                print("Align with cover");
            
            //CreepAlongCover();
            if (shoulderR == true)
            {
               // print("Cover Right");
                //aligh camera left
              
            }


            if (shoulderL == true)
            {
               // print("Cover Left");
                //aligh camera right
               // PlayerCam.Instance.camMode = PlayerCam.eCamState.nearR;
            }
        }


    }

    public void Update()
    {

    
    }

    // Check to see which cardinal direction the player is moving in
    int ReturnPrimaryPushingDirection()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // If inCover != -1, then first check in the direction of cover
        // This keeps the player in cover more easily, especially when using an analog stick.
        switch (inCover)
        {
            case 0:
                if (v > 0) return 0;
                break;

            case 1:
                if (h > 0) return 1;
                break;

            case 2:
                if (v < 0) return 2;
                break;

            case 3:
                if (h < 0) return 3;
                break;
        }

        if (Mathf.Abs(h) < 0.25f && Mathf.Abs(v) < 0.25f)
        {
            return -1;
        }
        if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            if (h > 0)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            if (v > 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }
    }

    void EnterCover(int pDir)
    {
        print("Enter cover");
        inCover = pDir;

        leaningAgainstWall = true;


        // Position character right next to cover
        Main_Char_Controller.Instance.anim.SetBool(Main_Char_Controller.Instance.againstWallHash, true);

        // tPUControl.enabled = false;
        //  tPCharacter.enabled = false;
        //   anim.SetBool("InCover", true);
        //rigid.isKinematic = true; // Disable physics from moving the character randomly
    }
 

    public void DoorwayCamCheck()
    {
    
                if (farLInWall == true )                                          //left = 1
                {
                if ( farL == false && farR == false)
                {
              //  PlayerCam.Instance.camMode = PlayerCam.eCamState.nearR;
                inDoorway = true;
                }
                
                }

                if (farRInWall == true)
                {                                                                                                  //right = 2
                if (farL == false && farR == false)
                {
            //    PlayerCam.Instance.camMode = PlayerCam.eCamState.nearR;
                inDoorway = true;
            }

        }


    }


    void ExitCover()
    {
        print("Exit cover");
        inCover = -1;

        Quaternion rot180degrees = Quaternion.Euler(0,180,0);
        this.transform.rotation = rot180degrees;
        //    anim.SetBool("InCover", false);
        //  tPUControl.enabled = true;
        // tPCharacter.enabled = true;
        //rigid.isKinematic = false; // Re-enable physics
     //   PlayerCam.Instance.camMode = PlayerCam.eCamState.far;
        leaningAgainstWall = false;
        
        if (inDoorway == true)
        {
            inDoorway = false;
        }

        Main_Char_Controller.Instance.anim.SetBool(Main_Char_Controller.Instance.againstWallHash, false);
        Main_Char_Controller.Instance.SetAnimFloatCreep(0);
    }

    void AlignWithCover()
    {
        // Rotate into the correct position
        transform.rotation = Quaternion.Euler(0, 90 * inCover, 0);
        print("in cover = " + inCover);

        //Entranceway Cover
        animMoveTimeDelta = Time.time - lastAnimMoveTime;
        
        if (inCover != -1)
        {
            CreepAlongCover(animMoveTimeDelta);
        }
            lastAnimMoveTime = Time.time;


    }

    void CreepAlongCover(float deltaTime)
    {


        // Allow creeping left and right while inCover
        sneak = GetSneakValue();                                                                     //Get input directions
    
        if (Mathf.Abs(sneak) < 0.1f)                                                                //if the absolute sneak value is less than 
        {
            //SetAnimFloatCreep(0);                                                                 //Set anim
            return;
        }
      

     
        Vector3 pos0 = transform.position;                                                              //starting POS
        Vector3 pos1 = pos0 + -inCoverMovementDirs[inCover] * (sneakSpeed * sneak * deltaTime);          //next POS
        
        print("POS 1 = " + pos1);
        
        this.transform.position = pos1;

        // Check to make sure it's not the doorway of the cover
        if ((inDoorway && sneak < 0) || (inDoorway && sneak > 0))
        {
            // This movement would have pushed the character outside of cover, 
            //  so return to previous position
            transform.position = pos0;
            // SetAnimFloatCreep(0.1f * sneak, creepWallEdgeEasing);                                          //Set anim
            return;
        }
        // We're not at the edge of cover, so allow move to new position and animate
       Main_Char_Controller.Instance.SetAnimFloatCreep(sneak);                                                                            //Set anim
    }

    float GetSneakValue()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        switch (inCover)
        {
            case 0:
                return h;
            case 1:
                return -v;
            case 2:
                // This breaks the pattern here, but it's much less confusing to the player.
                // Because the camera rotates 180 deg in near mode, the left and right keys
                //  need to be reversed in near mode. However, if the player is already holding
                //  one of those keys, that key should not be reversed until the key is released
                //  and then re-pressed. It's a bit complex, but it's the way that Metal Gear
                //  Solid handled the issue.
                //Debug.Log("TPWC:GetCreepValue()\tnewCamMode:" + newCamMode + "\tcreepLastCamMode:" + creepLastCamMode + "\th:" + h);
                return -h;
            case 3:
                return v;
        }

        return 0;
    }



}
