using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Audio
{
	class BeatDetector
	{
		private readonly float[] signalPowers; 
		private readonly MovingAverage[] signalAverages;
		private readonly int bandCount;
		private readonly MovingAverage[] bpmAverages;
		
		Utility.CircularBuffer<float> powerBuffer;
		FFTProcessor powerProcessor;

		private const float bpmLow = 50;
		private const float bpmHigh = 200;

		public BeatDetector(int bandCount, int length)
		{
			this.bandCount = bandCount;
			signalPowers = new float[bandCount];
			signalAverages = new MovingAverage[bandCount];
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i] = new MovingAverage(200);
			}

			powerBuffer = new Utility.CircularBuffer<float>(2048);
			powerProcessor = new FFTProcessor(11, 10, 1, true, true);
			bpmAverages = new MovingAverage[100];
			for(int i = 0; i < 100; ++i)
			{
				bpmAverages[i] = new MovingAverage(10);
			}
		}

		public void AddInput(float[] bands)
		{
			for (int i = 0; i < bandCount; ++i)
			{
				signalPowers[i] += bands[i];
			}
		}

		public void Analyze()
		{
			float power = 0;
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i].Add(signalPowers[i]);
				float v = signalPowers[i] - signalAverages[i].Value;
				power += v * v;
			}
			power /= bandCount;
			powerBuffer.Push(power);
				
			powerProcessor.Process(powerBuffer);
		}


		float lastPhase = 0;
		public bool HasBeat = false;
		public void Visualize(Controls.SpectrumVisualizerControl viz)
		{
			int count = 100;
			float[] amp = new float[count];
			float max = -10;
			int peakIndex = -1;
			for (int i = 0; i < count; ++i)
			{
				bpmAverages[i].Add(powerProcessor.Magnitudes[i]);
				if(i > 2 && bpmAverages[i].Value > max)
				{
					peakIndex = i;
					max = bpmAverages[i].Value;
				}

				amp[i] = bpmAverages[i].Value + 1;
			}

			float phase = powerProcessor.Phases[peakIndex];

			if(Math.Abs(phase) < 1.0f && phase * lastPhase < 0)
			{
				HasBeat = true;
			}else
			{
				HasBeat = false;
			}
			lastPhase = phase;


			if (viz != null)
			{
				viz.amplitudes = amp;
			}
		}

		public void Reset()
		{
			for(int i = 0; i < bandCount; ++i)
			{
				signalPowers[i] = 0;
			}
		}
	}
}
