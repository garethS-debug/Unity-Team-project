using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterController : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;

    public GameObject target; //new code
                              // target is the object you will take the rotations from


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical); (old code)
        rb.AddForce(moveVertical * target.transform.forward * speed); //new code
        rb.AddForce(moveHorizontal * target.transform.right * speed); //new code
                                                                      //rb.AddForce (movement * speed); (old code)


    }
}