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
		NeuralBeatDetector beatDetector;
		FFTProcessor sfftProcessor;
		MelAggregator sfftAggregator;
		float beatIntensity = 0;
		Utility.CircularBuffer<float> test = new Utility.CircularBuffer<float>(200);
		Utility.MovingAverage normMedian;

		public SpectrumVisualizerControl spectrumViz;

        public override void Run()
        {
			float[] powers = new float[120];
			capture.ReadPackets();
			float[] arr = new float[1024];
			float norm = -12;
			for (int c = 0; c < 2; ++c)
			{
				for(int i = 0; i < 1024; ++i)
				{
					arr[i] += capture.Channels[c][i];
				}
				processor.Process(capture.Channels[c]);
				aggregator.Aggregate(processor.Magnitudes, 0);
				norm = Math.Max(norm, aggregator.GetMaxNorm());
				aggregator.Normalize(normMedian.Value);
				holdFilters[c].Process(aggregator.Tones, 0.02f, 1);
            }
			normMedian.Add(norm);
			 //spectrumViz.amplitudes = aggregator.Tones;

			sfftProcessor.Process(arr);
			for(int i = 0; i < 1024; ++i)
			{
				sfftProcessor.Magnitudes[i] += 1e-10f;
			}
			sfftAggregator.Aggregate(sfftProcessor.Magnitudes);
			beatDetector.Feed(sfftAggregator.Result);
			float isBeat = beatDetector.IsBeat;
			test.Push(isBeat);
			spectrumViz.amplitudes = test.ToArray();
			isBeat = Math.Max(isBeat, beatIntensity);
			beatIntensity = Math.Max(0.0f, isBeat - 1.0f / 25.0f);
			
			for (int c = 0; c < 2; ++c)
			{
				for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
				{
					luna.pixels[c][i] = (float)Math.Pow(holdFilters[c].Values[i], 2.2f) * (1 + beatIntensity * 3) * colors.Colors[i];
				}
			}

			//luna.whiteLeft = luna.whiteRight = (float) Math.Pow(beatIntensity, 2.2f) * 0.05f;
        }

        public override void Exit()
        {
			capture.Dispose();
        }

        public SoundScript ()
        {
			int count = LunaConnectionBase.ledCount;
			capture = new AudioCapture(samplingRate, channelCount, 1 << windowSizePot);
			processor = new FFTProcessor(windowSizePot);
			aggregator = new ToneAggregator(count, lowFrequency, highFrequency, samplingRate, processor.WindowSize);
			colors = new RainbowPerOctave(count, lowFrequency, highFrequency);
			holdFilters = new HoldFilter[channelCount];

			for(int c = 0; c < channelCount; ++c){
				holdFilters[c] = new HoldFilter(count);
			}
			sfftProcessor = new FFTProcessor(10);
			sfftAggregator = new MelAggregator(count / 2, lowFrequency, highFrequency, sfftProcessor.WindowSize, samplingRate);
			beatDetector = new NeuralBeatDetector();
			normMedian = new Utility.MovingAverage(100);
			//beatDetector = new BeatDetector(100, count, 100);
		}
        
    }
}
