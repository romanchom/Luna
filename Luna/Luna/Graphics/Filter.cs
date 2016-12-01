using SharpDX.Direct3D11;
using System;

namespace Luna.Graphics {
	abstract class Filter : IDisposable {
        public virtual void Apply(DeviceContext context, ShaderResourceView source, RenderTargetView destination, float scale = 1f, float offset = 0f)
        {
            context.OutputMerger.SetTargets(destination);
            context.PixelShader.SetShaderResource(0, source);
            Texture2D tex2D = destination.ResourceAs<Texture2D>();
            context.Rasterizer.SetViewport(0f, 0f, (float)tex2D.Description.Width, (float)tex2D.Description.Height, 0f, 1f);
            tex2D.Dispose();
            ScreenQuad.Draw(context, scale, offset);
        }

        public abstract void Dispose();
    }
}
