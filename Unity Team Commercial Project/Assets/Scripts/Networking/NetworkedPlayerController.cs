using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]


public class NetworkedPlayerController : MonoBehaviour
{

	[Header("Anim Settings")]
	[HideInInspector] public Animator anim;                                                                                 //Anim settings


	[Header("Idle Anim Settings")]
	float idleAnimFloat;                                                                            //IDLE ANIM BLEND TREE - float variable
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
	[SerializeField] private CapsuleCollider m_Capsule;
	public float rotSpeed = 5f;
	[SerializeField] float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.

	[Header("New Move Settings")]
	Vector3 moveAmount;
	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
	Vector3 smoothMoveVelocity;

	[Header("Speed Settings")]
	public float movementSpeed = 5f;
	public float rotationSpeed = 5f;
	[Range(1,8)] 
	public float anim_walkSpeed;

	[Header("Sneak Settings")]
	[HideInInspector] public int sneakHash;
	[HideInInspector] public int againstWallHash;



	[Header("Jump Settings")]
	[SerializeField] bool grounded;

	[Header("Photon Settings")]
	PhotonView PV;

	[Header("CameraInverstion")]
	public int MovementInversion = 1;
	Vector3 movementWithInversion;

	[Header("Rotation")]
	Vector3 currentROT;
	[HideInInspector] public Vector3 toRot;
	public float lerpSpeed = 0.5f;
	public float durationTime;
	private float smooth;

	[Header("Camera")]
	public Camera camPrefab;
	Camera CameraPrefab;
	[HideInInspector] public PlayerCameraController _camControll;
	Vector3 newPOS;
	public float CamXOffset = 0.0f;
	public float CamYOffset = 10.0f;
	public float CamZOffset = 5.0f;
	public float camFollowDistance = 10.0f;
	[Range(0.1f, 1.0f)]
	public float camSmoothing = 0.1f;

	public GameObject cameraTargetOnSpawn;

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


	private void Awake()
	{

		//Settings 
		anim = this.gameObject.GetComponent<Animator>();                                            //Get reference to the animator
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		//Animation
		//Ide setup
		IdleHash = Animator.StringToHash("Idle_Float");                                             //Hash number for idle 


		//Movement Setup
		velocityHash = Animator.StringToHash("anim_velocity");                                      //Hash number for velocity  
																									//Turning
		turningHash = Animator.StringToHash("anim_turn");                                      //Hash number for turning   

		//sneak
		sneakHash = Animator.StringToHash("anim_sneak");                                      //Hash number for turning    
		againstWallHash = Animator.StringToHash("anim_isAgainstWall");                                      //Hash number for turning   

		//Photon
		PV = GetComponent<PhotonView>();

		//	testing
		

	}

	// Start is called before the first frame update
	void Start()
	{
		if (isInLobby)
        {
			CameraPrefab = Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
			//Camera
			_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
			_camControll._netControll = this.gameObject.GetComponent<NetworkedPlayerController>();

			CamXOffset = 13.58f;
			CamYOffset = 10.1f;
			CamZOffset = 19.07f;
			camFollowDistance = -16.87f;

			CameraPrefab.gameObject.transform.parent = this.transform;
			CameraPrefab.gameObject.transform.position = cameraTargetOnSpawn.transform.position;
			//Display the parent's name in the console.
		//	Debug.Log("Player's Parent: " + CameraPrefab.gameObject.transform.parent.name);

		}

		if (isInLobby == false)
        {
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
				_camControll._netControll = this.gameObject.GetComponent<NetworkedPlayerController>();

				CameraPrefab.gameObject.transform.parent = this.transform;
				CameraPrefab.gameObject.transform.position = cameraTargetOnSpawn.transform.position;
				
				CameraPrefab.gameObject.transform.LookAt(this.gameObject.transform);
			}
		}
	

		//Instantiate(camPrefab, camPrefab.GetComponent<PlayerCameraController>().FrontFacingPOS, Quaternion.identity);

		MovementInversion = 1;

		//lastPos = this.transform.position;

	}

	// Update is called once per frame
	void Update()
	{

		//Move();
		Jump();

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
		Move3();

		

	}



	public void Move4 ()
    {
		// player movement - forward, backward, left, right
		float horizontal = Input.GetAxis("Horizontal") * speedMove4;
		float vertical = Input.GetAxis("Vertical") * speedMove4;
		
		Vector3 camRightFlat = new Vector3(CameraPrefab.gameObject.transform.right.x, 0f, CameraPrefab.gameObject.transform.right.z).normalized;

		Vector3 camForwardFlat = new Vector3(CameraPrefab.gameObject.transform.forward.x, 0f, CameraPrefab.gameObject.transform.forward.z).normalized;

		this.transform.Translate((camRightFlat * horizontal + camForwardFlat * vertical) * Time.deltaTime);

		//Rotation of Character


		//Rotation of Character
		
			Quaternion toRotation = Quaternion.LookRotation(camForwardFlat, Vector3.up);

			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);


		UpdateCamPosition(_camControll.myDirection);

	}



	public void Move3()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

		movementDirection.Normalize();

		float normalDirection = movementDirection.magnitude;

		//Movement speed of Character

		movementSpeed = Mathf.Lerp(movementSpeed, (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), smoothTime);

		Vector3 worldInputMovement = transform.TransformDirection(movementDirection.normalized);
		//rigidbody.AddForce(worldInputMovement * moveSpeed * Time.deltaTime);

		//Move the character
		transform.Translate(worldInputMovement * movementSpeed * Time.deltaTime, Space.World);


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

		//m_TurnAmount = Mathf.Atan2(movementDirection.x, movementDirection.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).

		m_ForwardAmount = Mathf.Lerp(m_ForwardAmount, normalDirection * movementSpeed * anim_walkSpeed * Time.deltaTime, lerpSpeed * Time.deltaTime * 10);


	//	print(m_ForwardAmount);

		UpdateAnimator();                                                                               //Update the aniumation 

		UpdateCamPosition(_camControll.myDirection);
	}

	void UpdateAnimator()
	{
		anim.SetFloat(velocityHash, m_ForwardAmount);                                                               // update the velocity animator parameters
		anim.SetFloat(turningHash, m_TurnAmount);                                                                   // update the turning animator parameters
	}






	public void Move2(Vector3 move)
	{

		playerVector = new Vector3(-move.x * anim_walkSpeed, 0, -move.z * anim_walkSpeed);                                                //Players vector
		m_TurnAmount = Mathf.Atan2(move.x, move.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		float absTurn = Mathf.Abs(m_TurnAmount);                                                                            //Get the absolute number of the turning

		move = transform.InverseTransformDirection(playerVector);                                                       //Transforms a direction from world space to local space.
		m_ForwardAmount = move.z;
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);

		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

		//Update animation
		//UpdateAnimator(playerVector);                                                                               //Update the aniumation 
		//anim.SetFloat(velocityHash, idleAnimFloat);																//convert to single velocity number	
		// Transorm ROT or Quat Rot check
		if (absTurn < 0.1f && ThirdPersonCoverSystem.Instance.leaningAgainstWall == false /*angle < 0.1f */ )
		{
			//Moving
			m_Rigidbody.velocity = playerVector;                                                                        //Move the character
		}

	}

	/*

	void Move()
	{
		print("VECTOR 3 : " + UpdateControlPosition(_camControll.myDirection));

		//Animator
		float hDirection = Input.GetAxis("Horizontal");
		float vDirection = Input.GetAxis("Vertical");


		//	Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 moveDir = UpdateControlPosition(_camControll.myDirection);
		print("move DIR Hor: " + hDirection + "move Dir Vir:" + vDirection);



		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
		movementWithInversion = new Vector3(moveAmount.x * MovementInversion, moveAmount.y * MovementInversion, moveAmount.z * MovementInversion);

		//print("MoveDir = " + moveAmount + "MoveDir Inverted : " + movementWithInversion);





		m_TurnAmount = Mathf.Atan2(moveDir.x, moveDir.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		m_ForwardAmount = moveDir.z;
	


		UpdateAnimator();                                                                               //Update the aniumation 
	}

	*/


	void Jump()
	{
		if (!PV.IsMine)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			m_Rigidbody.AddForce(transform.up * jumpForce);
		}
	}


	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}


	public void Rotate(Vector3 newPOS)
	{
		currentROT = transform.localPosition;

		Quaternion newRot = Quaternion.Euler(transform.rotation.x, newPOS.y, transform.rotation.z);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 10 * lerpSpeed * Time.deltaTime);
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

}
