using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Audio
{
	class ToneAggregator
	{
		private readonly int toneCount;
		private readonly float lowFrequency;
		private readonly float octaveCount;
		private readonly float step;
		private readonly float fMul;

		public float[] Tones {
			get; private set;
		}

		public ToneAggregator(int toneCount, float lowFrequency, float highFrequency, int samplingRate, int windowSize)
		{
			this.toneCount = toneCount;
			this.lowFrequency = lowFrequency;

			octaveCount = (float)(Math.Log(highFrequency / lowFrequency) / Math.Log(2));
			step = octaveCount / toneCount;

			fMul = (float) windowSize / samplingRate;

			Tones = new float[toneCount];
		}

		public void Aggregate(float[] logMag, float norm)
		{
			int jl = 1;
			for (int i = 0; i < toneCount; ++i)
			{
				float f1 = lowFrequency * (float)Math.Pow(2, step * (i + 1));
				int jh = (int)(f1 * fMul);
				if (jh <= jl) jh = jl + 1;

				float max = -10;
				for (int j = jl; j < jh; ++j)
				{
					max = Math.Max(max, logMag[j]);
				}
				Tones[i] = max;
				norm = Math.Max(norm, max);

				jl = jh;
			}
			for (int i = 0; i < toneCount; ++i)
			{
				Tones[i] -= norm;
			}
		}
	}
}
