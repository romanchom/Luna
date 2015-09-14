using SharpDX.Direct3D11;
using System;

namespace Luna.Graphics {
	abstract class Filter : IDisposable {
		public virtual void Apply(DeviceContext context, ShaderResourceView source, RenderTargetView destination) {
			context.OutputMerger.SetTargets(destination);
			context.PixelShader.SetShaderResource(0, source);
			var tex2D = destination.ResourceAs<Texture2D>();
			context.Rasterizer.SetViewport(0, 0, tex2D.Description.Width, tex2D.Description.Height);
			tex2D.Dispose();
			ScreenQuad.Draw(context);
		}

		public abstract void Dispose();
	}
}
