#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4 AmbientColor;
float AmbientStrength;
float ShadowStrength; 

Texture2D DiffuseTexture;
sampler2D DiffuseSampler = sampler_state
{
	Texture = <DiffuseTexture>;
};

Texture2D ShadowTexture;
sampler2D ShadowSampler = sampler_state
{
	Texture = <ShadowTexture>;
};



struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 diffuse = tex2D(DiffuseSampler, input.TextureCoordinates);
	float4 shadow = tex2D(ShadowSampler, input.TextureCoordinates);
	
	float4 finalColor = (diffuse * (AmbientColor * AmbientStrength)) + ((diffuse * shadow) * ShadowStrength);
	return finalColor;

}

technique Combine
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};