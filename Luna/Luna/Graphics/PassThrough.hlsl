struct ps_in
{
	float4 pos : SV_POSITION;
	float2 uv : TEXTURE_COORD0;
};

texture2D source;
SamplerState sourceSampler : register(s0);

float4 pixel(ps_in input) : SV_Target
{
	return source.Sample(sourceSampler, input.uv);
}