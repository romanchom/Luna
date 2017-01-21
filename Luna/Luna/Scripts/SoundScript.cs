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
		BeatDetector beatDetector;
		float beatIntensity = 0;

        public SpectrumVisualizerControl spectrumViz;

        public override void Run()
        {
			float[] powers = new float[120];
			capture.ReadPackets();
			beatDetector.Reset();
			for (int c = 0; c < 2; ++c)
			{
				processor.Process(capture.Channels[c]);
				aggregator.Aggregate(processor.Magnitudes, -6);
				beatDetector.AddInput(aggregator.Tones);
				holdFilters[c].Process(aggregator.Tones, 0.02f, 1);
				for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
					luna.pixels[c][i] = colors.Colors[i] * (float) Math.Pow(holdFilters[c].Values[i], 2.2f) * 2;
                }
            }

			beatDetector.Analyze();

			beatDetector.Visualize(spectrumViz);

			if (beatDetector.HasBeat) beatIntensity = 0.1f;

			beatIntensity = Math.Max(0, beatIntensity - 0.01f);

			luna.whiteLeft = luna.whiteRight = beatIntensity;
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
			beatDetector = new BeatDetector(count, 100);
		}
        
    }
}
