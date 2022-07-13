Shader "pp/ScreenEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			static float r = 1;

			fixed luminance(fixed3 col)
			{
				return col.x * 0.299 + col.y * 0.587 + col.z * 0.114;
			}

			fixed4 frag (v2f i) : SV_Target
			{
			
				fixed3 col = tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,-1)).rgb * -1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,-1)).rgb * -2;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,-1)).rgb * -1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,0)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,0)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,0)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,1)).rgb * 1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,1)).rgb * 2;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,1)).rgb * 1;

				fixed gray = abs(luminance(col));

				col = tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,-1)).rgb * -1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,-1)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,-1)).rgb * 1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,0)).rgb * -2;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,0)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,0)).rgb * 2;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(-1,1)).rgb * -1;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(0,1)).rgb * 0;
				col += tex2D(_MainTex,i.uv + _MainTex_TexelSize.xy*r*fixed2(1,1)).rgb * 1;

				gray += abs(luminance(col));

				fixed3 enhance = fixed3(gray, gray, gray);

				//fixed3 ori = tex2D(_MainTex, i.uv).rgb + enhance * 0.5;

				return fixed4(enhance, 1);
			}

			ENDCG
		}

		
	}

	FallBack Off
}
