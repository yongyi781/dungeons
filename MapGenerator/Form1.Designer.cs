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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rcUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.copyButton = new System.Windows.Forms.Button();
            this.deleteDeadEndButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.seedUpDown = new System.Windows.Forms.NumericUpDown();
            this.randomSeedCheckbox = new System.Windows.Forms.CheckBox();
            this.randomRcCheckbox = new System.Windows.Forms.CheckBox();
            this.drawCritCheckBox = new System.Windows.Forms.CheckBox();
            this.drawDeadEndsCheckBox = new System.Windows.Forms.CheckBox();
            this.openStatsWindowButton = new System.Windows.Forms.Button();
            this.algorithmComboBox = new System.Windows.Forms.ComboBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.sizeComboBox = new System.Windows.Forms.ComboBox();
            this.sizeUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Black;
            this.pictureBox.Location = new System.Drawing.Point(10, 62);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox.MaximumSize = new System.Drawing.Size(664, 664);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Padding = new System.Windows.Forms.Padding(12);
            this.pictureBox.Size = new System.Drawing.Size(280, 280);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // generateButton
            // 
            this.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generateButton.Location = new System.Drawing.Point(281, 7);
            this.generateButton.Margin = new System.Windows.Forms.Padding(2);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "&Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Roomcount:";
            // 
            // rcUpDown
            // 
            this.rcUpDown.Location = new System.Drawing.Point(87, 9);
            this.rcUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.rcUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.rcUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rcUpDown.Name = "rcUpDown";
            this.rcUpDown.Size = new System.Drawing.Size(54, 23);
            this.rcUpDown.TabIndex = 1;
            this.rcUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(360, 62);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(499, 279);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // copyButton
            // 
            this.copyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.copyButton.Location = new System.Drawing.Point(451, 7);
            this.copyButton.Margin = new System.Windows.Forms.Padding(2);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 23);
            this.copyButton.TabIndex = 6;
            this.copyButton.Text = "&Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // deleteDeadEndButton
            // 
            this.deleteDeadEndButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteDeadEndButton.Location = new System.Drawing.Point(530, 7);
            this.deleteDeadEndButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteDeadEndButton.Name = "deleteDeadEndButton";
            this.deleteDeadEndButton.Size = new System.Drawing.Size(98, 23);
            this.deleteDeadEndButton.TabIndex = 7;
            this.deleteDeadEndButton.Text = "&Delete Dead End";
            this.deleteDeadEndButton.UseVisualStyleBackColor = true;
            this.deleteDeadEndButton.Click += new System.EventHandler(this.deleteDeadEndButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logTextBox.Location = new System.Drawing.Point(10, 347);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(848, 276);
            this.logTextBox.TabIndex = 4;
            this.logTextBox.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Seed:";
            // 
            // seedUpDown
            // 
            this.seedUpDown.Location = new System.Drawing.Point(184, 9);
            this.seedUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.seedUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.seedUpDown.Name = "seedUpDown";
            this.seedUpDown.Size = new System.Drawing.Size(93, 23);
            this.seedUpDown.TabIndex = 3;
            // 
            // randomSeedCheckbox
            // 
            this.randomSeedCheckbox.AutoSize = true;
            this.randomSeedCheckbox.Checked = true;
            this.randomSeedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.randomSeedCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomSeedCheckbox.Location = new System.Drawing.Point(172, 37);
            this.randomSeedCheckbox.Name = "randomSeedCheckbox";
            this.randomSeedCheckbox.Size = new System.Drawing.Size(118, 20);
            this.randomSeedCheckbox.TabIndex = 10;
            this.randomSeedCheckbox.Text = "&Randomize seed";
            this.randomSeedCheckbox.UseVisualStyleBackColor = true;
            // 
            // randomRcCheckbox
            // 
            this.randomRcCheckbox.AutoSize = true;
            this.randomRcCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomRcCheckbox.Location = new System.Drawing.Point(12, 37);
            this.randomRcCheckbox.Name = "randomRcCheckbox";
            this.randomRcCheckbox.Size = new System.Drawing.Size(154, 20);
            this.randomRcCheckbox.TabIndex = 9;
            this.randomRcCheckbox.Text = "Randomize roomcount";
            this.randomRcCheckbox.UseVisualStyleBackColor = true;
            // 
            // drawCritCheckBox
            // 
            this.drawCritCheckBox.AutoSize = true;
            this.drawCritCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.drawCritCheckBox.Location = new System.Drawing.Point(296, 37);
            this.drawCritCheckBox.Name = "drawCritCheckBox";
            this.drawCritCheckBox.Size = new System.Drawing.Size(79, 20);
            this.drawCritCheckBox.TabIndex = 11;
            this.drawCritCheckBox.Text = "Draw &crit";
            this.drawCritCheckBox.UseVisualStyleBackColor = true;
            this.drawCritCheckBox.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // drawDeadEndsCheckBox
            // 
            this.drawDeadEndsCheckBox.AutoSize = true;
            this.drawDeadEndsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.drawDeadEndsCheckBox.Location = new System.Drawing.Point(381, 37);
            this.drawDeadEndsCheckBox.Name = "drawDeadEndsCheckBox";
            this.drawDeadEndsCheckBox.Size = new System.Drawing.Size(116, 20);
            this.drawDeadEndsCheckBox.TabIndex = 12;
            this.drawDeadEndsCheckBox.Text = "Draw dead ends";
            this.drawDeadEndsCheckBox.UseVisualStyleBackColor = true;
            this.drawDeadEndsCheckBox.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // openStatsWindowButton
            // 
            this.openStatsWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.openStatsWindowButton.Location = new System.Drawing.Point(645, 7);
            this.openStatsWindowButton.Name = "openStatsWindowButton";
            this.openStatsWindowButton.Size = new System.Drawing.Size(128, 23);
            this.openStatsWindowButton.TabIndex = 8;
            this.openStatsWindowButton.Text = "&Open Stats Window";
            this.openStatsWindowButton.UseVisualStyleBackColor = true;
            this.openStatsWindowButton.Click += new System.EventHandler(this.openStatsWindowButton_Click);
            // 
            // algorithmComboBox
            // 
            this.algorithmComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmComboBox.FormattingEnabled = true;
            this.algorithmComboBox.Items.AddRange(new object[] {
            "Prim",
            "Uniform spanning tree",
            "Prim, room variant",
            "Random edges"});
            this.algorithmComboBox.Location = new System.Drawing.Point(703, 36);
            this.algorithmComboBox.Name = "algorithmComboBox";
            this.algorithmComboBox.Size = new System.Drawing.Size(155, 23);
            this.algorithmComboBox.TabIndex = 13;
            // 
            // browseButton
            // 
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseButton.Location = new System.Drawing.Point(360, 7);
            this.browseButton.Margin = new System.Windows.Forms.Padding(2);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "&Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // sizeComboBox
            // 
            this.sizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeComboBox.FormattingEnabled = true;
            this.sizeComboBox.Items.AddRange(new object[] {
            "Small (4 x 4)",
            "Medium (4 x 8)",
            "Large (8 x 8)",
            "Huge (12 x 12)",
            "Gigantic (16 x 16)",
            "Custom"});
            this.sizeComboBox.Location = new System.Drawing.Point(503, 36);
            this.sizeComboBox.Name = "sizeComboBox";
            this.sizeComboBox.Size = new System.Drawing.Size(125, 23);
            this.sizeComboBox.TabIndex = 13;
            this.sizeComboBox.SelectedIndexChanged += new System.EventHandler(this.sizeComboBox_SelectedIndexChanged);
            // 
            // sizeUpDown
            // 
            this.sizeUpDown.Location = new System.Drawing.Point(634, 37);
            this.sizeUpDown.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.sizeUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.sizeUpDown.Name = "sizeUpDown";
            this.sizeUpDown.Size = new System.Drawing.Size(63, 23);
            this.sizeUpDown.TabIndex = 14;
            this.sizeUpDown.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(870, 635);
            this.Controls.Add(this.sizeUpDown);
            this.Controls.Add(this.algorithmComboBox);
            this.Controls.Add(this.sizeComboBox);
            this.Controls.Add(this.openStatsWindowButton);
            this.Controls.Add(this.deleteDeadEndButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.drawDeadEndsCheckBox);
            this.Controls.Add(this.drawCritCheckBox);
            this.Controls.Add(this.randomSeedCheckbox);
            this.Controls.Add(this.randomRcCheckbox);
            this.Controls.Add(this.seedUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rcUpDown);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Map Generator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown rcUpDown;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button deleteDeadEndButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown seedUpDown;
        private System.Windows.Forms.CheckBox randomSeedCheckbox;
        private System.Windows.Forms.CheckBox randomRcCheckbox;
        private System.Windows.Forms.CheckBox drawCritCheckBox;
        private System.Windows.Forms.CheckBox drawDeadEndsCheckBox;
        private System.Windows.Forms.Button openStatsWindowButton;
        private System.Windows.Forms.ComboBox algorithmComboBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.ComboBox sizeComboBox;
        private System.Windows.Forms.NumericUpDown sizeUpDown;
    }
}

