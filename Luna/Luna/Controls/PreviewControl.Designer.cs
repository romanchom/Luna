namespace Luna.Controls {
	partial class PreviewControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.EmptyControl = new Luna.EmptyControl();
			this.SuspendLayout();
			// 
			// emptyControl1
			// 
			this.EmptyControl.Location = new System.Drawing.Point(41, 59);
			this.EmptyControl.Name = "emptyControl1";
			this.EmptyControl.Size = new System.Drawing.Size(56, 45);
			this.EmptyControl.TabIndex = 0;
			// 
			// PreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.EmptyControl);
			this.Name = "PreviewControl";
			this.ResumeLayout(false);

		}

		#endregion

		private EmptyControl EmptyControl;
	}
}
