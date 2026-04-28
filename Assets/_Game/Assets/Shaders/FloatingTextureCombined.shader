Shader "Custom/UI_FloatingTexture_Combined"
{
    Properties
    {
        [MainTexture] _MainTex ("Main Image (Base)", 2D) = "white" {}
        _FloatTex ("Floating Texture (Overlay)", 2D) = "black" {}
        _Fill ("Fill Amount", Range(0,1)) = 1.0
        [Header(Floating Settings)]
        _FloatColor ("Float Color Tint", Color) = (1, 1, 1, 1)
        _ScrollSpeed ("Scroll Speed (X, Y)", Vector) = (0.1, 0.1, 0, 0)
        _FloatDistortion ("Float Depth/Distortion", Range(0, 0.1)) = 0.02
        _PulseSpeed ("Pulse Speed", Float) = 2.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_FloatTex);
            SAMPLER(sampler_FloatTex);

            float4 _FloatColor, _ScrollSpeed;
            float _FloatDistortion, _PulseSpeed;
            float _Fill;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

           half4 frag(Varyings i) : SV_Target 
{
    half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

    float time = _Time.y;

    // =========================
    // FLOAT UV
    // =========================
    float2 floatUV = i.uv;

    floatUV.y += time * _ScrollSpeed.y;
    floatUV.y = frac(floatUV.y);

    float wobble = sin(time * _PulseSpeed + i.uv.y * 10.0) * _FloatDistortion;
    floatUV.x += wobble;

    half4 floatTex = SAMPLE_TEXTURE2D(_FloatTex, sampler_FloatTex, floatUV);

    float appear = smoothstep(0.0, 0.2, floatTex.a);

    // =========================
    // FILL MASK
    // =========================
    float fillMask = step(i.uv.y, _Fill);

    // =========================
    // FLOAT (INDEPENDENT)
    // =========================
    half3 floatLayer = floatTex.rgb * _FloatColor.rgb * appear;

    // =========================
    // BASE ONLY (NO FLOAT HERE)
    // =========================
    half3 baseFilled = mainTex.rgb;
    half3 baseUnfilled = mainTex.rgb * 0.25;

    half3 base = lerp(baseUnfilled, baseFilled, fillMask);

    // =========================
    // FINAL (FLOAT ALWAYS ON TOP)
    // =========================
    half3 finalRGB = base + floatLayer;

    float finalAlpha = lerp(mainTex.a * 0.25, mainTex.a, fillMask);

    return half4(finalRGB, finalAlpha);
}
            ENDHLSL
        }
    }
}