Shader "UI/BorderSweep"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BorderWidth ("Border Width", Range(0.001, 0.1)) = 0.02
        _SweepWidth ("Sweep Width", Range(0.01, 0.3)) = 0.1
        _Speed ("Speed", Float) = 1.0
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _Intensity ("Intensity", Float) = 2.0
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

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            float _BorderWidth;
            float _SweepWidth;
            float _Speed;
            float4 _GlowColor;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            float borderMask(float2 uv, float width)
            {
                float2 d = min(uv, 1 - uv);
                float dist = min(d.x, d.y);
                return smoothstep(width, width * 0.5, dist);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

             
                float edge = 1 - borderMask(i.uv, _BorderWidth);

             
                float t = frac(_Time.y * _Speed);

               
                float pos;
                if (i.uv.y >= 1 - _BorderWidth) pos = i.uv.x; 
                else if (i.uv.x >= 1 - _BorderWidth) pos = 1 + (1 - i.uv.y); 
                else if (i.uv.y <= _BorderWidth) pos = 2 + (1 - i.uv.x); 
                else if (i.uv.x <= _BorderWidth) pos = 3 + i.uv.y; 
                else pos = -1;

                pos /= 4.0;

                float sweep = smoothstep(t, t + _SweepWidth, pos) *
                              (1 - smoothstep(t + _SweepWidth, t + _SweepWidth * 2, pos));

                float glow = edge * sweep * _Intensity;

                col.rgb += _GlowColor.rgb * glow;

                return col;
            }
            ENDCG
        }
    }
}