using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WallRun : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Transform orientation;

        [Header("Wall running")]
        [SerializeField] private float wallRunGravity;
        [SerializeField] private float wallRunJumpForce;

        [Header("Detection")]
        [SerializeField] private float wallDistance = .5f;
        [SerializeField] private float minimumJumpHeight = 1.5f;

        bool wallLeft = false;
        bool wallRight = false;

        private Rigidbody rig;
        RaycastHit leftWallHit;
        RaycastHit rightWallHit;

        [Header("Camera")]
        [SerializeField] private Camera cam;
        [SerializeField] private float fov;
        [SerializeField] private float wallRunFov;
        [SerializeField] private float wallRunFovTime;
        [SerializeField] private float camTilt;
        [SerializeField] private float camTiltTime;

        public float Tilt { get; private set; }
        bool CanWallRun()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
        }
        private void Start()
        {
            rig = GetComponent<Rigidbody>();
        }
        void Update()
        {
            this.CheckWall();
            if (CanWallRun())
            {
                if (wallLeft)
                {
                    StartWallRun();
                }else if (wallRight)
                {
                    StartWallRun();
                }
                else
                {
                    StopWallRun();
                }
            }
            else
            {
                StopWallRun();
            }
        }

        void CheckWall()
        {
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
        }
        void StartWallRun()
        {
            rig.useGravity = false;

            rig.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

            if (wallLeft)
                Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
            else if(wallRight)
                Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);

            if (Input.GetButtonDown("Jump"))
            {
                if (wallLeft)
                {
                    Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                    rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
                    rig.AddForce(wallRunJumpDirection * wallRunJumpForce * 50, ForceMode.Force);
                }
                else if (wallRight)
                {
                    Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                    rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
                    rig.AddForce(wallRunJumpDirection * wallRunJumpForce * 50, ForceMode.Force);

                }
            }
        }

        void StopWallRun()
        {
            rig.useGravity = true;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
            Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);
        }
    }
}