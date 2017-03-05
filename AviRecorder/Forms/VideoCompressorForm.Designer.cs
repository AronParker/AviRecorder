namespace AviRecorder.Forms
{
    partial class VideoCompressorForm
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
            this._footerPanel = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._driverLabel = new System.Windows.Forms.Label();
            this._driverTextBox = new System.Windows.Forms.TextBox();
            this._fccLabel = new System.Windows.Forms.Label();
            this._aboutButton = new System.Windows.Forms.Button();
            this._fccTextBox = new System.Windows.Forms.TextBox();
            this._configureButton = new System.Windows.Forms.Button();
            this._nameLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._compressorListBox = new System.Windows.Forms.ListBox();
            this._instructionTextLabel = new System.Windows.Forms.Label();
            this._footerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _footerPanel
            // 
            this._footerPanel.BackColor = System.Drawing.SystemColors.Control;
            this._footerPanel.Controls.Add(this._cancelButton);
            this._footerPanel.Controls.Add(this._okButton);
            this._footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._footerPanel.Location = new System.Drawing.Point(0, 221);
            this._footerPanel.Name = "_footerPanel";
            this._footerPanel.Size = new System.Drawing.Size(584, 50);
            this._footerPanel.TabIndex = 10;
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
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(366, 13);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(100, 25);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _driverLabel
            // 
            this._driverLabel.Location = new System.Drawing.Point(260, 103);
            this._driverLabel.Name = "_driverLabel";
            this._driverLabel.Size = new System.Drawing.Size(100, 23);
            this._driverLabel.TabIndex = 6;
            this._driverLabel.Text = "Driver:";
            this._driverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _driverTextBox
            // 
            this._driverTextBox.Location = new System.Drawing.Point(366, 103);
            this._driverTextBox.Name = "_driverTextBox";
            this._driverTextBox.ReadOnly = true;
            this._driverTextBox.Size = new System.Drawing.Size(206, 23);
            this._driverTextBox.TabIndex = 7;
            // 
            // _fccLabel
            // 
            this._fccLabel.Location = new System.Drawing.Point(260, 74);
            this._fccLabel.Name = "_fccLabel";
            this._fccLabel.Size = new System.Drawing.Size(100, 23);
            this._fccLabel.TabIndex = 4;
            this._fccLabel.Text = "Fcc Handler:";
            this._fccLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _aboutButton
            // 
            this._aboutButton.Enabled = false;
            this._aboutButton.Location = new System.Drawing.Point(472, 132);
            this._aboutButton.Name = "_aboutButton";
            this._aboutButton.Size = new System.Drawing.Size(100, 25);
            this._aboutButton.TabIndex = 9;
            this._aboutButton.Text = "About...";
            this._aboutButton.UseVisualStyleBackColor = true;
            this._aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // _fccTextBox
            // 
            this._fccTextBox.Location = new System.Drawing.Point(366, 74);
            this._fccTextBox.Name = "_fccTextBox";
            this._fccTextBox.ReadOnly = true;
            this._fccTextBox.Size = new System.Drawing.Size(206, 23);
            this._fccTextBox.TabIndex = 5;
            // 
            // _configureButton
            // 
            this._configureButton.Enabled = false;
            this._configureButton.Location = new System.Drawing.Point(366, 132);
            this._configureButton.Name = "_configureButton";
            this._configureButton.Size = new System.Drawing.Size(100, 25);
            this._configureButton.TabIndex = 8;
            this._configureButton.Text = "Configure...";
            this._configureButton.UseVisualStyleBackColor = true;
            this._configureButton.Click += new System.EventHandler(this.ConfigureButton_Click);
            // 
            // _nameLabel
            // 
            this._nameLabel.Location = new System.Drawing.Point(260, 45);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(100, 23);
            this._nameLabel.TabIndex = 2;
            this._nameLabel.Text = "Name:";
            this._nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Location = new System.Drawing.Point(366, 45);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.ReadOnly = true;
            this._nameTextBox.Size = new System.Drawing.Size(206, 23);
            this._nameTextBox.TabIndex = 3;
            // 
            // _compressorListBox
            // 
            this._compressorListBox.FormattingEnabled = true;
            this._compressorListBox.ItemHeight = 15;
            this._compressorListBox.Location = new System.Drawing.Point(12, 45);
            this._compressorListBox.Name = "_compressorListBox";
            this._compressorListBox.Size = new System.Drawing.Size(242, 169);
            this._compressorListBox.TabIndex = 1;
            this._compressorListBox.SelectedIndexChanged += new System.EventHandler(this.CompressorListBox_SelectedIndexChanged);
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
            this._instructionTextLabel.Text = "Configure video codec settings";
            this._instructionTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VideoCompressorForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 271);
            this.Controls.Add(this._footerPanel);
            this.Controls.Add(this._driverLabel);
            this.Controls.Add(this._driverTextBox);
            this.Controls.Add(this._fccLabel);
            this.Controls.Add(this._aboutButton);
            this.Controls.Add(this._fccTextBox);
            this.Controls.Add(this._configureButton);
            this.Controls.Add(this._nameLabel);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this._compressorListBox);
            this.Controls.Add(this._instructionTextLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoCompressorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Video codec settings";
            this._footerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _footerPanel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Label _driverLabel;
        private System.Windows.Forms.TextBox _driverTextBox;
        private System.Windows.Forms.Label _fccLabel;
        private System.Windows.Forms.Button _aboutButton;
        private System.Windows.Forms.TextBox _fccTextBox;
        private System.Windows.Forms.Button _configureButton;
        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.ListBox _compressorListBox;
        private System.Windows.Forms.Label _instructionTextLabel;
    }
}