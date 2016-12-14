using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.CoreAudioApi;
using System.Linq;
using System;
using System.Numerics;
using Luna.Controls;
using System.Runtime.InteropServices;

namespace Luna
{
    class SoundScript : LunaScript
    {
        protected override int period => 10;

        private const int windowSizePot = 13;
        private const int windowSize = 1 << windowSizePot;
        private const int samplingRate = 48000;
        private const int channelCount = 2;
        private float lowFrequency = 100;
        private float highFrequency = 6400;

        AudioClient audioClient;
        AudioCaptureClient capClient;

        float[] recordBuffer;
        int recordBufferPtr;

        float[] windowFunction;

        NAudio.Dsp.Complex[] fftBuffer;

        float[,] fftResult;
        float[,] frequencyBins;

        Vector4[] baseColors = new Vector4[LunaConnectionBase.ledCount];
        float[,] pixelIntensities = new float[2, LunaConnectionBase.ledCount];

        public SpectrumVisualizerControl spectrumViz;

        public override void Run()
        {
            ReadNextPacket();
            FFT();
            for(int c = 0; c < 2; ++c)
            {
                for(int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    pixelIntensities[c, i] = Math.Max(0, Math.Max(pixelIntensities[c, i] - 0.02f, frequencyBins[c, i] + 1));
                    luna.pixels[c][i] = baseColors[i] * (float) Math.Pow(pixelIntensities[c, i], 2.2f) * 2;
                }
            }
        }

        public override void Exit()
        {
            capClient.Dispose();
            capClient = null;

            audioClient.Stop();
            audioClient.Dispose();
            audioClient = null;
        }

        public SoundScript ()
        {
            windowFunction = new float[windowSize];
            for(int i = 0; i < windowSize; ++i) {
                windowFunction[i] = (float) FastFourierTransform.HannWindow(i, windowSize);
            }

            fftBuffer = new NAudio.Dsp.Complex[windowSize];

            fftResult = new float[channelCount, windowSize];

            frequencyBins = new float[channelCount, LunaConnectionBase.ledCount];

            float octaveCount = (float)(Math.Log(highFrequency / lowFrequency) / Math.Log(2));
            float step = octaveCount / LunaConnectionBase.ledCount;
            for(int i = 0; i < LunaConnectionBase.ledCount; ++i)
            {
                baseColors[i] = hueToRGB(1 - (step * i) % 1);
            }

            InitializeAudioClient();
        }
        
        private void InitializeAudioClient()
        {
            var enumerator = new MMDeviceEnumerator();
            var captureDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            audioClient = captureDevice.AudioClient;

            int recordBufferLength = samplingRate; // 20ms worth of recording
            recordBuffer = new float[recordBufferLength * channelCount];

            long requestedDuration = 10000 * period * 2;

            audioClient.Initialize(AudioClientShareMode.Shared,
                AudioClientStreamFlags.Loopback,
                requestedDuration,
                0,
                WaveFormat.CreateIeeeFloatWaveFormat(samplingRate, channelCount),
                Guid.Empty);

            capClient = audioClient.AudioCaptureClient;
            audioClient.Start();
        } 

        private void ReadNextPacket()
        {
            int packetSize = capClient.GetNextPacketSize();

            while (packetSize != 0)
            {
                int framesAvailable;
                AudioClientBufferFlags flags;
                IntPtr srcBuffer = capClient.GetBuffer(out framesAvailable, out flags);

                while (true)
                {
                    int sampleCount = Math.Min(recordBuffer.Length, recordBufferPtr + framesAvailable * channelCount) - recordBufferPtr;
                    Marshal.Copy(srcBuffer, recordBuffer, recordBufferPtr, sampleCount);

                    recordBufferPtr += sampleCount;
                    if(recordBufferPtr == recordBuffer.Length)
                    {
                        recordBufferPtr = 0;
                    }else
                    {
                        break;
                    }
                }

                capClient.ReleaseBuffer(framesAvailable);
                packetSize = capClient.GetNextPacketSize();
            }
        }
        
        private void FFT()
        {
            Console.WriteLine("FT");
            fftMax = -3;
            FFTChannel(0);
            FFTChannel(1);

            Aggregate();
            if (spectrumViz != null)
            {
                int c = LunaConnectionBase.ledCount;
                float[] amp = new float[c];
                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    amp[i] = frequencyBins[0, i]+ 0.9f;
                }
                spectrumViz.amplitudes = amp;
            }

            fftGain = fftGain * 0.98f + fftMax * 0.02f;
        }

        private double fftGain;
        private double fftMax;
        private void FFTChannel(int channel)
        {
            int offset = recordBufferPtr - windowSize * 2 + channel + recordBuffer.Length;
            for (int i = 0; i < windowSize; ++i)
            {
                int index = (offset + i * channelCount + channel) % recordBuffer.Length;
                fftBuffer[i].X = recordBuffer[index] * windowFunction[i];
                fftBuffer[i].Y = 0;
            }
            FastFourierTransform.FFT(true, windowSizePot, fftBuffer);
            
            for (int i = 0; i < windowSize / 2; ++i)
            {
                double value = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                value = Math.Log10(value);
                fftMax = Math.Max(fftMax, value);
                fftResult[channel, i] = (float) value;
            }

            for (int i = 0; i < windowSize / 2; ++i)
            {
                fftResult[channel, i] = (float)((fftResult[channel, i] - fftGain));
            }
        }

        void Aggregate()
        {
            float octaveCount = (float) (Math.Log(highFrequency / lowFrequency) / Math.Log(2));
            float step = octaveCount / LunaConnectionBase.ledCount;
            for (int channel = 0; channel < channelCount; ++channel)
            {
                float f0 = lowFrequency;
                int jl = 1;
                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    float f1 = lowFrequency * (float)Math.Pow(2, step * (i + 1));
                    int jh = (int)(f1 * windowSize / samplingRate);

                    float max = -10;
                    for (int j = jl; j <= jh; ++j)
                    {
                        max = Math.Max(max, fftResult[channel, j]);
                    }
                    frequencyBins[channel, i] = max;

                    f0 = f1;
                    jl = jh;
                }
            }
        }

        private Vector4 hueToRGB(float hue)
        {
            float h6 = hue * 6;

            Vector4 ret = new Vector4(
                Math.Abs(3 - h6) - 1,
                2 - Math.Abs(2- h6),
                2 - Math.Abs(4 - h6),
                0);
            VectorExtension.Clamp0_1(ref ret);
            return ret;
        }
        
    }
}
