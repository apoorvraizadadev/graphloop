Shader "Shapes/Rectangle"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Size ("Size", Vector) = (0, 0, 0, 0)
        _MainTex ("Main Texture", 2D) = "" {}
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
            float4 _Size;
            sampler2D _MainTex;

            float BoxSDF(float2 p, float2 b, float r ) 
            {
                float2 q = abs(p)-b+r;
                return min(max(q.x,q.y),0.0) + length(max(q,0.0)) - r;
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
                float2 size = _Size.xy;
                float rad = min(min(_Size.x, _Size.y) / 2, _Size.z);

                float2 transformedCoord = (i.uv - 0.5f) * (size);

                float dst = (BoxSDF(transformedCoord, size / 2, rad));

                float delta = fwidth(dst);
                float alpha = smoothstep(0, 1, -dst * 1 / delta);

                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
