using System;
using System.Windows.Forms;
using System.Threading;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

using Luna.Graphics;
using System.Diagnostics;

namespace Luna.Controls {
	public partial class PreviewControl : UserControl
    {
        public float bottomPosition;
        public float topPosition;
        private const int mipLevel = 3;
        private bool threadStop;
        private Thread t;

        private ControlDevice cDev;
        private Texture2D cpuTexture;
        private Texture2D intermediateTexture;
        private Texture2D mipTexture;
        private RenderTargetView intermediateView;
        private ShaderResourceView mipView;
        private PassThroughFilter passThroughFilter;
        private ScreenCapture screenCapture;
        private Stopwatch watch = new Stopwatch();

        public event Action<System.Numerics.Vector4[], int, int, int> OnScreenCaptured;

        public PreviewControl()
        {
            InitializeComponent();
        }

        public void Cleanup(object sender, EventArgs e)
        {
            if (!base.DesignMode)
            {
                StopRenderLoop();
                ScreenQuad.Dispose();
                passThroughFilter.Dispose();
                screenCapture.Dispose();
                cDev.Dispose();
            }
        }

        private void InitD3D()
        {
            cDev = new ControlDevice(EmptyControl);
            MakeMipTexture();
            MakeIntermediateTexture();
            MakeCpuTexture();
            ScreenQuad.Init(cDev.device);
            passThroughFilter = new PassThroughFilter(cDev.device);
            screenCapture = new ScreenCapture(cDev.device);
            StartRenderLoop();
        }
        
        private void MakeCpuTexture()
        {
            Texture2DDescription description = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
                Height = screenCaptureHeight,
                Width = screenCaptureWidth,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 0,
                ArraySize = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Staging
            };
            cpuTexture = new Texture2D(cDev.device, description);
        }

        private void MakeIntermediateTexture()
        {
            Texture2DDescription description = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.None,
                BindFlags = BindFlags.RenderTarget,
                Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
                Height = screenCaptureHeight,
                Width = screenCaptureWidth,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 0,
                ArraySize = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };
            intermediateTexture = new Texture2D(cDev.device, description);
            intermediateView = new RenderTargetView(cDev.device, intermediateTexture);
        }

        private void MakeMipTexture()
        {
            Texture2DDescription description = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.None,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb,
                Height = Screen.PrimaryScreen.Bounds.Height,
                Width = Screen.PrimaryScreen.Bounds.Width,
                OptionFlags = ResourceOptionFlags.GenerateMipMaps,
                MipLevels = 0,
                ArraySize = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };
            mipTexture = new Texture2D(cDev.device, description);
            mipView = new ShaderResourceView(cDev.device, mipTexture);
        }

        protected override void OnCreateControl()
        {
            if (!base.DesignMode)
            {
                InitD3D();
            }
            base.Disposed += new EventHandler(Cleanup);
            base.OnCreateControl();
        }

        protected override void OnResize(EventArgs e)
        {
            Control control = this;
            Control emptyControl = EmptyControl;
            float num = ((float)Screen.PrimaryScreen.Bounds.Width) / ((float)Screen.PrimaryScreen.Bounds.Height);
            if ((((float)control.ClientRectangle.Width) / ((float)control.ClientRectangle.Height)) < num)
            {
                emptyControl.Width = control.ClientRectangle.Width;
                emptyControl.Height = (int)(((float)emptyControl.Width) / num);
                emptyControl.Top = (control.ClientRectangle.Height - emptyControl.Height) / 2;
                emptyControl.Left = 0;
            }
            else
            {
                emptyControl.Height = control.ClientRectangle.Height;
                emptyControl.Width = (int)(emptyControl.Height * num);
                emptyControl.Top = 0;
                emptyControl.Left = (control.ClientRectangle.Width - emptyControl.Width) / 2;
            }
            ResizeControlDevice();
            base.OnResize(e);
        }

        private void RenderLoop()
        {
            while (!threadStop)
            {
                DataStream stream;
                SharpDX.DXGI.Resource resource1 = screenCapture.AcquireFrame();
                if ((resource1 == null) && (OnScreenCaptured != null))
                {
                    OnScreenCaptured(null, 0, 0, 0);
                }
                Texture2D source = resource1.QueryInterface<Texture2D>();
                ResourceRegion? sourceRegion = null;
                cDev.context.CopySubresourceRegion(source, 0, sourceRegion, mipTexture, 0, 0, 0, 0);
                source.Dispose();
                resource1.Dispose();
                screenCapture.ReleaseFrame();
                cDev.context.GenerateMips(mipView);
                float scale = topPosition - bottomPosition;
                float offset = (topPosition + bottomPosition) - 1f;
                cDev.context.ClearRenderTargetView(intermediateView, Color4.Black);
                passThroughFilter.Apply(cDev.context, mipView, intermediateView, scale, offset);
                cDev.context.CopyResource(intermediateTexture, cpuTexture);
                cDev.context.ClearRenderTargetView(cDev.renderView, (Color4)SharpDX.Color.Black);
                passThroughFilter.Apply(cDev.context, mipView, cDev.renderView, 1f, 0f);
                cDev.Present();
                DataBox box = cDev.context.MapSubresource(cpuTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out stream);
                int count = (box.RowPitch / 0x10) * screenCaptureHeight;
                System.Numerics.Vector4[] vectorArray = stream.ReadRange<System.Numerics.Vector4>(count);
                stream.Dispose();
                cDev.context.UnmapSubresource(cpuTexture, 0);
                if (OnScreenCaptured != null)
                {
                    OnScreenCaptured(vectorArray, screenCaptureWidth, screenCaptureHeight, box.RowPitch / 0x10);
                }
            }
        }

        private void ResizeControlDevice()
        {
            if (!base.DesignMode && (cDev != null))
            {
                StopRenderLoop();
                cDev.Resize();
                StartRenderLoop();
            }
        }

        public void StartRenderLoop()
        {
            if ((t == null) || !t.IsAlive)
            {
                threadStop = false;
                t = new Thread(new ThreadStart(RenderLoop));
                t.Start();
                watch.Start();
            }
        }

        public void StopRenderLoop()
        {
            if ((t != null) && t.IsAlive)
            {
                threadStop = true;
                t.Join();
            }
        }

        private int screenCaptureHeight =>
            120;

        private int screenCaptureWidth =>
            ((screenCaptureHeight * 0x10) / 9);
    }
}
