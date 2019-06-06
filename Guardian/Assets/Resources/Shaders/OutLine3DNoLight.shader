Shader "Guardian/OutLine3DNoLight" {
	Properties {
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_Outline ("Outline", Range(0, 1)) = 0.1
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
	}
    SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		
		Pass {
			NAME "OUTLINE3D"
			
			Cull Front
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			float _Outline;
			fixed4 _OutlineColor;
			
			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			}; 
			
			struct v2f {
			    float4 pos : SV_POSITION;
			};
			
			v2f vert (a2v v) {
				v2f o;
				
				//把顶点和法线都转到世界空间下
				float4 pos = float4(UnityObjectToViewPos(v.vertex), 1.0);
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  //等价于mul((float3x3)UNITY_MATRIX_MV, v.normal);  

				normal.z = -0.5;
				pos = pos + float4(normalize(normal), 0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target { 
				return float4(_OutlineColor.rgb, 1);               
			}
			
			ENDCG
		}
		
		Pass {
		
			Cull Back
		
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag
			

		
			#include "UnityCG.cginc"
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
		
			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			}; 

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;

			};
			
			v2f vert (a2v v) {
				v2f o;
				
				o.pos = UnityObjectToClipPos( v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}
			
			float4 frag(v2f i) : SV_Target { 
	
				fixed4 c = tex2D (_MainTex, i.uv);
				fixed3 albedo = c.rgb * _Color.rgb;
				fixed3 diffuse =  albedo;
	
				return fixed4(diffuse, 1.0);
				//return fixed4(ambient + diffuse + specular, 1.0);
			}
		
			ENDCG
		}
	}
	FallBack "Diffuse"
}
