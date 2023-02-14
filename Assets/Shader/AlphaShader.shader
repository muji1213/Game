Shader "Custom/AlphaShader"
{
	Properties{
			_MainTex("Texture", 2D) = "white"{}
			_Alpha("Float with range", Range(0.0, 1.0)) = 1
	}
		SubShader{
			Tags { "Queue" = "Transparent" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard alpha:fade
			#pragma target 3.0

			struct Input {
				float2 uv_MainTex;
			};

			sampler2D _MainTex;

			float _Alpha;
			float _Color;
			void surf(Input IN, inout SurfaceOutputStandard o) {
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
				o.Alpha = _Alpha;
			}
			ENDCG
			}
				FallBack "Diffuse"
}
