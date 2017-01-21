using System;

namespace Luna.Utility
{
	class CircularBuffer<T>
	{
		public T[] Buffer { get; private set; }
		private int pointer;
		public int Count => Buffer.Length;

		public CircularBuffer(int count)
		{
			Buffer = new T[count];
			pointer = 0;
		}

		public T this[int index]{
			get {
				return Buffer[(pointer + index + Count) % Count];
			}
		}

		public void Push(T value)
		{
			Buffer[pointer] = value;
			++pointer;
			if (pointer == Count) pointer = 0;
		}
	}
}
