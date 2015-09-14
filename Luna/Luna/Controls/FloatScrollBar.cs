﻿using System.Windows.Forms;

namespace Luna {
	public partial class FloatSlider : TrackBar {
		private float precision = 0.001f;

		public float Precision {
			get { return precision; }
			set {
				precision = value;
				// todo: update the 5 properties below
			}
		}
		public new float LargeChange { get { return base.LargeChange * precision; } set { base.LargeChange = (int)System.Math.Round(value / precision); } }
		public new float Maximum { get { return base.Maximum * precision; } set { base.Maximum = (int)System.Math.Round(value / precision); } }
		public new float Minimum { get { return base.Minimum * precision; } set { base.Minimum = (int)System.Math.Round(value / precision); } }
		public new float SmallChange { get { return base.SmallChange * precision; } set { base.SmallChange = (int)System.Math.Round(value / precision); } }
		public new float Value { get { return base.Value * precision; } set { base.Value = (int)System.Math.Round(value / precision); } }

	}
}
