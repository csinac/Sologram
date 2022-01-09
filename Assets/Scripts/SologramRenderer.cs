using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.Sologram
{
    public class SologramRenderer : MonoBehaviour
    {
        private Camera mainCam;
        [SerializeField] private RawImage rawImage;

        public Vector3 CamPosition => mainCam.transform.position - transform.position;

        public Texture Image {
            get => rawImage.texture;
            set => rawImage.texture = value;
        }
    
        void Start() {
            mainCam = Camera.main;
        }

        void Update() {
            transform.LookAt(mainCam.transform);
        }
    }
}
