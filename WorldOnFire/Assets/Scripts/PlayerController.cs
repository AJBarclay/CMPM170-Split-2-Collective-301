using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    public float speed = 6;
    
    // camera and rotation
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float upLimit = -50;
    public float downLimit = 50;
    
  
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        gameObject.GetComponent<CharacterController>().detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10;
        }
        else
        {
            speed = 6;
        }

        if (gameObject.GetComponent<CharacterController>().velocity.magnitude > 0 && gameObject.GetComponent<CharacterController>().velocity.magnitude < 7)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
        else if (gameObject.GetComponent<CharacterController>().velocity.magnitude > 7)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            gameObject.GetComponent<Animator>().SetBool("isRunning", true);
        }
        else if (gameObject.GetComponent<CharacterController>().velocity.magnitude == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        if (characterController.isGrounded)
        {
            Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
            characterController.Move(speed * Time.deltaTime * move);
        }
        else
        {
            Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove + Vector3.down;
            characterController.Move(speed * Time.deltaTime * move);
        }
    }
    public void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");
        
        transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation*mouseSensitivity,0,0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }
}
