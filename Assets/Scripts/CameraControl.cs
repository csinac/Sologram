using System;
using UnityEngine;

namespace RectangleTrainer.Sologram.Control
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private bool demoMode = false;
        [SerializeField] private Transform demoCenter;

        [SerializeField] private RotationAxes axes = RotationAxes.MouseXAndY;
        [SerializeField] private float sensitivityX = 5f;
        [SerializeField] private float sensitivityY = 5f;

        [SerializeField] private float minimumX = -360f;
        [SerializeField] private float maximumX = 360f;

        [SerializeField] private float minimumY = -60f;
        [SerializeField] private float maximumY = 60f;

        [SerializeField] private float moveSpeed = 0.01f;

        private float rotationY = 0f;

        private Vector3 start;
        
        private void Start() {
            start = transform.position;
        }
        
        Vector3 RotateOnY(Vector3 p, Vector3 center, float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            float x = p.x - center.x;
            float z = p.z - center.z;

            x = p.x * c - p.z * s + center.x;
            z = p.x * s + p.z * c + center.z;

            return new Vector3(x, p.y + Mathf.Sin(angle * 2) * 2, z);
        }

        void Update() {
            if (demoMode) {
                transform.position = RotateOnY(start, demoCenter.position, Time.time);
                transform.LookAt(demoCenter);
                return;
            }
            if (Input.GetMouseButton(1)) {
                if (axes == RotationAxes.MouseXAndY) {
                    float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                }
                else if (axes == RotationAxes.MouseX) {
                    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
                }
                else {
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
                }
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float elevate = Input.GetAxis("Elevate");

            transform.position += transform.forward * (vertical * moveSpeed) + transform.right * (horizontal * moveSpeed) + transform.up * (elevate * moveSpeed);
        }
        
        private enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }
    }
}

