using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Utility
{
	class MovingMin
	{
		public int Length => history.Length;
		private float[] history;
		private int pointer;

		public MovingMin(int length)
		{
			history = new float[length];
			pointer = 0;
		}

		public void Add(float element)
		{
			history[pointer] = element;
			pointer = (pointer + 1) % Length;
		}
		
		public float Value
		{
			get
			{
				float min = float.PositiveInfinity;
				foreach(float f in history)
				{
					min = Math.Min(min, f);
				}
				return min;
			}
		}
	}
}
