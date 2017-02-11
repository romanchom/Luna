using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Luna.Audio
{
	class NeuralBeatDetector : IDisposable
	{
		private const string dllName = "tf_beat_detector.dll";
		[DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr NewBeatDetector();
		[DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern void DeleteBeatDetector(IntPtr ptr);
		[DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern float BeatDetectorIsBeat(IntPtr ptr, float[] sample, int sampleLength);

		private IntPtr cThis;

		public NeuralBeatDetector()
		{
			cThis = NewBeatDetector();
			ThresholdSet = 0.2f;
			ThresholdReset = 0.05f;
			history = new float[30, 60];
			sorted = new float[30];
			featureVector = new float[120];
		}

		public float ThresholdSet { get; set; }
		public float ThresholdReset { get; set; }

		bool hasBeat = false;
		bool hadBeat = false;
		public float IsBeat
		{
			get
			{
				return hasBeat ? 1.0f : 0.0f;
			}
		}

		private int t = 0;
		private float[,] history; 
		private float nnRet = 0;
		private float[] sorted;
		private float[] featureVector;
		public void Feed(float[] sample)
		{
			for(int i = 0; i < 60; ++i)
			{
				history[t, i] = sample[i];
				featureVector[i] = sample[i];
				for (int j = 0; j < 30; ++j)
				{
					sorted[j] = history[j, i];
				}
				Array.Sort(sorted);
				featureVector[i + 60] = Math.Max(0, sample[i] - sorted[15]);
			}
			float val = BeatDetectorIsBeat(cThis, featureVector, featureVector.Length);
			hadBeat |= hasBeat;
			hasBeat = (nnRet > ThresholdSet  && nnRet > val && !hadBeat);
			nnRet = val;
			if (nnRet < ThresholdReset) hadBeat = false; 
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				DeleteBeatDetector(cThis);
				cThis = (IntPtr)0;
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		~NeuralBeatDetector() {
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
