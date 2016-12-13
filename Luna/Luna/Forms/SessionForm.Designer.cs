namespace Luna {
	partial class SessionForm {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Button StopButton;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label11;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionForm));
            this.BlueSlider = new Luna.FloatSlider();
            this.GreenSlider = new Luna.FloatSlider();
            this.RedSlider = new Luna.FloatSlider();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.GammaLabel = new System.Windows.Forms.Label();
            this.GammaSlider = new Luna.FloatSlider();
            this.LightingTab = new System.Windows.Forms.TabPage();
            this.WhiteLevelBar = new Luna.FloatSlider();
            this.label1 = new System.Windows.Forms.Label();
            this.ColorPickerButton = new System.Windows.Forms.Button();
            this.ScreenTab = new System.Windows.Forms.TabPage();
            this.bottomPosition = new System.Windows.Forms.NumericUpDown();
            this.topPosition = new System.Windows.Forms.NumericUpDown();
            this.SharpenStrength = new System.Windows.Forms.NumericUpDown();
            this.SharpenRadius = new System.Windows.Forms.NumericUpDown();
            this.DepthControl = new System.Windows.Forms.NumericUpDown();
            this.previewControl1 = new Luna.Controls.PreviewControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TestButton = new System.Windows.Forms.Button();
            this.ScriptList = new System.Windows.Forms.ListBox();
            this.AddToListButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ScriptFileTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.spectrumVisualizerControl1 = new Luna.Controls.SpectrumVisualizerControl();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.NotificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            StopButton = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlueSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedSlider)).BeginInit();
            this.Tabs.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GammaSlider)).BeginInit();
            this.LightingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhiteLevelBar)).BeginInit();
            this.ScreenTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpenStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpenRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepthControl)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumVisualizerControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.BlueSlider);
            groupBox1.Controls.Add(this.GreenSlider);
            groupBox1.Controls.Add(this.RedSlider);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new System.Drawing.Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(733, 109);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Balans bieli";
            // 
            // BlueSlider
            // 
            this.BlueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlueSlider.AutoSize = false;
            this.BlueSlider.LargeChange = 0.1F;
            this.BlueSlider.Location = new System.Drawing.Point(83, 78);
            this.BlueSlider.Maximum = 1F;
            this.BlueSlider.Minimum = 0F;
            this.BlueSlider.Name = "BlueSlider";
            this.BlueSlider.Precision = 0.001F;
            this.BlueSlider.Size = new System.Drawing.Size(644, 23);
            this.BlueSlider.SmallChange = 0.01F;
            this.BlueSlider.TabIndex = 7;
            this.BlueSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.BlueSlider.Value = 1F;
            this.BlueSlider.ValueChanged += new System.EventHandler(this.BlueSlider_ValueChanged);
            // 
            // GreenSlider
            // 
            this.GreenSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GreenSlider.AutoSize = false;
            this.GreenSlider.LargeChange = 0.1F;
            this.GreenSlider.Location = new System.Drawing.Point(83, 49);
            this.GreenSlider.Maximum = 1F;
            this.GreenSlider.Minimum = 0F;
            this.GreenSlider.Name = "GreenSlider";
            this.GreenSlider.Precision = 0.001F;
            this.GreenSlider.Size = new System.Drawing.Size(644, 23);
            this.GreenSlider.SmallChange = 0.01F;
            this.GreenSlider.TabIndex = 6;
            this.GreenSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.GreenSlider.Value = 0.9990001F;
            this.GreenSlider.ValueChanged += new System.EventHandler(this.GreenSlider_ValueChanged);
            // 
            // RedSlider
            // 
            this.RedSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RedSlider.AutoSize = false;
            this.RedSlider.LargeChange = 0.1F;
            this.RedSlider.Location = new System.Drawing.Point(83, 20);
            this.RedSlider.Maximum = 1F;
            this.RedSlider.Minimum = 0F;
            this.RedSlider.Name = "RedSlider";
            this.RedSlider.Precision = 0.001F;
            this.RedSlider.Size = new System.Drawing.Size(644, 23);
            this.RedSlider.SmallChange = 0.01F;
            this.RedSlider.TabIndex = 5;
            this.RedSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.RedSlider.Value = 1F;
            this.RedSlider.ValueChanged += new System.EventHandler(this.RedSlider_ValueChanged);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 78);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(50, 13);
            label5.TabIndex = 4;
            label5.Text = "Niebieski";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 49);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(41, 13);
            label4.TabIndex = 2;
            label4.Text = "Zielony";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 20);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(53, 13);
            label3.TabIndex = 0;
            label3.Text = "Czerwony";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 121);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 13);
            label2.TabIndex = 5;
            label2.Text = "Gamma";
            // 
            // label7
            // 
            label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(6, 381);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(60, 13);
            label7.TabIndex = 2;
            label7.Text = "Głębokość";
            // 
            // label8
            // 
            label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(6, 407);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(103, 13);
            label8.TabIndex = 3;
            label8.Text = "Promień wyostrzania";
            // 
            // label9
            // 
            label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 433);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(101, 13);
            label9.TabIndex = 6;
            label9.Text = "Stopień wyostrzania";
            // 
            // StopButton
            // 
            StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            StopButton.Location = new System.Drawing.Point(412, 428);
            StopButton.Name = "StopButton";
            StopButton.Size = new System.Drawing.Size(105, 23);
            StopButton.TabIndex = 5;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // label10
            // 
            label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(226, 381);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(30, 13);
            label10.TabIndex = 7;
            label10.Text = "Góra";
            // 
            // label11
            // 
            label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(226, 412);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(25, 13);
            label11.TabIndex = 8;
            label11.Text = "Dół";
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.SettingsTab);
            this.Tabs.Controls.Add(this.LightingTab);
            this.Tabs.Controls.Add(this.ScreenTab);
            this.Tabs.Controls.Add(this.tabPage1);
            this.Tabs.Controls.Add(this.tabPage2);
            this.Tabs.Location = new System.Drawing.Point(9, 9);
            this.Tabs.Margin = new System.Windows.Forms.Padding(0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(753, 483);
            this.Tabs.TabIndex = 0;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.GammaLabel);
            this.SettingsTab.Controls.Add(this.GammaSlider);
            this.SettingsTab.Controls.Add(label2);
            this.SettingsTab.Controls.Add(groupBox1);
            this.SettingsTab.Location = new System.Drawing.Point(4, 22);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(745, 457);
            this.SettingsTab.TabIndex = 2;
            this.SettingsTab.Text = "Ustawienia";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // GammaLabel
            // 
            this.GammaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GammaLabel.AutoSize = true;
            this.GammaLabel.Location = new System.Drawing.Point(704, 148);
            this.GammaLabel.Name = "GammaLabel";
            this.GammaLabel.Size = new System.Drawing.Size(22, 13);
            this.GammaLabel.TabIndex = 9;
            this.GammaLabel.Text = "2,4";
            this.GammaLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GammaSlider
            // 
            this.GammaSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GammaSlider.AutoSize = false;
            this.GammaSlider.LargeChange = 0.1F;
            this.GammaSlider.Location = new System.Drawing.Point(89, 121);
            this.GammaSlider.Maximum = 3F;
            this.GammaSlider.Minimum = 1.5F;
            this.GammaSlider.Name = "GammaSlider";
            this.GammaSlider.Precision = 0.001F;
            this.GammaSlider.Size = new System.Drawing.Size(609, 23);
            this.GammaSlider.SmallChange = 0.01F;
            this.GammaSlider.TabIndex = 8;
            this.GammaSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.GammaSlider.Value = 2.4F;
            this.GammaSlider.ValueChanged += new System.EventHandler(this.GammaSlider_ValueChanged);
            // 
            // LightingTab
            // 
            this.LightingTab.Controls.Add(this.WhiteLevelBar);
            this.LightingTab.Controls.Add(this.label1);
            this.LightingTab.Controls.Add(this.ColorPickerButton);
            this.LightingTab.Location = new System.Drawing.Point(4, 22);
            this.LightingTab.Name = "LightingTab";
            this.LightingTab.Padding = new System.Windows.Forms.Padding(3);
            this.LightingTab.Size = new System.Drawing.Size(745, 457);
            this.LightingTab.TabIndex = 0;
            this.LightingTab.Text = "Oświetlenie";
            this.LightingTab.UseVisualStyleBackColor = true;
            // 
            // WhiteLevelBar
            // 
            this.WhiteLevelBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WhiteLevelBar.AutoSize = false;
            this.WhiteLevelBar.LargeChange = 0.1F;
            this.WhiteLevelBar.Location = new System.Drawing.Point(76, 22);
            this.WhiteLevelBar.Maximum = 1F;
            this.WhiteLevelBar.Minimum = 0F;
            this.WhiteLevelBar.Name = "WhiteLevelBar";
            this.WhiteLevelBar.Precision = 0.001F;
            this.WhiteLevelBar.Size = new System.Drawing.Size(663, 23);
            this.WhiteLevelBar.SmallChange = 0.1F;
            this.WhiteLevelBar.TabIndex = 6;
            this.WhiteLevelBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.WhiteLevelBar.Value = 1F;
            this.WhiteLevelBar.ValueChanged += new System.EventHandler(this.WhiteLevelBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Poziom bieli";
            // 
            // ColorPickerButton
            // 
            this.ColorPickerButton.Location = new System.Drawing.Point(6, 6);
            this.ColorPickerButton.Name = "ColorPickerButton";
            this.ColorPickerButton.Size = new System.Drawing.Size(64, 64);
            this.ColorPickerButton.TabIndex = 0;
            this.ColorPickerButton.Text = "Kolor";
            this.ColorPickerButton.UseVisualStyleBackColor = true;
            this.ColorPickerButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ScreenTab
            // 
            this.ScreenTab.Controls.Add(this.bottomPosition);
            this.ScreenTab.Controls.Add(this.topPosition);
            this.ScreenTab.Controls.Add(label11);
            this.ScreenTab.Controls.Add(label10);
            this.ScreenTab.Controls.Add(label9);
            this.ScreenTab.Controls.Add(this.SharpenStrength);
            this.ScreenTab.Controls.Add(this.SharpenRadius);
            this.ScreenTab.Controls.Add(label8);
            this.ScreenTab.Controls.Add(label7);
            this.ScreenTab.Controls.Add(this.DepthControl);
            this.ScreenTab.Controls.Add(this.previewControl1);
            this.ScreenTab.Location = new System.Drawing.Point(4, 22);
            this.ScreenTab.Name = "ScreenTab";
            this.ScreenTab.Padding = new System.Windows.Forms.Padding(3);
            this.ScreenTab.Size = new System.Drawing.Size(745, 457);
            this.ScreenTab.TabIndex = 1;
            this.ScreenTab.Text = "Ekran";
            this.ScreenTab.UseVisualStyleBackColor = true;
            // 
            // bottomPosition
            // 
            this.bottomPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bottomPosition.DecimalPlaces = 2;
            this.bottomPosition.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.bottomPosition.Location = new System.Drawing.Point(262, 405);
            this.bottomPosition.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bottomPosition.Name = "bottomPosition";
            this.bottomPosition.Size = new System.Drawing.Size(85, 20);
            this.bottomPosition.TabIndex = 10;
            this.bottomPosition.ValueChanged += new System.EventHandler(this.bottomPosition_ValueChanged);
            // 
            // topPosition
            // 
            this.topPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.topPosition.DecimalPlaces = 2;
            this.topPosition.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.topPosition.Location = new System.Drawing.Point(262, 379);
            this.topPosition.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.topPosition.Name = "topPosition";
            this.topPosition.Size = new System.Drawing.Size(85, 20);
            this.topPosition.TabIndex = 9;
            this.topPosition.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.topPosition.ValueChanged += new System.EventHandler(this.topPosition_ValueChanged);
            // 
            // SharpenStrength
            // 
            this.SharpenStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SharpenStrength.DecimalPlaces = 2;
            this.SharpenStrength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.SharpenStrength.Location = new System.Drawing.Point(114, 431);
            this.SharpenStrength.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.SharpenStrength.Name = "SharpenStrength";
            this.SharpenStrength.Size = new System.Drawing.Size(85, 20);
            this.SharpenStrength.TabIndex = 5;
            // 
            // SharpenRadius
            // 
            this.SharpenRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SharpenRadius.Location = new System.Drawing.Point(114, 405);
            this.SharpenRadius.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SharpenRadius.Name = "SharpenRadius";
            this.SharpenRadius.Size = new System.Drawing.Size(85, 20);
            this.SharpenRadius.TabIndex = 4;
            // 
            // DepthControl
            // 
            this.DepthControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DepthControl.Location = new System.Drawing.Point(114, 379);
            this.DepthControl.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.DepthControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DepthControl.Name = "DepthControl";
            this.DepthControl.Size = new System.Drawing.Size(85, 20);
            this.DepthControl.TabIndex = 1;
            this.DepthControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DepthControl.ValueChanged += new System.EventHandler(this.DepthControl_ValueChanged);
            // 
            // previewControl1
            // 
            this.previewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.previewControl1.Location = new System.Drawing.Point(6, 6);
            this.previewControl1.Name = "previewControl1";
            this.previewControl1.Size = new System.Drawing.Size(733, 367);
            this.previewControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(StopButton);
            this.tabPage1.Controls.Add(this.TestButton);
            this.tabPage1.Controls.Add(this.ScriptList);
            this.tabPage1.Controls.Add(this.AddToListButton);
            this.tabPage1.Controls.Add(this.LoadButton);
            this.tabPage1.Controls.Add(this.ScriptFileTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(745, 457);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Skrypty";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TestButton
            // 
            this.TestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TestButton.Location = new System.Drawing.Point(523, 428);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(105, 23);
            this.TestButton.TabIndex = 4;
            this.TestButton.Text = "Testuj";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // ScriptList
            // 
            this.ScriptList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptList.FormattingEnabled = true;
            this.ScriptList.Location = new System.Drawing.Point(6, 6);
            this.ScriptList.Name = "ScriptList";
            this.ScriptList.Size = new System.Drawing.Size(733, 381);
            this.ScriptList.TabIndex = 3;
            this.ScriptList.DoubleClick += new System.EventHandler(this.ScriptList_DoubleClick);
            this.ScriptList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScriptList_KeyDown);
            // 
            // AddToListButton
            // 
            this.AddToListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddToListButton.Location = new System.Drawing.Point(634, 428);
            this.AddToListButton.Name = "AddToListButton";
            this.AddToListButton.Size = new System.Drawing.Size(105, 23);
            this.AddToListButton.TabIndex = 2;
            this.AddToListButton.Text = "Dodaj do listy";
            this.AddToListButton.UseVisualStyleBackColor = true;
            this.AddToListButton.Click += new System.EventHandler(this.AddToListButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.Location = new System.Drawing.Point(634, 399);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(105, 23);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Przeglądaj";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadScriptButton_Click);
            // 
            // ScriptFileTextBox
            // 
            this.ScriptFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptFileTextBox.Location = new System.Drawing.Point(6, 401);
            this.ScriptFileTextBox.Name = "ScriptFileTextBox";
            this.ScriptFileTextBox.ReadOnly = true;
            this.ScriptFileTextBox.Size = new System.Drawing.Size(622, 20);
            this.ScriptFileTextBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.spectrumVisualizerControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(745, 457);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Wizualizator";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // spectrumVisualizerControl1
            // 
            this.spectrumVisualizerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectrumVisualizerControl1.Location = new System.Drawing.Point(0, 0);
            this.spectrumVisualizerControl1.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.spectrumVisualizerControl1.Name = "spectrumVisualizerControl1";
            this.spectrumVisualizerControl1.Size = new System.Drawing.Size(745, 457);
            this.spectrumVisualizerControl1.TabIndex = 0;
            this.spectrumVisualizerControl1.TabStop = false;
            // 
            // NotificationIcon
            // 
            this.NotificationIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NotificationIcon.BalloonTipText = "Kliknij aby przywrócić";
            this.NotificationIcon.BalloonTipTitle = "Łuna zminimalizowana";
            this.NotificationIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotificationIcon.Icon")));
            this.NotificationIcon.Text = "Łuna";
            this.NotificationIcon.Visible = true;
            this.NotificationIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotificationIcon_MouseDoubleClick);
            // 
            // SessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 501);
            this.Controls.Add(this.Tabs);
            this.Name = "SessionForm";
            this.Text = "Łuna";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SessionForm_FormClosed);
            this.Resize += new System.EventHandler(this.OnResize);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlueSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedSlider)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.SettingsTab.ResumeLayout(false);
            this.SettingsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GammaSlider)).EndInit();
            this.LightingTab.ResumeLayout(false);
            this.LightingTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhiteLevelBar)).EndInit();
            this.ScreenTab.ResumeLayout(false);
            this.ScreenTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpenStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpenRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepthControl)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectrumVisualizerControl1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl Tabs;
		private System.Windows.Forms.TabPage LightingTab;
		private System.Windows.Forms.TabPage ScreenTab;
		private System.Windows.Forms.Button ColorPickerButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.TabPage SettingsTab;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button LoadButton;
		private System.Windows.Forms.TextBox ScriptFileTextBox;
		private System.Windows.Forms.Button AddToListButton;
		private System.Windows.Forms.NotifyIcon NotificationIcon;
		private FloatSlider RedSlider;
		private FloatSlider BlueSlider;
		private FloatSlider GreenSlider;
		private FloatSlider GammaSlider;
		private FloatSlider WhiteLevelBar;
		private Controls.PreviewControl previewControl1;
		private System.Windows.Forms.Label GammaLabel;
		private System.Windows.Forms.NumericUpDown SharpenStrength;
		private System.Windows.Forms.NumericUpDown SharpenRadius;
		private System.Windows.Forms.NumericUpDown DepthControl;
		private System.Windows.Forms.ListBox ScriptList;
		private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.NumericUpDown bottomPosition;
        private System.Windows.Forms.NumericUpDown topPosition;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.SpectrumVisualizerControl spectrumVisualizerControl1;
    }
}