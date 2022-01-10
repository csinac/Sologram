using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.Sologram
{
    public class SologramRenderer : MonoBehaviour
    {
        private const int MAX_GLITCHES = 10;
        
        private Camera mainCam;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private bool realtimeUpdate = false;
        
        [Header("Bands")]
        [SerializeField, Min(0)] private float bandFrequency = 900;
        [SerializeField, Min(0.001f)] private float bandSharpness = 1;
        [SerializeField] private float bandSpeed = -0.25f;
        [Header("Glitches")]
        [SerializeField, Range(0, MAX_GLITCHES)] private int glitchCount = 3;
        [SerializeField, Min(0)] private float glitchFrequency = 0.1f;
        [SerializeField, Range(0, 1)] private float glitchThickness = 0.01f;
        [SerializeField, Range(0.01f, 0.5f)] private float glitchIntensity = 0.1f;
        [Header("Image")]
        [SerializeField] private int size = 512;
        [SerializeField] private Color tint = new Color(0, 0.53f, 1f);
        [SerializeField, Range(0f, 1f)] private float noise = 1f;
        [SerializeField] private float brightness = -0.7f;
        [SerializeField] private float contrast = 2.5f;
        [Header("Vignette")]
        [SerializeField] private float vignetteBoost = 1.2f;
        [SerializeField] private float vignetteInverseScale = 3.4f;

        public Vector3 CamPosition => mainCam.transform.position - transform.position;
        public Texture Image {
            get => rawImage.texture;
            set => rawImage.texture = value;
        }
        public int Size => size;

        private static readonly int GlitchIntensities = Shader.PropertyToID("_GlitchIntensities");
        private static readonly int GlitchPositions = Shader.PropertyToID("_GlitchPositions");
        private static readonly int GlitchCount = Shader.PropertyToID("_GlitchCount");
        private static readonly int GlitchThickness = Shader.PropertyToID("_GlitchThickness");
        
        private static readonly int BandFrequency = Shader.PropertyToID("_BandFrequency");
        private static readonly int BandSharpness = Shader.PropertyToID("_BandSharpness");
        private static readonly int BandSpeed = Shader.PropertyToID("_BandSpeed");
        
        private static readonly int Tint = Shader.PropertyToID("_Color");
        private static readonly int Noise = Shader.PropertyToID("_Noise");
        private static readonly int Brightness = Shader.PropertyToID("_Brightness");
        private static readonly int Contrast = Shader.PropertyToID("_Contrast");
        
        private static readonly int VignetteBoost = Shader.PropertyToID("_VignetteBoost");
        private static readonly int VignetteInverseScale = Shader.PropertyToID("_VignetteInverseScale");

        private float t = 0;

        void Start() {
            mainCam = Camera.main;
            
            rawImage.material.SetFloatArray(GlitchIntensities, new float[MAX_GLITCHES]);
            rawImage.material.SetFloatArray(GlitchPositions, new float[MAX_GLITCHES]);

            UpdateMaterial();
        }

        void Update() {
            if(realtimeUpdate)
                UpdateMaterial();
            
            transform.LookAt(mainCam.transform);
            GenerateGlitch();
        }

        void UpdateMaterial() {
            rawImage.material.SetFloat(BandFrequency, bandFrequency);
            rawImage.material.SetFloat(BandSharpness, bandSharpness);
            rawImage.material.SetFloat(BandSpeed, bandSpeed);
            
            rawImage.material.SetInt(GlitchCount, glitchCount);
            rawImage.material.SetFloat(GlitchThickness, glitchThickness);

            rawImage.material.SetColor(Tint, tint);
            rawImage.material.SetFloat(Noise, noise);
            rawImage.material.SetFloat(Brightness, brightness);
            rawImage.material.SetFloat(Contrast, contrast);
            
            rawImage.material.SetFloat(VignetteBoost, vignetteBoost);
            rawImage.material.SetFloat(VignetteInverseScale, vignetteInverseScale);
        }

        void GenerateGlitch() {
            t += Time.deltaTime;

            if (t < glitchFrequency)
                return;
            t = 0;
            
            if (glitchCount > 0) {
                float[] glitchIntensities = new float[glitchCount];
                float[] glitchPositions = new float[glitchCount];

                for (int i = 0; i < glitchCount; i++) {
                    glitchIntensities[i] = Random.Range(-glitchIntensity, glitchIntensity);
                    glitchPositions[i] = Random.Range(0f, 1f);
                }
            
                rawImage.material.SetFloatArray(GlitchIntensities, glitchIntensities);
                rawImage.material.SetFloatArray(GlitchPositions, glitchPositions);
            }
        }
    }
}
