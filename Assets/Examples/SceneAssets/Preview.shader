Shader "Hidden/Preview"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed ("Speed", Range (0, 10)) = 1.0
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

			float _Speed;
			
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
				float2 texOffset = float2(_Time.y * _Speed - 0.5, 0.0);
				fixed4 col = tex2D(_MainTex, i.uv + texOffset);
				
				if(i.uv.x > 0.49 && i.uv.x < 0.51)
					col = float4(1.0,0.0,0.0,1.0);

				return col;
			}
			ENDCG
		}
	}
}
