// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Worms"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_PaintMap("Paint Map", 2D) = "white" {}

		_Color("Tint", Color) = (1,1,1,1)
		_OutlineColor("Outline", Color) = (1,1,1,1)
		_MoveTexture("MovementTexture", 2D) = "white" {}
		_MoveFrequency("Move Frequency",Range(0,100)) = 1	
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
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

		Pass
	{
		CGPROGRAM
	#pragma vertex SpriteVert
#pragma fragment Frag
#pragma target 2.0
#pragma multi_compile_instancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

	sampler2D _PaintMap;
	sampler2D _MoveTexture;
	fixed4 _OutlineColor;
	half _MoveFrequency;

	fixed4 Frag(v2f IN) : SV_Target
	{
		half cosTime = cos(_Time.y * _MoveFrequency + cos(IN.texcoord.x * 4) + sin(IN.texcoord.y * 4));
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color * step(0,cosTime) +  tex2D(_MoveTexture, IN.texcoord)* IN.color * step(cosTime,0);		
		fixed4 paint = tex2D(_PaintMap, IN.texcoord);
		c.a *= step(0.1,paint.r);
		c.rgb = _OutlineColor * step(paint.r,0.9) + c.rgb * step(0.9,paint.r);
		c.rgb *= c.a;
		return c;
	}
		ENDCG
	}
	}
}
