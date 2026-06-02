Shader "Shapes/Circle"
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 centerCoords = (i.uv - 0.5) * 2;
                float sqrDistance = dot(centerCoords, centerCoords);
                float distance = sqrt(sqrDistance);

                float delta = fwidth(distance);
				float alpha = 1 - smoothstep(1 - delta * 2, 1, distance * distance);

                return float4(_Color.xyz, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
