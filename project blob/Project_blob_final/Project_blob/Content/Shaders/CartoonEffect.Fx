//-----------------------------------------------------------------------------
// CartoonEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// Camera settings.

float4x4 World;
float4x4 View;
float4x4 Projection;

float LightPower;

float3 blobCenter;

float4x4 LightWorldViewProjection;

// The light direction is shared between the Lambert and Toon lighting techniques.
float3 LightDirection = normalize(float3(1, 1, 1));

// Settings controlling the Lambert lighting technique.
float3 DiffuseLight = 0.5;
float3 AmbientLight = 0.5;

// Settings controlling the Toon lighting technique.
float ToonThresholds[2] = { 0.8, 0.4 };
float ToonBrightnessLevels[3] = { 1.3, 0.9, 0.5 };

// Is texturing enabled?
bool TextureEnabled;


float4 LightPos;

float MaxDepth;

Texture ShadowMap;

sampler ShadowMapSampler = sampler_state { texture = <ShadowMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};

//Texture ColoredTexture;

//sampler ColoredTextureSampler = sampler_state { texture = <ColoredTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

// The main texture applied to the object, and a sampler for reading it.
texture Texture;

float MaxShadowSize = 20.0;

sampler Sampler = sampler_state
{
    Texture = (Texture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};


// Vertex shader input structure.
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD1;
};


// Output structure for the vertex shader that applies lighting.
struct LightingVertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float LightAmount : TEXCOORD1;
    
    float4 ShadowMapSamplingPos : TEXCOORD2;
    
    float4 RealDistance        : TEXCOORD3;
	float3 Normal            : TEXCOORD4;
	float3 Position3D            : TEXCOORD5;
};


// Input structure for the Lambert and Toon pixel shaders.
struct LightingPixelShaderInput
{
    float2 TextureCoordinate : TEXCOORD0;
    float LightAmount : TEXCOORD1;
    
	float4 ShadowMapSamplingPos : TEXCOORD2;
	
	float4 RealDistance        : TEXCOORD3;
	float3 Normal            : TEXCOORD4;
	float3 Position3D            : TEXCOORD5;
};


// Vertex shader shared between the Lambert and Toon lighting techniques.
LightingVertexShaderOutput LightingVertexShader(VertexShaderInput input)
{
    LightingVertexShaderOutput output;

    // Apply camera matrices to the input position.
    output.Position = mul(mul(input.Position, View), Projection);
    
    // Copy across the input texture coordinate.
    output.TextureCoordinate = input.TextureCoordinate;

    // Compute the overall lighting brightness.
    float3 worldNormal = mul(input.Normal, World);
    
    output.LightAmount = dot(worldNormal, LightDirection);
    
    output.ShadowMapSamplingPos = mul(input.Position, LightWorldViewProjection);
    
    output.RealDistance = output.ShadowMapSamplingPos.z/MaxDepth;
    output.Normal = normalize(mul(input.Normal, (float3x3)World));
    output.Position3D = mul(input.Position, World);
    
    return output;
}

float DotProduct(float4 LightPos, float3 Pos3D, float3 Normal)
{
    float3 LightDir = normalize(LightPos - Pos3D);
    return dot(LightDir, Normal);
}

// Pixel shader applies a simple Lambert shading algorithm.
float4 LambertPixelShader(LightingPixelShaderInput input, uniform bool onBlob) : COLOR0
{
    float4 color = TextureEnabled ? tex2D(Sampler, input.TextureCoordinate) : 0;
    
    //float2 ProjectedTexCoords;
    //ProjectedTexCoords[0] = input.ShadowMapSamplingPos.x/input.ShadowMapSamplingPos.w/2.0f +0.5f;
    //ProjectedTexCoords[1] = -input.ShadowMapSamplingPos.y/input.ShadowMapSamplingPos.w/2.0f +0.5f;
    
    float2 xyCoord;
    xyCoord[0] = input.Position3D.x;
    xyCoord[1] = input.Position3D.z;
    
    float2 blobCenterVector;
    blobCenterVector[0] = blobCenter.x;
    blobCenterVector[1] = blobCenter.z;
    
	color.rgb *= saturate(input.LightAmount) * DiffuseLight + AmbientLight;
	
	if( !onBlob && input.Position3D.y < blobCenter.y && distance(xyCoord,blobCenterVector) < ( MaxShadowSize - distance(blobCenter.y ,input.Position3D.y) ) * 0.05f )
    {
		color.rgb *= 0.3f;
    }

    
    //if ((saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y))
    //{    
    //    float StoredDepthInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).x;    
    //    if ((input.RealDistance.x - 1.0f/100.0f) <= StoredDepthInShadowMap)    
    //    {
    //        color.rgb *= saturate(input.LightAmount) * DiffuseLight + AmbientLight;
    //    }
    //    else
    //    {
	//		color.rgb = 0;
    //    }
    //} 
    //else
    //{ 
	//	color.rgb *= saturate(input.LightAmount) * DiffuseLight + AmbientLight;
    //}
    
    return color;
}


// Pixel shader applies a cartoon shading algorithm.
float4 ToonPixelShader(LightingPixelShaderInput input) : COLOR0
{
    float4 color = TextureEnabled ? tex2D(Sampler, input.TextureCoordinate) : 0;
    
    float light;

    if (input.LightAmount > ToonThresholds[0])
        light = ToonBrightnessLevels[0];
    else if (input.LightAmount > ToonThresholds[1])
        light = ToonBrightnessLevels[1];
    else
        light = ToonBrightnessLevels[2];
                
    color.rgb *= light;
    
    return color;
}

// Vertex shader shared between the Lambert and Toon lighting techniques.
LightingVertexShaderOutput SkyboxVertexShader(VertexShaderInput input)
{
    LightingVertexShaderOutput output;

    // Apply camera matrices to the input position.
    output.Position = mul(mul(mul(input.Position, World), View), Projection);
    
    // Copy across the input texture coordinate.
    output.TextureCoordinate = input.TextureCoordinate;

    // Compute the overall lighting brightness.
    float3 worldNormal = mul(input.Normal, World);
    
    output.LightAmount = 1;
    
    output.RealDistance= 0;
    output.Normal = input.Normal;
    output.Position3D = 0;
    output.ShadowMapSamplingPos = 0;
    
    return output;
}

// Output structure for the vertex shader that renders normal and depth information.
struct NormalDepthVertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};


// Alternative vertex shader outputs normal and depth values, which are then
// used as an input for the edge detection filter in PostprocessEffect.fx.
NormalDepthVertexShaderOutput NormalDepthVertexShader(VertexShaderInput input)
{
    NormalDepthVertexShaderOutput output;

    // Apply camera matrices to the input position.
    output.Position = mul(mul(mul(input.Position, World), View), Projection);
    
    float3 worldNormal = mul(input.Normal, World);

    // The output color holds the normal, scaled to fit into a 0 to 1 range.
    output.Color.rgb = (worldNormal + 1) / 2;

    // The output alpha holds the depth, scaled to fit into a 0 to 1 range.
    output.Color.a = output.Position.z / output.Position.w;
    
    return output;    
}


// Simple pixel shader for rendering the normal and depth information.
float4 NormalDepthPixelShader(float4 color : COLOR0) : COLOR0
{
    return color;
}

// Pixel shader applies a simple Lambert shading algorithm.
float4 SkyBoxPixelShader(LightingPixelShaderInput input) : COLOR0
{
    float4 color = TextureEnabled ? tex2D(Sampler, input.TextureCoordinate) : 0;
    
    return color;
}


struct SMapVertexToPixel
{
	 float4 Position     : POSITION;
	 float3 Position2D    : TEXCOORD1;
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

// Technique draws the object using smooth Lambert shading.
technique Lambert
{
	
    pass P0
    {
        VertexShader = compile vs_1_1 LightingVertexShader();
        PixelShader = compile ps_2_0 LambertPixelShader(false);
    }
}

// Technique draws the object using smooth Lambert shading.
technique LambertOnBlob
{
	
    pass P0
    {   
        VertexShader = compile vs_1_1 LightingVertexShader();
        PixelShader = compile ps_2_0 LambertPixelShader(true);
    }
}


// Technique draws the object using banded cartoon shading.
technique Toon
{
    pass P0
    {
        VertexShader = compile vs_1_1 LightingVertexShader();
        PixelShader = compile ps_2_0 ToonPixelShader();
    }
}


// Technique draws the object as normal and depth values.
technique NormalDepth
{
    pass P0
    {
        VertexShader = compile vs_1_1 NormalDepthVertexShader();
        PixelShader = compile ps_1_1 NormalDepthPixelShader();
    }
}

// Technique draws the object as normal and depth values.
technique SkyBox
{
    pass P0
    {
        VertexShader = compile vs_1_1 SkyboxVertexShader();
        PixelShader = compile ps_1_1 SkyBoxPixelShader();
    }
}


technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowMapVertexShader();
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}