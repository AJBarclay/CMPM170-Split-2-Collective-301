using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public CharacterController controller;
    public float TurnSmoothingTime = 0.1f;
    float TurnSmoothingVelocity;
    public Transform camera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothingVelocity, TurnSmoothingTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            if (controller.isGrounded)
            {
                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                controller.Move(moveDir.normalized * this.speed * Time.deltaTime);
            }

            else
            {
                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.down;
                controller.Move(moveDir.normalized * this.speed * Time.deltaTime);
            }
        }
    }
}
