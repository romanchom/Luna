using NAudio.Dsp;
using System;

namespace Luna.Audio
{
	class FFTProcessor
	{
		private readonly Complex[] fftBuffer;
		private readonly float[] windowFunction;
		private readonly int exponent;
		private readonly float logMul;

		public int WindowSize {
			get; private set;
		}
		public float[] Bins
		{
			get;
			private set;
		}

		private float maximumEnergy;

		public FFTProcessor(int exponentOfTwo, float logarithmBase, float unitsPerDecade)
		{
			exponent = exponentOfTwo;
			WindowSize = 1 << exponentOfTwo;
			logMul = (float) (0.5 / Math.Log(logarithmBase)) * unitsPerDecade; // includes sqrt of manitude;

			fftBuffer = new Complex[WindowSize];
			windowFunction = new float[WindowSize];
			Bins = new float[WindowSize];

			for (int i = 0; i < WindowSize; ++i){
				windowFunction[i] = (float)FastFourierTransform.BlackmannHarrisWindow(i, WindowSize);
			}
		}

		public void Process(Utility.CircularBuffer<float> buffer)
		{
			for(int i = 0; i < WindowSize; ++i)
			{
				fftBuffer[i].X = buffer[i - WindowSize];
				fftBuffer[i].Y = 0;
			}

			FastFourierTransform.FFT(true, exponent, fftBuffer);

			for (int i = 0; i < WindowSize; ++i)
			{
				float mag = (float) Math.Log(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
				mag *= logMul; // apply correct logarithm base and sqrt of magnitude
				Bins[i] = mag;
			}
		}

		public void Normalize(float norm)
		{

		}
	}
}
