using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Device = SharpDX.Direct3D11.Device;
using System.Windows.Forms;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using SharpDX;
using System.Threading;
using SharpDX.Windows;
using Luna.Graphics;
using System;

namespace Luna.Controls {
	public partial class PreviewControl : UserControl {
		ControlDevice cDev;
		PassThroughFilter passThroughFilter;
		ScreenCapture screenCapture;
		const int mipLevel = 3;

		public PreviewControl() {
			InitializeComponent();
		}

		protected override void OnCreateControl() {
			if (!DesignMode) InitD3D();
			this.Disposed += Cleanup;
			base.OnCreateControl();
		}

		int screenCaptureHeight {
			get { return 120; }
		}

		int screenCaptureWidth {
			get { return screenCaptureHeight * 16 / 9; }
		}

		private void InitD3D() {
			cDev = new ControlDevice(EmptyControl);

			MakeMipTexture();
			MakeIntermediateTexture();
			MakeCpuTexture();

			ScreenQuad.Init(cDev.device);
			passThroughFilter = new PassThroughFilter(cDev.device);

			screenCapture = new ScreenCapture(cDev.device);

			StartRenderLoop();
		}

		private Texture2D mipTexture;
		private ShaderResourceView mipView;
		private void MakeMipTexture() {
			{
				Texture2DDescription desc = new Texture2DDescription() {
					CpuAccessFlags = CpuAccessFlags.None,
					BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
					Format = Format.B8G8R8A8_UNorm_SRgb,
					Height = Screen.PrimaryScreen.Bounds.Height,
					Width = Screen.PrimaryScreen.Bounds.Width,
					OptionFlags = ResourceOptionFlags.GenerateMipMaps,
					MipLevels = 0,
					ArraySize = 1,
					SampleDescription = new SampleDescription(1, 0),
					Usage = ResourceUsage.Default,
				};
				mipTexture = new Texture2D(cDev.device, desc);
				mipView = new ShaderResourceView(cDev.device, mipTexture);
			}
		}

		private Texture2D intermediateTexture;
		private RenderTargetView intermediateView;
		private void MakeIntermediateTexture() {

			Texture2DDescription desc = new Texture2DDescription() {
				CpuAccessFlags = CpuAccessFlags.None,
				BindFlags = BindFlags.RenderTarget,
				Format = Format.R32G32B32A32_Float,
				Height = screenCaptureHeight,
				Width = screenCaptureWidth,
				OptionFlags = ResourceOptionFlags.None,
				MipLevels = 0,
				ArraySize = 1,
				SampleDescription = new SampleDescription(1, 0),
				Usage = ResourceUsage.Default,
			};

			intermediateTexture = new Texture2D(cDev.device, desc);
			intermediateView = new RenderTargetView(cDev.device, intermediateTexture);
		}

		private Texture2D cpuTexture;
		private void MakeCpuTexture() {

			Texture2DDescription desc = new Texture2DDescription() {
				CpuAccessFlags = CpuAccessFlags.Read,
				BindFlags = BindFlags.None,
				Format = Format.R32G32B32A32_Float,
				Height = screenCaptureHeight,
				Width = screenCaptureWidth,
				OptionFlags = ResourceOptionFlags.None,
				MipLevels = 0,
				ArraySize = 1,
				SampleDescription = new SampleDescription(1, 0),
				Usage = ResourceUsage.Staging,
			};

			cpuTexture = new Texture2D(cDev.device, desc);
		}

		System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

		private Thread t;
		private bool threadStop = false;

		
		private void RenderLoop() {
			while (!threadStop) {
				SharpDX.DXGI.Resource screenResource = screenCapture.AcquireFrame();
				Texture2D original = screenResource.QueryInterface<Texture2D>();

				// make mipmaps
				cDev.context.CopySubresourceRegion(original, 0, null, mipTexture, 0);
				cDev.context.GenerateMips(mipView);

				//rescale
				passThroughFilter.Apply(cDev.context, mipView, intermediateView);
				cDev.context.CopyResource(intermediateTexture, cpuTexture);
				
				//draw preview
				cDev.context.ClearRenderTargetView(cDev.renderView, Color.DarkGray);
				passThroughFilter.Apply(cDev.context, mipView, cDev.renderView);
				cDev.Present();
				
				//cleanup
				original.Dispose();
				screenResource.Dispose();
				screenCapture.ReleaseFrame();

				//read from gpu
				DataStream stream;
				DataBox dataBox = cDev.context.MapSubresource(cpuTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out stream);

				int count = dataBox.RowPitch / 16 * screenCaptureHeight;
				System.Numerics.Vector4[] pixels = stream.ReadRange<System.Numerics.Vector4>(count);
				stream.Dispose();
				cDev.context.UnmapSubresource(cpuTexture, 0);

				if (OnScreenCaptured != null) OnScreenCaptured(pixels, screenCaptureWidth, screenCaptureHeight, dataBox.RowPitch / 16);
			}
        }

		public event System.Action<System.Numerics.Vector4[], int, int, int> OnScreenCaptured;

		public void StartRenderLoop() {
			if(t == null || t.ThreadState != ThreadState.Running) {
				threadStop = false;
				t = new Thread(RenderLoop);
				t.Start();
				watch.Start();
			}
		}

		public void StopRenderLoop() {
			if(t != null && t.ThreadState == ThreadState.Running) {
				threadStop = true;
				t.Join();
			}
		}

		private void ResizeControlDevice() {
			if (!DesignMode && cDev != null) {
				StopRenderLoop();
				cDev.Resize();
				StartRenderLoop();
			}
		}
		
		private void Cleanup(object sender, System.EventArgs e) {
			if (!DesignMode) {
				StopRenderLoop();
				ScreenQuad.Dispose();
				passThroughFilter.Dispose();
				screenCapture.Dispose();
				cDev.Dispose();
			}
		}

		protected override void OnResize(EventArgs e) {
			Control Container = this;
			Control ViewPort = EmptyControl;

			float ContainerRatio = (float) Container.ClientRectangle.Width / Container.ClientRectangle.Height;

			float TargetRatio = (float)Screen.PrimaryScreen.Bounds.Width / Screen.PrimaryScreen.Bounds.Height;

			if (ContainerRatio < TargetRatio) {
				ViewPort.Width = Container.ClientRectangle.Width;
				ViewPort.Height = (int)(ViewPort.Width / TargetRatio);
				ViewPort.Top = (Container.ClientRectangle.Height - ViewPort.Height) / 2;
				ViewPort.Left = 0;
			} else {
				ViewPort.Height = Container.ClientRectangle.Height;
				ViewPort.Width = (int)(ViewPort.Height * TargetRatio);
				ViewPort.Top = 0;
				ViewPort.Left = (Container.ClientRectangle.Width - ViewPort.Width) / 2;
			}

			ResizeControlDevice();

			base.OnResize(e);
		}
	}
}
