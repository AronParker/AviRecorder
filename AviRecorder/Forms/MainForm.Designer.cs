namespace AviRecorder.Forms
{
    partial class MainForm
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                _launcher.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._footerPanel = new System.Windows.Forms.Panel();
            this._aviLinkLabel = new System.Windows.Forms.LinkLabel();
            this._aboutButton = new System.Windows.Forms.Button();
            this._startButton = new System.Windows.Forms.Button();
            this._recordingSettingsLabel = new System.Windows.Forms.Label();
            this._recordingSettingsTextBox = new System.Windows.Forms.TextBox();
            this._recordingSettingsButton = new System.Windows.Forms.Button();
            this._lastFrameProcessedTextBox = new System.Windows.Forms.TextBox();
            this._lastFrameProcessedLabel = new System.Windows.Forms.Label();
            this._currentMovieTextBox = new System.Windows.Forms.TextBox();
            this._currentMovieLabel = new System.Windows.Forms.Label();
            this._notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._gameSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._recordingSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._openTgaDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openAviDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._gameSettingsLabel = new System.Windows.Forms.Label();
            this._gameSettingsButton = new System.Windows.Forms.Button();
            this._gameSettingsTextBox = new System.Windows.Forms.TextBox();
            this._startGameCheckBox = new System.Windows.Forms.CheckBox();
            this._footerPanel.SuspendLayout();
            this._notifyIconContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _footerPanel
            // 
            this._footerPanel.BackColor = System.Drawing.SystemColors.Control;
            this._footerPanel.Controls.Add(this._aviLinkLabel);
            this._footerPanel.Controls.Add(this._aboutButton);
            this._footerPanel.Controls.Add(this._startButton);
            this._footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._footerPanel.Location = new System.Drawing.Point(0, 161);
            this._footerPanel.Name = "_footerPanel";
            this._footerPanel.Size = new System.Drawing.Size(509, 50);
            this._footerPanel.TabIndex = 11;
            // 
            // _aviLinkLabel
            // 
            this._aviLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._aviLinkLabel.Location = new System.Drawing.Point(12, 12);
            this._aviLinkLabel.Name = "_aviLinkLabel";
            this._aviLinkLabel.Size = new System.Drawing.Size(223, 26);
            this._aviLinkLabel.TabIndex = 2;
            this._aviLinkLabel.TabStop = true;
            this._aviLinkLabel.Text = "Open AVI folder";
            this._aviLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._aviLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AviLinkLabel_LinkClicked);
            // 
            // _aboutButton
            // 
            this._aboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._aboutButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._aboutButton.Image = global::AviRecorder.Properties.Resources.information;
            this._aboutButton.Location = new System.Drawing.Point(397, 12);
            this._aboutButton.Name = "_aboutButton";
            this._aboutButton.Size = new System.Drawing.Size(100, 26);
            this._aboutButton.TabIndex = 1;
            this._aboutButton.Text = "About";
            this._aboutButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._aboutButton.UseVisualStyleBackColor = true;
            this._aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // _startButton
            // 
            this._startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._startButton.Location = new System.Drawing.Point(241, 12);
            this._startButton.Name = "_startButton";
            this._startButton.Size = new System.Drawing.Size(150, 26);
            this._startButton.TabIndex = 0;
            this._startButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this.StartButton_ClickAsync);
            // 
            // _recordingSettingsLabel
            // 
            this._recordingSettingsLabel.Location = new System.Drawing.Point(12, 44);
            this._recordingSettingsLabel.Name = "_recordingSettingsLabel";
            this._recordingSettingsLabel.Size = new System.Drawing.Size(125, 23);
            this._recordingSettingsLabel.TabIndex = 3;
            this._recordingSettingsLabel.Text = "Recording settings:";
            this._recordingSettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _recordingSettingsTextBox
            // 
            this._recordingSettingsTextBox.Location = new System.Drawing.Point(143, 44);
            this._recordingSettingsTextBox.Name = "_recordingSettingsTextBox";
            this._recordingSettingsTextBox.ReadOnly = true;
            this._recordingSettingsTextBox.Size = new System.Drawing.Size(248, 23);
            this._recordingSettingsTextBox.TabIndex = 4;
            // 
            // _recordingSettingsButton
            // 
            this._recordingSettingsButton.Image = global::AviRecorder.Properties.Resources.cog;
            this._recordingSettingsButton.Location = new System.Drawing.Point(397, 43);
            this._recordingSettingsButton.Name = "_recordingSettingsButton";
            this._recordingSettingsButton.Size = new System.Drawing.Size(100, 25);
            this._recordingSettingsButton.TabIndex = 5;
            this._recordingSettingsButton.Text = "Change...";
            this._recordingSettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._recordingSettingsButton.UseVisualStyleBackColor = true;
            this._recordingSettingsButton.Click += new System.EventHandler(this.RecordingSettingsButton_Click);
            // 
            // _lastFrameProcessedTextBox
            // 
            this._lastFrameProcessedTextBox.Location = new System.Drawing.Point(143, 103);
            this._lastFrameProcessedTextBox.Name = "_lastFrameProcessedTextBox";
            this._lastFrameProcessedTextBox.ReadOnly = true;
            this._lastFrameProcessedTextBox.Size = new System.Drawing.Size(354, 23);
            this._lastFrameProcessedTextBox.TabIndex = 9;
            this._lastFrameProcessedTextBox.Text = "-";
            // 
            // _lastFrameProcessedLabel
            // 
            this._lastFrameProcessedLabel.Location = new System.Drawing.Point(12, 103);
            this._lastFrameProcessedLabel.Name = "_lastFrameProcessedLabel";
            this._lastFrameProcessedLabel.Size = new System.Drawing.Size(125, 23);
            this._lastFrameProcessedLabel.TabIndex = 8;
            this._lastFrameProcessedLabel.Text = "Last frame processed:";
            this._lastFrameProcessedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _currentMovieTextBox
            // 
            this._currentMovieTextBox.Location = new System.Drawing.Point(143, 74);
            this._currentMovieTextBox.Name = "_currentMovieTextBox";
            this._currentMovieTextBox.ReadOnly = true;
            this._currentMovieTextBox.Size = new System.Drawing.Size(354, 23);
            this._currentMovieTextBox.TabIndex = 7;
            this._currentMovieTextBox.Text = "-";
            // 
            // _currentMovieLabel
            // 
            this._currentMovieLabel.Location = new System.Drawing.Point(12, 74);
            this._currentMovieLabel.Name = "_currentMovieLabel";
            this._currentMovieLabel.Size = new System.Drawing.Size(125, 23);
            this._currentMovieLabel.TabIndex = 6;
            this._currentMovieLabel.Text = "Current movie:";
            this._currentMovieLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _notifyIcon
            // 
            this._notifyIcon.ContextMenuStrip = this._notifyIconContextMenuStrip;
            this._notifyIcon.Text = "AVI Recorder";
            this._notifyIcon.Visible = true;
            this._notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // _notifyIconContextMenuStrip
            // 
            this._notifyIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._startToolStripMenuItem,
            this._toolStripSeparator1,
            this._gameSettingsToolStripMenuItem,
            this._recordingSettingsToolStripMenuItem,
            this._toolStripSeparator2,
            this._openTgaDirectoryToolStripMenuItem,
            this._openAviDirectoryToolStripMenuItem,
            this._toolStripSeparator3,
            this._exitToolStripMenuItem});
            this._notifyIconContextMenuStrip.Name = "_notifyIconContextMenuStrip";
            this._notifyIconContextMenuStrip.Size = new System.Drawing.Size(223, 154);
            // 
            // _startToolStripMenuItem
            // 
            this._startToolStripMenuItem.Name = "_startToolStripMenuItem";
            this._startToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._startToolStripMenuItem.Click += new System.EventHandler(this.StartButton_ClickAsync);
            // 
            // _toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "_toolStripSeparator1";
            this._toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // _gameSettingsToolStripMenuItem
            // 
            this._gameSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_gameSettingsToolStripMenuItem.Image")));
            this._gameSettingsToolStripMenuItem.Name = "_gameSettingsToolStripMenuItem";
            this._gameSettingsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._gameSettingsToolStripMenuItem.Text = "Change game settings...";
            this._gameSettingsToolStripMenuItem.Click += new System.EventHandler(this.GameSettingsButton_Click);
            // 
            // _recordingSettingsToolStripMenuItem
            // 
            this._recordingSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_recordingSettingsToolStripMenuItem.Image")));
            this._recordingSettingsToolStripMenuItem.Name = "_recordingSettingsToolStripMenuItem";
            this._recordingSettingsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._recordingSettingsToolStripMenuItem.Text = "Change recording settings...";
            this._recordingSettingsToolStripMenuItem.Click += new System.EventHandler(this.RecordingSettingsButton_Click);
            // 
            // _toolStripSeparator2
            // 
            this._toolStripSeparator2.Name = "_toolStripSeparator2";
            this._toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // _openTgaDirectoryToolStripMenuItem
            // 
            this._openTgaDirectoryToolStripMenuItem.Image = global::AviRecorder.Properties.Resources.folder;
            this._openTgaDirectoryToolStripMenuItem.Name = "_openTgaDirectoryToolStripMenuItem";
            this._openTgaDirectoryToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._openTgaDirectoryToolStripMenuItem.Text = "Open TGA folder";
            this._openTgaDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenTgaDirectoryToolStripMenuItem_Click);
            // 
            // _openAviDirectoryToolStripMenuItem
            // 
            this._openAviDirectoryToolStripMenuItem.Image = global::AviRecorder.Properties.Resources.folder;
            this._openAviDirectoryToolStripMenuItem.Name = "_openAviDirectoryToolStripMenuItem";
            this._openAviDirectoryToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._openAviDirectoryToolStripMenuItem.Text = "Open AVI folder";
            this._openAviDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenAviDirectoryToolStripMenuItem_Click);
            // 
            // _toolStripSeparator3
            // 
            this._toolStripSeparator3.Name = "_toolStripSeparator3";
            this._toolStripSeparator3.Size = new System.Drawing.Size(219, 6);
            // 
            // _exitToolStripMenuItem
            // 
            this._exitToolStripMenuItem.Image = global::AviRecorder.Properties.Resources.door_in;
            this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
            this._exitToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this._exitToolStripMenuItem.Text = "Exit";
            this._exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // _gameSettingsLabel
            // 
            this._gameSettingsLabel.Location = new System.Drawing.Point(12, 13);
            this._gameSettingsLabel.Name = "_gameSettingsLabel";
            this._gameSettingsLabel.Size = new System.Drawing.Size(125, 23);
            this._gameSettingsLabel.TabIndex = 0;
            this._gameSettingsLabel.Text = "Game settings:";
            this._gameSettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _gameSettingsButton
            // 
            this._gameSettingsButton.Image = global::AviRecorder.Properties.Resources.cog;
            this._gameSettingsButton.Location = new System.Drawing.Point(397, 12);
            this._gameSettingsButton.Name = "_gameSettingsButton";
            this._gameSettingsButton.Size = new System.Drawing.Size(100, 25);
            this._gameSettingsButton.TabIndex = 2;
            this._gameSettingsButton.Text = "Change...";
            this._gameSettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._gameSettingsButton.UseVisualStyleBackColor = true;
            this._gameSettingsButton.Click += new System.EventHandler(this.GameSettingsButton_Click);
            // 
            // _gameSettingsTextBox
            // 
            this._gameSettingsTextBox.Location = new System.Drawing.Point(143, 13);
            this._gameSettingsTextBox.Name = "_gameSettingsTextBox";
            this._gameSettingsTextBox.ReadOnly = true;
            this._gameSettingsTextBox.Size = new System.Drawing.Size(248, 23);
            this._gameSettingsTextBox.TabIndex = 1;
            // 
            // _startGameCheckBox
            // 
            this._startGameCheckBox.Location = new System.Drawing.Point(143, 132);
            this._startGameCheckBox.Name = "_startGameCheckBox";
            this._startGameCheckBox.Size = new System.Drawing.Size(354, 23);
            this._startGameCheckBox.TabIndex = 10;
            this._startGameCheckBox.Text = "Start game and create recording configs";
            this._startGameCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(509, 211);
            this.Controls.Add(this._startGameCheckBox);
            this.Controls.Add(this._lastFrameProcessedTextBox);
            this.Controls.Add(this._lastFrameProcessedLabel);
            this.Controls.Add(this._currentMovieTextBox);
            this.Controls.Add(this._currentMovieLabel);
            this.Controls.Add(this._gameSettingsTextBox);
            this.Controls.Add(this._recordingSettingsTextBox);
            this.Controls.Add(this._gameSettingsButton);
            this.Controls.Add(this._recordingSettingsButton);
            this.Controls.Add(this._gameSettingsLabel);
            this.Controls.Add(this._footerPanel);
            this.Controls.Add(this._recordingSettingsLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "AVI Recorder";
            this._footerPanel.ResumeLayout(false);
            this._notifyIconContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel _footerPanel;
        private System.Windows.Forms.Button _aboutButton;
        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.Label _recordingSettingsLabel;
        private System.Windows.Forms.TextBox _recordingSettingsTextBox;
        private System.Windows.Forms.Button _recordingSettingsButton;
        private System.Windows.Forms.TextBox _lastFrameProcessedTextBox;
        private System.Windows.Forms.Label _lastFrameProcessedLabel;
        private System.Windows.Forms.TextBox _currentMovieTextBox;
        private System.Windows.Forms.Label _currentMovieLabel;
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.Label _gameSettingsLabel;
        private System.Windows.Forms.Button _gameSettingsButton;
        private System.Windows.Forms.TextBox _gameSettingsTextBox;
        private System.Windows.Forms.LinkLabel _aviLinkLabel;
        private System.Windows.Forms.CheckBox _startGameCheckBox;
        private System.Windows.Forms.ContextMenuStrip _notifyIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _openTgaDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _openAviDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _gameSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _recordingSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator3;
    }
}