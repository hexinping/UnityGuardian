Shader "Guardian/PossEffect/Bloom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Bloom ("Bloom (RGB)", 2D) = "black" {}
		_LuminanceThreshold ("Luminance Threshold", Float) = 0.5
		_BlurSize ("Blur Size", Float) = 1.0
	}
	SubShader
	{
		CGINCLUDE
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			sampler2D _Bloom;
			float _LuminanceThreshold;
			float _BlurSize;

			struct v2f{
				float4 pos: SV_POSITION;
				half2 uv:TEXCOORD0;
			};

			//提取亮度顶点着色器 
			//appdata_img为UINITY提供的内置结构体，包含了顶点坐标和纹理坐标
			v2f vertExtractBright(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;

			}

			fixed luminance(fixed4 color) {
				return  0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b; 
			}

			//提取亮度像素着色器
			fixed4 fragExtractBright(v2f i) : SV_Target {
				fixed4 c = tex2D(_MainTex, i.uv);
				fixed val = clamp(luminance(c) - _LuminanceThreshold, 0.0, 1.0);
				
				return c * val;
			}


			//最后混合bloom效果数据结构
			struct v2fBloom {
				float4 pos : SV_POSITION; 
				half4 uv : TEXCOORD0;
			};

			//最后得到bloom效果的顶点着色器
			v2fBloom vertBloom(appdata_img v) {
				v2fBloom o;
				
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.xy = v.texcoord;		
				o.uv.zw = v.texcoord;
				
				//DirectX平台下，因为Unity开启了抗锯齿,主纹理和亮度纹理在竖直方向上是不一样的，亮部纹理需要翻转Y坐标
				//主纹理调用Graphics.Blit这个方法，Unity默认帮我们做好了
				#if UNITY_UV_STARTS_AT_TOP			
				if (_MainTex_TexelSize.y < 0.0)
					o.uv.w = 1.0 - o.uv.w;
				#endif
					        	
				return o; 
			}
			
			//最后得到bloom效果的像素着色器
			fixed4 fragBloom(v2fBloom i) : SV_Target {
				return tex2D(_MainTex, i.uv.xy) + tex2D(_Bloom, i.uv.zw);
			} 

		ENDCG

		ZTest Always Cull Off ZWrite Off

		//第一个pass 序号为0 提取图片亮度
		Pass {  
			CGPROGRAM  

			//定义使用顶点着色器和像素着色器 
			#pragma vertex vertExtractBright   
			#pragma fragment fragExtractBright  
			
			ENDCG  
		}

		
		//这里使用的是shader名叫“Shaders/Chapter12/GaussianBlur”里的pass名叫“GAUSSIAN_BLUR_VERTICAL”的pass

		//第二个pass, 序号为1 , 使用其他shader里的pass
		UsePass "Guardian/PossEffect/GaussianBlur/GAUSSIAN_BLUR_VERTICAL"
		
		//第三个pass, 序号为2 , 使用其他shader里的pass
		UsePass "Guardian/PossEffect/GaussianBlur/GAUSSIAN_BLUR_HORIZONTAL"

		//第四个pass 序号为3 得到最终的bloom效果
		Pass {  
			CGPROGRAM  
			#pragma vertex vertBloom  
			#pragma fragment fragBloom  
			
			ENDCG  
		}
	}
}
