float4x4 World;

float screenWidth;
float screenHeight;
float4 ambientColor;

float lightStrength;
float lightDecay;
float3 lightPosition;
float4 lightColor;
float lightRadius;
float specularStrength;

float3 coneDirection;
float coneAngle;
float coneDecay;

matrix Translation; 

Texture NormalMap;
sampler NormalMapSampler = sampler_state {
	texture = <NormalMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

Texture ColorMap;
sampler ColorMapSampler = sampler_state {
	texture = <ColorMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
	float4 Position: SV_POSITION;
};

struct VertexToPixel
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};



VertexToPixel MyVertexShader(float4 inPos: POSITION0, float2 texCoord: TEXCOORD0, float4 color: COLOR0)
{
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos; 
	Output.TexCoord = texCoord;
	Output.Color = Output.Position;
	
	return Output;
}

PixelToFrame PointLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = tex2D(NormalMapSampler, PSIn.TexCoord).rgb;
	normal = normalize(normal * 2.0f - 1.0f); 

	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;
	

	float3 lightDirection = lightPosition - PSIn.Position;
	float3 lightDirNorm = normalize(lightDirection);
	float3 halfVec = float3(0, 0, 1);
	
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = saturate(1.0f - length(lightDirection) / lightDecay); 
	
	float3 reflect = normalize(2 * amount * normal - lightDirNorm);
	float specular = min(pow(saturate(dot(reflect, halfVec)), 10), amount);

	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation * specularStrength);
	
	return Output;
}

// Newest version of the Spot Light shader technique, this includes normal mapping and specular lightning
PixelToFrame SpotLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 lightVector = normalize(lightPosition - pixelPosition);
    // cosine of the angle between spotdirection and lightvector
    float SdL = dot(coneDirection, -lightVector);
	
	float3 shading = float3(0, 0, 0);
	if (SdL > coneAngle) 
	{
		float3 lightPos = float3(lightPosition.x, lightPosition.y, lightPosition.z);
		float3 lightVector = lightPos - pixelPosition;
		lightVector = normalize(lightVector);
		
		float3 coneDirectionTemp = coneDirection;
		//coneDirectionTemp.z = 50.0f;
		float spotIntensity = pow(SdL, coneDecay);
	
		float3 lightDirection = lightPos - pixelPosition;
		float3 halfVec = float3(0, 0, 1);
		
		float amount = max(dot(normal, lightVector), 0);
		float coneAttenuation = saturate(1.0f - length(lightDirection) / lightDecay); 
		
		float3 reflect = normalize(2 * amount * normal - lightVector);

		float3 r = normalize(2 * dot(lightVector, normal) * normal - lightVector);
		float3 v = normalize(mul(normalize(coneDirectionTemp), World));
		float dotProduct = dot(r, v);

		//float4 specular = light.specPower * light.specularColor * max(pow(dotProduct, 10), 0) * length(inColor);
		float specular = min(pow(saturate(dot(reflect, halfVec)), 10), 1);
		
		//shading =  ((lightStrength * amount * coneAttenuation * lightColor * depth) * spotIntensity) + ((specular * lightSpecularStrength * specularColor) * spotIntensity);
		//shading = (colorMap * coneAttenuation * lightColor * lightStrength * depth) * spotIntensity;
		//shading += (coneAttenuation * amount * lightColor * lightNormalStrength * depth) * spotIntensity;
		//shading += (coneAttenuation * specular * lightSpecularStrength * depth) * spotIntensity;
		shading = lightColor * lightStrength;
		shading += specular * specularStrength;
		shading += amount * 0.5;
		shading *= coneAttenuation * spotIntensity;
	}
	Output.Color = float4(shading.r, shading.g, shading.b, 1.0f);
	return Output;
}

technique DeferredPointLight
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 MyVertexShader();
        PixelShader = compile ps_4_0 PointLightShader();
    }
}

technique DeferredSpotLight
{
    pass Pass1
    {
		VertexShader = compile vs_4_0 MyVertexShader();
        PixelShader = compile ps_4_0 SpotLightShader();
    }
}