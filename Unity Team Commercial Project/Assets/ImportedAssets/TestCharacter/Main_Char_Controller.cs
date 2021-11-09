using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class Main_Char_Controller : MonoBehaviour {

	//Singleton
	private static Main_Char_Controller _instance;
	public static Main_Char_Controller Instance { get { return _instance; } }

	[Header("Anim Settings")]
	[HideInInspector] public Animator anim;																					//Anim settings


	[Header("Idle Anim Settings")]
	float idleAnimFloat;																			//IDLE ANIM BLEND TREE - float variable
	int IdleHash;                                                                                   //Idle anim hash code
	
	[Header("Velocity Anim Settings")]
	int velocityHash;                                                                               //movement anim hash code	
	float vlocityFloat;
	Vector3 playerVector;
	public float DampTime = 1.1f;                                                                           //Animation dampining time

	[Header("Turning  Settings")]
	int turningHash;
	public bool turning;

	[Header("Movement Settings")]
	private float hDirection;
	private float vDirection;
	[SerializeField] private Rigidbody m_Rigidbody;
	[SerializeField]  private CapsuleCollider m_Capsule;
    public float rotSpeed = 5f;
	[SerializeField]  float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;


	[Header("Speed Settings")]
	public float speed = 5f;

	[Header("Sneak Settings")]
	[HideInInspector] public int sneakHash;
	[HideInInspector] public int againstWallHash;


	private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.


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




	void Start () 
	{
		//Settings 
		anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		//Animation
		//Ide setup
		IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 
		IdleAnimTree();
		//Movement Setup
		velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
		//Turning
		turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   

		//sneak
		sneakHash = Animator.StringToHash("anim_sneak");                                      //Hash number for turning    
		againstWallHash = Animator.StringToHash("anim_isAgainstWall");                                      //Hash number for turning   
	}


	
	// Update is called once per frame
	void Update () 
	{	

            

               

    }


	void FixedUpdate()
	{

		//INput directions																				//Get the input directions 
		float hDirection = Input.GetAxis("Horizontal");
		float vDirection = Input.GetAxis("Vertical");


		m_Move = vDirection * Vector3.forward + hDirection * Vector3.right;                             // send the input directions into a vector 3 


		//Turning the character in opposity diretion
		Vector3 movementDirection = new Vector3(hDirection, 0, vDirection);
		
		movementDirection.Normalize();

		if (movementDirection != Vector3.zero)
        {
			if (m_Move == null)
            {
				print("Error 1");

            }


			//	Move(m_Move);

			Moving(m_Move);
		}

		else
		{
			anim.SetFloat(turningHash, 0);                                                                          // When not moving turning is 0
			anim.SetFloat(velocityHash, 0);                                                                          // When not moving set move is 0
		}







		//Move the character on the fixed update using the m_move direction


	}


	// idle anim
	public void IdleAnimTree()																						//IDLE ANIM BLEND TREE - Function for randomizing the idle animation 
	{
		idleAnimFloat = Random.Range(0.0f, 1.0f);
		anim.SetFloat(IdleHash, idleAnimFloat);
    }

	public void Moving(Vector3 moving)
    {
	
		playerVector = new Vector3(-moving.x * speed, 0, -moving.z * speed);                                                //Players vector
		m_TurnAmount = Mathf.Atan2(moving.x, moving.z);																		//Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		float absTurn = Mathf.Abs(m_TurnAmount);                                                                            //Get the absolute number of the turning
		
		moving = transform.InverseTransformDirection(playerVector);                                                       //Transforms a direction from world space to local space.
		m_ForwardAmount = moving.z;
		m_TurnAmount = Mathf.Atan2(moving.x, moving.z);
		float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);

		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);



		if (absTurn < 0.1f /*&& ThirdPersonCoverSystem.Instance.leaningAgainstWall == false angle < 0.1f */ )
		{
			//Moving
			m_Rigidbody.velocity = playerVector;                                                                        //Move the character
		}


		UpdateAnimator(playerVector);

	//	print("Error 2" + m_ForwardAmount);
	}



	void UpdateAnimator(Vector3 move)
	{
		if (move == null)
		{
			print("Error 2");

		}

		if (move.magnitude > 1f)                                                                                    //Magnitude Returns the length of this vector.The length of the vector is square root of(x * x + y * y + z * z).
				{
					move.Normalize();                                                                                       //Makes this vector have a magnitude of 1.
				}

				
				//m_GroundNormal = Vector3.up;                                                                                //Shorthand for writing Vector3(0, 1, 0).
				//move = Vector3.ProjectOnPlane(move, m_GroundNormal);                                                        //Vector3 The location of the vector on the plane.

				
					
				anim.SetFloat(velocityHash, m_ForwardAmount);																// update the velocity animator parameters
				anim.SetFloat(turningHash, m_TurnAmount);																	// update the turning animator parameters


	}

	public void SetAnimFloatCreep(float val, float easing = 1)
	{
		val = (1 - easing) * anim.GetFloat(sneakHash) + easing * val;
		anim.SetFloat(sneakHash, val);
	}


}
