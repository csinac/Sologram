using UnityEngine;

namespace RectangleTrainer.Sologram.Control
{
    public class CameraControl : MonoBehaviour
    {
        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }

        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 5f;
        public float sensitivityY = 5f;

        public float minimumX = -360f;
        public float maximumX = 360f;

        public float minimumY = -60f;
        public float maximumY = 60f;

        public float moveSpeed = 0.01f;

        float rotationY = 0f;

        void Update() {
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
    }
}

