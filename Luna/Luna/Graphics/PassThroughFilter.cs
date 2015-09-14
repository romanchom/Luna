using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;

namespace Luna.Graphics {
	class PassThroughFilter : Filter {
		private PixelShader pixelShader;
		private CompilationResult pixelShaderByteCode;
		private SamplerState sampler;

		public PassThroughFilter(Device device) {
			pixelShaderByteCode = ShaderBytecode.CompileFromFile("Graphics/PassThrough.hlsl", "pixel", "ps_4_0");
			pixelShader = new PixelShader(device, pixelShaderByteCode);
			sampler = new SamplerState(device, new SamplerStateDescription() {
				MaximumAnisotropy = 16,
				Filter = SharpDX.Direct3D11.Filter.Anisotropic,
				AddressU = TextureAddressMode.Clamp,
				AddressV = TextureAddressMode.Clamp,
				AddressW = TextureAddressMode.Clamp,
				MinimumLod = 0,
				MaximumLod = 100,
			});
		}

		public override void Apply(DeviceContext context, ShaderResourceView source, RenderTargetView destination) {
			context.PixelShader.Set(pixelShader);
			context.PixelShader.SetSampler(0, sampler);
			base.Apply(context, source, destination);
		}

		public override void Dispose() {
			sampler.Dispose();
			pixelShader.Dispose();
			pixelShaderByteCode.Dispose();
		}
	}
}
