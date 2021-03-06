﻿using Luna.Utility;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Luna.Audio
{
	class AudioCapture : IDisposable
	{
		private AudioClient audioClient;
		private AudioCaptureClient audioCapClient;

		private int channelCount;

		float[] captureBuffer;

		public ICollection<CircularBuffer<float>> Channels
		{
			get
			{
				return channels;
			}
		}


		private CircularBuffer<float>[] channels;

		public AudioCapture(int samplingRate, int channelCount, int sampleCount)
		{
			this.channelCount = channelCount;

			channels = new CircularBuffer<float>[channelCount];
			for(int c = 0; c < channelCount; ++c){
				channels[c] = new CircularBuffer<float>(sampleCount);
			}

			var enumerator = new MMDeviceEnumerator();
			var captureDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
			audioClient = captureDevice.AudioClient;

			long requestedDuration = (long) ((double) sampleCount * 20000000 / samplingRate); // number of 100ns intervals

			audioClient.Initialize(AudioClientShareMode.Shared,
				AudioClientStreamFlags.Loopback,
				requestedDuration,
				0,
				WaveFormat.CreateIeeeFloatWaveFormat(samplingRate, channelCount),
				Guid.Empty);

			audioCapClient = audioClient.AudioCaptureClient;
			audioClient.Start();
		}

		public void ReadPackets()
		{
			int packetSize = audioCapClient.GetNextPacketSize();

			while (packetSize != 0)
			{
				int framesAvailable;
				AudioClientBufferFlags flags;
				IntPtr srcBuffer = audioCapClient.GetBuffer(out framesAvailable, out flags);
				int totalSamples = framesAvailable * channelCount;

				if (captureBuffer == null || captureBuffer.Length < totalSamples) captureBuffer = new float[totalSamples];
				
				Marshal.Copy(srcBuffer, captureBuffer, 0, totalSamples);
				
				audioCapClient.ReleaseBuffer(framesAvailable);

				for (int c = 0; c < channelCount; ++c)
				{
					for (int i = 0; i < framesAvailable; ++i)
					{
						channels[c].Push(captureBuffer[c + channelCount * i]);
					}
				}

				packetSize = audioCapClient.GetNextPacketSize();
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					audioClient.Dispose();
				}
				
				disposedValue = true;
			}
		}
		
		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
