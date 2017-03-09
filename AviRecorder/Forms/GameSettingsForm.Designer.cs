namespace AviRecorder.Forms
{
    partial class GameSettingsForm
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
            this._instructionTextLabel = new System.Windows.Forms.Label();
            this._customCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this._customContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._launchOptionsTextBox = new System.Windows.Forms.TextBox();
            this._customLabel = new System.Windows.Forms.Label();
            this._userComboBox = new System.Windows.Forms.ComboBox();
            this._launchOptionsLabel = new System.Windows.Forms.Label();
            this._userLabel = new System.Windows.Forms.Label();
            this._gameComboBox = new System.Windows.Forms.ComboBox();
            this._gameLabel = new System.Windows.Forms.Label();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._importLinkLabel = new System.Windows.Forms.LinkLabel();
            this._footerPanel = new System.Windows.Forms.Panel();
            this._endmovieCommandsLabel = new System.Windows.Forms.Label();
            this._startmovieCommandsLabel = new System.Windows.Forms.Label();
            this._startmovieCommandsTextBox = new System.Windows.Forms.TextBox();
            this._endmovieCommandsTextBox = new System.Windows.Forms.TextBox();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._customContextMenuStrip.SuspendLayout();
            this._footerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _instructionTextLabel
            // 
            this._instructionTextLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._instructionTextLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._instructionTextLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this._instructionTextLabel.Location = new System.Drawing.Point(0, 0);
            this._instructionTextLabel.Name = "_instructionTextLabel";
            this._instructionTextLabel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this._instructionTextLabel.Size = new System.Drawing.Size(473, 42);
            this._instructionTextLabel.TabIndex = 0;
            this._instructionTextLabel.Text = "Configure game settings";
            this._instructionTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _customCheckedListBox
            // 
            this._customCheckedListBox.CheckOnClick = true;
            this._customCheckedListBox.ContextMenuStrip = this._customContextMenuStrip;
            this._customCheckedListBox.FormattingEnabled = true;
            this._customCheckedListBox.Location = new System.Drawing.Point(148, 132);
            this._customCheckedListBox.Name = "_customCheckedListBox";
            this._customCheckedListBox.Size = new System.Drawing.Size(318, 148);
            this._customCheckedListBox.TabIndex = 8;
            // 
            // _customContextMenuStrip
            // 
            this._customContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._selectAllToolStripMenuItem,
            this._selectNoneToolStripMenuItem,
            this._invertSelectionToolStripMenuItem});
            this._customContextMenuStrip.Name = "_customContextMenuStrip";
            this._customContextMenuStrip.Size = new System.Drawing.Size(155, 70);
            // 
            // _selectAllToolStripMenuItem
            // 
            this._selectAllToolStripMenuItem.Name = "_selectAllToolStripMenuItem";
            this._selectAllToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this._selectAllToolStripMenuItem.Text = "Select all";
            this._selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectToolStripMenuItem_Click);
            // 
            // _selectNoneToolStripMenuItem
            // 
            this._selectNoneToolStripMenuItem.Name = "_selectNoneToolStripMenuItem";
            this._selectNoneToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this._selectNoneToolStripMenuItem.Text = "Select none";
            this._selectNoneToolStripMenuItem.Click += new System.EventHandler(this.SelectToolStripMenuItem_Click);
            // 
            // _invertSelectionToolStripMenuItem
            // 
            this._invertSelectionToolStripMenuItem.Name = "_invertSelectionToolStripMenuItem";
            this._invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this._invertSelectionToolStripMenuItem.Text = "Invert selection";
            this._invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.InvertSelectionToolStripMenuItem_Click);
            // 
            // _launchOptionsTextBox
            // 
            this._launchOptionsTextBox.Location = new System.Drawing.Point(148, 103);
            this._launchOptionsTextBox.Name = "_launchOptionsTextBox";
            this._launchOptionsTextBox.Size = new System.Drawing.Size(318, 23);
            this._launchOptionsTextBox.TabIndex = 6;
            // 
            // _customLabel
            // 
            this._customLabel.Location = new System.Drawing.Point(12, 132);
            this._customLabel.Name = "_customLabel";
            this._customLabel.Size = new System.Drawing.Size(130, 23);
            this._customLabel.TabIndex = 7;
            this._customLabel.Text = "Customization:";
            this._customLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _userComboBox
            // 
            this._userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._userComboBox.FormattingEnabled = true;
            this._userComboBox.Location = new System.Drawing.Point(148, 74);
            this._userComboBox.Name = "_userComboBox";
            this._userComboBox.Size = new System.Drawing.Size(318, 23);
            this._userComboBox.TabIndex = 4;
            // 
            // _launchOptionsLabel
            // 
            this._launchOptionsLabel.Location = new System.Drawing.Point(12, 103);
            this._launchOptionsLabel.Name = "_launchOptionsLabel";
            this._launchOptionsLabel.Size = new System.Drawing.Size(130, 23);
            this._launchOptionsLabel.TabIndex = 5;
            this._launchOptionsLabel.Text = "Launch options:";
            this._launchOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _userLabel
            // 
            this._userLabel.Location = new System.Drawing.Point(12, 74);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Size = new System.Drawing.Size(130, 23);
            this._userLabel.TabIndex = 3;
            this._userLabel.Text = "User:";
            this._userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _gameComboBox
            // 
            this._gameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._gameComboBox.FormattingEnabled = true;
            this._gameComboBox.Location = new System.Drawing.Point(148, 45);
            this._gameComboBox.Name = "_gameComboBox";
            this._gameComboBox.Size = new System.Drawing.Size(318, 23);
            this._gameComboBox.TabIndex = 2;
            this._gameComboBox.SelectedIndexChanged += new System.EventHandler(this.GameComboBox_SelectedIndexChanged);
            // 
            // _gameLabel
            // 
            this._gameLabel.Location = new System.Drawing.Point(12, 45);
            this._gameLabel.Name = "_gameLabel";
            this._gameLabel.Size = new System.Drawing.Size(130, 23);
            this._gameLabel.TabIndex = 1;
            this._gameLabel.Text = "Game:";
            this._gameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.Location = new System.Drawing.Point(255, 13);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(100, 25);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(361, 13);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 25);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _importLinkLabel
            // 
            this._importLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._importLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._importLinkLabel.Location = new System.Drawing.Point(12, 13);
            this._importLinkLabel.Name = "_importLinkLabel";
            this._importLinkLabel.Size = new System.Drawing.Size(237, 25);
            this._importLinkLabel.TabIndex = 2;
            this._importLinkLabel.TabStop = true;
            this._importLinkLabel.Text = "Import settings";
            this._importLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._importLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ImportLinkLabel_LinkClicked);
            // 
            // _footerPanel
            // 
            this._footerPanel.BackColor = System.Drawing.SystemColors.Control;
            this._footerPanel.Controls.Add(this._importLinkLabel);
            this._footerPanel.Controls.Add(this._cancelButton);
            this._footerPanel.Controls.Add(this._okButton);
            this._footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._footerPanel.Location = new System.Drawing.Point(0, 344);
            this._footerPanel.Name = "_footerPanel";
            this._footerPanel.Size = new System.Drawing.Size(473, 50);
            this._footerPanel.TabIndex = 13;
            // 
            // _endmovieCommandsLabel
            // 
            this._endmovieCommandsLabel.Location = new System.Drawing.Point(12, 315);
            this._endmovieCommandsLabel.Name = "_endmovieCommandsLabel";
            this._endmovieCommandsLabel.Size = new System.Drawing.Size(130, 23);
            this._endmovieCommandsLabel.TabIndex = 11;
            this._endmovieCommandsLabel.Text = "Endmovie commands:";
            this._endmovieCommandsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _startmovieCommandsLabel
            // 
            this._startmovieCommandsLabel.Location = new System.Drawing.Point(12, 286);
            this._startmovieCommandsLabel.Name = "_startmovieCommandsLabel";
            this._startmovieCommandsLabel.Size = new System.Drawing.Size(130, 23);
            this._startmovieCommandsLabel.TabIndex = 9;
            this._startmovieCommandsLabel.Text = "Startmovie commands:";
            this._startmovieCommandsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _startmovieCommandsTextBox
            // 
            this._startmovieCommandsTextBox.AllowDrop = true;
            this._startmovieCommandsTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._startmovieCommandsTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._startmovieCommandsTextBox.Location = new System.Drawing.Point(148, 286);
            this._startmovieCommandsTextBox.Name = "_startmovieCommandsTextBox";
            this._startmovieCommandsTextBox.Size = new System.Drawing.Size(318, 23);
            this._startmovieCommandsTextBox.TabIndex = 10;
            // 
            // _endmovieCommandsTextBox
            // 
            this._endmovieCommandsTextBox.AllowDrop = true;
            this._endmovieCommandsTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._endmovieCommandsTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._endmovieCommandsTextBox.Location = new System.Drawing.Point(148, 315);
            this._endmovieCommandsTextBox.Name = "_endmovieCommandsTextBox";
            this._endmovieCommandsTextBox.Size = new System.Drawing.Size(318, 23);
            this._endmovieCommandsTextBox.TabIndex = 12;
            // 
            // GameSettingsForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(473, 394);
            this.Controls.Add(this._endmovieCommandsLabel);
            this.Controls.Add(this._startmovieCommandsLabel);
            this.Controls.Add(this._startmovieCommandsTextBox);
            this.Controls.Add(this._endmovieCommandsTextBox);
            this.Controls.Add(this._customCheckedListBox);
            this.Controls.Add(this._launchOptionsTextBox);
            this.Controls.Add(this._customLabel);
            this.Controls.Add(this._userComboBox);
            this.Controls.Add(this._launchOptionsLabel);
            this.Controls.Add(this._userLabel);
            this.Controls.Add(this._gameComboBox);
            this.Controls.Add(this._gameLabel);
            this.Controls.Add(this._footerPanel);
            this.Controls.Add(this._instructionTextLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Game settings";
            this._customContextMenuStrip.ResumeLayout(false);
            this._footerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _instructionTextLabel;
        private System.Windows.Forms.CheckedListBox _customCheckedListBox;
        private System.Windows.Forms.TextBox _launchOptionsTextBox;
        private System.Windows.Forms.Label _customLabel;
        private System.Windows.Forms.ComboBox _userComboBox;
        private System.Windows.Forms.Label _launchOptionsLabel;
        private System.Windows.Forms.Label _userLabel;
        private System.Windows.Forms.ComboBox _gameComboBox;
        private System.Windows.Forms.Label _gameLabel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.LinkLabel _importLinkLabel;
        private System.Windows.Forms.Panel _footerPanel;
        private System.Windows.Forms.ContextMenuStrip _customContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _invertSelectionToolStripMenuItem;
        private System.Windows.Forms.Label _endmovieCommandsLabel;
        private System.Windows.Forms.Label _startmovieCommandsLabel;
        private System.Windows.Forms.TextBox _startmovieCommandsTextBox;
        private System.Windows.Forms.TextBox _endmovieCommandsTextBox;
        private System.Windows.Forms.ToolTip _toolTip;
    }
}