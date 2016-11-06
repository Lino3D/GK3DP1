#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif


#define NUMSPOTLIGHTS 2 
#define NUMPOINTLIGHTS 2


float4x4 World;
float4x4 View;
float4x4 Projection;

float3 AmbientLightColor = float3(.15, .15, .15);
float3 DiffuseColor = float3(.85, .85, .85);

float3 LightPosition[4];
float3 LightColor[4];

//    float3 LightPositionSpot[NUMSPOTLIGHTS];
//    float3 LightColorSpot[NUMSPOTLIGHTS];
 float3 LightDirectionSpot[4];

float ConeAngle = 45;


float LightAttenuation = 1000;
float LightFalloff = 20;
//float4x4 WorldInverseTranspose;

//float3 DiffuseLightDirection = float3(1, 0, 0);
float DiffuseIntensity = 0.2;


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

    float att = 1;
    float a = cos(ConeAngle);

	for (int i = 0; i < 4; i++)
    {
        float3 lightDir = normalize(LightPosition[i] - input.WorldPosition);
        float3 diffuse = saturate(dot(normalize(input.Normal), lightDir));


        if (i < NUMPOINTLIGHTS)
        {
        float d = distance(LightPosition[i], input.WorldPosition);
            att = 1 - pow(clamp(d / LightAttenuation, 0, 1),
   LightFalloff);
        }
        else
            {
            float a = cos(ConeAngle);
            att=0;
            float d = dot(-lightDir, normalize(LightDirectionSpot[i]));
        if (a < d)
            att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);
            }

		totalLight += diffuse * att * LightColor[i];
	}


	float3 output = saturate(totalLight) * diffuseColor;

	return float4(output, 1);
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}



    //for (int j = 0; j < NUMSPOTLIGHTS; j++)
    //{
    //     lightDir = normalize(LightPositionSpot[j] - input.WorldPosition);
    //     diffuse = saturate(dot(normalize(input.Normal), lightDir));
        
    //     att = 0;
    //  //  if (a < d)
    //        att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);

    //    totalLight += diffuse * att * LightColorSpot[j];
    //}