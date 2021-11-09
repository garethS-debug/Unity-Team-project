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


    }

    // Update is called once per frame
    void Update()
    {
        Move();
        m_Rigidbody.MovePosition(m_Rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
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

    private void FixedUpdate()
    {
     
    }


    void UpdateAnimator()
    {
        anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
        anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters
    }

}

