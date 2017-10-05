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
		Audio.SpectrumVizualizer[] vizualizers;

		NeuralBeatDetector beatDetector;
		FFTProcessor sfftProcessor;
		MelAggregator sfftAggregator;
		float beatIntensity = 0;

		public SpectrumVisualizerControl spectrumViz;

        public override void Run()
        {
			capture.ReadPackets();
			int c = 0;
			foreach (var channel in capture.Channels)
			{
				vizualizers[c].Process(channel);
				++c;
            }
			spectrumViz.amplitudes = aggregator.Tones;



			float[] arr = new float[1024];
			foreach (var channel in capture.Channels)
			{
				for (int i = 0; i < 1024; ++i)
				{
					arr[i] += channel[i];
				}
			}
			sfftProcessor.Process(arr);
			for(int i = 0; i < 1024; ++i)
			{
				sfftProcessor.Magnitudes[i] += 1e-10f;
			}
			sfftAggregator.Aggregate(sfftProcessor.Magnitudes);
			beatDetector.Feed(sfftAggregator.Result);
			float isBeat = beatDetector.IsBeat;
			isBeat = Math.Max(isBeat, beatIntensity);
			beatIntensity = Math.Max(0.0f, isBeat * 0.93f);
			
			for (c = 0; c < 2; ++c)
			{
				for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
				{
					luna.pixels[c][i] = (float)Math.Pow(holdFilters[c].Values[i], 2.2f) * (1 + beatIntensity * 3) * colors.Colors[i];
				}
			}

			luna.whiteLeft = luna.whiteRight = beatIntensity * 0.05f;
        }

        public override void Exit()
        {
			capture.Dispose();
        }

        public SoundScript ()
        {
			int count = LunaConnectionBase.ledCount;
			capture = new AudioCapture(samplingRate, channelCount, 1 << windowSizePot);
			vizualizers = new SpectrumVizualizer[channelCount];
			for(int i = 0; i < channelCount; ++i)
			{
				vizualizers[i] = new SpectrumVizualizer(LunaConnectionBase.ledCount, lowFrequency, highFrequency, samplingRate);
			}
			

			sfftProcessor = new FFTProcessor(10);
			sfftAggregator = new MelAggregator(count / 2, lowFrequency, highFrequency, sfftProcessor.WindowSize, samplingRate);
			beatDetector = new NeuralBeatDetector();
		}
        
    }
}
