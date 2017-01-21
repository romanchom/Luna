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
		public float[] Magnitudes
		{
			get;
			private set;
		}
		public float[] Phases
		{
			get;
			private set;
		}

		public FFTProcessor(int exponentOfTwo, float logarithmBase, float unitsPerDecade, bool magnitude = true, bool phase = false)
		{
			exponent = exponentOfTwo;
			WindowSize = 1 << exponentOfTwo;
			logMul = (float) (0.5 / Math.Log(logarithmBase)) * unitsPerDecade; // includes sqrt of manitude;

			fftBuffer = new Complex[WindowSize];
			windowFunction = new float[WindowSize];
			if(magnitude) Magnitudes = new float[WindowSize];
			if (phase) Phases = new float[WindowSize];

			for (int i = 0; i < WindowSize; ++i){
				windowFunction[i] = (float)FastFourierTransform.BlackmannHarrisWindow(i, WindowSize);
			}
		}

		public void Process(Utility.CircularBuffer<float> buffer)
		{
			for(int i = 0; i < WindowSize; ++i)
			{
				fftBuffer[i].X = buffer[-i - 1];
				fftBuffer[i].Y = 0;
			}

			FastFourierTransform.FFT(true, exponent, fftBuffer);

			if (Magnitudes != null)
			{
				for (int i = 0; i < WindowSize; ++i)
				{
					float mag = (float)Math.Log(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
					mag *= logMul; // apply correct logarithm base and sqrt of magnitude
					Magnitudes[i] = mag;
				}
			}

			if (Phases != null)
			{
				for (int i = 0; i < WindowSize; ++i)
				{
					Phases[i] = (float)Math.Atan2(fftBuffer[i].Y, fftBuffer[i].X);
				}
			}
		}
	}
}
