Shader "RectangleTrainer/Sologram"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BandFrequency("Band Frequency", Float) = 200
        _BandSharpness("Band Sharpness", Range(0.01, 10)) = 1
        _BandSpeed("Band Speed", Float) = 1
        _MinimumAlpha("Minimum Alpha", Range(0, 0.99)) = 0.1
        _Color ("Tint", Color) = (1,1,1,1)
        _Noise("Noise", Range(0, 1)) = 1
        _VignetteBoost("Vignette Boost", Float) = 1
        _VignetteInverseScale("Vignette Inverse Scale", Float) = 3

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One One
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float _BandFrequency;
            float _BandSharpness;
            fixed _BandSpeed;
            fixed _MinimumAlpha;

            fixed _Noise;

            int _GlitchCount;
            float _GlitchThickness;
            float _GlitchIntensities[20];
            float _GlitchPositions[20];

            float _VignetteBoost;
            float _VignetteInverseScale;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            //https://gist.github.com/h3r/3a92295517b2bee8a82c1de1456431dc
            float rand(float2 seed, float3 dotDir = float3(12.9898, 78.233, 37.719)){
                float3 timedSeed;
                timedSeed.xy = seed;
                timedSeed.z = _Time;
	            float3 smallValue = sin(timedSeed);
	            float random = dot(smallValue, dotDir);
	            random = frac(sin(random) * 143758.5453) - 0.5;
	            return random;
            }

            float SND(float x) {
                return pow(2.71828, -x*x * 2);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;

                for(int i = 0; i < _GlitchCount; i++)
                {
                    float dist = abs(uv.y - _GlitchPositions[i]);
                    if(dist < _GlitchThickness)
                    {
                        float glitchFade = (_GlitchThickness - dist) / _GlitchThickness;
                        glitchFade = pow(glitchFade, 2);
                        uv.x += _GlitchIntensities[i] * glitchFade;
                    }
                }
                
                half4 pixel = tex2D(_MainTex, uv);
                
                half4 color = ((pixel + _TextureSampleAdd) + IN.color);
                color = clamp(color + rand(IN.texcoord) * _Noise, 0, 1);
                color *= pixel.a;

                float bandAlpha = (sin((IN.texcoord.y + _Time * _BandSpeed) * _BandFrequency) + 1) / 2;
                bandAlpha = pow(bandAlpha, _BandSharpness);
                bandAlpha = (1-_MinimumAlpha) * bandAlpha + _MinimumAlpha;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                float2 vignetteUV = IN.texcoord;
                vignetteUV -= 0.5;
                vignetteUV *= _VignetteInverseScale;

                float middleDistance = sqrt(vignetteUV.x * vignetteUV.x + vignetteUV.y * vignetteUV.y);
                middleDistance = SND(middleDistance);
                middleDistance = clamp(middleDistance * _VignetteBoost, 0, 1);
                
                return color * bandAlpha * middleDistance;
            }
        ENDCG
        }
    }
}
