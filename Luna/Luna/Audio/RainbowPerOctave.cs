using System;
using System.Numerics;

namespace Luna.Audio
{
	class RainbowPerOctave
	{
		public Vector4[] Colors { get; private set; }

		public RainbowPerOctave(int toneCount, float lowFrequency, float highFrequency)
		{
			float octaveCount = (float)(Math.Log(highFrequency / lowFrequency) / Math.Log(2));
			float step = octaveCount / toneCount;
			Colors = new Vector4[toneCount];
			for (int i = 0; i < toneCount; ++i)
			{
				Colors[i] = hueToRGB(1 - (step * i) % 1);
			}
		}

		private static Vector4 hueToRGB(float hue)
		{
			float h6 = hue * 6;

			Vector4 ret = new Vector4(
				Math.Abs(3 - h6) - 1,
				2 - Math.Abs(2 - h6),
				2 - Math.Abs(4 - h6),
				0);
			VectorExtension.Clamp0_1(ref ret);
			return ret;
		}
	}
}
