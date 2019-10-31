Shader "Hidden/PixelationImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelAmmount("Pixel Ammount", float) = 128
		[HideInInspector] _AspectRatio("This text isn't shown", float) = 1.7
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

			float _AspectRatio;
			float _PixelAmmount;


            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;

				// Make sure the pixels are squares
				float xPix = _PixelAmmount * _AspectRatio;
				float yPix = _PixelAmmount;

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
