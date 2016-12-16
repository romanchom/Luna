using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
    class PlasmaScript : LunaScript
    {
        private Perlin3D noise = new Perlin3D();
        private System.Diagnostics.Stopwatch time;
        double[] offsets = new double[2];
         
        private double animationSpeed = 0.2f;
        private double noiseFrequency = 6;
        private double hueWidth = 1.0 / 3.0;
        private double hueChangeSpeed = 0.002f;

        public PlasmaScript()
        {
            time = new System.Diagnostics.Stopwatch();
            time.Start();
            Random r = new Random();
            for(int i = 0; i < 2; ++i)
            {
                offsets[i] = r.NextDouble() * 100 + 100;
            }
        }

        public override void Run()
        {
            double t = time.Elapsed.TotalSeconds * animationSpeed;
            double hueOffset = time.Elapsed.TotalSeconds * hueChangeSpeed;

            double hueMul = hueWidth;
            for (int c = 0; c < 2; ++c)
            {
                double y = offsets[c] + t;
                for(int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    double x = noiseFrequency * i / LunaConnectionBase.ledCount;
                    luna.pixels[c][i] = hueToRGB((float) (noise.At(x, y) * hueMul + hueOffset));
                }
            }
        }

        private static Vector4 hueToRGB(float hue)
        {
            hue = hue - (float) Math.Floor(hue);
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
