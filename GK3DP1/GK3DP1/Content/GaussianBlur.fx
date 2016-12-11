#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

sampler TextureSampler : register(s0);
Texture2D myTex2D;

float Pixels[13] =
{
    -6,
   -5,
   -4,
   -3,
   -2,
   -1,
    0,
    1,
    2,
    3,
    4,
    5,
    6,
};

float BlurWeights[13] =
{
    0.002216,
   0.008764,
   0.026995,
   0.064759,
   0.120985,
   0.176033,
   0.199471,
   0.176033,
   0.120985,
   0.064759,
   0.026995,
   0.008764,
   0.002216,
};


float4 PixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : SV_TARGET0
{
    // Pixel width
    //float pixelWidth = 1.0f;

    //float4 color = { 0, 0, 0, 1 };
    //float2 blur = TextureCoordinate;
    //blur.y = TextureCoordinate.y;

    //for (int i = 0; i < 13; i++)
    //{
    //    blur.x = TextureCoordinate.x + Pixels[i] * pixelWidth;
    //    color += tex2D(TextureSampler, blur.xy) * BlurWeights[i];
    //}

    //return color;
    float4 tex;
    tex = myTex2D.Sample(TextureSampler, TextureCoordinate.xy) * .6f;
    tex += myTex2D.Sample(TextureSampler, TextureCoordinate.xy + (0.005)) * .2f;
    return tex;
}
technique BasicColorDrawing
{
	pass P0
	{
		//VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};