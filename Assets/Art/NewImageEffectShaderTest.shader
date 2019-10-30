Shader "Hidden/NewImageEffectShaderTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PixelAmmount("Pixel Ammount", float) = 128
		_outlineColor("Outline Color", Color) = (1, 1, 1, 1)
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

			fixed4 _outlineColor;

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




				// Make outline alpha changeable
				half4 outlineC = _outlineColor;
				outlineC.a *= ceil(col.a);
				outlineC.rgb *= outlineC.a;

				// check alpha of pixels around me
				// if any are 0 then make it outline color here
				/*fixed upAlpha = tex2D(_MainTex, uv + fixed2(0, _MainTex_TexelSize.y)).a;
				fixed downAlpha = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y)).a;
				fixed rightAlpha = tex2D(_MainTex, uv + fixed2(_MainTex_TexelSize.x, 0)).a;
				fixed leftAlpha = tex2D(_MainTex, uv - fixed2(_MainTex_TexelSize.x, 0)).a;

				// if any of the above are not 0 then do outline color
				col = lerp(col, outlineC, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));*/
				

				//fixed colorToOutline = 1;
				//fixed testRed = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y)).r; //r,g,b doesn't matter since currently all grey values in scene
				//col = step(0.8, testRed) - step(1, testRed);

				/*
				// for now: just look at lower pixel
				fixed downBlu = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y)).b; //r,g,b doesn't matter since currently all grey values in scene
																						 //well, if post processing is on it matters: blue edges for example
				// DON'T USE IFS
				//if (downBlu < 0.5 && downBlu > 0.4)
				if (downBlu < 0.33 && downBlu > 0.3)
					col = outlineC;
				*/

				/*
				fixed4 up = tex2D(_MainTex, uv + fixed2(0, _MainTex_TexelSize.y));
				fixed4 down = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y));
				fixed4 right = tex2D(_MainTex, uv + fixed2(_MainTex_TexelSize.x, 0));
				fixed4 left = tex2D(_MainTex, uv - fixed2(_MainTex_TexelSize.x, 0));

				fixed4 avg = (up + down + right + left) / 4;
				fixed avgCol = (avg.r + avg.g + avg.b) / 3;

				// DON'T USE IFS
				if (avgCol < 0.4 && avgCol > 0.3)
					col = outlineC;*/


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
