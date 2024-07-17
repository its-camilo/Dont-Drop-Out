using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speedMovement = 50f;
    float speedRotation = 140f;
    float gravity = 50f;
    float jumpForce = 15f;
    Vector3 jumpVector = Vector3.zero;
    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cc.Move((transform.forward * speedMovement * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
           cc.Move(transform.forward * -speedMovement * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * speedRotation * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpVector.y = jumpForce; 
        }

        jumpVector.y -= gravity * Time.deltaTime;
        cc.Move(jumpVector*Time.deltaTime);
    }
    
}
