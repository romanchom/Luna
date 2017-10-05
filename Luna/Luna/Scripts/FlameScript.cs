using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;

namespace Luna
{
    class FlameScript : LunaScript
    {
        protected override int period => 10;
        private float[,] temperatures = new float[2, LunaConnectionBase.ledCount];
        private int position = 0;
        private Random random = new Random();

        private float[] sourceBurnRates = new float[2];
        private float sourceTemperatureLow = 1000;
        private float sourceTemperatureHigh = 3700;
        private float sourceChangeSpeed = 0.2f;

        private float brightness = 1;

        public override void Run()
        {
            float coolRate = sourceTemperatureHigh / LunaConnectionBase.ledCount;
            for(int c = 0; c < 2; ++c)
            {
                for(int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    temperatures[c, i] -= coolRate; 
                }

                sourceBurnRates[c] += (float)(random.NextDouble() * 2 - 1) * sourceChangeSpeed;
                sourceBurnRates[c] = (sourceBurnRates[c] - 1) * (1 - sourceChangeSpeed) + 1.0f;
               
                temperatures[c, position] = sourceBurnRates[c] * (sourceTemperatureHigh - sourceTemperatureLow) + sourceTemperatureLow;

                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    float t = temperatures[c, (i + position) % LunaConnectionBase.ledCount];
                    luna.pixels[c][i] = temperatureToRGB(t) * Math.Min(1.0f, i * 0.1f);
                }
            }
            position = (position + (LunaConnectionBase.ledCount - 1)) % LunaConnectionBase.ledCount;
        }
        
        private Matrix4x4 colorSpaceTransform = ColorSpace.ws2812.FromXYZToRGB;
        private Vector4 temperatureToRGB(float t)
        {
            // SCIENCE BITCH!!
            // compute Planckian locus of xy chromaticity coords
            // then transform to our color space
            float t2 = t * t;
            float t3 = t2 * t;

            float x, y;
            if(t <= 4000)
            {
                x = -0.2661239e9f / t3
                    -0.2343580e6f / t2
                    + 0.8776956e3f / t
                    + 0.179910f;
            }else
            {
                x = -3.0258469e9f / t3
                    + 2.1070379e6f / t2
                    + 0.2226347e3f / t
                    + 0.240390f;
            }

            float x2 = x * x;
            float x3 = x2 * x;

            if(t < 2222)
            {
                y = -1.1063814f * x3
                    - 1.34811020f * x2
                    + 2.18555832f * x
                    - 0.20219683f;
            }else if(t < 4000)
            {
                y = -0.9549476f * x3
                    - 1.37418593f * x2
                    + 2.09137015f * x
                    - 0.16748867f;
            }
            else
            {
                y = 3.0817580f* x3
                    - 5.8733867f * x2
                    + 3.75112997f * x
                    - 0.37001483f;
            }

            float z = 1 - x - y;
            float energyMultiplier = Math.Max(0, (t - 1100)) * brightness / sourceTemperatureHigh;
            return Vector4.Transform(new Vector4(x, y, z, 0) * (energyMultiplier / y), colorSpaceTransform);
        }
    }
}
