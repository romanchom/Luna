using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luna.Graphics {
	class ScreenCapture : IDisposable {
		private OutputDuplication duplicatedOutput;

		public ScreenCapture(SharpDX.Direct3D11.Device device) {
			using (Factory1 DXGIfactory = new Factory1()) {
				Output1 output = DXGIfactory.Adapters1[0].Outputs[0].QueryInterface<Output1>();
				duplicatedOutput = output.DuplicateOutput(device);
			}
		}

		public Resource AcquireFrame() {
			Resource screenResource;
			OutputDuplicateFrameInformation duplicateFrameInformation;
			duplicatedOutput.AcquireNextFrame(1000, out duplicateFrameInformation, out screenResource);
			return screenResource;
			
		}

		public void ReleaseFrame() {
			duplicatedOutput.ReleaseFrame();
		}

		public void Dispose() {
			duplicatedOutput.Dispose();
		}
	}
}
