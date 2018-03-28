Shader "TextureCurve/Flickering" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
		_RoughnessTex ("Roughness", 2D) = "white" {}
		_MetallicTex ("Metallic", 2D) = "black" {}
		_MaskTex ("Mask", 2D) = "black" {}
		_CurveTex("Curve", 2D) = "black" {}
		_Speed("Speed", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _RoughnessTex;
		sampler2D _MetallicTex;
		sampler2D _MaskTex;
		sampler2D _CurveTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;
		float _Speed;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float m = tex2D (_MaskTex, IN.uv_MainTex).r;
			float f = tex2D (_CurveTex, float2(_Time.y * _Speed, 0)).g;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			float scan = lerp(0.2, 1.0, step(frac(IN.worldPos.y * 8.0 + _Time.w * 0.2), 0.5) * 0.85);

			o.Albedo = c.rgb;
			o.Emission = c * m * f * scan * 10;
			o.Normal = UnpackNormal (tex2D (_NormalTex, IN.uv_MainTex));
			o.Metallic = tex2D (_MetallicTex, IN.uv_MainTex);
			o.Smoothness = 1-tex2D (_RoughnessTex, IN.uv_MainTex);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
