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
            GameObject cameraGO = new GameObject();
            cam = cameraGO.AddComponent<Camera>();
            cam.name = "Sologram Cam";
            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = Color.clear;
            cam.transform.SetParent(transform);
            cam.transform.localScale = Vector3.one;

            rt = new RenderTexture(512, 512, 32);
            cam.targetTexture = rt;
            sologram.Image = rt;
        
            cam.cullingMask = 1 << LayerMask.NameToLayer("SologramObject");
        }

        void Update() {
            cam.transform.localPosition = sologram.CamPosition.normalized * distance;
            cam.transform.LookAt(transform);
        }
    }
}
