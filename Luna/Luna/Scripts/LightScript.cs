using Luna.Properties;
using System.Numerics;
using System;

namespace Luna {
	class LightScript : LunaScript {
		public override void Run() {
			float gamma = Settings.Default.Gamma;
			Vector4 color = new Vector4(
				Settings.Default.BacklightColor.R,
				Settings.Default.BacklightColor.G,
				Settings.Default.BacklightColor.B, 0);
			color /= 255.0f;

			color.X = (float)Math.Pow(color.X, gamma);
			color.Y = (float)Math.Pow(color.Y, gamma);
			color.Z = (float)Math.Pow(color.Z, gamma);

			for (int i = 0; i < LunaSerial.ledCount; ++i) {
				luna.pixelsLeft[i] = luna.pixelsRight[i] = color;
			}

			luna.whiteLeft = luna.whiteRight = (float) Math.Pow(Settings.Default.WhiteLevel, gamma);
		}
	}
}
