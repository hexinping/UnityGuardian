Shader "Guardian/PossEffect/RotationDistortEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "black"{}
	}
	SubShader
	{
		ZTest Always Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float _DistortFactor;	//扭曲强度
			uniform  float4 _DistortCenter;	//扭曲中心点xy值（0-1）屏幕空间
			sampler2D _NoiseTex;
			float _DistortStrength;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//平移坐标点到中心点,同时也是当前像素点到中心的方向
				fixed2 dir = i.uv - _DistortCenter.xy;
				//计算旋转的角度：对于像素点来说，距离中心越远，旋转越少，所以除以距离。相当于用DistortFactor作为旋转的角度值Distort/180 * π，π/180 = 0.1745
				float rot = _DistortFactor * 0.1745 / (length(dir) + 0.001);//+0.001防止除零
				//计算sin值与cos值，构建旋转矩阵
				fixed sinval, cosval;
				sincos(rot, sinval, cosval);
				float2x2  rotmatrix = float2x2(cosval, -sinval, sinval, cosval);
				//旋转
				dir = mul(dir, rotmatrix);
				//再平移回原位置
				dir += _DistortCenter.xy;
				//采样noise图
				fixed4 noise = tex2D(_NoiseTex, i.uv);
				//noise的权重 = 参数 * 距离，越靠近外边的部分，扰动越严重
				float2 noiseOffset = noise.xy * _DistortStrength * dir;
				//用偏移过的uv+扰动采样MainTex
				return tex2D(_MainTex, dir + noiseOffset);

			}
			ENDCG
		}
	}
}
