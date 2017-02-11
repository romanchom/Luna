using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna.Utility
{
	class MovingMedian<T> where T : IComparable<T>
	{
		private CircularBuffer<T> buffer;
		private T[] sorted;

		public MovingMedian(int length)
		{
			buffer = new CircularBuffer<T>(length);
			sorted = new T[length];
		}

		public void Push(T value)
		{
			buffer.Push(value);
			buffer.Buffer.CopyTo(sorted, 0);
			Array.Sort(sorted);
		}

		public T Value
		{
			get
			{
				return sorted[sorted.Length / 2];
			}
		}
	}
}
