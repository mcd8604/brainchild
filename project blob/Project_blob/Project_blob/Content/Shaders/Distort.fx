//-----------------------------------------------------------------------------
// Distort.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

sampler SceneTexture : register(s0);
sampler DistortionMap : register(s1);

#define SAMPLE_COUNT 13
float2 SampleOffsets[SAMPLE_COUNT];
float SampleWeights[SAMPLE_COUNT];

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection = normalize(float3(1, 1, 1));

float2 blobCenter;

// The Distortion map represents zero displacement as 0.5, but in an 8 bit color
// channel there is no exact value for 0.5. ZeroOffset adjusts for this error.
const float ZeroOffset = 0.5f / 255.0f;


float4 Distort_PixelShader(float2 TexCoord : TEXCOORD0, 
    uniform bool distortionBlur) : COLOR0
{
    // Look up the displacement
    float2 displacement = tex2D(DistortionMap, TexCoord).rg;
    float4 green = 0;
    green.g = .5;
    
    
    
    float4 finalColor = 0;
    // We need to constrain the area potentially subjected to the gaussian blur to the
    // distorted parts of the scene texture.  Therefore, we can sample for the color
    // we used to clear the distortion map (black).  We used 0 to avoid any potential
    // rounding errors.
    if ((displacement.x == 0) && (displacement.y == 0))
    {
        finalColor = tex2D(SceneTexture, TexCoord);
    }
    else
    {
        // Convert from [0,1] to [-.5, .5) 
        // .5 is excluded by adjustment for zero
        displacement -= .5 + ZeroOffset;
        float4 displacementSum = .7;
		displacementSum = (displacementSum - ((abs(displacement.r) + abs(displacement.g)) * 20)) + green;
        clamp(displacementSum,0,1);
        
        if (distortionBlur)
        {
			finalColor = 0; 
			
			
            // Combine a number of weighted displaced-image filter taps
            for (int i = 0; i < SAMPLE_COUNT; ++i)
            {
               finalColor += tex2D(SceneTexture, TexCoord.xy + displacement + SampleOffsets[i]) * SampleWeights[i] ;
               finalColor = (finalColor * .95) + (displacementSum  *  .05);
            }
            
        }
        else
        {
            // Look up the displaced color, without multisampling
            finalColor = tex2D(SceneTexture, TexCoord.xy + displacement);
            //finalColor.r += .3;
			//finalColor.g += (abs(displacement.g)*20);
            
        }
    }

	 
	
    return finalColor;
}

technique Distort
{
    pass
    {
        PixelShader = compile ps_2_0 Distort_PixelShader(false);
    }
}

technique DistortBlur
{
    pass
    {
		//VertexShader= compile vs_2_0 LightingVertexShader();
        PixelShader = compile ps_2_0 Distort_PixelShader(true);
    }
}