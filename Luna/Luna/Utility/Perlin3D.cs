using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
    class Perlin3D
    {
        private int[] p;

        public Perlin3D()
        {
            Random random = new Random();
            p = new int[512];
            for(int i = 0; i < 256; ++i)
            {
                p[i] = i;
            }
            int n = 256;
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1);
                int temp = p[i];
                p[i] = p[n];
                p[n] = temp;
            }
            for (int i = 0; i < 256; ++i)
            {
                p[i + 256] = p[i];
            }
        }

        public double At(double x, double y = 0, double z = 0)
        {
            int X = (int)Math.Floor(x) & 255,                  // FIND UNIT CUBE THAT
                Y = (int)Math.Floor(y) & 255,                  // CONTAINS POINT.
                Z = (int)Math.Floor(z) & 255;
            x -= Math.Floor(x);                                // FIND RELATIVE X,Y,Z
            y -= Math.Floor(y);                                // OF POINT IN CUBE.
            z -= Math.Floor(z);
            double u = Fade(x),                                // COMPUTE FADE CURVES
                   v = Fade(y),                                // FOR EACH OF X,Y,Z.
                   w = Fade(z);
            int A = p[X] + Y, AA = p[A] + Z, AB = p[A + 1] + Z,      // HASH COORDINATES OF
                B = p[X + 1] + Y, BA = p[B] + Z, BB = p[B + 1] + Z;      // THE 8 CUBE CORNERS,

            return Lerp(w, Lerp(v, Lerp(u, Grad(p[AA], x, y, z),  // AND ADD
                                           Grad(p[BA], x - 1, y, z)), // BLENDED
                                   Lerp(u, Grad(p[AB], x, y - 1, z),  // RESULTS
                                           Grad(p[BB], x - 1, y - 1, z))),// FROM  8
                           Lerp(v, Lerp(u, Grad(p[AA + 1], x, y, z - 1),  // CORNERS
                                           Grad(p[BA + 1], x - 1, y, z - 1)), // OF CUBE
                                   Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1),
                                           Grad(p[BB + 1], x - 1, y - 1, z - 1))));
        }
        static double Fade(double t) { return t * t * t * (t * (t * 6 - 15) + 10); }
        static double Lerp(double t, double a, double b) { return a + t * (b - a); }
        static double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
            double u = h < 8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
                   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}
