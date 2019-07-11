Shader "Unlit/ScreenSpaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
				float aspect = _ScreenParams.x / _ScreenParams.y;
				textureCoordinate.x = textureCoordinate.x * aspect;

				textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
                // sample the texture
                fixed4 col = tex2D(_MainTex, textureCoordinate);
				col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
