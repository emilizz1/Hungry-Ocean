// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TunnelEffect/TunnelCap" {
	Properties {
		_BackgroundColor ("Transition Color", Color) = (1,1,1)
	}
   	SubShader {
       Tags {
	       "Queue"="Geometry+100"
	       "RenderType"="Opaque"
       }
       Cull Off

       Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest
        
		half4  _BackgroundColor;
		
		struct appdata {
			float4 vertex : POSITION;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			
		};
		
		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
    	
		fixed4 frag(v2f i) : SV_Target {
			return _BackgroundColor;
		}
			
		ENDCG
    }
  }  
}