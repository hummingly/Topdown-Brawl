// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)


Shader "Sprites/SpriteDefaultHDR"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[HDR]_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		//[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Int) = 3
		//[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Int) = 3

		//_Stencil("Stencil ID", Float) = 0
		//_StencilWriteMask("Stencil Write Mask", Float) = 255
		//_StencilReadMask("Stencil Read Mask", Float) = 255

		//_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		//ZTest[unity_GUIZTestMode]
		//Blend SrcAlpha OneMinusSrcAlpha
		//ColorMask[_ColorMask]
		
		BlendOp Max

		Pass
		{
		/*Stencil
		{
			//Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			//ReadMask[_StencilReadMask]
			//WriteMask[_StencilWriteMask]
		}*/


		/*Stencil {
				Ref 0
				Comp Equal
				Pass IncrSat
				Fail IncrSat
			}*/
		CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment SpriteFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile_local _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"
		ENDCG
		}
	}
}