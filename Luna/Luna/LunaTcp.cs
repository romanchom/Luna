using System;
using System.Net.Sockets;

namespace Luna {
	class LunaTcp : LunaConnectionBase {
		TcpClient tcp;
		public LunaTcp() {
			tcp = new TcpClient("192.168.0.106", 1234);
			SendTurnOn();
		}

		private bool isDisposed = false;

		public override void Dispose() {
			if (!isDisposed) {
				SendTurnOff();
				tcp.Dispose();
				isDisposed = true;
			}
		}

		protected override void Write(byte[] data, int start, int count) {
			tcp.GetStream().Write(data, start, count);
			tcp.GetStream().Flush();
		}
	}
}
