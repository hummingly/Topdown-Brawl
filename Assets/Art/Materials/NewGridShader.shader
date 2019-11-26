// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GridShader"
{
	Properties
	{
		[PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}

		_HighlightRange("Highlight Range", Range(0, 10)) = 1.0
		_HotspotIntensity("Hotspot Intensity", Range(0,10)) = 2
		_playerPos("Player Position", vector) = (0, 0, 0, 0) //to light one object on grid
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"PreviewType" = "Plane"
			}
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata //defines what vertex data we get from the mesh (list in untiy docs)
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
				};

				struct v2f //vertex shader output, defines what information passed to fragment function
				{
					float4 color : COLOR;
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;

					float3 worldPos : TEXCOORD1;
				};



				v2f vert(appdata v) //Vertex function (local coordinates), position etc locked in
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex); //goes from world coords to screen position
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					o.uv = v.uv;
					o.color = v.color;

					return o;
				}


				sampler2D _MainTex;
				float _HighlightRange;
				float _HotspotIntensity;
				vector _playerPos;


				uniform int maxLightingObjects = 1000;
				uniform vector lightingObjects[1000]; //carefull, size cant be changed
				//float highlightRanges[1000]; 
				//float intensities[1000];

				float totalBrigthness = 0;


				float4 frag(v2f i) : SV_Target //Fragment function (pixels) turns pixels on the screen into a color
				{
					float4 color = tex2D(_MainTex, i.uv) *i.color;

					//TODO: instead of looping in shader write a texture from monobehaviour and pass this on to shader
					for (int j = 0; j < maxLightingObjects; j++)
					{
						if (lightingObjects[j].x != 0 && lightingObjects[j].y != 0) //prevents flicker at center, but when shooting its still too slow
						{
							float dist = distance(i.worldPos, lightingObjects[j].xy);
							if (dist < _HighlightRange)
							{
								color.rgb += (_HighlightRange - dist) / _HotspotIntensity; //the nearer the brigther
								//break; //no overlapping lights?
							}
						}
					}

					color.rgb += totalBrigthness;

					//float dist = distance(i.worldPos, _playerPos);
					//if (dist < _HighlightRange) 
					//	color.rgb += (_HighlightRange - dist) / _HotspotIntensity; //the nearer the brigther

					//color.rgb *= 1.2;


					//if (color.a >= 1) color.a = 1; //avoid ugly overflow

					return color;
				}
				ENDCG
			}
		}
}