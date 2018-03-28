Shader "TextureCurve/Scaling" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
		_RoughnessTex ("Roughness", 2D) = "white" {}
		_MetallicTex ("Metallic", 2D) = "black" {}
		_CurveTex("Curve", 2D) = "black" {}
		_Speed("Speed", Float) = 1.0
		_Pivot("Pivot", Vector) = (0,0,0,0)

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _RoughnessTex;
		sampler2D _MetallicTex;
		sampler2D _CurveTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		float _Speed;
		float4 _Pivot;

		void vert (inout appdata_full v) 
		{
			float4 worldVertex = mul( unity_ObjectToWorld, v.vertex );
			float4 delta = _Pivot - worldVertex;
			
			float2 texOffset = float2(_Time.y * _Speed, 0.0);
			float4 lut = 1-tex2Dlod (_CurveTex, float4(texOffset,0,0));

			v.vertex.xyz += lut.rgb * delta.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Normal = UnpackNormal (tex2D (_NormalTex, IN.uv_MainTex));
			o.Metallic = tex2D (_MetallicTex, IN.uv_MainTex);
			o.Smoothness = 1-tex2D (_RoughnessTex, IN.uv_MainTex);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
