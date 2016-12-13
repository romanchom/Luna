using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luna.Controls
{
    public class SpectrumVisualizerControl : Control, ISupportInitialize
    {
        private PointF[] points = new PointF[1];

        public float[] amplitudes
        {
            set
            {
                var p = new PointF[value.Length];
                for(int i = 0; i < value.Length; ++i)
                {
                    p[i].X = (float) i * Size.Width / value.Length;
                    p[i].Y = (1 - value[i]) * Size.Height;
                }
                points = p;
                this.Invalidate();
            }
        }

        public SpectrumVisualizerControl()
        {

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var g = pe.Graphics;
            g.Clear(Color.Black);
            if(points!= null && points.Length > 1) g.DrawLines(Pens.White, points);
        }

        public void BeginInit()
        {

        }

        public void EndInit()
        {

        }
    }
}
