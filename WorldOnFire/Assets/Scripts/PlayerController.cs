using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    public float speed = 6;
    
    // camera and rotation
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float upLimit = -50;
    public float downLimit = 50;
	public LineRenderer streamLine;
	public float rangeStretch = 5.0f;
	public float rangeDelta = 0.0f;
	public float range = 10.0f;

    // extinguisher fuel
    private Image barImage;
    Fuel fuel;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        barImage = GameObject.Find("fuelBar").GetComponent<Image>();
        fuel = new Fuel();
    }

    void Start()
    {
        gameObject.GetComponent<CharacterController>().detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {
		//Debug.Log(rangeDelta);
        if (gameObject.GetComponent<Animator>().GetBool("isWalking"))
        {
            AudioManager.Instance.Play(0);
        } else if (gameObject.GetComponent<Animator>().GetBool("isWalking") == false)
        {
            AudioManager.Instance.Stop(0);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10;
        }
        else
        {
            speed = 6;
        }
		streamLine.enabled = false;
        if (Input.GetMouseButton(1))
        {
            transform.GetChild(2).GetComponent<Camera>().fieldOfView = 40;
            if (Input.GetMouseButton(0) && barImage.fillAmount > 0)
            {
                Debug.Log("bar is filled: " + barImage.fillAmount);
                fuel.usingFuel(0.2f);
                Vector3 rayOrigin = transform.GetChild(2).GetComponent<Camera>().ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, transform.GetChild(2).GetComponent<Camera>().transform.forward, out hit, rangeDelta + range))
                {
					if (rangeDelta < rangeStretch) {rangeDelta += Time.deltaTime;}
					if (rangeDelta > rangeStretch) {rangeDelta = rangeStretch;}
					
					Vector3[] positions = new Vector3 [2] {transform.position, hit.point};
					streamLine.SetPositions(positions);
					streamLine.enabled = true;
                    if (hit.collider.gameObject.CompareTag("Fire"))
                    {
                        FireSpreading hitFireScript = hit.collider.gameObject.GetComponent<FireSpreading>();
						hitFireScript.Extinguish();
                    }
					if (hit.collider.gameObject.CompareTag("House"))
					{
						houseCatchingFire hitHouseScript = hit.collider.gameObject.GetComponent<houseCatchingFire>();
						hitHouseScript.houseExtinguish();
					}
                    
                }
				else
				{
					if (rangeDelta > 0 ) {rangeDelta -= Time.deltaTime;}
					if (rangeDelta < 0) {rangeDelta = 0;}
					streamLine.enabled = false;
				}
            }
			else
			{
				if (rangeDelta > 0 ) {rangeDelta -= Time.deltaTime;}
				if (rangeDelta < 0) {rangeDelta = 0;}
			}
        }
        else
        {
			if (rangeDelta > 0) {rangeDelta -= Time.deltaTime;}
			if (rangeDelta < 0) {rangeDelta = 0;}
            transform.GetChild(2).GetComponent<Camera>().fieldOfView = 60;
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
