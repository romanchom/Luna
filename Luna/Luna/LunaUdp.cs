using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Luna
{
    class LunaUdp : LunaConnectionBase
    {
        private UdpClient udp;

        private const int port = 1234;

        private byte[] buffer = new byte[1024];

        public LunaUdp()
        {
            byte[] hello = Encoding.ASCII.GetBytes("\u0001LunaDaemon");
            udp = new UdpClient(1234);
            udp.Client.ReceiveTimeout = 1000;
            IPEndPoint dst = new IPEndPoint(IPAddress.Broadcast, 1234);
            udp.Send(hello, hello.Length, dst);
            this.udp.Receive(ref dst);
            dst = new IPEndPoint(IPAddress.Any, 1234);
            try
            {
                hello = udp.Receive(ref dst);
            }
            catch
            {
                udp.Dispose();
                throw;
            }
            udp.Connect(dst);
            SendTurnOn();
        }


        private bool isDisposed = false;

        public override void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                base.SendTurnOff();
                this.udp.Dispose();
            }
        }

        protected override void Write(byte[] data, int start, int count)
        {
            buffer[0] = 101;
            data.CopyTo(buffer, 1);
            udp.Send(buffer, count + 1);
        }
    }
}
