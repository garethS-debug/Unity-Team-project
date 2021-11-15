using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class LobbyPlayerController : MonoBehaviour
{

    [Header("Anim Settings")]
    [HideInInspector] public Animator anim;                                                                                 //Anim settings


    [Header("Idle Anim Settings")]
    float idleAnimFloat;                                                                            //IDLE ANIM BLEND TREE - float variable
    int IdleHash;                                                                                   //Idle anim hash code

    [Header("Velocity Anim Settings")]
    int velocityHash;                                                                               //movement anim hash code	
    float vlocityFloat;

    [Header("Turning  Settings")]
    int turningHash;
    public bool turning;

    [Header("New Move Settings")]
    Vector3 moveAmount;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    Vector3 smoothMoveVelocity;
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_Capsule;

    [Header("Movement Settings")]
    private float hDirection;
    private float vDirection;
    public float rotSpeed = 5f;
    [SerializeField] float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    private Vector3 m_Move;


    [Header("Speed Settings")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;
    [Range(1, 8)]
    public float anim_walkSpeed;



    [Header("Camera")]
    public Camera camPrefab;
    Camera CameraPrefab;
    [HideInInspector] public PlayerCameraController _camControll;
    Vector3 newPOS;
    public float CamYOffset = 10.0f;
    public float CamZOffset = 5.0f;
    public float camFollowDistance = 10.0f;
    [Range(0.1f, 1.0f)]
    public float camSmoothing = 0.1f;

    [Header("Rotation")]
    Vector3 currentROT;
    [HideInInspector] public Vector3 toRot;
    public float lerpSpeed = 0.5f;
    public float durationTime;
    private float smooth;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        //Settings 
        anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
       

        //Animation
        //Ide setup
        IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 


        //Movement Setup
        velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
                                                                                                    //Turning
        turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   


        CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);

        //Camera
        _camControll = CameraPrefab.GetComponent<PlayerCameraController>();
       // _camControll._netControll = this.gameObject.GetComponent<LobbyPlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        Move3();
       // m_Rigidbody.MovePosition(m_Rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        UpdateCamPosition(_camControll.myDirection);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        //Animator
        float hDirection = Input.GetAxis("Horizontal");
        float vDirection = Input.GetAxis("Vertical");

        m_TurnAmount = Mathf.Atan2(moveDir.x, moveDir.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
        m_ForwardAmount = moveDir.z;


        UpdateAnimator();
    }

    public void Move3()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        movementDirection.Normalize();

        float normalDirection = movementDirection.magnitude;

        //	print("normal direction : " + normalDirection * movementSpeed* 2 * Time.deltaTime);
        //Movement of Character

        movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), smoothTime);


        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);


        //Rotation of Character
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        //float test = movementDirection.Normalize();


        if (verticalInput == 0 && horizontalInput == 0)
        {
            m_ForwardAmount = 0;

        }

        //m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).

        m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


        //	print(m_ForwardAmount);

        UpdateAnimator();                                                                               //Update the aniumation 
    }

    private void FixedUpdate()
    {
     
    }


    void UpdateAnimator()
    {
        anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
        anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters
    }


    public void UpdateCamPosition(PlayerCameraController.CameraPosition cam)
    {

        switch (cam)
        {
            case PlayerCameraController.CameraPosition.FrontFacing:

                /*
				Vector3 oldPOs = CameraPrefab.transform.position;
				 newPOS = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z - 15);


				//CameraPrefab.transform.localPosition = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z - 15) ;

				CameraPrefab.transform.position =  Vector3.Lerp(oldPOs, newPOS, 10);
				CameraPrefab.transform.LookAt(this.gameObject.transform);
	
				*/

                Vector3 oldPOs = CameraPrefab.transform.position;
                Vector3 offset1 = new Vector3(transform.position.x, transform.position.y + CamYOffset, transform.position.z - CamZOffset);

                Vector3 newPOS1 = Vector3.Lerp(oldPOs, offset1 - transform.right * camFollowDistance, 0.1f);

                CameraPrefab.transform.position = newPOS1;
                CameraPrefab.transform.LookAt(transform.position);


                //Player Controls



                break;
            case PlayerCameraController.CameraPosition.OverShoulder:


                Vector3 oldPOS = CameraPrefab.transform.position;
                newPOS = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y + 10, this.gameObject.transform.position.z);


                //CameraPrefab.transform.position = Vector3.Lerp(CameraPrefab.transform.position, newPOS, Time.deltaTime );
                //CameraPrefab.transform.localPosition = new Vector3(this.gameObject.transform.position.x - 15, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z);



                Vector3 offset = new Vector3(transform.position.x, transform.position.y + CamYOffset, transform.position.z);
                Vector3 newPOS2 = Vector3.Lerp(oldPOS, offset - -transform.forward * camFollowDistance, 0.1f);

                CameraPrefab.transform.position = newPOS2;
                CameraPrefab.transform.LookAt(transform.position);

                /*
								// Move
								Vector3 newPosition = transform.position - transform.forward * offset.z - transform.up * offset.y;
								CameraPrefab.transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * speed);

				*/



                CameraPrefab.transform.LookAt(this.gameObject.transform);



                //Player Controls





                break;

            default:
                print("Incorrect intelligence level.");
                break;
        }

    }
}

