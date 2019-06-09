namespace Dungeons
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.calibrateButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataLabel = new System.Windows.Forms.Label();
            this.saveMapButton = new System.Windows.Forms.Button();
            this.savedLabel = new System.Windows.Forms.Label();
            this.saveLabelHideTimer = new System.Windows.Forms.Timer(this.components);
            this.clearAnnotationsButton = new System.Windows.Forms.Button();
            this.topMostCheckBox = new System.Windows.Forms.CheckBox();
            this.timerLabel = new System.Windows.Forms.Label();
            this.resetTimerButton = new System.Windows.Forms.Button();
            this.plusTenButton = new System.Windows.Forms.Button();
            this.plusOneButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mapPictureBox = new Dungeons.MapPictureBox();
            this.minusTenButton = new System.Windows.Forms.Button();
            this.winterfaceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // calibrateButton
            // 
            this.calibrateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.calibrateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.calibrateButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.calibrateButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.calibrateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.calibrateButton.Location = new System.Drawing.Point(12, 354);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 25);
            this.calibrateButton.TabIndex = 7;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = false;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 600;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataLabel
            // 
            this.dataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(9, 329);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(0, 15);
            this.dataLabel.TabIndex = 6;
            // 
            // saveMapButton
            // 
            this.saveMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.saveMapButton.Enabled = false;
            this.saveMapButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.saveMapButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.saveMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveMapButton.Location = new System.Drawing.Point(174, 354);
            this.saveMapButton.Name = "saveMapButton";
            this.saveMapButton.Size = new System.Drawing.Size(75, 25);
            this.saveMapButton.TabIndex = 9;
            this.saveMapButton.Text = "Save Map";
            this.saveMapButton.UseVisualStyleBackColor = false;
            this.saveMapButton.Click += new System.EventHandler(this.saveMapButton_Click);
            // 
            // savedLabel
            // 
            this.savedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.savedLabel.AutoSize = true;
            this.savedLabel.ForeColor = System.Drawing.Color.Lime;
            this.savedLabel.Location = new System.Drawing.Point(227, 390);
            this.savedLabel.Name = "savedLabel";
            this.savedLabel.Size = new System.Drawing.Size(41, 15);
            this.savedLabel.TabIndex = 12;
            this.savedLabel.Text = "Saved!";
            this.savedLabel.Visible = false;
            // 
            // saveLabelHideTimer
            // 
            this.saveLabelHideTimer.Interval = 1500;
            this.saveLabelHideTimer.Tick += new System.EventHandler(this.saveLabelHideTimer_Tick);
            // 
            // clearAnnotationsButton
            // 
            this.clearAnnotationsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearAnnotationsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.clearAnnotationsButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.clearAnnotationsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.clearAnnotationsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearAnnotationsButton.Location = new System.Drawing.Point(12, 385);
            this.clearAnnotationsButton.Name = "clearAnnotationsButton";
            this.clearAnnotationsButton.Size = new System.Drawing.Size(122, 25);
            this.clearAnnotationsButton.TabIndex = 10;
            this.clearAnnotationsButton.Text = "Clear Annotations";
            this.clearAnnotationsButton.UseVisualStyleBackColor = false;
            this.clearAnnotationsButton.Click += new System.EventHandler(this.clearAnnotationsButton_Click);
            // 
            // topMostCheckBox
            // 
            this.topMostCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.topMostCheckBox.AutoSize = true;
            this.topMostCheckBox.Location = new System.Drawing.Point(140, 389);
            this.topMostCheckBox.Name = "topMostCheckBox";
            this.topMostCheckBox.Size = new System.Drawing.Size(77, 19);
            this.topMostCheckBox.TabIndex = 11;
            this.topMostCheckBox.Text = "Top-most";
            this.topMostCheckBox.UseVisualStyleBackColor = true;
            this.topMostCheckBox.CheckedChanged += new System.EventHandler(this.topMostCheckBox_CheckedChanged);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timerLabel.Font = new System.Drawing.Font("Georgia", 12F);
            this.timerLabel.Location = new System.Drawing.Point(76, 300);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(58, 18);
            this.timerLabel.TabIndex = 2;
            this.timerLabel.Text = "0:00";
            this.timerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // resetTimerButton
            // 
            this.resetTimerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetTimerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.resetTimerButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.resetTimerButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.resetTimerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetTimerButton.Location = new System.Drawing.Point(220, 298);
            this.resetTimerButton.Name = "resetTimerButton";
            this.resetTimerButton.Size = new System.Drawing.Size(48, 25);
            this.resetTimerButton.TabIndex = 5;
            this.resetTimerButton.Text = "Reset";
            this.resetTimerButton.UseVisualStyleBackColor = false;
            this.resetTimerButton.Click += new System.EventHandler(this.resetTimerButton_Click);
            // 
            // plusTenButton
            // 
            this.plusTenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.plusTenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.plusTenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.plusTenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.plusTenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusTenButton.Location = new System.Drawing.Point(180, 298);
            this.plusTenButton.Name = "plusTenButton";
            this.plusTenButton.Size = new System.Drawing.Size(37, 25);
            this.plusTenButton.TabIndex = 4;
            this.plusTenButton.Text = "+10";
            this.plusTenButton.UseVisualStyleBackColor = false;
            this.plusTenButton.Click += new System.EventHandler(this.plusOneOrTenButton_Click);
            // 
            // plusOneButton
            // 
            this.plusOneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.plusOneButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.plusOneButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.plusOneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.plusOneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusOneButton.Location = new System.Drawing.Point(140, 298);
            this.plusOneButton.Name = "plusOneButton";
            this.plusOneButton.Size = new System.Drawing.Size(37, 25);
            this.plusOneButton.TabIndex = 3;
            this.plusOneButton.Text = "+1";
            this.plusOneButton.UseVisualStyleBackColor = false;
            this.plusOneButton.Click += new System.EventHandler(this.plusOneOrTenButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.mapPictureBox, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 280);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mapPictureBox.DrawDistancesEnabled = false;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.SelectedLocation = new System.Drawing.Point(-1, -1);
            this.mapPictureBox.Size = new System.Drawing.Size(280, 280);
            this.mapPictureBox.TabIndex = 1;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseDown);
            // 
            // minusTenButton
            // 
            this.minusTenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.minusTenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.minusTenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.minusTenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.minusTenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusTenButton.Location = new System.Drawing.Point(33, 298);
            this.minusTenButton.Name = "minusTenButton";
            this.minusTenButton.Size = new System.Drawing.Size(37, 25);
            this.minusTenButton.TabIndex = 1;
            this.minusTenButton.Tag = "-1";
            this.minusTenButton.Text = "-10";
            this.minusTenButton.UseVisualStyleBackColor = false;
            this.minusTenButton.Click += new System.EventHandler(this.plusOneOrTenButton_Click);
            // 
            // winterfaceButton
            // 
            this.winterfaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.winterfaceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.winterfaceButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.winterfaceButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.winterfaceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.winterfaceButton.Location = new System.Drawing.Point(93, 354);
            this.winterfaceButton.Name = "winterfaceButton";
            this.winterfaceButton.Size = new System.Drawing.Size(75, 25);
            this.winterfaceButton.TabIndex = 8;
            this.winterfaceButton.Text = "Winterface";
            this.winterfaceButton.UseVisualStyleBackColor = false;
            this.winterfaceButton.Click += new System.EventHandler(this.WinterfaceButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(280, 422);
            this.Controls.Add(this.winterfaceButton);
            this.Controls.Add(this.minusTenButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.plusOneButton);
            this.Controls.Add(this.plusTenButton);
            this.Controls.Add(this.resetTimerButton);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.topMostCheckBox);
            this.Controls.Add(this.clearAnnotationsButton);
            this.Controls.Add(this.savedLabel);
            this.Controls.Add(this.saveMapButton);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.calibrateButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Dungeons";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Button saveMapButton;
        private System.Windows.Forms.Label savedLabel;
        private System.Windows.Forms.Timer saveLabelHideTimer;
        private System.Windows.Forms.Button clearAnnotationsButton;
        private System.Windows.Forms.CheckBox topMostCheckBox;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Button resetTimerButton;
        private System.Windows.Forms.Button plusTenButton;
        private System.Windows.Forms.Button plusOneButton;
        private MapPictureBox mapPictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button minusTenButton;
        private System.Windows.Forms.Button winterfaceButton;
    }
}

