﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 AmbientLightColor = float3(.15, .15, .15);
float3 DiffuseColor = float3(.85, .85, .85);
float3 LightPosition = float3(0, 3, 0);
float3 LightColor = float3(1, 1, 1);
float LightAttenuation = 5000;
float LightFalloff = 2;
float4x4 WorldInverseTranspose;

//float3 DiffuseLightDirection = float3(1, 0, 0);
//float DiffuseIntensity = 0.2;


bool TextureEnabled = true;

texture BasicTexture;

sampler BasicTextureSampler = sampler_state
{
	texture = <BasicTexture>;
};



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
  
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.WorldPosition = worldPosition;
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = DiffuseColor;

	if (TextureEnabled)
		diffuseColor *= tex2D(BasicTextureSampler, input.UV).rgb;
	float3 totalLight = float3(0, 0, 0);
  
	totalLight += AmbientLightColor;
 
	float3 lightDir = normalize(LightPosition - input.WorldPosition);
	float diffuse = saturate(dot(normalize(input.Normal), lightDir));
	float d = distance(LightPosition, input.WorldPosition);
	float att = 1 - pow(clamp(d / LightAttenuation, 0, 1),
   LightFalloff);

	totalLight += diffuse * att * LightColor;

	return float4(diffuseColor * totalLight, 1);
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}