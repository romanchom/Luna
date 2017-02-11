using NAudio.Dsp;
using System;
using System.Collections.Generic;

namespace Luna.Audio
{
	class FFTProcessor
	{
		private readonly Complex[] fftBuffer;
		private readonly float[] windowFunction;
		private readonly int exponent;

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

		public FFTProcessor(int exponentOfTwo, bool magnitude = true, bool phase = false)
		{
			exponent = exponentOfTwo;
			WindowSize = 1 << exponentOfTwo;

			fftBuffer = new Complex[WindowSize];
			windowFunction = new float[WindowSize];
			if(magnitude) Magnitudes = new float[WindowSize];
			if (phase) Phases = new float[WindowSize];

			for (int i = 0; i < WindowSize; ++i){
				windowFunction[i] = (float)FastFourierTransform.BlackmannHarrisWindow(i, WindowSize);
			}
		}

		public void Process(IEnumerable<float> buffer)
		{
			using (var enumerator = buffer.GetEnumerator())
			{
				for (int i = 0; i < WindowSize; ++i)
				{
					enumerator.MoveNext();
					fftBuffer[i].X = enumerator.Current;
					fftBuffer[i].Y = 0;
				}
			}

			FastFourierTransform.FFT(true, exponent, fftBuffer);

			if (Magnitudes != null)
			{
				for (int i = 0; i < WindowSize; ++i)
				{
					float mag = (float) (fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
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
