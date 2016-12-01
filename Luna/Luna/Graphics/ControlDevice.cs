using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Windows.Forms;

namespace Luna.Graphics {
	class ControlDevice : IDisposable {
		public SharpDX.Direct3D11.Device device;
		public DeviceContext context;
		private SwapChain swapChain;
		private Texture2D backBuffer;
		public RenderTargetView renderView;
		private Control control;

		public ControlDevice(Control control) {
			{
				this.control = control;
				var desc = new SwapChainDescription() {
					BufferCount = 1,
					ModeDescription = new ModeDescription(control.Width, control.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm_SRgb),
					IsWindowed = true,
					OutputHandle = control.Handle,
					SampleDescription = new SampleDescription(1, 0),
					SwapEffect = SwapEffect.Discard,
					Usage = Usage.RenderTargetOutput,
				};

                SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out this.device, out this.swapChain);
                this.context = this.device.ImmediateContext;
                this.backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(this.swapChain, 0);
                this.renderView = new RenderTargetView(this.device, this.backBuffer);
                this.context.Rasterizer.SetViewport(new Viewport(0, 0, control.Width, control.Height, 0f, 1f));
                RasterizerStateDescription desc2 = RasterizerStateDescription.Default();
                desc2.CullMode = CullMode.None;
                RasterizerState state = new RasterizerState(this.device, desc2);
                this.context.Rasterizer.State = state;
            }
		}

		public void Resize() {
			backBuffer.Dispose();
			renderView.Dispose();

			swapChain.ResizeBuffers(0, control.Width, control.Height, Format.Unknown, SwapChainFlags.None);

			backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
			renderView = new RenderTargetView(device, backBuffer);

			context.Rasterizer.SetViewport(new Viewport(0, 0, control.Width, control.Height, 0.0f, 1.0f));
			context.OutputMerger.SetTargets(renderView);

		}

		public void Present() {
			swapChain.Present(0, PresentFlags.None);
		}

		public void Dispose() {
			renderView.Dispose();
			backBuffer.Dispose();
			
			context.ClearState();
			context.Flush();
			device.Dispose();
			context.Dispose();
			swapChain.Dispose();
		}
	}
}
