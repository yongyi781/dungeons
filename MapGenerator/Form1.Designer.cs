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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.slowCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(10, 60);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 280);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // generateButton
            // 
            this.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generateButton.Location = new System.Drawing.Point(281, 7);
            this.generateButton.Margin = new System.Windows.Forms.Padding(2);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 22);
            this.generateButton.TabIndex = 1;
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
            this.label1.TabIndex = 2;
            this.label1.Text = "Roomcount:";
            // 
            // rcUpDown
            // 
            this.rcUpDown.Location = new System.Drawing.Point(87, 9);
            this.rcUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.rcUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.rcUpDown.Minimum = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.rcUpDown.Name = "rcUpDown";
            this.rcUpDown.Size = new System.Drawing.Size(54, 23);
            this.rcUpDown.TabIndex = 3;
            this.rcUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Consolas", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(295, 60);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(390, 279);
            this.textBox1.TabIndex = 4;
            // 
            // copyButton
            // 
            this.copyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.copyButton.Location = new System.Drawing.Point(360, 7);
            this.copyButton.Margin = new System.Windows.Forms.Padding(2);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 22);
            this.copyButton.TabIndex = 5;
            this.copyButton.Text = "&Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // deleteDeadEndButton
            // 
            this.deleteDeadEndButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteDeadEndButton.Location = new System.Drawing.Point(439, 7);
            this.deleteDeadEndButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteDeadEndButton.Name = "deleteDeadEndButton";
            this.deleteDeadEndButton.Size = new System.Drawing.Size(98, 22);
            this.deleteDeadEndButton.TabIndex = 6;
            this.deleteDeadEndButton.Text = "&Delete Dead End";
            this.deleteDeadEndButton.UseVisualStyleBackColor = true;
            this.deleteDeadEndButton.Click += new System.EventHandler(this.deleteDeadEndButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logTextBox.Location = new System.Drawing.Point(10, 345);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(675, 222);
            this.logTextBox.TabIndex = 4;
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
            this.randomSeedCheckbox.TabIndex = 7;
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
            this.randomRcCheckbox.TabIndex = 7;
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
            this.drawCritCheckBox.TabIndex = 7;
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
            this.drawDeadEndsCheckBox.TabIndex = 7;
            this.drawDeadEndsCheckBox.Text = "Draw dead ends";
            this.drawDeadEndsCheckBox.UseVisualStyleBackColor = true;
            this.drawDeadEndsCheckBox.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // slowCheckBox
            // 
            this.slowCheckBox.AutoSize = true;
            this.slowCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slowCheckBox.Location = new System.Drawing.Point(503, 37);
            this.slowCheckBox.Name = "slowCheckBox";
            this.slowCheckBox.Size = new System.Drawing.Size(57, 20);
            this.slowCheckBox.TabIndex = 7;
            this.slowCheckBox.Text = "Slow";
            this.slowCheckBox.UseVisualStyleBackColor = true;
            this.slowCheckBox.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(697, 579);
            this.Controls.Add(this.slowCheckBox);
            this.Controls.Add(this.drawDeadEndsCheckBox);
            this.Controls.Add(this.drawCritCheckBox);
            this.Controls.Add(this.randomSeedCheckbox);
            this.Controls.Add(this.randomRcCheckbox);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.deleteDeadEndButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.seedUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rcUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Map Generator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.CheckBox slowCheckBox;
    }
}

