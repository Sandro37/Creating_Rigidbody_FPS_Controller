using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform orientation;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float movementMultiplier = 10f;
        [SerializeField] private float airMultiplier = 0.4f;
        private float rbDrag = 6f;

        float horizontalMovement;
        float verticalMovement;
        Vector3 moveDirection;
        Rigidbody rig;

        [Header("Sprinting")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float acceleration = 10f;


        [Header("Jumping")]
        [SerializeField] private float jumpForce = 5f;
        float playerHeight = 2f;

        [Header("Drag")]
        [SerializeField] private float groundDrag = 6f; 
        [SerializeField] private float airDrag = 2f;

        [Header("Ground detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] LayerMask groundMask;
        bool isGrounded;
        float groundDistance = 0.4f;
        
        RaycastHit slopeHit;
        Vector3 slopeMoveDirection;
        // Start is called before the first frame update
        void Start()
        {
            rig = GetComponent<Rigidbody>();
            rig.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            MyInput();
            ControlDrag();
            ControlSpeed();

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }

        private void ControlSpeed()
        {
            if(Input.GetButton("Fire3") && isGrounded)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private bool OnSlope()
        {
            if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
            {
                if(slopeHit.normal != Vector3.up)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void ControlDrag()
        {
            if (isGrounded)
            {
                rig.drag = groundDrag;
            }
            else
            {
                rig.drag = airDrag;
            }
        }

        private void MyInput()
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }


        private void MovePlayer()
        {
            if (isGrounded && !OnSlope())
            {
                rig.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else if(isGrounded && OnSlope())
            {
                rig.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rig.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            }
        }

        private void Jump()
        {
            rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
            rig.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}