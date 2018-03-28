Shader "TextureCurve/Displacement" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_CurveTex ("LUT (RGBA)", 2D) = "white" {}
		_Speed ("Speed", Range (0, 10)) = 1.0
		_Amount ("Amount", Vector) = (1,1,1,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _CurveTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Speed;
		float4 _Amount;

		void vert (inout appdata_full v) 
		{
			float2 texcoord = float2(_Time.y * _Speed, 0.0);
			float4 curve = tex2Dlod (_CurveTex, float4(texcoord,0,0));
			v.vertex.xyz += curve.rgb * _Amount.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
