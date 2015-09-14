
struct vs_in
{
	float4 pos : POSITION;
	float2 uv : TEXTURE_COORD0;
};

struct vs_to_ps
{
	float4 pos : SV_POSITION;
	float2 uv : TEXTURE_COORD0;
};

vs_to_ps vertex(vs_in i)
{
	vs_to_ps o;
	o.pos = i.pos;
	o.uv = i.uv;
	return o;
}