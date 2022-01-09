using UnityEngine;

namespace RectangleTrainer.Sologram.Helpers
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        void Update() {
            transform.Rotate(Vector3.up, Time.deltaTime * speed);
        }
    }
}