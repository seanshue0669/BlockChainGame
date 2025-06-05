Shader "Custom/HLSL_MaskBlend_WithWear"
{
    Properties
    {
        _MainTex    ("Main Texture",    2D) = "white" {}
        _DirtyTex   ("Dirty Texture",   2D) = "black" {}
        _MaskTex    ("Mask Texture",    2D) = "black" {}
        _Wear       ("Wear Amount",     Range(0,1)) = 0.1
        _Glossiness ("Specular Gloss",  Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);      SAMPLER(sampler_MainTex);      float4 _MainTex_ST;
            TEXTURE2D(_DirtyTex);     SAMPLER(sampler_DirtyTex);     float4 _DirtyTex_ST;
            TEXTURE2D(_MaskTex);      SAMPLER(sampler_MaskTex);      float4 _MaskTex_ST;

            float _Wear;
            float _Glossiness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                float2 uv2        : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float2 uv2         : TEXCOORD1;
                float3 normalWS    : TEXCOORD2;
                float3 positionWS  : TEXCOORD3;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv          = TRANSFORM_TEX(input.uv, _MainTex);
                output.uv2         = TRANSFORM_TEX(input.uv2, _MaskTex);
                output.normalWS    = TransformObjectToWorldNormal(input.normalOS);
                output.positionWS  = TransformObjectToWorld(input.positionOS.xyz); // ? �ץ��o��
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                float4 maskColor = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, input.uv2);
                float4 blendedColor = lerp(baseColor, maskColor, _Strength);

                float threshold = maskColor.r;
                float wearFactor = _Wear * 0.1;

                float4 blendedColor;
                if (threshold < 1e-5 || wearFactor >= threshold)
                {
                    blendedColor = dirtyColor;
                }
                else
                {
                    float blendFactor = saturate(wearFactor / threshold);
                    blendedColor = lerp(cleanColor, dirtyColor, blendFactor);
                }

                float3 normalWS = normalize(input.normalWS);

                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float NdotL = max(0, dot(normalWS, lightDir));
                float3 lambert = blendedColor.rgb * mainLight.color * NdotL;

                float3 viewDir = normalize(_WorldSpaceCameraPos - input.positionWS.xyz);
                float3 halfDir = normalize(lightDir + viewDir);
                float spec = pow(max(0, dot(normalWS, halfDir)), 32.0) * _Glossiness*50;

                if (threshold < 1e-5 || wearFactor >= threshold)
                {
                    spec = 0;
                }

                float3 ambient = SampleSH(normalWS);

                float3 finalColor = lambert + ambient * blendedColor.rgb + spec;

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
