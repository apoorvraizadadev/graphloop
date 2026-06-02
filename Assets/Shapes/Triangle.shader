Shader "Shapes/Triangle"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Rad ("Radius", float) = 0
        _Scale ("Scale", float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }

        ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Rad;
            float _Scale;

            float EqTriSDF(float2 p, float r, float2 o, float rad)
            {
                const float k = sqrt(3.0);
                p -= o;
                p.x = abs(p.x);
                p -= float2(0.5,0.5*k)*max(p.x+k*p.y,0.0);
                p -= float2(clamp(p.x,-r,r),-r/k );
                return length(p)*sign(-p.y) - rad;
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 transformedCoord = i.uv * 2 - 1;

                float dst = (EqTriSDF(transformedCoord, _Scale, float2(0, -0.25f) * _Scale, _Rad));

                float delta = fwidth(dst);
                float alpha = smoothstep(0, 1, -dst * 1 / delta);

                return float4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}
