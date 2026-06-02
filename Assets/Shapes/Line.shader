Shader "Shapes/Line"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
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
            float _Length;
            float _Thickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float dstToHorizontalSegment(float2 p) 
            {
                p.x -= min(_Length, max(0, p.x));
                return length(p);
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 coords = float2(i.uv.x * (_Length + _Thickness) - (_Thickness / 2), (i.uv.y - 0.5) * _Thickness);
                float dst = dstToHorizontalSegment(coords)  / (_Thickness/2);

                float delta = fwidth(dst);
                float alpha = 1 - smoothstep(0.5f - delta, 0.5f + delta, dst * dst);

                return float4(_Color.xyz, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
