// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Burn"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_EmberColorTint1("Ember Color Tint", Color) = (0.9926471,0.6777384,0,1)
		_GlowEmissionMultiplier1("Glow Emission Multiplier", Range( 0 , 30)) = 1
		_GlowColorIntensity1("Glow Color Intensity", Range( 0 , 10)) = 2
		_BurnOffset1("Burn Offset", Range( 0 , 5)) = 1
		_CharcoalNormalTile1("Charcoal Normal Tile", Range( 2 , 5)) = 5
		_BurnTilling1("Burn Tilling", Range( 0.1 , 1)) = 1
		_GlowBaseFrequency1("Glow Base Frequency", Range( 0 , 5)) = 1.1
		_GlowOverride1("Glow Override", Range( 0 , 10)) = 1
		_Masks1("Masks", 2D) = "white" {}
		_BurntTileNormals1("Burnt Tile Normals", 2D) = "white" {}
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _Masks1;
			uniform float _BurnOffset1;
			uniform float _BurnTilling1;
			uniform float4 _EmberColorTint1;
			uniform float _GlowColorIntensity1;
			uniform float _GlowBaseFrequency1;
			uniform float _GlowOverride1;
			uniform sampler2D _BurntTileNormals1;
			uniform float _CharcoalNormalTile1;
			uniform float _GlowEmissionMultiplier1;
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv021 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner27 = ( _BurnOffset1 * float2( 1,0.5 ) + ( uv021 * _BurnTilling1 ));
				float4 tex2DNode28 = tex2D( _Masks1, panner27 );
				float2 uv024 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode29 = tex2D( _BurntTileNormals1, ( uv024 * _CharcoalNormalTile1 ) );
				float4 temp_cast_0 = (0.0).xxxx;
				float4 temp_cast_1 = (100.0).xxxx;
				float4 clampResult56 = clamp( ( ( tex2DNode28.r * ( ( ( ( _EmberColorTint1 * _GlowColorIntensity1 ) * ( ( sin( ( _Time.y * _GlowBaseFrequency1 ) ) * 0.5 ) + ( _GlowOverride1 * ( tex2DNode28.r * tex2DNode29.a ) ) ) ) * tex2DNode28.g ) * tex2DNode29.a ) ) * _GlowEmissionMultiplier1 ) , temp_cast_0 , temp_cast_1 );
				float2 uv_TextureSample1 = IN.texcoord.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				
				half4 color = ( clampResult56 + ( tex2D( _TextureSample1, uv_TextureSample1 ) * ( 1.0 - tex2DNode29.a ) ) );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18000
478;69;1920;1133;-163.3448;537.5784;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;19;-3088.621,-1376.939;Inherit;False;1100.833;414.0387;Comment;6;28;27;23;22;21;20;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-3038.621,-1198.367;Float;False;Property;_BurnTilling1;Burn Tilling;5;0;Create;True;0;0;False;0;1;0.179;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2987.12,-1326.284;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-2712.662,-1326.939;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3037.669,-1118.037;Float;False;Property;_BurnOffset1;Burn Offset;3;0;Create;True;0;0;False;0;1;0.22;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-2585.063,308.3878;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-2625.281,576.4147;Float;False;Property;_CharcoalNormalTile1;Charcoal Normal Tile;4;0;Create;True;0;0;False;0;5;2;2;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;45;-2097.262,103.4634;Inherit;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-2145.544,315.6017;Float;False;Property;_GlowBaseFrequency1;Glow Base Frequency;6;0;Create;True;0;0;False;0;1.1;2.35;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-2320.062,458.2878;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;27;-2512.662,-1162.738;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;28;-2307.79,-1192.9;Inherit;True;Property;_Masks1;Masks;8;0;Create;True;0;0;False;0;-1;None;None;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;-2141.898,429.5228;Inherit;True;Property;_BurntTileNormals1;Burnt Tile Normals;9;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1826.15,208.3843;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1694.685,731.6927;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;47;-1598.061,186.1639;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1703.347,622.8257;Float;False;Property;_GlowOverride1;Glow Override;7;0;Create;True;0;0;False;0;1;1.07;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1564.035,408.2778;Float;False;Constant;_GlowDuration1;Glow Duration;-1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1537.498,-7.560669;Float;False;Property;_GlowColorIntensity1;Glow Color Intensity;2;0;Create;True;0;0;False;0;2;0.56;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1341.647,628.0257;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-1461.017,-268.1004;Float;False;Property;_EmberColorTint1;Ember Color Tint;0;0;Create;True;0;0;False;0;0.9926471,0.6777384,0,1;0.966,0.1062519,0.004325263,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1367.767,189.5927;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1240.919,-98.19989;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1055.64,312.9257;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1021.436,-89.3988;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-784.0513,-90.10449;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-562.0583,-83.32098;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-315.1274,-291.1115;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-272.2569,-8.467957;Float;False;Property;_GlowEmissionMultiplier1;Glow Emission Multiplier;1;0;Create;True;0;0;False;0;1;12.6;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-800.1243,544.9537;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-392.3093,172.1053;Inherit;True;Property;_TextureSample1;Texture Sample 0;10;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;43.50867,-283.5412;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;605.6292,42.7406;Float;False;Constant;_RangedFloatNode179;RangedFloatNode 178;-1;0;Create;True;0;0;False;0;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;604.1606,-55.41797;Float;False;Constant;_RangedFloatNode178;RangedFloatNode 177;-1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;54.35327,515.3088;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;56;845.429,-277.4595;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1840.682,-1313.125;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;1207.545,-150.0086;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1379.782,-149.2037;Float;False;True;-1;2;ASEMaterialInspector;0;4;Custom/Burn;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;26;0;24;0
WireConnection;26;1;25;0
WireConnection;27;0;22;0
WireConnection;27;1;23;0
WireConnection;28;1;27;0
WireConnection;29;1;26;0
WireConnection;46;0;45;2
WireConnection;46;1;44;0
WireConnection;41;0;28;1
WireConnection;41;1;29;4
WireConnection;47;0;46;0
WireConnection;42;0;31;0
WireConnection;42;1;41;0
WireConnection;48;0;47;0
WireConnection;48;1;30;0
WireConnection;50;0;32;0
WireConnection;50;1;51;0
WireConnection;43;0;48;0
WireConnection;43;1;42;0
WireConnection;49;0;50;0
WireConnection;49;1;43;0
WireConnection;52;0;49;0
WireConnection;52;1;28;2
WireConnection;53;0;52;0
WireConnection;53;1;29;4
WireConnection;54;0;28;1
WireConnection;54;1;53;0
WireConnection;35;0;29;4
WireConnection;55;0;54;0
WireConnection;55;1;33;0
WireConnection;38;0;34;0
WireConnection;38;1;35;0
WireConnection;56;0;55;0
WireConnection;56;1;36;0
WireConnection;56;2;37;0
WireConnection;40;0;28;1
WireConnection;39;0;56;0
WireConnection;39;1;38;0
WireConnection;1;0;39;0
ASEEND*/
//CHKSM=132DF6DB5239B157D3A5739B300474566CFA10D1