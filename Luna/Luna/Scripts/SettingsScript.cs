using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna {
	class SettingsScript : LunaScript {
		public override void Run() {
			System.Numerics.Vector4 white = System.Numerics.Vector4.One;

			for (int i = 0; i < LunaSerial.ledCount; ++i) {
				luna.pixelsLeft[i] = luna.pixelsRight[i] = white;
			}

			luna.whiteLeft = luna.whiteRight = 0;
		}
	}
}
