namespace MapGenerator
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox = new PictureBox();
            generateButton = new Button();
            label1 = new Label();
            rcUpDown = new NumericUpDown();
            textBox1 = new TextBox();
            copyButton = new Button();
            deleteDeadEndButton = new Button();
            logTextBox = new TextBox();
            label2 = new Label();
            seedUpDown = new NumericUpDown();
            randomSeedCheckbox = new CheckBox();
            randomRcCheckbox = new CheckBox();
            drawCritCheckBox = new CheckBox();
            drawDeadEndsCheckBox = new CheckBox();
            openStatsWindowButton = new Button();
            algorithmComboBox = new ComboBox();
            browseButton = new Button();
            sizeComboBox = new ComboBox();
            sizeUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rcUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)seedUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sizeUpDown).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.Black;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(10, 62);
            pictureBox.Margin = new Padding(2);
            pictureBox.MaximumSize = new Size(664, 664);
            pictureBox.Name = "pictureBox";
            pictureBox.Padding = new Padding(12);
            pictureBox.Size = new Size(280, 279);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.DpiChangedAfterParent += pictureBox_DpiChangedAfterParent;
            // 
            // generateButton
            // 
            generateButton.FlatStyle = FlatStyle.System;
            generateButton.Location = new Point(281, 7);
            generateButton.Margin = new Padding(2);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(75, 23);
            generateButton.TabIndex = 4;
            generateButton.Text = "&Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += GenerateButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 11);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Roomcount:";
            // 
            // rcUpDown
            // 
            rcUpDown.Location = new Point(87, 9);
            rcUpDown.Margin = new Padding(2);
            rcUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            rcUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            rcUpDown.Name = "rcUpDown";
            rcUpDown.Size = new Size(54, 23);
            rcUpDown.TabIndex = 1;
            rcUpDown.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            textBox1.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(428, 62);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(431, 279);
            textBox1.TabIndex = 4;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // copyButton
            // 
            copyButton.FlatStyle = FlatStyle.System;
            copyButton.Location = new Point(451, 7);
            copyButton.Margin = new Padding(2);
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(75, 23);
            copyButton.TabIndex = 6;
            copyButton.Text = "&Copy";
            copyButton.UseVisualStyleBackColor = true;
            copyButton.Click += CopyButton_Click;
            // 
            // deleteDeadEndButton
            // 
            deleteDeadEndButton.FlatStyle = FlatStyle.System;
            deleteDeadEndButton.Location = new Point(530, 7);
            deleteDeadEndButton.Margin = new Padding(2);
            deleteDeadEndButton.Name = "deleteDeadEndButton";
            deleteDeadEndButton.Size = new Size(110, 23);
            deleteDeadEndButton.TabIndex = 7;
            deleteDeadEndButton.Text = "&Delete Bonus DEs";
            deleteDeadEndButton.UseVisualStyleBackColor = true;
            deleteDeadEndButton.Click += DeleteDeadEndButton_Click;
            // 
            // logTextBox
            // 
            logTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logTextBox.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            logTextBox.Location = new Point(10, 347);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ScrollBars = ScrollBars.Both;
            logTextBox.Size = new Size(848, 276);
            logTextBox.TabIndex = 4;
            logTextBox.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(145, 11);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 2;
            label2.Text = "Seed:";
            // 
            // seedUpDown
            // 
            seedUpDown.Location = new Point(184, 9);
            seedUpDown.Margin = new Padding(2);
            seedUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            seedUpDown.Name = "seedUpDown";
            seedUpDown.Size = new Size(93, 23);
            seedUpDown.TabIndex = 3;
            // 
            // randomSeedCheckbox
            // 
            randomSeedCheckbox.AutoSize = true;
            randomSeedCheckbox.Checked = true;
            randomSeedCheckbox.CheckState = CheckState.Checked;
            randomSeedCheckbox.FlatStyle = FlatStyle.System;
            randomSeedCheckbox.Location = new Point(172, 37);
            randomSeedCheckbox.Name = "randomSeedCheckbox";
            randomSeedCheckbox.Size = new Size(118, 20);
            randomSeedCheckbox.TabIndex = 10;
            randomSeedCheckbox.Text = "&Randomize seed";
            randomSeedCheckbox.UseVisualStyleBackColor = true;
            // 
            // randomRcCheckbox
            // 
            randomRcCheckbox.AutoSize = true;
            randomRcCheckbox.FlatStyle = FlatStyle.System;
            randomRcCheckbox.Location = new Point(12, 37);
            randomRcCheckbox.Name = "randomRcCheckbox";
            randomRcCheckbox.Size = new Size(154, 20);
            randomRcCheckbox.TabIndex = 9;
            randomRcCheckbox.Text = "Randomize roomcount";
            randomRcCheckbox.UseVisualStyleBackColor = true;
            // 
            // drawCritCheckBox
            // 
            drawCritCheckBox.AutoSize = true;
            drawCritCheckBox.FlatStyle = FlatStyle.System;
            drawCritCheckBox.Location = new Point(296, 37);
            drawCritCheckBox.Name = "drawCritCheckBox";
            drawCritCheckBox.Size = new Size(79, 20);
            drawCritCheckBox.TabIndex = 11;
            drawCritCheckBox.Text = "Draw &crit";
            drawCritCheckBox.UseVisualStyleBackColor = true;
            drawCritCheckBox.CheckedChanged += DrawBox_CheckedChanged;
            // 
            // drawDeadEndsCheckBox
            // 
            drawDeadEndsCheckBox.AutoSize = true;
            drawDeadEndsCheckBox.FlatStyle = FlatStyle.System;
            drawDeadEndsCheckBox.Location = new Point(381, 37);
            drawDeadEndsCheckBox.Name = "drawDeadEndsCheckBox";
            drawDeadEndsCheckBox.Size = new Size(116, 20);
            drawDeadEndsCheckBox.TabIndex = 12;
            drawDeadEndsCheckBox.Text = "Draw dead ends";
            drawDeadEndsCheckBox.UseVisualStyleBackColor = true;
            drawDeadEndsCheckBox.CheckedChanged += DrawBox_CheckedChanged;
            // 
            // openStatsWindowButton
            // 
            openStatsWindowButton.FlatStyle = FlatStyle.System;
            openStatsWindowButton.Location = new Point(657, 7);
            openStatsWindowButton.Name = "openStatsWindowButton";
            openStatsWindowButton.Size = new Size(128, 23);
            openStatsWindowButton.TabIndex = 8;
            openStatsWindowButton.Text = "&Open Stats Window";
            openStatsWindowButton.UseVisualStyleBackColor = true;
            openStatsWindowButton.Click += OpenStatsWindowButton_Click;
            // 
            // algorithmComboBox
            // 
            algorithmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithmComboBox.FormattingEnabled = true;
            algorithmComboBox.Items.AddRange(new object[] { "Prim", "Uniform spanning tree", "Prim, room variant", "Random edges" });
            algorithmComboBox.Location = new Point(703, 36);
            algorithmComboBox.Name = "algorithmComboBox";
            algorithmComboBox.Size = new Size(155, 23);
            algorithmComboBox.TabIndex = 13;
            // 
            // browseButton
            // 
            browseButton.FlatStyle = FlatStyle.System;
            browseButton.Location = new Point(360, 7);
            browseButton.Margin = new Padding(2);
            browseButton.Name = "browseButton";
            browseButton.Size = new Size(75, 23);
            browseButton.TabIndex = 5;
            browseButton.Text = "&Browse...";
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += BrowseButton_Click;
            // 
            // sizeComboBox
            // 
            sizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sizeComboBox.FormattingEnabled = true;
            sizeComboBox.Items.AddRange(new object[] { "Small (4 x 4)", "Medium (4 x 8)", "Large (8 x 8)", "Huge (12 x 12)", "Gigantic (16 x 16)", "Custom" });
            sizeComboBox.Location = new Point(503, 36);
            sizeComboBox.Name = "sizeComboBox";
            sizeComboBox.Size = new Size(125, 23);
            sizeComboBox.TabIndex = 13;
            sizeComboBox.SelectedIndexChanged += SizeComboBox_SelectedIndexChanged;
            // 
            // sizeUpDown
            // 
            sizeUpDown.Location = new Point(634, 37);
            sizeUpDown.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            sizeUpDown.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            sizeUpDown.Name = "sizeUpDown";
            sizeUpDown.Size = new Size(63, 23);
            sizeUpDown.TabIndex = 14;
            sizeUpDown.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // Form1
            // 
            AcceptButton = generateButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(870, 635);
            Controls.Add(sizeUpDown);
            Controls.Add(algorithmComboBox);
            Controls.Add(sizeComboBox);
            Controls.Add(openStatsWindowButton);
            Controls.Add(deleteDeadEndButton);
            Controls.Add(browseButton);
            Controls.Add(copyButton);
            Controls.Add(logTextBox);
            Controls.Add(pictureBox);
            Controls.Add(textBox1);
            Controls.Add(generateButton);
            Controls.Add(drawDeadEndsCheckBox);
            Controls.Add(drawCritCheckBox);
            Controls.Add(randomSeedCheckbox);
            Controls.Add(randomRcCheckbox);
            Controls.Add(seedUpDown);
            Controls.Add(label2);
            Controls.Add(rcUpDown);
            Controls.Add(label1);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Map Generator";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)rcUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)seedUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)sizeUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private Button generateButton;
        private Label label1;
        private NumericUpDown rcUpDown;
        private TextBox textBox1;
        private Button copyButton;
        private Button deleteDeadEndButton;
        private TextBox logTextBox;
        private Label label2;
        private NumericUpDown seedUpDown;
        private CheckBox randomSeedCheckbox;
        private CheckBox randomRcCheckbox;
        private CheckBox drawCritCheckBox;
        private CheckBox drawDeadEndsCheckBox;
        private Button openStatsWindowButton;
        private ComboBox algorithmComboBox;
        private Button browseButton;
        private ComboBox sizeComboBox;
        private NumericUpDown sizeUpDown;
    }
}

