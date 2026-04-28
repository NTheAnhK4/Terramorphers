Shader "Custom/UI_Coin_Shine_Glow"
{
    Properties
    {
        [MainTexture] _MainTex ("Coin Texture", 2D) = "white" {}
        
        [Header(Shine Effect)]
        _ShineColor ("Shine Color", Color) = (1, 1, 1, 1)
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.1
        _ShineSpeed ("Shine Speed", Float) = 1.0
        _ShineInterval ("Interval (Time between shines)", Float) = 3.0
        
        [Header(Glow Effect)]
        _GlowColor ("Base Glow Color", Color) = (1, 0.8, 0, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 2)) = 0.5
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
            
            float4 _ShineColor, _GlowColor;
            float _ShineWidth, _ShineSpeed, _ShineInterval, _GlowIntensity;

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
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // 1. Hiệu ứng Shine (Vệt sáng chéo)
                // Tạo một đường thẳng chéo: x + y (từ dưới trái lên trên phải)
                float projection = i.uv.x + i.uv.y;
                
                // Điều khiển thời gian để có khoảng nghỉ giữa các lần lóe sáng
                float time = _Time.y * _ShineSpeed;
                float cycle = fmod(time, _ShineInterval); 
                
                // Di chuyển vệt sáng từ -1.0 đến 2.0 để nó đi hết hình
                float shinePos = lerp(-1.0, 2.0, cycle / (_ShineInterval * 0.5));
                
                // Dùng hàm Smoothstep để tạo độ nhòe cho vệt sáng
                float shineMask = smoothstep(shinePos - _ShineWidth, shinePos, projection) - 
                                  smoothstep(shinePos, shinePos + _ShineWidth, projection);
                shineMask = saturate(shineMask);

                // 2. Hiệu ứng Glow (Nhấp nháy nhẹ toàn thân)
                float pulse = (sin(_Time.y * 2.0) * 0.5 + 0.5) * _GlowIntensity;
                half3 baseGlow = _GlowColor.rgb * pulse;

                // 3. Kết hợp
                half3 finalRGB = tex.rgb;
                
                // Cộng thêm ánh kim loại khi vệt sáng đi qua
                finalRGB += _ShineColor.rgb * shineMask * tex.a; 
                
                // Cộng thêm một chút glow nền
                finalRGB += baseGlow * tex.a;

                return half4(finalRGB, tex.a);
            }
            ENDHLSL
        }
    }
}