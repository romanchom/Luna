using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
    class TimeAverage
    {
        public int Length
        {
            get;
            private set;
        }
        private double[] history;
        private int pointer;
        private double sum;

        public TimeAverage(int length)
        {
            Length = length;
            history = new double[length];
            pointer = 0;
            sum = 0.0;
        }

        public void Add(double element)
        {
            history[pointer] = element;
            sum += element;
            pointer = (pointer + 1) % Length;
            sum -= history[pointer];
        }

        public double Value
        {
            get
            {
                return sum / Length;
            }
        }
    }
}
