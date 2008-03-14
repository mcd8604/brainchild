struct PixelToFrame
{
    float4 Color        : COLOR0;
};



 float4x4 xCameraViewProjection;
 float4x4 xLightViewProjection;
 float4x4 xWorld;
 


float4 xLightPos;
float4 xCameraPos;
float4 xLamppostPos[2];
float xAmbient;
float xLightPower;
float xMaxDepth;
bool xUseBrownInsteadOfTextures;

Texture xColoredTexture;

sampler ColoredTextureSampler = sampler_state { texture = <xColoredTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
Texture xShadowMap;

sampler ShadowMapSampler = sampler_state { texture = <xShadowMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};
Texture xCarLightTexture;

sampler CarLightSampler = sampler_state { texture = <xCarLightTexture> ; magfilter = LINEAR; minfilter=LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp;};

//------- Technique: ShadowMap --------

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



     float4x4 preLightWorldViewProjection = mul (xWorld, xLightViewProjection);
     Output.Position = mul(inPos, preLightWorldViewProjection);


    Output.Position2D = Output.Position;

    return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
    SMapPixelToFrame Output = (SMapPixelToFrame)0;

    Output.Color = PSIn.Position2D.z/xMaxDepth;

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

//------- Technique: ShadowedScene --------

struct SSceneVertexToPixel
{
    float4 Position             : POSITION;
    float4 ShadowMapSamplingPos : TEXCOORD0;
    float4 RealDistance            : TEXCOORD1;
    float2 TexCoords            : TEXCOORD2;
    float3 Normal                : TEXCOORD3;
    float3 Position3D            : TEXCOORD4;
    float4 Position2D            : TEXCOORD5;
};

struct SScenePixelToFrame
{
    float4 Color : COLOR0;
};

SSceneVertexToPixel ShadowedSceneVertexShader( float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
    SSceneVertexToPixel Output = (SSceneVertexToPixel)0;


     float4x4 preLightWorldViewProjection = mul (xWorld, xLightViewProjection);
     float4x4 preCameraWorldViewProjection = mul (xWorld, xCameraViewProjection);
 
     Output.Position = mul(inPos, preCameraWorldViewProjection);
     Output.ShadowMapSamplingPos = mul(inPos, preLightWorldViewProjection);


    Output.RealDistance = Output.ShadowMapSamplingPos.z/xMaxDepth;
    Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
    Output.Position3D = mul(inPos, xWorld);
    Output.Position2D = Output.Position;

    Output.TexCoords = inTexCoords;
    if (xUseBrownInsteadOfTextures)
        Output.TexCoords = (0,0);

    return Output;
}

float DotProduct(float4 LightPos, float3 Pos3D, float3 Normal)
{
    float3 LightDir = normalize(LightPos - Pos3D);
    return dot(LightDir, Normal);
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
    SScenePixelToFrame Output = (SScenePixelToFrame)0;

    float2 ProjectedTexCoords;
    ProjectedTexCoords[0] = PSIn.ShadowMapSamplingPos.x/PSIn.ShadowMapSamplingPos.w/2.0f +0.5f;
    ProjectedTexCoords[1] = -PSIn.ShadowMapSamplingPos.y/PSIn.ShadowMapSamplingPos.w/2.0f +0.5f;
    float4 ColorComponent = tex2D(ColoredTextureSampler, PSIn.TexCoords);

    if ((saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y))
    {
        float StoredDepthInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).x;
        if ((PSIn.RealDistance.x - 1.0f/100.0f) <= StoredDepthInShadowMap)
        {
            float LightTextureFactor = tex2D(CarLightSampler, ProjectedTexCoords).r;
            float DiffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
            Output.Color = ColorComponent*LightTextureFactor*DiffuseLightingFactor*xLightPower;
        }
    }

    float2 ScreenPos;
    ScreenPos[0] = PSIn.Position2D.x/PSIn.Position2D.w/2.0f +0.5f;
    ScreenPos[1] = -PSIn.Position2D.y/PSIn.Position2D.w/2.0f +0.5f;

    Output.Color += ColorComponent*xAmbient;

    for (int CurrentLight=0; CurrentLight<2; CurrentLight++)
    {


         float4 Light2DPos = mul(xLamppostPos[CurrentLight],xCameraViewProjection);


        float2 LightScreenPos;
        LightScreenPos[0] = Light2DPos.x/Light2DPos.w/2.0f +0.5f;
        LightScreenPos[1] = -Light2DPos.y/Light2DPos.w/2.0f +0.5f;

        float dist = distance(ScreenPos, LightScreenPos);
        float radius = 3.5f/distance(xCameraPos, xLamppostPos[CurrentLight]);
        if (dist < radius)
        {
             Output.Color.rgb += (radius-dist)*8.0f;
        }
    }

    return Output;
}

technique ShadowedScene
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
        PixelShader = compile ps_2_0 ShadowedScenePixelShader();
    }
}