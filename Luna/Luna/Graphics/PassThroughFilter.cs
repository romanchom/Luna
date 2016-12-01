using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;

namespace Luna.Graphics {
	class PassThroughFilter : Filter {
        private PixelShader pixelShader;

        private CompilationResult pixelShaderByteCode;

        private SamplerState sampler;

        public PassThroughFilter(Device device)
        {
            this.pixelShaderByteCode = ShaderBytecode.CompileFromFile("Graphics/PassThrough.hlsl", "pixel", "ps_4_0", ShaderFlags.OptimizationLevel1, EffectFlags.None, null, null);
            this.pixelShader = new PixelShader(device, this.pixelShaderByteCode, null);
            this.sampler = new SamplerState(device, new SamplerStateDescription
            {
                MaximumAnisotropy = 16,
                Filter = SharpDX.Direct3D11.Filter.Anisotropic,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                MinimumLod = 0f,
                MaximumLod = 100f
            });
        }

        public override void Apply(DeviceContext context, ShaderResourceView source, RenderTargetView destination, float scale = 1f, float offset = 0f)
        {
            context.PixelShader.Set(this.pixelShader);
            context.PixelShader.SetSampler(0, this.sampler);
            base.Apply(context, source, destination, scale, offset);
        }

        public override void Dispose()
        {
            this.sampler.Dispose();
            this.pixelShader.Dispose();
            this.pixelShaderByteCode.Dispose();
        }
    }
}
