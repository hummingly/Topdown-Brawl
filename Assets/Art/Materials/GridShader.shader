Shader "Custom/GridShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DispalceTex("Displacement Texture", 2d) = "white" {}
		_AlphaTex("Alpha Texture", 2d) = "white" {}
		_Color("Color", Color) = (0,0,0,0.5)
		_DisplaceMag("Displacement Magnitude", Range(0,0.1)) = 0.002 //max wiggle distance
		_TotalAlphaMagn("Total Alpha Level", Range(0,1)) = 0.5
		_AlphaScrollSpeed("Alpha Scroll Speed", Range(0,4)) = 0.5 //("Alpha Scroll Speeds", vector) = (-5, -20, 0, 0) //where alpha goes, so moving the alpha texture, not the dispalcement one
		_wiggleSpeed("Wiggle speed", Range(0,2)) = 0.5

			//[MaterialToggle] _isBright("Is Bright", Float) = 0
			_HighlightRange("Highlight Range", Range(0, 10)) = 1.0
			_HotspotIntensity("Hotspot Intensity", Range(0,5)) = 2
			_playerPos("Player Position", vector) = (0, 0, 0, 0)
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
					float2 uv : TEXCOORD0;
				};

				struct v2f //vertex shader output, defines what information passed to fragment function
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;

					float3 worldPos : TEXCOORD1;
				};

				/*struct Input
				{
					//float2 uv_MainTex;
					float3 playerPos;
				};*/

				float _AlphaScrollSpeed; //float4 _AlphaScrollSpeed;
				float4 _AlphaTex_ST;



				v2f vert(appdata v) //Vertex function (local coordinates), position etc locked in
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex); //goes from world coords to screen position


					o.worldPos = mul(unity_ObjectToWorld, v.vertex);

					o.uv = v.uv;  //just passes on uv
					// instead of for each sprite, do it across the screen.. or world
					//o.uv = o.worldPos.xy / o.worldPos.w;

					/*o.uv = TRANSFORM_TEX(v.uv, _AlphaTex);

					o.uv += _AlphaScrollSpeeds * _Time.x; // Shift the uvs over time.

					UNITY_TRANSFER_FOG(o,o.vertex);*/

					/*v.uv2.x += _Time * 20;
					v.uv2.y += _Time * 20;
					o.uv2 = v.uv2;*/
					//o.uv.xy = v.uv.xy + frac(_Time.y * float2(2, 2));

					return o;
				}

				sampler2D _MainTex;
				sampler2D _DispalceTex;
				sampler2D _AlphaTex;
				float4 _Color;
				float _DisplaceMag;
				float _TotalAlphaMagn;  //used to make the grid pulsate over time (change whole alpha)
				float _MoveDisplaceTex; //used to make the mroe and less transparent spots on the grid all over the place
				float _wiggleSpeed;
				float _HighlightRange;
				//vector _playerPos;
				float _HotspotIntensity;
				vector _playerPos;


	

				float4 frag(v2f i) : SV_Target //Fragment function (pixels) turns pixels on the screen into a color
				{
					// instead of for each sprite, do it across the screen.. or world
					//i.uv = i.worldPos.xy / i.worldPos.w;//i.screenPos.xy / i.screenPos.w;
					//i.uv = i.worldPos.xy * scale;

					// Displacement (over time)
					float2 distuv = float2(i.uv.x + _Time.x * _wiggleSpeed, i.uv.y + _Time.x * _wiggleSpeed);

					float2 posDisp = tex2D(_DispalceTex, distuv).xy;
					posDisp = ((posDisp * 2) - 1) * _DisplaceMag; //remaps from 0...1 to -1...1 and then multiplies with the magnitude modifier

					float4 color = tex2D(_MainTex, i.uv + posDisp); //get the pixel color at the procided UV value (the 4 vertexes with linear interpolation) now with plugged in displacement via random texture


					// Alpha stuff

					float2 distAl = float2(i.uv.x + _Time.x * _AlphaScrollSpeed, i.uv.y + _Time.x * _AlphaScrollSpeed);  //changes the displacement over time

					//increase alph depending on player posD
					//float2 screenUV = (i.screenPos.xy / i.screenPos.z) * 0.5f + 0.5f;
					float dist = distance(i.worldPos, _playerPos); //i.screenPos    i.vertex  _WorldSpaceCameraPos
					if (dist < _HighlightRange) _TotalAlphaMagn += (_HighlightRange - dist) / _HotspotIntensity; //the nearer the brigther
					//if(i.uv.x > 0.5f) _TotalAlphaMagn = 0.5f;
					//i.uv.x and y are between 0 and 1 the corners of image

					float2 alphaDisp = tex2D(_AlphaTex, i.uv + distAl).xy;
					alphaDisp = alphaDisp * _TotalAlphaMagn;

					color.a = color.a * alphaDisp;

					if (color.a >= 1) color.a = 1; //avoid ugly overflow

					return color;
				}
				ENDCG
			}
		}
}