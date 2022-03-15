using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;

        [SerializeField] private Transform cam;
        [SerializeField] private Transform orientation;

        [Header("References")]
        [SerializeField] private WallRun wallRun;

        float mouseX;
        float mouseY;

        float multiplier = 0.01f;

        float xRotation;
        float yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            MyInput();
            
        }

        private void MyInput()
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.Tilt);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);

        }
    }
}