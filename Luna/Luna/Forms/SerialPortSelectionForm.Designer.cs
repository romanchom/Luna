namespace Luna {
	partial class SerialPortSelectionForm {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.SerialPortsComboBox = new System.Windows.Forms.ComboBox();
			this.ConnectButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// SerialPortsComboBox
			// 
			this.SerialPortsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SerialPortsComboBox.FormattingEnabled = true;
			this.SerialPortsComboBox.Location = new System.Drawing.Point(12, 12);
			this.SerialPortsComboBox.Name = "SerialPortsComboBox";
			this.SerialPortsComboBox.Size = new System.Drawing.Size(256, 21);
			this.SerialPortsComboBox.TabIndex = 0;
			this.SerialPortsComboBox.DropDown += new System.EventHandler(this.SerialPortsComboBoxDropDown);
			// 
			// ConnectButton
			// 
			this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectButton.Location = new System.Drawing.Point(13, 40);
			this.ConnectButton.Name = "ConnectButton";
			this.ConnectButton.Size = new System.Drawing.Size(255, 23);
			this.ConnectButton.TabIndex = 1;
			this.ConnectButton.Text = "Połącz";
			this.ConnectButton.UseVisualStyleBackColor = true;
			this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
			// 
			// SerialPortSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(280, 102);
			this.Controls.Add(this.ConnectButton);
			this.Controls.Add(this.SerialPortsComboBox);
			this.Name = "SerialPortSelectionForm";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox SerialPortsComboBox;
		private System.Windows.Forms.Button ConnectButton;
	}
}

