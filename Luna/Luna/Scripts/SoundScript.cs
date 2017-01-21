using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.CoreAudioApi;
using System.Linq;
using System;
using System.Numerics;
using Luna.Controls;
using System.Runtime.InteropServices;
using Luna.Audio;

namespace Luna
{
    class SoundScript : LunaScript
    {
        protected override int period => 10;

        private const int windowSizePot = 13;
        private const int samplingRate = 48000;
        private const int channelCount = 2;
        private float lowFrequency = 100;
        private float highFrequency = 12000;

		AudioCapture capture;
		FFTProcessor processor;
		ToneAggregator aggregator;
		RainbowPerOctave colors;
		HoldFilter[] holdFilters;

		Utility.CircularBuffer<float> powerBuffer;
		FFTProcessor powerProcessor;
		MovingAverage[] averages;

        public SpectrumVisualizerControl spectrumViz;

        public override void Run()
        {
			float[] powers = new float[120];
			capture.ReadPackets();
			for (int c = 0; c < 2; ++c)
			{
				processor.Process(capture.Channels[c]);
				aggregator.Aggregate(processor.Bins, -6);
				for(int i = 0; i < 120; ++i)
				{
					averages[i].Add(aggregator.Tones[i]);
					powers[i] += aggregator.Tones[i];
				}
				holdFilters[c].Process(aggregator.Tones, 0.02f, 1);
				for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
					luna.pixels[c][i] = colors.Colors[i] * (float) Math.Pow(holdFilters[c].Values[i], 2.2f) * 2;
                }
            }

			float power = 0;
			for (int i = 0; i < 120; ++i)
			{
				averages[i].Add(powers[i]);
				float v = powers[i] - averages[i].Value;
				power += v * v;
			}

			powerBuffer.Push(power / 10000 + 0.5f);
			powerProcessor.Process(powerBuffer);

			int count = 100;
			float[] amp = new float[count];
			for(int i = 0; i < count; ++i)
			{
				amp[i] = powerProcessor.Bins[i + 1] + 2.5f;
			}

			if(spectrumViz != null)
			{
				spectrumViz.amplitudes = amp;
			}
        }

        public override void Exit()
        {
			capture.Dispose();
        }

        public SoundScript ()
        {
			int count = LunaConnectionBase.ledCount;
			capture = new AudioCapture(samplingRate, channelCount, 1 << windowSizePot);
			processor = new FFTProcessor(windowSizePot, 10, 2);
			aggregator = new ToneAggregator(count, lowFrequency, highFrequency, samplingRate, processor.WindowSize);
			colors = new RainbowPerOctave(count, lowFrequency, highFrequency);
			holdFilters = new HoldFilter[channelCount];

			for(int c = 0; c < channelCount; ++c){
				holdFilters[c] = new HoldFilter(count);
			}

			powerBuffer = new Utility.CircularBuffer<float>(2048);
			powerProcessor = new FFTProcessor(11, 10, 1);
			averages = new MovingAverage[count];
			for(int i = 0; i < count; ++i)
			{
				averages[i] = new MovingAverage(200);
			}
		}
        
    }
}
