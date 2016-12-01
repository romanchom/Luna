using Luna.Properties;
using System;
using System.Windows.Forms;
using System.Numerics;

namespace Luna {
	public partial class SessionForm : Form {
		LunaConnectionBase luna;
		
		private LunaScript script;

        public SessionForm(LunaConnectionBase luna)
        {
            InitializeComponent();
            LoadSettings();
            this.luna = luna;
            previewControl1.OnScreenCaptured += new Action<Vector4[], int, int, int>(PreviewControl1_OnScreenCaptured);
            switchTo(0);
        }

        private void AddToListButton_Click(object sender, EventArgs e)
        {
            ScriptList.Items.Add(ScriptFileTextBox.Text);
        }

        private void BlueSlider_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.BlueMultiplier = BlueSlider.Value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ColorPickerButton.BackColor = colorDialog1.Color;
                Settings.Default.BacklightColor = colorDialog1.Color;
            }
        }

        private void DepthControl_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.LunaDepth = (int)DepthControl.Value;
        }

        private void GammaSlider_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Gamma = GammaSlider.Value;
            GammaLabel.Text = GammaSlider.Value.ToString();
        }

        private void GreenSlider_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.GreenMultiplier = GreenSlider.Value;
        }

        
        private void LoadScriptButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "C# (.cs)|*.cs",
                FilterIndex = 1,
                Multiselect = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.ScriptFile = dialog.FileName;
                ScriptFileTextBox.Text = dialog.FileName;
            }
        }

        private void LoadSettings()
        {
            WhiteLevelBar.Value = Settings.Default.WhiteLevel;
            ColorPickerButton.BackColor = Settings.Default.BacklightColor;
            RedSlider.Value = Settings.Default.RedMultiplier;
            GreenSlider.Value = Settings.Default.GreenMultiplier;
            BlueSlider.Value = Settings.Default.BlueMultiplier;
            GammaSlider.Value = Settings.Default.Gamma;
            if (Settings.Default.Scripts != null)
            {
                foreach (string str in Settings.Default.Scripts)
                {
                    ScriptList.Items.Add(str);
                }
            }
            previewControl1.topPosition = Settings.Default.TopPosition;
            topPosition.Value = (decimal)Settings.Default.TopPosition;
            previewControl1.bottomPosition = Settings.Default.BottomPosition;
            bottomPosition.Value = (decimal)Settings.Default.BottomPosition;
        }

        private void NotificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.Visible = true;
            base.WindowState = FormWindowState.Normal;
            NotificationIcon.Visible = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            switchTo(-1);
            luna.Dispose();
            Settings.Default.Scripts = new System.Collections.Specialized.StringCollection();
            foreach (object obj2 in ScriptList.Items)
            {
                Settings.Default.Scripts.Add(obj2.ToString());
            }
            Settings.Default.Save();
            base.OnFormClosed(e);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == base.WindowState)
            {
                NotificationIcon.Visible = true;
                NotificationIcon.ShowBalloonTip(2);
                base.Hide();
            }
            else if (base.WindowState == FormWindowState.Normal)
            {
                NotificationIcon.Visible = false;
            }
        }

        private void PreviewControl1_OnScreenCaptured(Vector4[] pixels, int width, int height, int rowStride)
        {
            Matrix4x4 matrix = new Matrix4x4(
                0.4191028f, 0.4395272f, 0.1033829f, 0f,
                -0.1699648f, 1.134867f, 0.02449566f, 0f,
                0.026151f, -0.1052335f, 1.073805f, 0f,
                0f, 0f, 0f, 1f);
            matrix = Matrix4x4.Transpose(matrix);
           
            Vector4[] pixelsLeft = luna.pixelsLeft;
            Vector4[] pixelsRight = luna.pixelsRight;
            int lunaDepth = Settings.Default.LunaDepth;
            for (int i = 0; i < 120; i++)
            {
                int num3 = rowStride * i;
                Vector4 vector = new Vector4();
                for (int j = 0; j < lunaDepth; j++)
                {
                    vector += (Vector4)((lunaDepth - j) * pixels[num3 + j]);
                }
                vector = (Vector4)(vector * (2f / ((float)((1 + lunaDepth) * lunaDepth))));
                pixelsLeft[i] = Vector4.Transform(vector, matrix);
                vector = new Vector4();
                num3 += width - 1;
                for (int k = 0; k < lunaDepth; k++)
                {
                    vector += (Vector4)((lunaDepth - k) * pixels[num3 - k]);
                }
                vector = (Vector4)(vector * (2f / ((float)((1 + lunaDepth) * lunaDepth))));
                pixelsRight[i] = Vector4.Transform(vector, matrix);
            }
            luna.whiteLeft *= 0.97f;
            luna.whiteRight *= 0.97f;
            luna.Send();
        }

        private void RedSlider_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.RedMultiplier = RedSlider.Value;
        }

        private void RunScript(string file)
        {
            if (script != null)
            {
                script.Stop();
            }
            script = LunaScript.CompileScript(file);
            if (script != null)
            {
                script.Start(luna);
            }
        }

        private void ScriptList_DoubleClick(object sender, EventArgs e)
        {
            if (ScriptList.SelectedIndex >= 0)
            {
                RunScript(ScriptList.SelectedItem.ToString());
            }
        }

        private void ScriptList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Delete) && (ScriptList.SelectedIndex >= 0))
            {
                ScriptList.Items.RemoveAt(ScriptList.SelectedIndex);
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            script.Stop();
            script = null;
        }

        private void switchTo(int index)
        {
            switch (previousTabIndex)
            {
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
                    script.Stop();
                    break;
            }
            previousTabIndex = index;
            switch (previousTabIndex)
            {
                case 0:
                    script = new SettingsScript();
                    script.Start(luna);
                    return;

                case 1:
                    script = new LightScript();
                    script.Start(luna);
                    return;

                case 2:
                    previewControl1.StartRenderLoop();
                    return;
            }
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switchTo(Tabs.SelectedIndex);
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            RunScript(Settings.Default.ScriptFile);
        }

        private void bottomPosition_ValueChanged(object sender, EventArgs e)
        {
            previewControl1.bottomPosition = (float)bottomPosition.Value;
            Settings.Default.BottomPosition = (float)bottomPosition.Value;
        }

        private void topPosition_ValueChanged(object sender, EventArgs e)
        {
            previewControl1.topPosition = (float)topPosition.Value;
            Settings.Default.TopPosition = (float)topPosition.Value;
        }

        private void WhiteLevelBar_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.WhiteLevel = WhiteLevelBar.Value;
        }
       

        private void SessionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            switchTo(-1);
            if (script != null) script.Stop();
			Settings.Default.Scripts = new System.Collections.Specialized.StringCollection();
            foreach (var str in ScriptList.Items) {
				Settings.Default.Scripts.Add(str.ToString());
			}
			Settings.Default.Save();
            luna.Dispose();
        }
        
		int previousTabIndex = -1;
    }
}
