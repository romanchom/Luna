using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Luna
{
    class ColorSpace
    {
        private Matrix4x4 rgbToXYZ;
        private Matrix4x4 xyzToRGB;

        public ColorSpace(Vector2 white, Vector2 red, Vector2 green, Vector2 blue)
        {
            rgbToXYZ = new Matrix4x4(
                red.X, green.X, blue.X, 0,
                red.Y, green.Y, blue.Y, 0,
                1 - red.X - red.Y, 1 - green.X - green.Y, 1 - blue.X - blue.Y, 0,
                0, 0, 0, 1);

            //rgbToXYZ = Matrix4x4.Transpose(rgbToXYZ);
            Matrix4x4.Invert(rgbToXYZ, out xyzToRGB);

            Vector4 scale = new Vector4(white.X, white.Y, 1 - white.X - white.Y, 0);
            scale /= white.Y;
            scale = Vector4.Transform(scale, Matrix4x4.Transpose(xyzToRGB));
            Matrix4x4 scaleMatrix = new Matrix4x4(
                scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                0, 0, 0, 1);
            rgbToXYZ = rgbToXYZ * scaleMatrix;
            Matrix4x4.Invert(rgbToXYZ, out xyzToRGB);

            rgbToXYZ = Matrix4x4.Transpose(rgbToXYZ);
            xyzToRGB = Matrix4x4.Transpose(xyzToRGB);
        }

        public Matrix4x4 FromXYZToRGB => xyzToRGB;

        public Matrix4x4 FromRGBToXYZ => rgbToXYZ;

        public Matrix4x4 FromTo(ColorSpace other)
        {
            return rgbToXYZ * other.xyzToRGB;
        }

        public static ColorSpace sRGB => new ColorSpace(
                new Vector2(0.31271f, 0.32902f),
                new Vector2(0.64f, 0.33f),
                new Vector2(0.3f, 0.6f),
                new Vector2(0.15f, 0.06f));

        public static ColorSpace rec2020 => new ColorSpace(
                new Vector2(0.31271f, 0.32902f),
                new Vector2(0.708f, 0.292f),
                new Vector2(0.170f, 0.797f),
                new Vector2(0.131f, 0.046f));

        public static ColorSpace ws2812 => new ColorSpace(
                new Vector2(0.31271f, 0.32902f),
                new Vector2(0.6918f, 0.3047f),
                new Vector2(0.1887f, 0.7161f),
                new Vector2(0.131f, 0.07f));
    }
}
