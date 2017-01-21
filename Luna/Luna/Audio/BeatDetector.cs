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

		private const float bpmLow = 40;
		private const float bpmHigh = 220;
		private readonly int lowBinIndex;
		private readonly int highBinIndex;
		private readonly int binCount;



		public BeatDetector(int sampligRate, int bandCount, int length)
		{
			this.bandCount = bandCount;
			signalPowers = new float[bandCount];
			signalAverages = new MovingAverage[bandCount];
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i] = new MovingAverage(50);
			}

			powerProcessor = new FFTProcessor(10, 10, 1, true, true);
			powerBuffer = new Utility.CircularBuffer<float>(powerProcessor.WindowSize);

			lowBinIndex = (int)Math.Floor(powerProcessor.WindowSize * bpmLow / (60 * sampligRate));
			highBinIndex = (int)Math.Ceiling(powerProcessor.WindowSize * bpmHigh / (60 * sampligRate)) + 1;
			binCount = highBinIndex - lowBinIndex;

			bpmAverages = new MovingAverage[binCount];
			for(int i = 0; i < binCount; ++i)
			{
				bpmAverages[i] = new MovingAverage(50);
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

		public void Analyze()
		{
			float power = 0;
			for (int i = 0; i < bandCount; ++i)
			{
				signalAverages[i].Add(signalPowers[i]);
				float v = signalPowers[i] - signalAverages[i].Value;
				v = Math.Max(1e-5f, v);
				v *= v;
				power += v;
			}
			power /= bandCount;
			powerBuffer.Push(power);
				
			powerProcessor.Process(powerBuffer);

			for(int i = 0; i < binCount; ++i)
			{
				MovingAverage avg = bpmAverages[i];
				avg.Add(powerProcessor.Magnitudes[lowBinIndex + i]);
				peakiness[i] = (avg.Value - avg.Variance * 2) + 5;
			}

			float maxScore = float.NegativeInfinity;
			int index = -1;
			for (int i = 0; i < binCount; ++i)
			{
				scores[i] = GetScore(i + lowBinIndex) * 0.2f;
				if(scores[i] > maxScore)
				{
					index = i;
					maxScore = scores[i];
				}
			}

			const float phaseOffset = (float) (Math.PI * (4 - 0));
			const float PI = (float)(Math.PI);
			if (index != 1)
			{
				float phase = powerProcessor.Phases[index + lowBinIndex];
				phase = (phase + phaseOffset) % (2 * PI) - PI;
				

				if (Math.Abs(phase) < PI * 0.2f && phase * lastPhase < 0)
				{
					HasBeat = true;
				}
				else
				{
					HasBeat = false;
				}
				lastPhase = phase;
			}
		}

		float GetScore(float index)
		{
			float ret = peakiness[(int)Math.Round(index) - lowBinIndex];
			//ret += GetLowerHarmonicsScore(index / 2);
			//ret += GetHigherHarmonicsScore(index * 2, 0.5f);
			return ret;
		}

		float GetLowerHarmonicsScore(float index)
		{
			float ret = 0;
			if (index > lowBinIndex)
			{
				ret += peakiness[(int)Math.Round(index) - lowBinIndex];
				//ret += GetLowerHarmonicsScore(index / 2) / 2;
			}
			return ret;
		}

		float GetHigherHarmonicsScore(float index, float width)
		{
			int iMin = (int)Math.Round(index - width) - lowBinIndex;
			iMin = Math.Min(iMin, highBinIndex);
			int iMax = (int)Math.Round(index + width) - lowBinIndex;
			iMax = Math.Min(iMax, highBinIndex);

			float ret = 0;
			for(int i = iMin; i < iMax; ++i)
			{
				ret += peakiness[i];
			}
			//if (ret != 0) ret += GetHigherHarmonicsScore(index * 2, width * 2);
			return ret;
		}


		float lastPhase = 0;
		public bool HasBeat = false;
		public void Visualize(Controls.SpectrumVisualizerControl viz)
		{
			const float scale = 0.7f;
			const float offset = 0.0f;
			float[] src = scores;
			float[] arr = new float[src.Length];
			for(int i = 0; i < arr.Length; ++i)
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
