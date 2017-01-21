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
		private readonly float[] peakiness;
		private readonly float[] scores;
		
		Utility.CircularBuffer<float> powerBuffer;
		FFTProcessor powerProcessor;

		private const float bpmLow = 60;
		private const float bpmHigh = 180;
		private readonly int lowBinIndex;
		private readonly int highBinIndex;
		private readonly int binCount;
		private readonly float period;


		private float lastPhase = 0;
		private int periodsSinceBeat;
		public float BPS { get; private set; }
		public bool HasBeat	{ get; private set; }

		public BeatDetector(int sampligRate, int bandCount, int length)
		{
			this.bandCount = bandCount;
			period = 1.0f / sampligRate;
			signalPowers = new float[bandCount];
			signalAverages = new MovingAverage[bandCount];
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i] = new MovingAverage(100);
			}

			powerProcessor = new FFTProcessor(10, 10, 1, true, true);
			powerBuffer = new Utility.CircularBuffer<float>(powerProcessor.WindowSize);

			lowBinIndex = (int)Math.Floor(powerProcessor.WindowSize * bpmLow / (60 * sampligRate));
			highBinIndex = (int)Math.Ceiling(powerProcessor.WindowSize * bpmHigh / (60 * sampligRate)) + 1;
			binCount = highBinIndex - lowBinIndex;

			bpmAverages = new MovingAverage[binCount];
			for(int i = 0; i < binCount; ++i)
			{
				bpmAverages[i] = new MovingAverage(200);
			}
			peakiness = new float[binCount];
			scores = new float[binCount];

		}

		public void AddInput(float[] bands)
		{
			for (int i = 0; i < bandCount; ++i)
			{
				signalPowers[i] += bands[i];
			}
		}

		private int beatIndex = -1;
		public void Analyze()
		{
			float power = 0;
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i].Add(signalPowers[i]);
				float v = signalPowers[i];
				v *= v;
				power += v;
			}
			power += 1e-5f;
			powerBuffer.Push(power);
				
			powerProcessor.Process(powerBuffer);
			

			for(int i = 0; i < binCount; ++i)
			{
				MovingAverage avg = bpmAverages[i];
				avg.Add(powerProcessor.Magnitudes[lowBinIndex + i] + powerProcessor.Magnitudes[(lowBinIndex + i) * 2]);
				peakiness[i] = avg.Value;
			}

			if (beatIndex != -1)
			{
				float phase = powerProcessor.Phases[beatIndex + lowBinIndex];

				if (Math.Abs(phase) < 3.14f * 0.5f && phase * lastPhase < 0)
				{
					HasBeat = true;
					BPS = 1.0f / (periodsSinceBeat * period);
					periodsSinceBeat = 0;
					Console.WriteLine(BPS);
					FindBeat();
				}
				else
				{
					periodsSinceBeat++;
					HasBeat = false;
				}
				lastPhase = phase;
			} else
			{
				FindBeat();
			}
		}

		private void FindBeat()
		{
			int index = -1;
			float maxScore = float.NegativeInfinity;
			for (int i = 0; i < binCount; ++i)
			{
				if (peakiness[i] > maxScore)
				{
					index = i;
					maxScore = peakiness[i];
				}
			}

			float currentScore = beatIndex != -1 ? peakiness[beatIndex] : float.NegativeInfinity;
			if(index != -1 && currentScore < maxScore - 0.2f)
			{
				beatIndex = index;
			}
		}

		float GetScore(int index)
		{
			float ret = peakiness[index - lowBinIndex];
			
			return ret;
		}
		
		public void Visualize(Controls.SpectrumVisualizerControl viz)
		{
			float[] src = peakiness;
			float min = float.PositiveInfinity;
			float max = float.NegativeInfinity;
			int count = src.Length;
			//count = 100;
			for (int i = 0; i < count; ++i)
			{
				min = Math.Min(min, src[i]);
				max = Math.Max(max, src[i]);
			}

			float scale = 1.0f / (max - min);
			float offset = -min * scale;
			float[] arr = new float[count];
			for(int i = 0; i < count; ++i)
			{
				arr[i] = src[i] * scale + offset;
			}			
			

			if (viz != null)
			{
				viz.amplitudes = arr;
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
