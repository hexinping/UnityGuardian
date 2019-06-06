Shader "Guardian/PossEffect/RadialBlurEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurTex("Blur Tex", 2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		
		LOD 100

		ZTest Always Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			uniform sampler2D _BlurTex;
			uniform float _BlurFactor;	//模糊强度（0-0.05）
			uniform float _LerpFactor;  //插值的强度（0-1）
			uniform float4 _BlurCenter; //模糊中心点xy值（0-1）屏幕空间
			
			#define SAMPLE_COUNT 6		//迭代次数
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				////dx中纹理从左上角为初始坐标，需要反向(在写rt的时候需要注意)
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1 - o.uv.y;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//模糊方向为模糊中点指向边缘（当前像素点），而越边缘该值越大，越模糊
				float2 dir = i.uv - _BlurCenter.xy;
				float4 outColor = 0;

				//计算采样uv值：正常uv值+从中间向边缘逐渐增加的采样距离
				float2 uv = i.uv - _BlurFactor * dir * 0;
				outColor += tex2D(_MainTex, uv);

				uv = i.uv - _BlurFactor * dir * 1;
				outColor += tex2D(_MainTex, uv);

				uv = i.uv - _BlurFactor * dir * 2;
				outColor += tex2D(_MainTex, uv);

				uv = i.uv - _BlurFactor * dir * 3;
				outColor += tex2D(_MainTex, uv);

				uv = i.uv - _BlurFactor * dir * 4;
				outColor += tex2D(_MainTex, uv);

				uv = i.uv - _BlurFactor * dir * 5;
				outColor += tex2D(_MainTex, uv);

				//取平均值
				outColor /= SAMPLE_COUNT;
				
				return outColor;
			}
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			uniform sampler2D _BlurTex;
			uniform float _BlurFactor;	//模糊强度（0-0.05）
			uniform float _LerpFactor;  //插值的强度（0-1）
			uniform float4 _BlurCenter; //模糊中心点xy值（0-1）屏幕空间
			
			#define SAMPLE_COUNT 6		//迭代次数
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//dx中纹理从左上角为初始坐标，需要反向(在写rt的时候需要注意)
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1 - o.uv.y;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 dir = i.uv - _BlurCenter.xy;
				float dis = length(dir);
				fixed4 oriTex = tex2D(_MainTex, i.uv);
				fixed4 blurTex = tex2D(_BlurTex, i.uv);
				//按照距离乘以插值系数在原图和模糊图之间差值
				return lerp(oriTex, blurTex, _LerpFactor * dis);
	
			}
			ENDCG
		}
	}
}
