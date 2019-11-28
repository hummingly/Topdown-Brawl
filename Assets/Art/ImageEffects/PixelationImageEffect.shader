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

		/*float rand(float2 co) 
		{
			//return frac(sin( dot(co.xy ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
		}*/
		float rand(float2 co)
		{
			float a = 12.9898;
			float b = 78.233;
			float c = 43758.5453;
			float dt = dot(co.xy, float2(a, b));
			float sn = dt % 3.14;
			return frac(sin(sn) * c);
		}

		float hash(float n)
		{
			return frac(sin(n)*43758.5453);
		}

		float noise(float3 x)
		{
			// The noise function returns a value in the range -1.0f -> 1.0f

			float3 p = floor(x);
			float3 f = frac(x);

			f = f * f*(3.0 - 2.0*f);
			float n = p.x + p.y*57.0 + 113.0*p.z;

			return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
				lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
				lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
					lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
		}

		float randomSerieNew2(float x, float freq, float t) {
			//return step(.1, random(floor(x*freq) - floor(t)));
			//return randomNew(x * freq - t);
			return noise(x * freq - t);
		}

		float randomNew(in float x) {
			float x0 = floor(x);
			float x1 = x0 + 1.;
			float v0 = frac(sin(x0*.014686)*31718.927 + x0);
			float v1 = frac(sin(x1*.014686)*31718.927 + x1);

			return (v0*(1. - frac(x)) + v1 * (frac(x)))*2. - 1.*sin(x);
		}

		float randomSerieNew(float x, float freq, float t) {
			//return step(.1, random(floor(x*freq) - floor(t)));
			return randomNew(x * freq - t);
		}


		float random(in float x) {
			return frac(sin(x)*1e4);
		}

		float randomSerie(float x, float freq, float t) {
			//return step(.1, random(floor(x*freq) - floor(t)));
			return smoothstep(.1, .2, random(floor(x*freq) - floor(t)));
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

				//float sine = sin(uv.y * _Time * 1);
				//uv += float2(0, 1) * sine * 1;

				//uv.y += _Time;
				//uv.y += _SinTime;

				//float am = 1;
				//float offset = am * cos(_Time * 1 + uv.x);
				//uv += float2(0, 1) * offset * 1;

				// uv.x verschieben... je nachdem auf welcher y höhe?
				//uv.x += (uv.y * 1)-0.5;

				//float y = cos(_Time * 1 + uv.y) * 100;
				//uv.x += 0.0025f * sin(y);
				// -> sucks, only edges of screen should be affected and not every object
				
				/*float y = cos(_Time * 1 + uv.y) * 100;
				float centerX = uv.x - 0.5f;
				//centerX *= 0.05; // how much the x pos affects distort
				centerX = pow(centerX, 6);
				uv.x += 1.5 * sin(y) * abs(centerX); //less effect clsoer to center.. but maybe none?? cutoff?
				*/

				// *** STILL SUCKS, because objects will always be affected at edges
				// --> DONT CHANGE UV, just change color at edges...

				/*float y = cos(_Time * 1 + uv.y) * 100; //how stretched
				float centerX = (uv.x - 0.5f) * 2; //-1 bis 1 mit 0 at center
				centerX = abs(centerX);
				//centerX = centerX < 0.7 ? 0 : centerX; //where to cut off, so where to start distort at edges
				centerX = centerX < 0.8 ? 0 : centerX; //TODO: smooth cutoff
				uv.x += 0.1 * sin(y) * centerX;*/



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

				// Pixelated
				fixed4 col = tex2D(_MainTex, uv);

				// trippy color change -> col *= uv.y + _SinTime; or col += cos(_Time + uv.y);


				
				// -------- flicker animation from top to bottom, over UVs -------

				/* WORKING SHADER TOY
				void mainImage( out vec4 fragColor, in vec2 fragCoord )
				{
					// Normalized pixel coordinates (from 0 to 1)
					vec2 uv = fragCoord/iResolution.xy;

					// Time varying pixel color
					vec3 col =vec3(0,0,0);
    
					//col += 0.5;
    
					col += 0.5*cos(iTime+uv.y); //+vec3(0,2,4)

					// Output to screen
					fragColor = vec4(col,1.0);
				}
				*/


				//col = col * uv.y;
				//col = uv.y + _Time % 1.5f;

				/*float brigthness = uv.y - 0.5f; //start at top
				brigthness = brigthness % 1;
				brigthness += _SinTime;
				brigthness = smoothstep(0.3f,0.6f,brigthness); // shaper edge
				col += brigthness;*/

				
				/*float brigthness = 0.75f; //0.1f;
				//float t = smoothstep(0.3f, 0.6f, _Time); //sharper edges
				float multi = brigthness * cos(_Time * 40 + uv.y);
				//multi = smoothstep(0.3f, 0.6f, multi); //sharper edges
				col = col + multi;
				col = clamp(col, 0, 1);*/

				// TODO: wie dünnerer streifen?


				

				//uv.y += rand(uv * 0.01);
				//uv.y *= rand(uv * 0.001) * 0.1;
				//float f = rand(float2(uv.x, uv.y + _Time[1])) * .5;
				float f = rand(float2(uv.x, uv.y)) * .5;
				//uv.y += f * .25;

				// Screen scroll white bar
				/*float brigthness = 1.5f; //0.1f;
				float multi = brigthness * cos(_Time * 50 + uv.y);
				multi = clamp(multi, 1, 1.5);
				col = col * multi;*/
				/*float freq = abs(atan(_Time)*.1);// + random(floor(st.y));//how thick lines
				float t = _Time * (1.0 - freq)*400; //how to scroll em
				float val = randomSerie(-uv.y, freq*100, t);
				col = col * (val + 1);*/
				/*float val = 1.1f - randomSerie(-uv.y,  5, _Time * 333);
				val = clamp(val, 1, 100);
				col = col * val;*/
				//float brigthness = 1.6f;//0.45f; //0.5f;
				//float val = 0.45f * randomSerieNew(-uv.y, 1, _Time * 150);
				float val = 1.4f * randomSerieNew2(-uv.y, 0.5f, _Time * 50);
				val = clamp(val, 1, 1.4f); //100f);
				col = col * val;


				
				// TODO: noch horizontales zucken? wirklich uv ändern sucks because changes objects

				// Edge flickering
				float y = cos(_Time * 1 + uv.y) * 100;
				float centerX = uv.x - 0.5f;
				//centerX *= f;
				float size = 6;// *f;
				centerX = pow(centerX, size);
				col += 1.0 * sin(y) * abs(centerX); //less effect clsoer to center.. 

				//col = rand(uv * 0.005) * 1;
				//col = rand(float2(uv.x + _Time[1], uv.y)) * .5 + .5;



				/*
				make both effects more random (meaningfull noise...)
				-> for example roll from top to bottom more delayed sometimes
				-> or also distort roll a bit, so cutoff on y/x like a grid/ delete some lines

				make variables public for better tweaking
				*/





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
