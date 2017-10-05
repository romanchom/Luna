using System;

namespace Luna.Audio
{
	class HoldFilter
	{
		public float[] Values { get; private set; }

		public HoldFilter(int count)
		{
			Values = new float[count];
		}

		public void Process(float[] input, float decrese, float offset)
		{
			for(int i = 0; i < Values.Length; ++i)
			{
				Values[i] = Math.Max(0, Math.Max(Values[i] - decrese, input[i] + offset));
			}
		}
	}
}
