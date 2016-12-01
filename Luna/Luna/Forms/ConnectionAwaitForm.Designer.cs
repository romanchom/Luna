using System.Drawing;
using System.Windows.Forms;

namespace Luna
{
    partial class ConnectionAwaitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ProgressBar bar = new ProgressBar();
            Label label = new Label();
            base.SuspendLayout();
            bar.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            bar.Location = new Point(12, 0x53);
            bar.MarqueeAnimationSpeed = 0x10;
            bar.Name = "progressBar1";
            bar.Size = new Size(0x229, 0x25);
            bar.Step = 1;
            bar.Style = ProgressBarStyle.Marquee;
            bar.TabIndex = 0;
            label.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            label.AutoSize = true;
            label.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
            label.Location = new Point(0xbc, 0x1f);
            label.Name = "label1";
            label.Size = new Size(0xce, 0x1a);
            label.TabIndex = 1;
            label.Text = "Łączę... cierpliwości";
            label.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x241, 0x84);
            base.Controls.Add(label);
            base.Controls.Add(bar);
            base.Name = "ConnectionAwaitForm";
            this.Text = "Łączę z Łuną";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        #endregion
    }
}