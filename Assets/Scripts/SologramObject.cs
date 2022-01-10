using Unity.VisualScripting;
using UnityEngine;

namespace RectangleTrainer.Sologram
{
    public class SologramObject : MonoBehaviour
    {
        [SerializeField] private float distance = 5;
        [SerializeField] private SologramRenderer sologram;

        private RenderTexture rt;
        private Camera cam;

        void Start() {
            CreateCamera();
            CreateRenderTexture();
        }

        private void CreateCamera() {
            if (sologram == null) {
                Debug.LogWarning("Attach a sologram renderer");
            }
            GameObject cameraGO = new GameObject();
            cam = cameraGO.AddComponent<Camera>();
            cam.name = "Sologram Cam";
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.clear;
            cam.transform.SetParent(transform);
            cam.transform.localScale = Vector3.one;
            
            cam.cullingMask = 1 << LayerMask.NameToLayer("SologramObject");
        }
        
        public void CreateRenderTexture() {
            rt = new RenderTexture(sologram.Size, sologram.Size, 32);
            cam.targetTexture = rt;
            sologram.Image = rt;
        }

        void Update() {
            if (sologram)
                cam.transform.localPosition = sologram.CamPosition.normalized * distance;
            
            cam.transform.LookAt(transform);
        }
    }
}
