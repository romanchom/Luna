using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luna.Graphics {
	class ScreenCapture : IDisposable {
private OutputDuplication duplicatedOutput;

		private SharpDX.Direct3D11.Device device;

		public ScreenCapture(SharpDX.Direct3D11.Device device)
		{
			this.device = device;
			this.Reset();
		}

		public SharpDX.DXGI.Resource AcquireFrame()
		{
			SharpDX.DXGI.Resource result;
			while (true)
			{
				try
				{
					OutputDuplicateFrameInformation duplicateFrameInformation;
					SharpDX.DXGI.Resource screenResource;
					this.duplicatedOutput.AcquireNextFrame(4500, out duplicateFrameInformation, out screenResource);
					result = screenResource;
				}
				catch (SharpDXException exp)
				{
					if ((long)exp.HResult != (long)(-2005270489))
					{
						this.duplicatedOutput.Dispose();
						while (true)
						{
							try
							{
								this.Reset();
							}
							catch
							{
								continue;
							}
							break;
						}
						continue;
					}
					result = null;
				}
				break;
			}
			return result;
		}

		public void ReleaseFrame()
		{
			try
			{
				this.duplicatedOutput.ReleaseFrame();
			}
			catch (SharpDXException)
			{
				this.duplicatedOutput.Dispose();
				while (true)
				{
					try
					{
						this.Reset();
					}
					catch
					{
						continue;
					}
					break;
				}
			}
		}

		public void Dispose()
		{
			this.duplicatedOutput.Dispose();
		}

		private void Reset()
		{
			using (Factory1 DXGIfactory = new Factory1())
			{
				Adapter1 adapter;
				if (DXGIfactory.Adapters1[0].Outputs.Length == 0)
				{
					adapter = DXGIfactory.Adapters1[1];
				}
				else
				{
					adapter = DXGIfactory.Adapters1[0];
				}
				Output1 output = adapter.Outputs[0].QueryInterface<Output1>();
				this.duplicatedOutput = output.DuplicateOutput(this.device);
			}
		}
	}
}
