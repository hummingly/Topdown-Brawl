Shader "Hidden/BlurImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_BinaryThreshold("Binary Threshold", float) = 0.5
		_outlineColor("Outline Color", Color) = (1, 1, 1, 1)
		_KernelSize("Kernel Size (N)", Int) = 3
    }

	CGINCLUDE
		
		float remap(float value, float low1, float high1, float low2, float high2) 
		{
			return(low2 + (high2 - low2) * (value - low1) / (high1 - low1));
		}

		float getGrey(sampler2D _MainTex, float2 uv)
		{
			fixed4 col = tex2D(_MainTex, uv);
			return (col.r + col.g + col.b) / 3;
		}

	ENDCG

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

			int _KernelSize;
			float _BinaryThreshold;
			fixed4 _outlineColor;



            fixed4 frag (v2f i) : SV_Target
            {
		
				//https://danielilett.com/2019-05-08-tut1-3-smo-blur/
				// could be made more efficient with 2 passes because blur is linear

				fixed3 sum = fixed3(0.0, 0.0, 0.0);

				// Bounds for kernel
				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					for (int y = lower; y <= upper; ++y)
					{
						fixed2 offset = fixed2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
						sum += tex2D(_MainTex, i.uv + offset);
					}
				}

				//avg
				sum /= (_KernelSize * _KernelSize);

				return fixed4(sum, 1.0);
				//return col;
				



				// Just invert the colors
				/* fixed4 col = tex2D(_MainTex, i.uv);
                 * col.rgb = 1 - col.rgb;
                 * return col;*/
            }

            ENDCG
        }
    }
}
