using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]




public class NetworkedPlayerController : MonoBehaviour
{

	[Header("Anim Settings")]
	[HideInInspector] public Animator anim;                                                                                 //Anim settings


	////[Header("Idle Anim Settings")]
	//float idleAnimFloat;                                                                            //IDLE ANIM BLEND TREE - float variable
	//int IdleHash;                                                                                   //Idle anim hash code

	[Header("Velocity Anim Settings")]
	int velocityHash;                                                                               //movement anim hash code	
	//float vlocityFloat;
	//Vector3 playerVector;
	public float DampTime = 1.1f;                                                                           //Animation dampining time

	[Header("Turning  Settings")]
	int turningHash;
	public bool turning;

	[Header("Movement Settings")]
	//private float hDirection;
	//private float vDirection;
	//[SerializeField] private Rigidbody m_Rigidbody;
	[SerializeField] private CapsuleCollider m_Capsule;
	//public float rotSpeed = 5f;
	[SerializeField] float m_TurnAmount;
	float m_ForwardAmount;
//	Vector3 m_GroundNormal;
	private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.

	[Header("Jumping")]
	private float verticalVelocity;
	public float gravity = 15.0f;
	public float JumpForce = 10.0f;
	public KeyCode JumpInput = KeyCode.Space;
	[SerializeField] bool grounded;
	//	Vector3 smoothMoveVelocity;
	int jumpHash;
	float jumpDirForward; //-1 is Backwards, +1 is Forwards
	float jumpDirLeftRight; //-1 is Backwards, +1 is Forwards

	//[Header("Sneak Settings")]
	//[HideInInspector] public int sneakHash;
	//[HideInInspector] public int againstWallHash;






	[Header("Photon Settings")]
	PhotonView PV;


	[Header("Bonfire")]
	public GameObject bonfireSpawn;
	//[Header("CameraInverstion")]
	//public int MovementInversion = 1;
	//Vector3 movementWithInversion;

	[Header("Rotation")]
	//Vector3 currentROT;
	[HideInInspector] public Vector3 toRot;
	public float lerpSpeed = 0.5f;
	public float durationTime;
	private float smooth;

	[Header("Camera")]
	public Camera camPrefab;
	Camera CameraPrefab;
	[HideInInspector] public PlayerCameraController _camControll;
	//Vector3 newPOS;
	//public float CamXOffset = 0.0f;
	//public float CamYOffset = 10.0f;
	//public float CamZOffset = 5.0f;
	//public float camFollowDistance = 10.0f;
	//[Range(0.1f, 1.0f)]
	//public float camSmoothing = 0.1f;

	//public GameObject cameraTargetOnSpawn;

	[Header("LobbySettings")]
	[HideInInspector] public bool isInLobby = false;



	/// <summary>
	/// TO DO :
	/// 
	/// Either neeed to 
	/// reset 'keys' in relation to player orientation trelatie to the world
	/// Resetting player movement based on rotation
	/// 
	/// + Camera moving away from buildings
	/// </summary>
	/// <param name="move"></param>

	//Testing
	GameObject target;
	public float speedMove4;

	//Vector3 lastPos;
	//public Transform objToMonitor; // drag the object to monitor here
	//float threshold = 0.0f; // minimum displacement to recognize a 

	[Header("NewPLayerMovement")]
	public CharacterController controller;              //Motor that drives the player
	public KeyCode SprintInput = KeyCode.LeftShift;
	public Transform cam;
	[Range(0, 1)]
	public float turnSmoothTime = 0.1f;
	float turnSmoothvelocity;

	[Header("Speed Settings")]
	//public float rotationSpeed = 5f;
	[Range(1, 8)]
	public float anim_walkSpeed;
		[SerializeField] float  sprintSpeed, walkspeed = 8f , smoothTime;
	private float movementSpeed;


	[Header("Perform Action")]
	public KeyCode PerformAction = KeyCode.F;
	public bool PermormingAction;

	private void Awake()
	{

		//Settings 
		anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
	//	m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		//Animation
		//Ide setup
	//	IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 


		//Movement Setup
		velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
		jumpHash = Animator.StringToHash("anim_JumpFloat");
		turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   

		//sneak
		//sneakHash = Animator.StringToHash("anim_sneak");                                      //Hash number for turning    
	//	againstWallHash = Animator.StringToHash("anim_isAgainstWall");                                      //Hash number for turning   

		//Photon
		PV = GetComponent<PhotonView>();

		//	controller
		controller = this.gameObject.GetComponent<CharacterController>();

	}

	// Start is called before the first frame update
	void Start()
	{
		if (isInLobby)
        {
			CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
			//Camera
			_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
			_camControll.parent = this.gameObject;
			cam = CameraPrefab.gameObject.transform;
			//Display the parent's name in the console.
			//	Debug.Log("Player's Parent: " + CameraPrefab.gameObject.transform.parent.name);
			controller = this.gameObject.GetComponent<CharacterController>();
		}

		if (isInLobby == false)
        {

			//If multiplayer and not my game object
			if (!PV.IsMine)
			{
				if (GetComponentInChildren<Camera>() != null)
				{
					Destroy(GetComponentInChildren<Camera>().gameObject);
				}

			}

			if (PV.IsMine)
			{
				CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
				//Camera
				_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
				_camControll.parent = this.gameObject;

				cam = CameraPrefab.gameObject.transform;
				controller = this.gameObject.GetComponent<CharacterController>();
			}
		}
	

		//Instantiate(camPrefab, camPrefab.GetComponent<PlayerCameraController>().FrontFacingPOS, Quaternion.identity);

		//MovementInversion = 1;

		//lastPos = this.transform.position;

	}

	// Update is called once per frame
	void Update()
	{

		//Move();
	

		//this wont allow for backwards movement
		//Rotate(toRot);


	}

	private void LateUpdate()
	{

	}


	void FixedUpdate()
	{
		if (isInLobby == false)
        {
			if (!PV.IsMine)
			{
				return;
			}
		}
		

		//Existing Movement Script
		//m_Rigidbody.MovePosition(m_Rigidbody.position + transform.TransformDirection(movementWithInversion) * Time.fixedDeltaTime);

		//Move 3 is the current edition 
		Move5();
		Jump();
		PerformActionCheck();
	
	}



    public void Move5()
    {
		float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1 and +1 (-1 for left , + 1 for right)
		float verticalInput = Input.GetAxisRaw("Vertical"); // -1 and +1  (+ 1 up, - 1 down) 

		Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

		if (direction.magnitude >= 0.1)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //How much to rotate the player on the y axis to point in the movement direction. ATan2 is a math function that returns an angle between the x axis and an angle starting 0 and terminating at x,y taking into account unity forward 
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime); //Smoothed angle of rotaiton 

			transform.rotation = Quaternion.Euler(0f, angle, 0f);


			movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(SprintInput) ? sprintSpeed : walkspeed), smoothTime);

			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;        //move in direction of camera
			controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);


			//m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
			float normalDirection = moveDir.magnitude;
			m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


			 jumpDirForward = moveDir.normalized.z; //-1 is Backwards, +1 is Forwards
			 jumpDirLeftRight = moveDir.normalized.x; //-1 is Backwards, +1 is Forwards

		//	print("Move Dir Forward" + moveDir.normalized.z + "Move Dir Side" + moveDir.normalized.x);
		


			//	jumpDir = 
			//	print(m_ForwardAmount);

			UpdateAnimator();                                                                               //Update the aniumation 

		}

		else
        {
			m_ForwardAmount = 0;
			m_TurnAmount = 0;
			UpdateAnimator();
		}
	}




	/// <summary>
	/// DEPRECEATED 
	/// </summary>
	/// 

	/*
	public void Move3()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

		movementDirection.Normalize();

		float normalDirection = movementDirection.magnitude;

		//Movement speed of Character

		movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), smoothTime);

	


		//Rotation of Character
		if (movementDirection != Vector3.zero)
		{
			Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
		}


		if (verticalInput == 0 && horizontalInput ==0 )
        {
			m_ForwardAmount = 0;

		}

		//transform the input direction from the player's local space to world space,
		//Vector3 worldInputMovement = transform.TransformDirection(movementDirection.normalized);


		//Move the character
		transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

		//m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).

		m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


	//	print(m_ForwardAmount);

		UpdateAnimator();                                                                               //Update the aniumation 

		UpdateCamPosition(_camControll.myDirection);
	}
	*/
	void UpdateAnimator()
	{
		anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
		anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters
	}









	void Jump()
	{


		if (isInLobby == true)
		{
			if (controller.isGrounded)
			{
				verticalVelocity = -gravity * Time.deltaTime;

				if (Input.GetKey(JumpInput))
				{
					verticalVelocity = JumpForce;
					anim.SetBool("anim_Jumping", true); // Set jumping 
					
					if (jumpDirForward == 0 && jumpDirLeftRight == 0)
                    {
						anim.SetFloat(jumpHash, 0);
					}

					if (jumpDirForward > 0 && jumpDirLeftRight <= 0)
                    {
						//	print("Jump foward " + jumpDirForward );
						anim.SetFloat(jumpHash, 1);
					}

					if ( jumpDirForward < 0 && jumpDirLeftRight >= 0)
                    {
						//print("Jump Backwards " + jumpDirForward);
						anim.SetFloat(jumpHash, 1);
					}

					if (jumpDirLeftRight > 0 && jumpDirForward >= 0)
					{
						//print("Jump Side - forward " + jumpDirLeftRight);
						anim.SetFloat(jumpHash, 1);
					}

					if (jumpDirLeftRight < 0 && jumpDirForward <= 0)
					{
						//print("Jump Side - forward " + jumpDirLeftRight);
						anim.SetFloat(jumpHash, 1);
					}


					//	anim.SetFloat(jumpHash, );

				}
			}

			else
			{
				verticalVelocity -= gravity * Time.deltaTime;
				anim.SetBool("anim_Jumping", false); // Set jumping 
			}

			Vector3 jumpvector = new Vector3(0, verticalVelocity, 0);
			controller.Move(jumpvector * Time.deltaTime);
			/*
					if (Input.GetKeyDown(JumpInput) && grounded)
					{
					//	m_Rigidbody.AddForce(transform.up * jumpForce);
					}

					*/
		}

		if (isInLobby == false)
		{

			//If multiplayer and not my game object
			if (!PV.IsMine)
			{
				return;
			}

			if (PV.IsMine)
			{
				if (controller.isGrounded)
				{
					verticalVelocity = -gravity * Time.deltaTime;

					if (Input.GetKey(JumpInput))
					{
						verticalVelocity = JumpForce;
						anim.SetBool("anim_Jumping", true); // Set jumping 

						if (jumpDirForward == 0 && jumpDirLeftRight == 0)
						{
							anim.SetFloat(jumpHash, 0);
						}

						if (jumpDirForward > 0 && jumpDirLeftRight <= 0)
						{
							//	print("Jump foward " + jumpDirForward );
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirForward < 0 && jumpDirLeftRight >= 0)
						{
							//print("Jump Backwards " + jumpDirForward);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight > 0 && jumpDirForward >= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}

						if (jumpDirLeftRight < 0 && jumpDirForward <= 0)
						{
							//print("Jump Side - forward " + jumpDirLeftRight);
							anim.SetFloat(jumpHash, 1);
						}


						//	anim.SetFloat(jumpHash, );

					}
				}

				else
				{
					verticalVelocity -= gravity * Time.deltaTime;
					anim.SetBool("anim_Jumping", false); // Set jumping 
				}

				Vector3 jumpvector = new Vector3(0, verticalVelocity, 0);
				controller.Move(jumpvector * Time.deltaTime);
				/*
						if (Input.GetKeyDown(JumpInput) && grounded)
						{
						//	m_Rigidbody.AddForce(transform.up * jumpForce);
						}

						*/
			}
		}


	}


	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}







	public void UpdateCamPosition(PlayerCameraController.CameraPosition cam)
	{

		switch (cam)
		{
			case PlayerCameraController.CameraPosition.FrontFacing:

				/*

					Vector3 oldPOs = CameraPrefab.transform.position;
					Vector3 offset1 = new Vector3(transform.position.x + CamXOffset, transform.position.y + CamYOffset, transform.position.z - CamZOffset);

					Vector3 newPOS1 = Vector3.Lerp(oldPOs, offset1 - transform.right * camFollowDistance, 0.1f);



					//Change in Position 



					Vector3 offsetToCheck = objToMonitor.position - lastPos;
					if (offsetToCheck.x > threshold)
					{
						lastPos = objToMonitor.position; // update lastPos
						print("// code to execute when X is getting bigger : " );                         // code to execute when X is getting bigger
					}
					else
					if (offsetToCheck.x < -threshold)
					{
						lastPos = objToMonitor.position; // update lastPos
						print(" // code to execute when X is getting smaller  : " );                   // code to execute when X is getting smaller 
					}

				//TESTING



				CameraPrefab.transform.position = newPOS1;

						*/


				CameraPrefab.transform.LookAt(transform.position);


				//Player Controls



				break;
			case PlayerCameraController.CameraPosition.OverShoulder:

				/*
			Vector3 oldPOS = CameraPrefab.transform.position;
			newPOS = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y + 10, this.gameObject.transform.position.z);

			Vector3 offset = new Vector3(transform.position.x, transform.position.y + CamYOffset, transform.position.z);
			Vector3 newPOS2 = Vector3.Lerp(oldPOS, offset - -transform.forward * camFollowDistance, 0.1f);

			print("OverShoulder Vector Difference in Cam : " + (oldPOS - newPOS2));


			CameraPrefab.transform.position = newPOS2;
			CameraPrefab.transform.LookAt(transform.position);

				*/
				CameraPrefab.transform.LookAt(this.gameObject.transform);



				//Player Controls





				break;

			default:
				print("Incorrect intelligence level.");
				break;
		}

	}

	public Vector3 UpdateControlPosition(PlayerCameraController.CameraPosition cam)
	{

		switch (cam)
		{
			case PlayerCameraController.CameraPosition.FrontFacing:

				float moveHorizontal = Input.GetAxis("Horizontal");
				float moveVertical = Input.GetAxis("Vertical");
				Vector3 moveDir;

				if (moveHorizontal > 0.1f || moveHorizontal < 0)
				{
					Vector3 moveDirFrontFacing = new Vector3(0, 0, Input.GetAxisRaw("Horizontal")).normalized; //Lock movement to forward and back 

					moveDir = moveDirFrontFacing;
					return moveDir;
				}

				else if (moveVertical > 0.1f || moveVertical < 0.0f)
				{
					Vector3 moveDirFrontFacing2 = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized; //Lock movement to forward and back 
					moveDir = moveDirFrontFacing2;
					return moveDir;
				}

				else
				{
					moveDir = new Vector3(0, 0, Input.GetAxisRaw("Horizontal")).normalized;
					return moveDir;
				}

				//Player Controls



				break;
			case PlayerCameraController.CameraPosition.OverShoulder:



				//Player Controls

				Vector3 moveDireOverShoulder = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;


				//Player Controls
				return moveDireOverShoulder;




				break;

			default:

				//Player Controls
				Vector3 test3 = new Vector3(0, 0, 0);

				//Player Controls
				return test3;
				print("Incorrect intelligence level.");
				break;
		}



	}

	void OnDrawGizmosSelected()
	{
		// Draws a 5 unit long red line in front of the object
		Gizmos.color = Color.red;
		Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
		Gizmos.DrawRay(transform.position, direction);
	}


	private void PerformActionCheck()
	{
	 if (Input.GetKey(PerformAction))
        {
			PermormingAction = true;
			print("player controller performing action");
        }
	 else
        {
			PermormingAction = false;
        }
	}

}
