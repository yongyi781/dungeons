namespace Dungeons
{
    partial class MapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataLabel = new System.Windows.Forms.Label();
            this.clearAnnotationsButton = new System.Windows.Forms.Button();
            this.topMostCheckBox = new System.Windows.Forms.CheckBox();
            this.timerLabel = new System.Windows.Forms.Label();
            this.resetTimerButton = new System.Windows.Forms.Button();
            this.plusTenButton = new System.Windows.Forms.Button();
            this.plusOneButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.minusTenButton = new System.Windows.Forms.Button();
            this.mapPictureBox = new Dungeons.MapPictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 300;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // dataLabel
            // 
            this.dataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(9, 285);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(0, 15);
            this.dataLabel.TabIndex = 6;
            // 
            // clearAnnotationsButton
            // 
            this.clearAnnotationsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearAnnotationsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.clearAnnotationsButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.clearAnnotationsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.clearAnnotationsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearAnnotationsButton.Location = new System.Drawing.Point(12, 356);
            this.clearAnnotationsButton.Name = "clearAnnotationsButton";
            this.clearAnnotationsButton.Size = new System.Drawing.Size(122, 25);
            this.clearAnnotationsButton.TabIndex = 10;
            this.clearAnnotationsButton.Text = "Clear Annotations";
            this.clearAnnotationsButton.UseVisualStyleBackColor = false;
            this.clearAnnotationsButton.Click += new System.EventHandler(this.ClearAnnotationsButton_Click);
            // 
            // topMostCheckBox
            // 
            this.topMostCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.topMostCheckBox.AutoSize = true;
            this.topMostCheckBox.Location = new System.Drawing.Point(140, 360);
            this.topMostCheckBox.Name = "topMostCheckBox";
            this.topMostCheckBox.Size = new System.Drawing.Size(77, 19);
            this.topMostCheckBox.TabIndex = 11;
            this.topMostCheckBox.Text = "Top-most";
            this.topMostCheckBox.UseVisualStyleBackColor = true;
            this.topMostCheckBox.CheckedChanged += new System.EventHandler(this.TopMostCheckBox_CheckedChanged);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timerLabel.Font = new System.Drawing.Font("Georgia", 12F);
            this.timerLabel.Location = new System.Drawing.Point(76, 327);
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
            this.resetTimerButton.Location = new System.Drawing.Point(220, 325);
            this.resetTimerButton.Name = "resetTimerButton";
            this.resetTimerButton.Size = new System.Drawing.Size(48, 25);
            this.resetTimerButton.TabIndex = 5;
            this.resetTimerButton.Text = "Reset";
            this.resetTimerButton.UseVisualStyleBackColor = false;
            this.resetTimerButton.Click += new System.EventHandler(this.ResetTimerButton_Click);
            // 
            // plusTenButton
            // 
            this.plusTenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.plusTenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.plusTenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.plusTenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.plusTenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusTenButton.Location = new System.Drawing.Point(180, 325);
            this.plusTenButton.Name = "plusTenButton";
            this.plusTenButton.Size = new System.Drawing.Size(37, 25);
            this.plusTenButton.TabIndex = 4;
            this.plusTenButton.Text = "+10";
            this.plusTenButton.UseVisualStyleBackColor = false;
            this.plusTenButton.Click += new System.EventHandler(this.PlusOneOrTenButton_Click);
            // 
            // plusOneButton
            // 
            this.plusOneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.plusOneButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.plusOneButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.plusOneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.plusOneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusOneButton.Location = new System.Drawing.Point(140, 325);
            this.plusOneButton.Name = "plusOneButton";
            this.plusOneButton.Size = new System.Drawing.Size(37, 25);
            this.plusOneButton.TabIndex = 3;
            this.plusOneButton.Text = "+1";
            this.plusOneButton.UseVisualStyleBackColor = false;
            this.plusOneButton.Click += new System.EventHandler(this.PlusOneOrTenButton_Click);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 282);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // minusTenButton
            // 
            this.minusTenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.minusTenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.minusTenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.minusTenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.minusTenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusTenButton.Location = new System.Drawing.Point(33, 325);
            this.minusTenButton.Name = "minusTenButton";
            this.minusTenButton.Size = new System.Drawing.Size(37, 25);
            this.minusTenButton.TabIndex = 1;
            this.minusTenButton.Tag = "-1";
            this.minusTenButton.Text = "-10";
            this.minusTenButton.UseVisualStyleBackColor = false;
            this.minusTenButton.Click += new System.EventHandler(this.PlusOneOrTenButton_Click);
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mapPictureBox.DrawDistancesEnabled = false;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 1);
            this.mapPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.SelectedLocation = new System.Drawing.Point(-1, -1);
            this.mapPictureBox.Size = new System.Drawing.Size(280, 280);
            this.mapPictureBox.TabIndex = 1;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPictureBox_MouseDown);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(280, 392);
            this.ControlBox = false;
            this.Controls.Add(this.minusTenButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.plusOneButton);
            this.Controls.Add(this.plusTenButton);
            this.Controls.Add(this.resetTimerButton);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.topMostCheckBox);
            this.Controls.Add(this.clearAnnotationsButton);
            this.Controls.Add(this.dataLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Dungeons";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Button clearAnnotationsButton;
        private System.Windows.Forms.CheckBox topMostCheckBox;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Button resetTimerButton;
        private System.Windows.Forms.Button plusTenButton;
        private System.Windows.Forms.Button plusOneButton;
        private MapPictureBox mapPictureBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button minusTenButton;
    }
}

