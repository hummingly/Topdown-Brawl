Shader "Hidden/OutlineImageEffect"
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

		float toGrey(fixed3 col)
		{
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
				float2 uv = i.uv;


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

				if (avg.r < 0.5)
				{
					if (avgCol < 0.4 && avgCol > 0.3)
						col = outlineC;
				} */


				/*
				fixed4 up = tex2D(_MainTex, uv + fixed2(0, _MainTex_TexelSize.y));
				fixed4 down = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y));
				fixed4 right = tex2D(_MainTex, uv + fixed2(_MainTex_TexelSize.x, 0));
				fixed4 left = tex2D(_MainTex, uv - fixed2(_MainTex_TexelSize.x, 0));

				fixed4 avg = (up + down + right + left) / 4;
				fixed avgCol = (avg.r + avg.g + avg.b) / 3;

				if (avgCol < 0.4 && avgCol > 0.3)
					col = outlineC;
				*/


				/*
				// to binary for easier edge detection?
				fixed myGrey = (col.r + col.g + col.b) / 3;

				//if (myGrey > _BinaryThreshold) col = 1;
				//else col = 0;

				// if this pixel would be black and one around me would be white, make this the outline color
				if (myGrey < _BinaryThreshold)
				{
					fixed upGrey = getGrey(_MainTex, uv + fixed2(0, _MainTex_TexelSize.y));
					fixed downGrey = getGrey(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y));
					fixed rightGrey = getGrey(_MainTex, uv + fixed2(_MainTex_TexelSize.x, 0));
					fixed leftGrey = getGrey(_MainTex, uv - fixed2(_MainTex_TexelSize.x, 0));

					fixed upRight = getGrey(_MainTex, uv + fixed2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
					fixed upLeft = getGrey(_MainTex, uv + fixed2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y));
					fixed downRight = getGrey(_MainTex, uv + fixed2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y));
					fixed downLeft = getGrey(_MainTex, uv + fixed2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y));


					if(upGrey > _BinaryThreshold || downGrey > _BinaryThreshold || leftGrey > _BinaryThreshold || rightGrey > _BinaryThreshold
						|| upRight > _BinaryThreshold || upLeft > _BinaryThreshold || downRight > _BinaryThreshold || downLeft > _BinaryThreshold)
						col = 0;
				}

				return col;
				*/

				

				



				fixed sum = 0;

				// Bounds for kernel
				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					for (int y = lower; y <= upper; ++y)
					{
						fixed2 offset = fixed2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
						fixed3 color = tex2D(_MainTex, i.uv + offset);
						fixed grey = toGrey(color);
						//grey = grey - _BinaryThreshold;

						if (grey > _BinaryThreshold) grey = 1;
						else grey = 0;
						sum += grey;

						//if (toGrey(tex2D(_MainTex, i.uv + offset)) < _BinaryThreshold)
							//return outlineC;
					}
				}


				fixed myGrey = (col.r + col.g + col.b) / 3;

				if (sum > 0 && myGrey < _BinaryThreshold)
					return outlineC;

				return col;
				

				//TODO: Also when using extra camera set ortho? In effectscript



				// Just invert the colors
				/* fixed4 col = tex2D(_MainTex, i.uv);
                 * col.rgb = 1 - col.rgb;
                 * return col;*/
            }

            ENDCG
        }
    }
}
