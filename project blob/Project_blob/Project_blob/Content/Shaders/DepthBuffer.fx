//float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;
float4x4 World;
float4 LightPos;
//float LightPower;

float MaxDepth;
//bool UseBrownInsteadOfTextures;

Texture ColoredTexture;

sampler ColoredTextureSampler = sampler_state { texture = <ColoredTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

struct SMapVertexToPixel
{
	 float4 Position     : POSITION;
	 float3 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
	float4 Color : COLOR0;
};

SMapVertexToPixel ShadowMapVertexShader( float4 inPos : POSITION)
{
	 SMapVertexToPixel Output = (SMapVertexToPixel)0;

	 Output.Position = mul(inPos, LightWorldViewProjection);
	 Output.Position2D = Output.Position;

	 return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
	SMapPixelToFrame Output = (SMapPixelToFrame)0;

	Output.Color = PSIn.Position2D.z/MaxDepth;

	return Output;
}

technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowMapVertexShader();
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}