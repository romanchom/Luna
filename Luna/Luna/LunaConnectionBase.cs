using Luna.Properties;
using System;
using System.Numerics;

namespace Luna {
	public abstract class LunaConnectionBase : IDisposable {
		public Vector4[] pixelsLeft;
		public Vector4[] pixelsRight;
        public Vector4[][] pixels;
		public float whiteLeft;
		public float whiteRight;

		public const int ledCount = 120;

		protected byte[] dataBuffer = new byte[1 + 2 + 2 + ledCount * 6]; //header(1), white(4), leds left+right 3 bytes per pixel

		public abstract void Dispose();

		public LunaConnectionBase() {
			pixelsLeft = new Vector4[ledCount];
			pixelsRight = new Vector4[ledCount];
            pixels = new Vector4[][] { pixelsLeft, pixelsRight };
		}

        public void SendTurnOn()
        {
            dataBuffer[0] = 1;
            Write(this.dataBuffer, 0, 1);
        }

        protected void SendTurnOff()
        {
            dataBuffer[0] = 2;
            Write(this.dataBuffer, 0, 1);
        }

        public void SendDupa()
        {
            dataBuffer[0] = 0;
            Write(this.dataBuffer, 0, 1);
        }


        public void Send() {
            int index = 0;
            ushort arg_2B_0 = (ushort)(this.whiteLeft * 65535f);
            ushort wr = (ushort)(this.whiteRight * 65535f);
            dataBuffer[index] = 61;
            index++;
            BitConverter.GetBytes(arg_2B_0).CopyTo(dataBuffer, index);
            index += 2;
            BitConverter.GetBytes(wr).CopyTo(dataBuffer, index);
            index += 2;
            Vector4 multiplier = new Vector4(Settings.Default.RedMultiplier, Settings.Default.GreenMultiplier, Settings.Default.BlueMultiplier, 0f);
            multiplier *= 255f;
            Vector4 error = new Vector4();
            for (int i = 0; i < 120; i++)
            {
                Vector4 pixel = pixelsLeft[i];
                pixel *= multiplier;
                pixel += error;
                Vector4 value = VectorExtension.Round(pixel);
                error = pixel - value;
                VectorExtension.Clamp0_255(ref value);
                dataBuffer[index++] = (byte)value.X;
                dataBuffer[index++] = (byte)value.Y;
                dataBuffer[index++] = (byte)value.Z;
            }
            error = new Vector4();
            for (int i = 0; i < 120; i++)
            {
                Vector4 pixel = pixelsRight[i];
                pixel *= multiplier;
                pixel += error;
                Vector4 value = VectorExtension.Round(pixel);
                error = pixel - value;
                VectorExtension.Clamp0_255(ref value);
                dataBuffer[index++] = (byte)value.X;
                dataBuffer[index++] = (byte)value.Y;
                dataBuffer[index++] = (byte)value.Z;
            }
            Write(dataBuffer, 0, index);
		}

		protected abstract void Write(byte[] data, int start, int count);
	}
}
