using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.CoreAudioApi;
using System.Linq;
using System;
using System.Numerics;
using Luna.Controls;

namespace Luna
{
    class SoundScript : LunaScript
    {
        WasapiCapture soundInput;
        float[] buffer;
        NAudio.Dsp.Complex[] fftBuffer;
        float[] fftResult;
        float[] fftResultCopy;
        float[] tempPixelArray0;
        float[] tempPixelArray1;
        Vector4[] hues;
        int[] indicies;
        int dstBufferPos;
        const int windowSizePot = 12;
        const int windowSize = 1 << windowSizePot;
        const int samplingRate = 48000;
        const int channelCount = 2;
        const float startFrequency = 100;
        const float endFrequency = 10000;

        public SpectrumVisualizerControl spectrumViz;

        public SoundScript ()
        {
            buffer = new float[windowSize * 2]; // 2 channels
            fftBuffer = new NAudio.Dsp.Complex[windowSize];
            fftResult = new float[windowSize]; // 2 channels
            fftResultCopy = new float[windowSize];
            hues = new Vector4[LunaConnectionBase.ledCount];
            indicies = new int[LunaConnectionBase.ledCount + 1];
            tempPixelArray0 = new float[LunaConnectionBase.ledCount];
            tempPixelArray1 = new float[LunaConnectionBase.ledCount];

            for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
            {
                float frequency = (float)Math.Pow(endFrequency / startFrequency, (double)i / (double)LunaConnectionBase.ledCount) * startFrequency;
                indicies[i + 1] = (int)Math.Floor(frequency / samplingRate * windowSize);
                float tone = (float)(Math.Log(frequency) / Math.Log(2));
                hues[i] = hueToRGB(tone % 1) * (float)Math.Log(tone);
            }
            indicies[0] = 0;

            dstBufferPos = 0;

            var enumerator = new MMDeviceEnumerator();
            var captureDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToArray();
            soundInput = new WasapiCapture(captureDevices[0], true);

            soundInput.ShareMode = AudioClientShareMode.Shared;
            soundInput.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(samplingRate, channelCount);
            soundInput.DataAvailable += CaptureOnDataAvailable;
            soundInput.StartRecording();
        }

        private void CaptureOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            int srcBufferPos = 0;
            int srcBytesCount = waveInEventArgs.BytesRecorded;
            while (true)
            {
                int count =  Math.Min(windowSize * 2, dstBufferPos + srcBytesCount) - dstBufferPos;
                Buffer.BlockCopy(waveInEventArgs.Buffer, srcBufferPos, buffer, dstBufferPos, count);

                dstBufferPos += count;
                srcBufferPos += count;
                srcBytesCount -= count;

                if (dstBufferPos == windowSize * 2)
                {
                    FFT();
                    dstBufferPos = 0;
                }else
                {
                    break;
                }
            }
        }

        float totalPower = 0;
        float lastPower = 0;
        float avgPower = 0;
        bool beat = false;
        private void FFT()
        {
            Console.WriteLine("FT");
            lastPower = totalPower;
            totalPower = 0;
            FFTChannel(0);
            FFTChannel(1);

            for (int c = 0; c < channelCount; ++c)
            {
                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    float sum = 0;
                    int lo = indicies[i];
                    int hi = indicies[i + 1];
                    for (int j = lo; j < hi; ++j)
                    {
                        sum += fftResult[j * channelCount + 1];
                    }
                    totalPower -= sum;
                    sum /= hi - lo;
                    tempPixelArray0[i] = sum;
                    if (hi - lo == 0) tempPixelArray0[i] = -100;
                }
                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    const float lerp = 0.3f;
                    const int radius = 40;
                    int lo = Math.Max(0, i - radius);
                    int hi = Math.Min(LunaConnectionBase.ledCount - 1, i + radius);
                    float max = -4f;
                    for (int j = lo; j <= hi; ++j)
                    {
                        max = Math.Max(tempPixelArray0[j], max);
                    }
                    float value = (tempPixelArray0[i] - max) * 10 + 1;
                    value = Math.Max(0, value);
                    value = Math.Min(1, value);
                    value = (float)Math.Pow(value, 2.2);

                    pixels[c, i] = Math.Max(pixels[c, i] - 1.0f / 60.0f, value);
                }
            }

            if(spectrumViz != null)
            {
                float[] amp = new float[windowSize / 2];
                for (int i = 0; i < windowSize / 2; ++i)
                {
                    amp[i] = fftResult[i * 2];
                }
                spectrumViz.amplitudes = amp;
            }
        }

        private void FFTChannel(int channel)
        {
            for (int i = 0; i < windowSize; ++i)
            {
                fftBuffer[i].X = (float)(buffer[i * channelCount + channel] * FastFourierTransform.HammingWindow(i, windowSize));
                fftBuffer[i].Y = 0;
            }
            FastFourierTransform.FFT(true, windowSizePot, fftBuffer);

            double gain = -5;
            float scale = 1;
            for (int i = 0; i < windowSize / 2; ++i)
            {
                double value = (float)Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                value *= 30;
                //value = Math.Log10(value);
                //gain = Math.Max(value, gain);
                fftResult[i * channelCount + channel] = (float) value;
            }
            for (int i = 0; i < windowSize / 2; ++i)
            {
                //fftResult[i * channelCount + channel] = (fftResult[i * channelCount + channel] - (float) gain) * scale;
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

        float[,] pixels = new float[2, LunaConnectionBase.ledCount];
        public override void Run()
        {
            float white = beat ? 0.1f : 0.0f;
            luna.whiteLeft = white;
            luna.whiteRight = white;
            for(int c = 0; c < channelCount; ++c)
            {
                for (int i = 0; i < LunaConnectionBase.ledCount; ++i)
                {
                    luna.pixels[c][i] = hues[i] * 0;
                }
            }
        }

        public override void Exit()
        {
            soundInput.StopRecording();
        }
    }
}
