Shader "Unlit/DeathblowDot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Colour ("Colour", Color) = (1, 0, 0, 0.5) 
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Colour;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				
				col *= _Colour;
                
				float2 spoke = i.uv - float2(0.5, 0.5);
				float dist = length(spoke);
				float t = (_SinTime.w + 1) * 0.25;
				col.a *= dist < t;
				
                return col;
            }
            ENDCG
        }
    }
}
