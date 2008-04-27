struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float2 TexCoords        : TEXCOORD0;
    float3 Normal        : TEXCOORD1;
    float3 Position3D        : TEXCOORD2;
    
      float LightingFactor: TEXCOORD3;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float xAmbient;
bool xEnableLighting;
bool xShowNormals;
float4 xLightPos;
float xLightPower;

float4 xCameraPos;

bool xUseBrownInsteadOfTextures;

//------- Texture Samplers --------

Texture xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Pretransformed --------

VertexToPixel PretransformedVS( float4 inPos : POSITION, float4 inColor: COLOR)
{	
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.Color = inColor;
    
	return Output;    
}

PixelToFrame PretransformedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;

	return Output;
}

technique Pretransformed
{
	pass Pass0
    {   
    	VertexShader = compile vs_1_1 PretransformedVS();
        PixelShader  = compile ps_1_1 PretransformedPS();
    }
}

//------- Technique: Colored --------

VertexToPixel ColoredVS( float4 inPos : POSITION, float4 inColor: COLOR, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = inColor;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor =  ( 1 - (distance(inPos, xLightPos ) * 0.075));
    
	return Output;    
}

PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    
	Output.Color = PSIn.Color*clamp((PSIn.LightingFactor + xAmbient),0,1);
	
	return Output;
}

technique Colored
{
	pass Pass0
    {   
    	VertexShader = compile vs_1_1 ColoredVS();
        PixelShader  = compile ps_1_1 ColoredPS();
    }
}

//------- Technique: Textured --------

VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD1)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TexCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
	//+ (dot(reflect((xLightPos - inPos), Normal), xCameraPos) * 0.01)
		Output.LightingFactor = dot(Normal, (xLightPos - inPos)) * clamp( 1 - (distance(inPos, xLightPos ) * 0.075), 0, 1) ;
		
	return Output;    
}

PixelToFrame TexturedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TexCoords)*clamp(PSIn.LightingFactor + xAmbient,0,1);

	return Output;
}

technique Textured
{
	pass Pass0
    {   
		CullMode = CCW;
    	VertexShader = compile vs_1_1 TexturedVS();
        PixelShader  = compile ps_1_1 TexturedPS();
    }
}

//------- Technique: PointSprites --------

struct SpritesVertexToPixel
{
    float4 Position   	: POSITION;
    float4 Color    	: COLOR0;
    float1 Size 		: PSIZE;
};

SpritesVertexToPixel PointSpritesVS (float4 Position : POSITION, float4 Color : COLOR0, float1 Size : PSIZE)
{
    SpritesVertexToPixel Output = (SpritesVertexToPixel)0;
     
    float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection); 
    Output.Position = mul(Position, preWorldViewProjection);    
    Output.Color = Color;
    Output.Size = 1/(pow(Output.Position.z,2)+1	) * Size;
    
    return Output;    
}

PixelToFrame PointSpritesPS(SpritesVertexToPixel PSIn, float2 TexCoords  : TEXCOORD0)
{ 
    PixelToFrame Output = (PixelToFrame)0;

    Output.Color = tex2D(TextureSampler, TexCoords);
    
    return Output;
}

technique PointSprites
{
	pass Pass0
    {   
    	PointSpriteEnable = true;
    	VertexShader = compile vs_1_1 PointSpritesVS();
        PixelShader  = compile ps_1_1 PointSpritesPS();
    }
}

// Output structure for the vertex shader that renders normal and depth information.
struct NormalDepthVertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

// Vertex shader input structure.
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};


// Alternative vertex shader outputs normal and depth values, which are then
// used as an input for the edge detection filter in PostprocessEffect.fx.
NormalDepthVertexShaderOutput NormalDepthVertexShader(VertexShaderInput input)
{
    NormalDepthVertexShaderOutput output;

    // Apply camera matrices to the input position.
    output.Position = mul(mul(mul(input.Position, xWorld), xView), xProjection);
    
    float3 worldNormal = mul(input.Normal, xWorld);

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

// Technique draws the object as normal and depth values.
technique NormalDepth
{
    pass P0
    {
        VertexShader = compile vs_1_1 NormalDepthVertexShader();
        PixelShader = compile ps_1_1 NormalDepthPixelShader();
    }
}