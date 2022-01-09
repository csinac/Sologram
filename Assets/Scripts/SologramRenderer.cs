using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.Sologram
{
    public class SologramRenderer : MonoBehaviour
    {
        private Camera mainCam;
        [SerializeField] private RawImage rawImage;
        [SerializeField, Range(0, 10)] private int glitchCount = 3;
        [SerializeField, Range(0.01f, 0.5f)] private float glitchIntensity = 0.1f;

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

            if (glitchCount > 0) {
                float[] glitchIntensities = new float[glitchCount];
                float[] glitchPositions = new float[glitchCount];

                for (int i = 0; i < glitchCount; i++) {
                    glitchIntensities[i] = Random.Range(-glitchIntensity, glitchIntensity);
                    glitchPositions[i] = Random.Range(0f, 1f);
                }
            
                rawImage.material.SetInt("_GlitchCount", glitchCount);
                rawImage.material.SetFloatArray("_GlitchIntensities", glitchIntensities);
                rawImage.material.SetFloatArray("_GlitchPositions", glitchPositions);
            }
            else {
                rawImage.material.SetInt("_GlitchCount", 0);
            }
        }
    }
}
