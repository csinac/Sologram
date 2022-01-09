using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.Sologram
{
    public class SologramRenderer : MonoBehaviour
    {
        private const int MAX_GLITCHES = 10;
        
        private Camera mainCam;
        [SerializeField] private RawImage rawImage;
        [SerializeField, Range(0, MAX_GLITCHES)] private int glitchCount = 3;
        [SerializeField, Min(0)] private float glitchFrequency = 0.1f;
        [SerializeField, Range(0, 1)] private float glitchThickness = 0.01f;
        [SerializeField, Range(0.01f, 0.5f)] private float glitchIntensity = 0.1f;

        public Vector3 CamPosition => mainCam.transform.position - transform.position;
        public Texture Image {
            get => rawImage.texture;
            set => rawImage.texture = value;
        }

        private float t = 0;
        private static readonly int GlitchIntensities = Shader.PropertyToID("_GlitchIntensities");
        private static readonly int GlitchPositions = Shader.PropertyToID("_GlitchPositions");
        private static readonly int GlitchCount = Shader.PropertyToID("_GlitchCount");
        private static readonly int GlitchThickness = Shader.PropertyToID("_GlitchThickness");

        void Start() {
            mainCam = Camera.main;

            rawImage.material.SetFloatArray(GlitchIntensities, new float[MAX_GLITCHES]);
            rawImage.material.SetFloatArray(GlitchPositions, new float[MAX_GLITCHES]);
        }

        void Update() {
            t += Time.deltaTime;
            transform.LookAt(mainCam.transform);

            if (t < glitchFrequency)
                return;
            t = 0;
            
            if (glitchCount > 0) {
                Debug.Log(glitchCount);
                float[] glitchIntensities = new float[glitchCount];
                float[] glitchPositions = new float[glitchCount];

                for (int i = 0; i < glitchCount; i++) {
                    glitchIntensities[i] = Random.Range(-glitchIntensity, glitchIntensity);
                    glitchPositions[i] = Random.Range(0f, 1f);
                }
            
                rawImage.material.SetInt(GlitchCount, glitchCount);
                rawImage.material.SetFloat(GlitchThickness, glitchThickness);
                rawImage.material.SetFloatArray(GlitchIntensities, glitchIntensities);
                rawImage.material.SetFloatArray(GlitchPositions, glitchPositions);
            }
            else {
                rawImage.material.SetInt(GlitchCount, 0);
            }
        }
    }
}
