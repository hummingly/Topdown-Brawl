Shader "Hidden/PixelationImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelAmmountL("Pixel Ammount (lowest)", float) = 128 //how many pixels at lowest ortho
		_PixelAmmountH("Pixel Ammount (highest)", float) = 256 //how many pixels at highest ortho
		_OrthoMin("Min Ortho Scale", float) = 5
		_OrthoMax("Max Ortho Scale", float) = 20
		[HideInInspector] _OrthoScale("current scale", float) = 5
		[HideInInspector] _AspectRatio("This text isn't shown", float) = 1.7
    }

	CGINCLUDE
		
		float remap(float value, float low1, float high1, float low2, float high2) 
		{
			return(low2 + (high2 - low2) * (value - low1) / (high1 - low1));
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

			float _AspectRatio;
			float _PixelAmmountL;
			float _PixelAmmountH;
			float _OrthoMin;
			float _OrthoMax;
			float _OrthoScale;


            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;

				// Make sure the pixels are squares
				float pixelAmmount = remap(_OrthoScale, _OrthoMin, _OrthoMax, _PixelAmmountL, _PixelAmmountH);
				float xPix = pixelAmmount * _AspectRatio;
				float yPix = pixelAmmount;

				// UV not between 0 and 1 anymore, but 0 and _Columns
				uv.x *= xPix;
				uv.y *= yPix;
                
				// Scale back down with lower res
				uv.x = round(uv.x);
				uv.y = round(uv.y);
				uv.x /= xPix;
				uv.y /= yPix;

				fixed4 col = tex2D(_MainTex, uv);

				// HERE outline before... had different effect when was in same shader?
				// WELL now the script order matters, but this pixel script at the bottom of camera object (https://answers.unity.com/questions/1085180/chain-multiple-custom-camera-image-effects.html)

				return col;

				// Just invert the colors
				/* fixed4 col = tex2D(_MainTex, i.uv);
                 * col.rgb = 1 - col.rgb;
                 * return col;*/
            }

            ENDCG
        }
    }
}
