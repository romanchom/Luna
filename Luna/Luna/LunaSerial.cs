using System.IO.Ports;

namespace Luna {
	public class LunaSerial : LunaConnectionBase {
		SerialPort port;
		
		public LunaSerial(string portName) : base() {
			try {
				port = new SerialPort(portName);
				port.BaudRate = 115200;
                port.WriteTimeout = 1000;
				port.ReadTimeout = 1000;
				port.WriteBufferSize = 1024;
				port.DataBits = 8;
				port.DiscardNull = false;
				port.Open();
				SendTurnOn();
			} catch {
				port.Dispose();
				throw;
			}
		}

		private bool isDisposed = false;
		
		public override void Dispose() {
			if (!isDisposed) {
				SendTurnOff();
				port.Dispose();
				isDisposed = true;
			}
		}

		protected override void Write(byte[] data, int start, int count) {
			port.Write(data, start, count);
		}
	}
}
