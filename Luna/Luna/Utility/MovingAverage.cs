using System;

namespace Luna.Utility
{
    class MovingAverage
    {
		public int Length => history.Length;
        private float[] history;
        private int pointer;
        private float sum;
		private float compensation;

        public MovingAverage(int length)
        {
            history = new float[length];
            pointer = 0;
            sum = 0.0f;
        }

        public void Add(float element)
        {
            history[pointer] = element;
			AddNumber(element);
            pointer = (pointer + 1) % Length;
			AddNumber(-history[pointer]);
        }

		private void AddNumber(float number)
		{
			// Kahan summation algorithm
			float y = number - compensation;
			float t = sum + y;
			compensation = (t - sum) - y;
			sum = t;
		}

        public float Value
        {
            get
            {
                return sum / Length;
            }
        }

		public float Variance
		{
			get {
				float v = 0;
				for (int i = 0; i < history.Length; ++i) {
					v += history[i] * history[i];
				}
				return v / Length - Value * Value;
			}
		}
    }
}
