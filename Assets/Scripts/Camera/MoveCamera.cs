using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera_
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraPosition;
        void Update()
        {
            if (cameraPosition != null)
                transform.position = cameraPosition.position;
        }
    }
}