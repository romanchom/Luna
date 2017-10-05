using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Audio
{
	class SpectrumVizualizer
	{
		public SpectrumVizualizer(int pixelCount, float fLow, float fHigh, int samplingRate)
		{
			fft = new FFTProcessor(13);
			aggregator = new ToneAggregator(pixelCount, fLow, fHigh, samplingRate, 1 << 13);
			colors = new RainbowPerOctave(pixelCount, fLow, fHigh);
			filter = new HoldFilter(pixelCount);
			Colors = new Vector4[pixelCount];
		}

		public void Process(IEnumerable<float> samples)
		{
			fft.Process(samples);
			aggregator.Aggregate(fft.Magnitudes);
			aggregator.Normalize(-3);
			filter.Process(aggregator.Tones, 0.02f, 0.0f);
			for(int i = 0; i < Colors.Length; ++i)
			{
				Colors[i] = filter.Values[i] * colors.Colors[i];
			}
		}

		public Vector4[] Colors { get; private set; }

		private FFTProcessor fft;
		private ToneAggregator aggregator;
		private RainbowPerOctave colors;
		private HoldFilter filter;


	}
}
