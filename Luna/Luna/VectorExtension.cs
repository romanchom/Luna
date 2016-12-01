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

        public static void Clamp0_255(ref Vector4 v)
        {
            v.X = Math.Max(Math.Min(v.X, 255f), 0f);
            v.Y = Math.Max(Math.Min(v.Y, 255f), 0f);
            v.Z = Math.Max(Math.Min(v.Z, 255f), 0f);
            v.W = Math.Max(Math.Min(v.W, 255f), 0f);
        }

        public static Vector4 Round(Vector4 value)
        {
            Vector4 ret;
            ret.X = (float) Math.Round(value.X);
            ret.Y = (float) Math.Round(value.Y);
            ret.Z = (float) Math.Round(value.Z);
            ret.W = 0;
            return ret;
        }
    }
}
