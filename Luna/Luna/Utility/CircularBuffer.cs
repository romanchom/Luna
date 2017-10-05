using System;
using System.Collections;
using System.Collections.Generic;

namespace Luna.Utility
{
	class CircularBuffer<T> : ICollection<T>
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

		public bool IsReadOnly => false;

		public IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		public void Add(T item)
		{
			Buffer[pointer] = item;
			++pointer;
			if (pointer == Count) pointer = 0;
		}

		public void Clear()
		{
			Array.Clear(Buffer, 0, Buffer.Length);
		}

		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		private class Enumerator : IEnumerator<T>
		{
			private int p;
			private CircularBuffer<T> buffer;
			public Enumerator(CircularBuffer<T> buffer)
			{
				p = -1;
				this.buffer = buffer;
			}

			public T Current
			{
				get
				{
					return buffer[-p - 1];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			public bool MoveNext()
			{
				++p;
				return p < buffer.Count;
			}

			public void Reset()
			{
				p = -1;
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

					// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
					// TODO: set large fields to null.

					disposedValue = true;
				}
			}

			// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
			// ~Enumerator() {
			//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			//   Dispose(false);
			// }

			// This code added to correctly implement the disposable pattern.
			public void Dispose()
			{
				// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
				Dispose(true);
				// TODO: uncomment the following line if the finalizer is overridden above.
				// GC.SuppressFinalize(this);
			}
			#endregion

		}
	}
}
