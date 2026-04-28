Shader "Custom/UI_HealthBar_FixedBorder"
{
    Properties
    {
        [MainTexture] _MainTex ("Bar Texture", 2D) = "white" {}
        _Health ("Health", Range(0, 1)) = 1.0
        
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _EmptyColor ("Inner Background Color", Color) = (0,0,0,0.25)
        
        _BorderThickness ("Border Thickness", Range(0, 0.5)) = 0.05
        _AspectRatio ("Width / Height Ratio", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float _Health, _BorderThickness, _AspectRatio;
            float4 _BorderColor, _EmptyColor;

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes v) {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

           half4 frag(Varyings i) : SV_Target {
    float2 uv = i.uv;

    // 🔥 Tính pixel size theo màn hình (auto theo scale)
    float2 pixelSize = fwidth(uv);

    // Border thickness theo pixel (ổn định dù scale)
    float2 thickness = pixelSize * (_BorderThickness * 100);

    // Mask border
    float2 borderMask2D = step(thickness, uv) * step(uv, 1.0 - thickness);
    float isInside = borderMask2D.x * borderMask2D.y;

    // Remap UV vào vùng trong
    float2 innerUV = (uv - thickness) / (1.0 - 2.0 * thickness);

    // Sample texture
    half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, innerUV);

    // Màu máu
    half4 colorRed = half4(1, 0.1, 0.1, 1);
    half4 colorGreen = half4(0.1, 1, 0.1, 1);
    half4 healthColor = lerp(colorRed, colorGreen, smoothstep(0.2, 0.7, _Health));

    // Mask theo health
    float healthMask = step(innerUV.x, _Health);
    half4 innerContent = lerp(_EmptyColor, half4(tex.rgb * healthColor.rgb, tex.a), healthMask);

    // Final
    return lerp(_BorderColor, innerContent, isInside);
}
            ENDHLSL
        }
    }
}