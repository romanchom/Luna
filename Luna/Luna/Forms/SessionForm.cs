using Luna.Properties;
using System;
using System.Windows.Forms;
using System.Numerics;

namespace Luna {
	public partial class SessionForm : Form {
		LunaConnectionBase luna;
		
		private LunaScript script;
		

		public SessionForm(string portName) {
			
			InitializeComponent();

			LoadSettings();

			//luna = new LunaSerial(portName);
			luna = new LunaTcp();

			previewControl1.OnScreenCaptured += PreviewControl1_OnScreenCaptured;
		}

		private void PreviewControl1_OnScreenCaptured(Vector4[] pixels, int width, int height, int rowStride) {
			Vector4[] leftPixels = luna.pixelsLeft;
			Vector4[] rightPixels = luna.pixelsRight;

			int depth = Settings.Default.LunaDepth;

			for (int i = 0; i < LunaSerial.ledCount; ++i) {
				int baseIndex = rowStride * i;
				for (int j = 0; j < depth; ++j) {
					leftPixels[i] += (float)j  * pixels[baseIndex + j];
				}
				leftPixels[i] *= 2.0f / (depth * depth);

				int end = height - 1 - depth;
				for (int j = 0; j < depth; ++j) {
					rightPixels[i] += (float)j * pixels[baseIndex + width - 1 - j];
				}
				rightPixels[i] *= 2.0f / (depth * depth);
			}

			luna.Send();
			System.Console.WriteLine(leftPixels[0]);
		}

		private void LoadSettings() {
			WhiteLevelBar.Value = Settings.Default.WhiteLevel;
			ColorPickerButton.BackColor = Settings.Default.BacklightColor;

			RedSlider.Value = Settings.Default.RedMultiplier;
			GreenSlider.Value = Settings.Default.GreenMultiplier;
			BlueSlider.Value = Settings.Default.BlueMultiplier;

			GammaSlider.Value = Settings.Default.Gamma;

			DitherComboBox.SelectedIndex = Settings.Default.Dithering;
			
			if(Settings.Default.Scripts != null) {
				foreach (var str in Settings.Default.Scripts) {
					ScriptList.Items.Add(str);
				}
			}
		}

		private void SessionForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (script != null) script.Stop();
			luna.Dispose();
			Settings.Default.Scripts = new System.Collections.Specialized.StringCollection();
			foreach(var str in ScriptList.Items) {
				Settings.Default.Scripts.Add(str.ToString());
			}
			Settings.Default.Save();
        }

		private void button1_Click(object sender, EventArgs e) {
			DialogResult result = colorDialog1.ShowDialog();

			if (result == DialogResult.OK) {
				ColorPickerButton.BackColor = colorDialog1.Color;
				Settings.Default.BacklightColor = colorDialog1.Color;
			}
		}
		
		private void WhiteLevelBar_ValueChanged(object sender, EventArgs e) {
			Settings.Default.WhiteLevel = WhiteLevelBar.Value;
        }

		private void RedSlider_ValueChanged(object sender, EventArgs e) {
			Settings.Default.RedMultiplier = RedSlider.Value;
		}

		private void GreenSlider_ValueChanged(object sender, EventArgs e) {
			Settings.Default.GreenMultiplier = GreenSlider.Value;
		}

		private void BlueSlider_ValueChanged(object sender, EventArgs e) {
			Settings.Default.BlueMultiplier = BlueSlider.Value;
		}

		private void LoadScriptButton_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			// Set filter options and filter index.
			openFileDialog1.Filter = "C# (.cs)|*.cs";
			openFileDialog1.FilterIndex = 1;

			openFileDialog1.Multiselect = true;

			DialogResult userClickedOK = openFileDialog1.ShowDialog();
			
			if (userClickedOK == DialogResult.OK) {
				Settings.Default.ScriptFile = openFileDialog1.FileName;
				ScriptFileTextBox.Text = openFileDialog1.FileName;
            }
		}
		
		private void DitherComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			Settings.Default.Dithering = DitherComboBox.SelectedIndex;
		}

		int previousTabIndex = -1;

		private void Tabs_SelectedIndexChanged(object sender, EventArgs e) {
			switch (previousTabIndex) {
			case 0:
				script.Stop();
				break;
			case 1:
				script.Stop();
				break;
			case 2:
				previewControl1.StopRenderLoop();
				break;
			case 3:
				if(script != null)
					script.Stop();
				break;
			}

			previousTabIndex = Tabs.SelectedIndex;

			switch(Tabs.SelectedIndex) {
			case 0:
				script = new SettingsScript();
				script.Start(luna);
				break;
			case 1:
				script = new LightScript();
				script.Start(luna);
				break;
			case 2:
				previewControl1.StartRenderLoop();
				break;
			}
		}

		private void NotificationIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (Visible) {
				Hide();
				NotificationIcon.ShowBalloonTip(2);
			} else {
				Show();
				WindowState = FormWindowState.Normal;
			}
		}

		private void OnResize(object sender, EventArgs e) {
			if (WindowState == FormWindowState.Minimized) {
				Hide();
				NotificationIcon.ShowBalloonTip(2);
			} else {
				Show();
			}
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			Application.Exit();
		}

		private void GammaSlider_ValueChanged(object sender, EventArgs e) {
			Settings.Default.Gamma = GammaSlider.Value;
			GammaLabel.Text = GammaSlider.Value.ToString();
		}

		private void TestButton_Click(object sender, EventArgs e) {
			RunScript(Settings.Default.ScriptFile);
		}

		private void AddToListButton_Click(object sender, EventArgs e) {
			ScriptList.Items.Add(ScriptFileTextBox.Text);
		}

		private void ScriptList_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyData == Keys.Delete && ScriptList.SelectedIndex >= 0) 
					ScriptList.Items.RemoveAt(ScriptList.SelectedIndex);
		}

		private void ScriptList_DoubleClick(object sender, EventArgs e) {
			if(ScriptList.SelectedIndex >= 0)
				RunScript(ScriptList.SelectedItem.ToString());
		}

		private void StopButton_Click(object sender, EventArgs e) {
			script.Stop();
			script = null;
		}

		private void RunScript(string file) {
			if (script != null) {
				script.Stop();
			}
			script = LunaScript.CompileScript(file);
			if (script != null) script.Start(luna);
		}
	}
}
