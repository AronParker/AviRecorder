namespace AviRecorder.Forms
{
    partial class RecordingSettingsForm
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
            this._recordingNotificationsCheckBox = new System.Windows.Forms.CheckBox();
            this._deleteOnCloseCheckBox = new System.Windows.Forms.CheckBox();
            this._instructionTextLabel = new System.Windows.Forms.Label();
            this._options = new System.Windows.Forms.Label();
            this._hostFramerateLabel = new System.Windows.Forms.Label();
            this._compressorLabel = new System.Windows.Forms.Label();
            this._hostFramerateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._footerPanel = new System.Windows.Forms.Panel();
            this._resetLinkLabel = new System.Windows.Forms.LinkLabel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._aviLabel = new System.Windows.Forms.Label();
            this._tgaLabel = new System.Windows.Forms.Label();
            this._tgaTextBox = new System.Windows.Forms.TextBox();
            this._tgaButton = new System.Windows.Forms.Button();
            this._framesToProcessLabel = new System.Windows.Forms.Label();
            this._aviTextBox = new System.Windows.Forms.TextBox();
            this._frameBlendingFactorLabel = new System.Windows.Forms.Label();
            this._frameRateLabel = new System.Windows.Forms.Label();
            this._frameRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._compressorButton = new System.Windows.Forms.Button();
            this._frameBlendingFactorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._compressorTextBox = new System.Windows.Forms.TextBox();
            this._framesToProcessNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._aviButton = new System.Windows.Forms.Button();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._hostFramerateNumericUpDown)).BeginInit();
            this._footerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._frameRateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._frameBlendingFactorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._framesToProcessNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _recordingNotificationsCheckBox
            // 
            this._recordingNotificationsCheckBox.Location = new System.Drawing.Point(148, 219);
            this._recordingNotificationsCheckBox.Name = "_recordingNotificationsCheckBox";
            this._recordingNotificationsCheckBox.Size = new System.Drawing.Size(424, 23);
            this._recordingNotificationsCheckBox.TabIndex = 20;
            this._recordingNotificationsCheckBox.Text = "Display recording notifications";
            this._recordingNotificationsCheckBox.UseVisualStyleBackColor = true;
            // 
            // _deleteOnCloseCheckBox
            // 
            this._deleteOnCloseCheckBox.Location = new System.Drawing.Point(148, 190);
            this._deleteOnCloseCheckBox.Name = "_deleteOnCloseCheckBox";
            this._deleteOnCloseCheckBox.Size = new System.Drawing.Size(424, 23);
            this._deleteOnCloseCheckBox.TabIndex = 19;
            this._deleteOnCloseCheckBox.Text = "Delete frames after processing (recommended)";
            this._deleteOnCloseCheckBox.UseVisualStyleBackColor = true;
            // 
            // _instructionTextLabel
            // 
            this._instructionTextLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._instructionTextLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._instructionTextLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this._instructionTextLabel.Location = new System.Drawing.Point(0, 0);
            this._instructionTextLabel.Name = "_instructionTextLabel";
            this._instructionTextLabel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this._instructionTextLabel.Size = new System.Drawing.Size(584, 42);
            this._instructionTextLabel.TabIndex = 0;
            this._instructionTextLabel.Text = "Configure recording settings";
            this._instructionTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _options
            // 
            this._options.Location = new System.Drawing.Point(12, 190);
            this._options.Name = "_options";
            this._options.Size = new System.Drawing.Size(130, 23);
            this._options.TabIndex = 18;
            this._options.Text = "Options:";
            this._options.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _hostFramerateLabel
            // 
            this._hostFramerateLabel.Location = new System.Drawing.Point(12, 161);
            this._hostFramerateLabel.Name = "_hostFramerateLabel";
            this._hostFramerateLabel.Size = new System.Drawing.Size(130, 23);
            this._hostFramerateLabel.TabIndex = 16;
            this._hostFramerateLabel.Text = "host_framerate:";
            this._hostFramerateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _compressorLabel
            // 
            this._compressorLabel.Location = new System.Drawing.Point(12, 103);
            this._compressorLabel.Name = "_compressorLabel";
            this._compressorLabel.Size = new System.Drawing.Size(130, 23);
            this._compressorLabel.TabIndex = 7;
            this._compressorLabel.Text = "Video codec:";
            this._compressorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _hostFramerateNumericUpDown
            // 
            this._hostFramerateNumericUpDown.Location = new System.Drawing.Point(148, 161);
            this._hostFramerateNumericUpDown.Name = "_hostFramerateNumericUpDown";
            this._hostFramerateNumericUpDown.ReadOnly = true;
            this._hostFramerateNumericUpDown.Size = new System.Drawing.Size(140, 23);
            this._hostFramerateNumericUpDown.TabIndex = 17;
            // 
            // _footerPanel
            // 
            this._footerPanel.BackColor = System.Drawing.SystemColors.Control;
            this._footerPanel.Controls.Add(this._resetLinkLabel);
            this._footerPanel.Controls.Add(this._cancelButton);
            this._footerPanel.Controls.Add(this._okButton);
            this._footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._footerPanel.Location = new System.Drawing.Point(0, 248);
            this._footerPanel.Name = "_footerPanel";
            this._footerPanel.Size = new System.Drawing.Size(584, 50);
            this._footerPanel.TabIndex = 21;
            // 
            // _resetLinkLabel
            // 
            this._resetLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._resetLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._resetLinkLabel.Location = new System.Drawing.Point(12, 13);
            this._resetLinkLabel.Name = "_resetLinkLabel";
            this._resetLinkLabel.Size = new System.Drawing.Size(348, 25);
            this._resetLinkLabel.TabIndex = 2;
            this._resetLinkLabel.TabStop = true;
            this._resetLinkLabel.Text = "Reset settings";
            this._resetLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._resetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ResetLinkLabel_LinkClicked);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(472, 13);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 25);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.Location = new System.Drawing.Point(366, 13);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(100, 25);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _aviLabel
            // 
            this._aviLabel.Location = new System.Drawing.Point(12, 74);
            this._aviLabel.Name = "_aviLabel";
            this._aviLabel.Size = new System.Drawing.Size(130, 23);
            this._aviLabel.TabIndex = 2;
            this._aviLabel.Text = "AVI folder:";
            this._aviLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tgaLabel
            // 
            this._tgaLabel.Location = new System.Drawing.Point(12, 45);
            this._tgaLabel.Name = "_tgaLabel";
            this._tgaLabel.Size = new System.Drawing.Size(130, 23);
            this._tgaLabel.TabIndex = 1;
            this._tgaLabel.Text = "TGA folder:";
            this._tgaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tgaTextBox
            // 
            this._tgaTextBox.AllowDrop = true;
            this._tgaTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._tgaTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._tgaTextBox.Location = new System.Drawing.Point(148, 45);
            this._tgaTextBox.Name = "_tgaTextBox";
            this._tgaTextBox.Size = new System.Drawing.Size(318, 23);
            this._tgaTextBox.TabIndex = 3;
            // 
            // _tgaButton
            // 
            this._tgaButton.Location = new System.Drawing.Point(472, 45);
            this._tgaButton.Name = "_tgaButton";
            this._tgaButton.Size = new System.Drawing.Size(100, 23);
            this._tgaButton.TabIndex = 5;
            this._tgaButton.Text = "Browse...";
            this._tgaButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._tgaButton.UseVisualStyleBackColor = true;
            this._tgaButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // _framesToProcessLabel
            // 
            this._framesToProcessLabel.Location = new System.Drawing.Point(296, 161);
            this._framesToProcessLabel.Name = "_framesToProcessLabel";
            this._framesToProcessLabel.Size = new System.Drawing.Size(130, 23);
            this._framesToProcessLabel.TabIndex = 14;
            this._framesToProcessLabel.Text = "Frames to process:";
            this._framesToProcessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _aviTextBox
            // 
            this._aviTextBox.AllowDrop = true;
            this._aviTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._aviTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._aviTextBox.Location = new System.Drawing.Point(148, 74);
            this._aviTextBox.Name = "_aviTextBox";
            this._aviTextBox.Size = new System.Drawing.Size(318, 23);
            this._aviTextBox.TabIndex = 4;
            // 
            // _frameBlendingFactorLabel
            // 
            this._frameBlendingFactorLabel.Location = new System.Drawing.Point(296, 132);
            this._frameBlendingFactorLabel.Name = "_frameBlendingFactorLabel";
            this._frameBlendingFactorLabel.Size = new System.Drawing.Size(130, 23);
            this._frameBlendingFactorLabel.TabIndex = 12;
            this._frameBlendingFactorLabel.Text = "Frame blending factor:";
            this._frameBlendingFactorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _frameRateLabel
            // 
            this._frameRateLabel.Location = new System.Drawing.Point(12, 132);
            this._frameRateLabel.Name = "_frameRateLabel";
            this._frameRateLabel.Size = new System.Drawing.Size(130, 23);
            this._frameRateLabel.TabIndex = 10;
            this._frameRateLabel.Text = "Frame rate:";
            this._frameRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _frameRateNumericUpDown
            // 
            this._frameRateNumericUpDown.Location = new System.Drawing.Point(148, 132);
            this._frameRateNumericUpDown.Name = "_frameRateNumericUpDown";
            this._frameRateNumericUpDown.Size = new System.Drawing.Size(140, 23);
            this._frameRateNumericUpDown.TabIndex = 11;
            this._frameRateNumericUpDown.ValueChanged += new System.EventHandler(this.FrameRateNumericUpDown_ValueChanged);
            // 
            // _compressorButton
            // 
            this._compressorButton.Location = new System.Drawing.Point(472, 103);
            this._compressorButton.Name = "_compressorButton";
            this._compressorButton.Size = new System.Drawing.Size(100, 23);
            this._compressorButton.TabIndex = 9;
            this._compressorButton.Text = "Change...";
            this._compressorButton.UseVisualStyleBackColor = true;
            this._compressorButton.Click += new System.EventHandler(this.CompressorButton_Click);
            // 
            // _frameBlendingFactorNumericUpDown
            // 
            this._frameBlendingFactorNumericUpDown.Location = new System.Drawing.Point(432, 132);
            this._frameBlendingFactorNumericUpDown.Name = "_frameBlendingFactorNumericUpDown";
            this._frameBlendingFactorNumericUpDown.Size = new System.Drawing.Size(140, 23);
            this._frameBlendingFactorNumericUpDown.TabIndex = 13;
            this._frameBlendingFactorNumericUpDown.ValueChanged += new System.EventHandler(this.FrameBlendingFactorNumericUpDown_ValueChanged);
            // 
            // _compressorTextBox
            // 
            this._compressorTextBox.Location = new System.Drawing.Point(148, 103);
            this._compressorTextBox.Name = "_compressorTextBox";
            this._compressorTextBox.ReadOnly = true;
            this._compressorTextBox.Size = new System.Drawing.Size(318, 23);
            this._compressorTextBox.TabIndex = 8;
            this._compressorTextBox.TabStop = false;
            // 
            // _framesToProcessNumericUpDown
            // 
            this._framesToProcessNumericUpDown.Location = new System.Drawing.Point(432, 161);
            this._framesToProcessNumericUpDown.Name = "_framesToProcessNumericUpDown";
            this._framesToProcessNumericUpDown.Size = new System.Drawing.Size(140, 23);
            this._framesToProcessNumericUpDown.TabIndex = 15;
            // 
            // _aviButton
            // 
            this._aviButton.Location = new System.Drawing.Point(472, 74);
            this._aviButton.Name = "_aviButton";
            this._aviButton.Size = new System.Drawing.Size(100, 23);
            this._aviButton.TabIndex = 6;
            this._aviButton.Text = "Browse...";
            this._aviButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._aviButton.UseVisualStyleBackColor = true;
            this._aviButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // RecordingSettingsForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 298);
            this.Controls.Add(this._recordingNotificationsCheckBox);
            this.Controls.Add(this._deleteOnCloseCheckBox);
            this.Controls.Add(this._instructionTextLabel);
            this.Controls.Add(this._options);
            this.Controls.Add(this._hostFramerateLabel);
            this.Controls.Add(this._compressorLabel);
            this.Controls.Add(this._hostFramerateNumericUpDown);
            this.Controls.Add(this._footerPanel);
            this.Controls.Add(this._aviLabel);
            this.Controls.Add(this._tgaLabel);
            this.Controls.Add(this._tgaTextBox);
            this.Controls.Add(this._tgaButton);
            this.Controls.Add(this._framesToProcessLabel);
            this.Controls.Add(this._aviTextBox);
            this.Controls.Add(this._frameBlendingFactorLabel);
            this.Controls.Add(this._frameRateLabel);
            this.Controls.Add(this._frameRateNumericUpDown);
            this.Controls.Add(this._compressorButton);
            this.Controls.Add(this._frameBlendingFactorNumericUpDown);
            this.Controls.Add(this._compressorTextBox);
            this.Controls.Add(this._framesToProcessNumericUpDown);
            this.Controls.Add(this._aviButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecordingSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Recording settings";
            ((System.ComponentModel.ISupportInitialize)(this._hostFramerateNumericUpDown)).EndInit();
            this._footerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._frameRateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._frameBlendingFactorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._framesToProcessNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _recordingNotificationsCheckBox;
        private System.Windows.Forms.CheckBox _deleteOnCloseCheckBox;
        private System.Windows.Forms.Label _instructionTextLabel;
        private System.Windows.Forms.Label _options;
        private System.Windows.Forms.Label _hostFramerateLabel;
        private System.Windows.Forms.Label _compressorLabel;
        private System.Windows.Forms.NumericUpDown _hostFramerateNumericUpDown;
        private System.Windows.Forms.Panel _footerPanel;
        private System.Windows.Forms.LinkLabel _resetLinkLabel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Label _aviLabel;
        private System.Windows.Forms.Label _tgaLabel;
        private System.Windows.Forms.TextBox _tgaTextBox;
        private System.Windows.Forms.Button _tgaButton;
        private System.Windows.Forms.Label _framesToProcessLabel;
        private System.Windows.Forms.TextBox _aviTextBox;
        private System.Windows.Forms.Label _frameBlendingFactorLabel;
        private System.Windows.Forms.Label _frameRateLabel;
        private System.Windows.Forms.NumericUpDown _frameRateNumericUpDown;
        private System.Windows.Forms.Button _compressorButton;
        private System.Windows.Forms.NumericUpDown _frameBlendingFactorNumericUpDown;
        private System.Windows.Forms.TextBox _compressorTextBox;
        private System.Windows.Forms.NumericUpDown _framesToProcessNumericUpDown;
        private System.Windows.Forms.Button _aviButton;
        private System.Windows.Forms.ToolTip _toolTip;
    }
}