using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NetworkedThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;              //Motor that drives the player

    public float speed = 6f;

    public Transform cam;

    [Range(0,1)]
    public float turnSmoothTime;
    float turnSmoothvelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1 and +1 (-1 for left , + 1 for right)
        float verticalInput = Input.GetAxisRaw("Vertical"); // -1 and +1  (+ 1 up, - 1 down) 

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //How much to rotate the player on the y axis to point in the movement direction. ATan2 is a math function that returns an angle between the x axis and an angle starting 0 and terminating at x,y taking into account unity forward 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime ); //Smoothed angle of rotaiton 
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

    
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;        //move in direction of camera
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
