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
	public float speed = 5f;

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
	public float CamYOffset = 10.0f;
	public float CamZOffset = 5.0f;
	public float camFollowDistance = 10.0f;
	[Range(0.1f, 1.0f)]
	public float camSmoothing = 0.1f;





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

	//	UpdateCamPosition(PlayerCameraController.CameraPosition.FrontFacing);

	}

	// Start is called before the first frame update
	void Start()
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
				CameraPrefab = 	Instantiate(camPrefab, this.transform.position, camPrefab.transform.rotation);
			//Camera
			_camControll = CameraPrefab.GetComponent<PlayerCameraController>();
			_camControll._netControll = this.gameObject.GetComponent<NetworkedPlayerController>();
		}

		//Instantiate(camPrefab, camPrefab.GetComponent<PlayerCameraController>().FrontFacingPOS, Quaternion.identity);

		MovementInversion = 1;


	}

	// Update is called once per frame
	void Update()
	{

		Move();
		Jump();
		Rotate(toRot);

	
	}

    private void LateUpdate()
    {
	
	}


	void FixedUpdate()
	{
		if (!PV.IsMine)
		{
			return;
		}

		//Existing Movement Script
		m_Rigidbody.MovePosition(m_Rigidbody.position + transform.TransformDirection(movementWithInversion) * Time.fixedDeltaTime);
		
		UpdateCamPosition(_camControll.myDirection);
		


		


	}


	public void Move2(Vector3 move)
	{

		playerVector = new Vector3(-move.x * speed, 0, -move.z * speed);                                                //Players vector
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








	void Move()
	{
		print("VECTOR 3 : " + UpdateControlPosition(_camControll.myDirection));



		//	Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 moveDir = UpdateControlPosition(_camControll.myDirection);
		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
		movementWithInversion = new Vector3(moveAmount.x * MovementInversion, moveAmount.y * MovementInversion, moveAmount.z * MovementInversion);

		//print("MoveDir = " + moveAmount + "MoveDir Inverted : " + movementWithInversion);



		//Animator
		float hDirection = Input.GetAxis("Horizontal");
		float vDirection = Input.GetAxis("Vertical");

		m_TurnAmount = Mathf.Atan2(moveDir.x, moveDir.z);                                                                     //Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y).
		m_ForwardAmount = moveDir.z;
	


		UpdateAnimator();                                                                               //Update the aniumation 
	}


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

	/*
	public void NewRotation(float newRot)
    {
		this.transform.rotation = transform.rotation = Quaternion.Euler(transform.rotation.x, newRot, transform.rotation.z);
	}
	*/

	public void Rotate(Vector3 newPOS)
	{
		currentROT = transform.localPosition;

		Quaternion newRot = Quaternion.Euler(transform.rotation.x, newPOS.y, transform.rotation.z);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 10 * lerpSpeed * Time.deltaTime);







		/*
			smooth = Time.deltaTime * durationTime;
			Vector3 rotationDirection = new Vector3(transform.rotation.eulerAngles.x, newPOS.y, transform.rotation.eulerAngles.z);
			transform.Rotate(rotationDirection * smooth);

		*/
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
				newPOS = new Vector3(this.gameObject.transform.position.x -10, this.gameObject.transform.position.y + 10, this.gameObject.transform.position.z );


				//CameraPrefab.transform.position = Vector3.Lerp(CameraPrefab.transform.position, newPOS, Time.deltaTime );
				//CameraPrefab.transform.localPosition = new Vector3(this.gameObject.transform.position.x - 15, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z);



				Vector3 offset = new Vector3 (transform.position.x, transform.position.y + CamYOffset, transform.position.z) ;
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

	public Vector3 UpdateControlPosition(PlayerCameraController.CameraPosition cam)
	{

		switch (cam)
		{
			case PlayerCameraController.CameraPosition.FrontFacing:

			
				Vector3 moveDirFrontFacing = new Vector3(0, 0,  Input.GetAxisRaw("Horizontal")).normalized; //Lock movement to forward and back 


				//Player Controls
				return moveDirFrontFacing;


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
}
