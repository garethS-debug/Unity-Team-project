using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerController : MonoBehaviour
{

    [Header("New Move Settings")]
    Vector3 moveAmount;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    Vector3 smoothMoveVelocity;
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_Capsule;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
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
    }

    private void FixedUpdate()
    {

    }
}
