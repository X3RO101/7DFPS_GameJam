using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInfo info;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 dir = Vector3.zero;

    public float speed = 5.0f;
    public float jumpForce = 13.0f;
    public float antiBump = 4.5f;
    public float gravity = 30.0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        info = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    private void Update()
    {
        DefaultMovement();
    }

    private void FixedUpdate()
    {
        controller.Move(dir * Time.deltaTime);
    }

    private void DefaultMovement()
    {
        //Recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (controller.isGrounded)
        {
            float currSpeedX =  Input.GetAxis("Vertical") * speed;
            float currSpeedY = Input.GetAxis("Horizontal") * speed;

            dir = (forward * currSpeedX) + (right * currSpeedY);
            dir.y = -antiBump;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
        {
            dir.y -= gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        dir.y += jumpForce;
    }
}
