using System;
using System.Numerics;
namespace Luna {
	public class VectorExtension {
		public static void Pow(ref Vector4 vector, float exp) {
			vector.X = (float)Math.Pow(vector.X, exp);
			vector.Y = (float)Math.Pow(vector.Y, exp);
			vector.Z = (float)Math.Pow(vector.Z, exp);
			vector.W = (float)Math.Pow(vector.W, exp);
		}
	}
}
