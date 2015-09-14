using Luna.Properties;
using System;
using System.Numerics;

namespace Luna {
	public abstract class LunaConnectionBase : IDisposable {
		public Vector4[] pixelsLeft;
		public Vector4[] pixelsRight;
		public float whiteLeft;
		public float whiteRight;

		public const int ledCount = 120;

		protected byte[] dataBuffer = new byte[3 + 4 + ledCount * 6]; //lenght (2), header(1), white(4), leds left+right 3 bytes per pixel

		public abstract void Dispose();

		public LunaConnectionBase() {
			pixelsLeft = new Vector4[ledCount];
			pixelsRight = new Vector4[ledCount];
		}

		private readonly float[][] dithers = new float[][] {
			new float[] {
				0
			},
			new float[] {
				0,
				0.5f
			},
			new float[] {
				0,
				0.75f,
				0.25f,
				0.5f,
			},
			new float[] {
				0 / 6.0f,
				5 / 6.0f,
				2 / 6.0f,
				3 / 6.0f,
				1 / 6.0f,
				4 / 6.0f,
			},
			new float[] {
				0 / 8.0f,
				7 / 8.0f,
				2 / 8.0f,
				5 / 8.0f,
				1 / 8.0f,
				6 / 8.0f,
				3 / 8.0f,
				4 / 8.0f,
			}
		};
		
		public void SendTurnOn() {
			dataBuffer[0] = 1;
			dataBuffer[1] = 0;
			dataBuffer[2] = 0;
			Write(dataBuffer, 0, 3);
		}

		protected void SendTurnOff() {
			dataBuffer[0] = 1;
			dataBuffer[1] = 0;
			dataBuffer[2] = 1;
			Write(dataBuffer, 0, 3);
		}


		public void Send() {
			int index = 0;
			UInt16 length = 1 + 2 + 2 + ledCount * 6;
			UInt16 wl = (UInt16)(whiteLeft * UInt16.MaxValue);
			UInt16 wr = (UInt16)(whiteRight * UInt16.MaxValue);

			BitConverter.GetBytes(length).CopyTo(dataBuffer, index);
			index += 2;
			dataBuffer[index] = 2;
			index++;
			BitConverter.GetBytes(wl).CopyTo(dataBuffer, index);
			index += 2;
			BitConverter.GetBytes(wr).CopyTo(dataBuffer, index);
			index += 2;
			
			for(int i = 3; i < 7; ++i) {
				Console.Write("0x{0:X2}\t", dataBuffer[i]);
			}
			Console.WriteLine();

			float[] dither = dithers[Settings.Default.Dithering];
			int ditherLength = dither.Length;

			Vector4 multiplier = new Vector4(Settings.Default.RedMultiplier, Settings.Default.GreenMultiplier, Settings.Default.BlueMultiplier, 0);
			VectorExtension.Pow(ref multiplier, Settings.Default.Gamma);

			multiplier *= 255;

			for (int i = 0; i < ledCount; ++i) {
				Vector4 pixel = pixelsLeft[i];
				float d = dither[i % ditherLength];
				Vector4 di = new Vector4(d, d, d, 0);
				pixel = pixel * multiplier + di;
				dataBuffer[index++] = (byte)(pixel.X);
				dataBuffer[index++] = (byte)(pixel.Y);
				dataBuffer[index++] = (byte)(pixel.Z);
			}

			for (int i = 0; i < ledCount; ++i) {
				Vector4 pixel = pixelsRight[i];
				float d = dither[i % ditherLength];
				Vector4 di = new Vector4(d, d, d, 0);
				pixel = pixel * multiplier + di;
				dataBuffer[index++] = (byte)(pixel.X);
				dataBuffer[index++] = (byte)(pixel.Y);
				dataBuffer[index++] = (byte)(pixel.Z);
			}

			Write(dataBuffer, 0, dataBuffer.Length);
			/*if (port.BytesToRead > 0) {
				Console.Write(port.ReadExisting());
			}*/
		}


		protected abstract void Write(byte[] data, int start, int count);
	}
}
