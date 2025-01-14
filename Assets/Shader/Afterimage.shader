Shader "Custom/Afterimage"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 0)
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
        _Blur ("Blur", Range(1, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                half3 normal : NORMAL;
            };
            
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            half4 _EdgeColor;
            float _Blur;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positions.positionCS;
                OUT.positionWS = positions.positionWS;
                OUT.uv = IN.uv;
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // 深度テクスチャをサンプリング.
                float2 uv = IN.positionCS.xy / _ScaledScreenParams.xy;
                float sceneDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;

                // 深度を線形な値に変換.
                sceneDepth = LinearEyeDepth(sceneDepth, _ZBufferParams);
                float depth = LinearEyeDepth(IN.positionCS.z, _ZBufferParams);
                
                // sceneDepth < depth = 手前にオブジェクトが描画されていたらピクセルを破棄.
                clip(sceneDepth - depth);
                
                // カメラへの方向ベクトルと法線の内積を使って色を補完.
                // 輪郭に近いほどEdgeColorに、遠いほどBaseColorに近づく.
                half3 cameraDirection = normalize(_WorldSpaceCameraPos - IN.positionWS);
                half t = max(0, dot(IN.normal, cameraDirection));
                return lerp(_EdgeColor, _BaseColor, pow(t, _Blur));
            }
            ENDHLSL
        }
    }
}
