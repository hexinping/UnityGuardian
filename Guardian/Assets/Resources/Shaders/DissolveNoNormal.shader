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
					float3 normal : NORMAL;
					float4 tangent : TANGENT;
					float4 texcoord: TEXCOORD0;
				};


				struct v2f {
					float4 pos : SV_POSITION;
					float2 uvMainTex : TEXCOORD0;
					float2 uvBurnMap : TEXCOORD1;
					float3 lightDir : TEXCOORD2;
					float3 worldPos : TEXCOORD3;
					fixed3 worldNormal: TEXCOORD4;
					SHADOW_COORDS(5)
				};

				v2f vert(a2v v){
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);

					//各自的纹理坐标
					o.uvMainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uvBurnMap = TRANSFORM_TEX(v.texcoord, _BurnMap);

					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

					o.lightDir = UnityWorldSpaceLightDir(o.worldPos);

  					o.worldNormal = UnityObjectToWorldNormal(v.vertex);
  					

  					//计算声明的阴影纹理坐标
	  					//使用TRANSFER_SHADOW 注意：
	  					// 1 必须保证a2v中顶点坐标名为vertex 
	  					// 2 顶点着色器的输入形参名必须为v
	  					// 3 v2f的顶点变量名必须为pos

  					//总结下：a2v中必须要有vertex表示顶点位置 v2f中必须有pos表是裁剪空间的位置 形参必须得是v
  					TRANSFER_SHADOW(o);
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

					float3 worldLightDir = normalize(o.lightDir);
					fixed3 worldNormal = normalize(o.worldNormal);

					fixed3 albedo = textureColor.rgb;

					//环境光照
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

					//漫反射光照
					fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

					//插值计算燃烧颜色
					fixed t = 1 - smoothstep(0.0, _LineWidth, burn.r - _BurnAmount);
					fixed3 burnColor = lerp(_BurnFirstColor, _BurnSecondColor, t);
					burnColor = pow(burnColor, 5);

					//计算阴影和衰减值
					UNITY_LIGHT_ATTENUATION(atten, o, o.worldPos);

					fixed3 finalColor = lerp(ambient + diffuse * atten, burnColor, t * step(0.0001, _BurnAmount));

					return fixed4(finalColor, 1);

				}

			
			ENDCG
		}

		// Pass to render object as a shadow caster
		Pass {
			Tags { "LightMode" = "ShadowCaster" }
			
			CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				
				#pragma multi_compile_shadowcaster
				
				#include "UnityCG.cginc"
				
				fixed _BurnAmount;
				sampler2D _BurnMap;
				float4 _BurnMap_ST;
				
				struct v2f {
					V2F_SHADOW_CASTER;
					float2 uvBurnMap : TEXCOORD1;
				};
				
				v2f vert(appdata_base v) {
					v2f o;
					
					//使用TRANSFER_SHADOW_CASTER_NORMALOFFSET注意
					// 1 顶点着色器函数形参为v
					// 2 形参类型必须含有vertex 和 normal字段
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					
					o.uvBurnMap = TRANSFORM_TEX(v.texcoord, _BurnMap);
					
					return o;
				}
				
				fixed4 frag(v2f i) : SV_Target {
					fixed3 burn = tex2D(_BurnMap, i.uvBurnMap).rgb;
					
					clip(burn.r - _BurnAmount);
					
					SHADOW_CASTER_FRAGMENT(i)
				}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
