Shader "Guardian/DissolveNoNormal"
{
	Properties
	{
		//消融程度
		_BurnAmount ("Burn Amount", Range(0.0,1.0)) = 0.0
		//燃烧时线宽
		_LineWidth ("Burn Line Width", Range(0.0,0.5)) = 0.2

		//漫反射贴图
		_MainTex ("Base (RGB)", 2D) = "white" {}
	
		

		//火焰边缘的两种颜色
		_BurnFirstColor ("Burn First Color", Color) = (1,0,0,1)
		_BurnSecondColor ("Burn Second Color", Color) = (1,0,0,1)

		//噪点图
		_BurnMap("Burn Map", 2D) = "white"{}
	}

	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			Cull off //关闭背面剔除

			CGPROGRAM
			
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				#pragma multi_compile_fwdbase

				#pragma vertex vert
				#pragma fragment frag


				fixed _BurnAmount;
				fixed _LineWidth;
				sampler2D _MainTex;
	
				fixed4 _BurnFirstColor;
				fixed4 _BurnSecondColor;
				sampler2D _BurnMap;
				
				//对应的纹理坐标
				float4 _MainTex_ST;
				float4 _BurnMap_ST;

				struct a2v{
					float4 vertex : POSITION;
					float4 texcoord: TEXCOORD0;
				};


				struct v2f {
					float4 pos : SV_POSITION;
					float2 uvMainTex : TEXCOORD0;
					float2 uvBurnMap : TEXCOORD1;
				};

				v2f vert(a2v v){
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);

					//各自的纹理坐标
					o.uvMainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uvBurnMap = TRANSFORM_TEX(v.texcoord, _BurnMap);

					return o;

				}

				fixed4 frag(v2f o):SV_Target{
					
					//透明图直接丢弃
					fixed4 textureColor = tex2D(_MainTex, o.uvMainTex);
					float a = textureColor.a;
					clip (a - 0.1);
				
					//得到噪点值
					fixed3 burn = tex2D(_BurnMap, o.uvBurnMap).rgb;

					//如果 如果burn.r<_BurnAmount 就丢弃
					clip(burn.r - _BurnAmount-0.1);

					//插值计算燃烧颜色
					fixed t = 1 - smoothstep(0.0, _LineWidth, burn.r - _BurnAmount);
					fixed3 burnColor = lerp(_BurnFirstColor, _BurnSecondColor, t);
					burnColor = pow(burnColor, 5);

					fixed3 finalColor = lerp(textureColor, burnColor, t * step(0.0001, _BurnAmount));

					return fixed4(finalColor, 1);

				}

			
			ENDCG
		}

	}
	FallBack "Diffuse"
}
