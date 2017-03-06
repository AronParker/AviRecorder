using System;
using System.IO;
using System.Windows.Forms;
using AviRecorder.Controller;
using AviRecorder.Video.Compression;
using AviRecorder.Video.TgaSequences;

namespace AviRecorder.Forms
{
    public partial class RecordingSettingsForm : Form
    {
        public RecordingSettingsForm()
        {
            InitializeComponent();

            _compressorTextBox.Text = CompressorSettings.NoCompressor;
            _frameRateNumericUpDown.Minimum = 1;
            _frameRateNumericUpDown.Maximum = TgaSequenceConverter.MaxSupportedFrameRate;
            _hostFramerateNumericUpDown.Minimum = 1;
            _hostFramerateNumericUpDown.Maximum = (decimal)TgaSequenceConverter.MaxSupportedFrameRate * int.MaxValue;
            _frameBlendingFactorNumericUpDown.Minimum = 1;
            _frameBlendingFactorNumericUpDown.Maximum = int.MaxValue;
            _framesToProcessNumericUpDown.Minimum = 1;
            _framesToProcessNumericUpDown.Maximum = _framesToProcessNumericUpDown.Value;
        }

        public string TgaDirectory
        {
            get => _tgaTextBox.Text;
            set => _tgaTextBox.Text = value;
        }

        public string AviDirectory
        {
            get => _aviTextBox.Text;
            set => _aviTextBox.Text = value;
        }

        public uint? CompressorFcc { get; private set; }

        public string CompressorName
        {
            get
            {
                if (CompressorFcc == null)
                    return null;

                return _compressorTextBox.Text;
            }
            private set
            {
                if (value == null)
                    value = CompressorSettings.NoCompressor;

                _compressorTextBox.Text = value;
            }
        }

        public byte[] CompressorState { get; private set; }

        public int FrameRate
        {
            get => (int)_frameRateNumericUpDown.Value;
            set => _frameRateNumericUpDown.Value = value;
        }

        public int FrameBlendingFactor
        {
            get => (int)_frameBlendingFactorNumericUpDown.Value;
            set => _frameBlendingFactorNumericUpDown.Value = value;
        }

        public int FramesToProcess
        {
            get => (int)_framesToProcessNumericUpDown.Value;
            set => _framesToProcessNumericUpDown.Value = value;
        }

        public bool DeleteOnClose
        {
            get => _deleteOnCloseCheckBox.Checked;
            set => _deleteOnCloseCheckBox.Checked = value;
        }

        public bool RecordingNotifications
        {
            get => _recordingNotificationsCheckBox.Checked;
            set => _recordingNotificationsCheckBox.Checked = value;
        }
        
        public void ChangeCompressor(CompressorSettings compressor)
        {
            CompressorFcc = compressor.Fcc;
            CompressorName = compressor.Name;
            CompressorState = compressor.State;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (sender == _tgaButton)
                {
                    folderBrowserDialog.Description = "Please select the TGA folder.";
                    folderBrowserDialog.SelectedPath = _tgaTextBox.Text;
                }
                else if (sender == _aviButton)
                {
                    folderBrowserDialog.Description = "Please select the AVI folder.";
                    folderBrowserDialog.SelectedPath = _aviTextBox.Text;
                }

                if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                if (sender == _tgaButton)
                    _tgaTextBox.Text = folderBrowserDialog.SelectedPath;
                else if (sender == _aviButton)
                    _aviTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void CompressorButton_Click(object sender, EventArgs e)
        {
            using (var form = new VideoCompressorForm())
            {
                try
                {
                    if (CompressorFcc != null)
                    {
                        var compressor = VideoCompressor.Open(CompressorFcc.Value);

                        if (CompressorState != null)
                        {
                            try
                            {
                                compressor.SetState(CompressorState);
                            }
                            catch (VideoCompressorException ex)
                            {
                                MessageBox.Show(ex.Message, "Failed to set video compressor state", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        form.Compressor = compressor;
                    }
                }
                catch (VideoCompressorException ex)
                {
                    MessageBox.Show(ex.Message,
                                    "Failed to load video compressor",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CompressorFcc = form.Compressor?.Info.FccHandler;
                    CompressorName = form.Compressor?.Info.Description;

                    try
                    {
                        CompressorState = form.Compressor?.GetState();
                    }
                    catch (VideoCompressorException ex)
                    {
                        MessageBox.Show(ex.Message, "Failed to get video compressor state", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                form.Compressor?.Dispose();
            }
        }

        private void FrameRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _hostFramerateNumericUpDown.Value = _frameRateNumericUpDown.Value * _frameBlendingFactorNumericUpDown.Value;
        }

        private void FrameBlendingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _hostFramerateNumericUpDown.Value = _frameRateNumericUpDown.Value * _frameBlendingFactorNumericUpDown.Value;
            _framesToProcessNumericUpDown.Maximum = _frameBlendingFactorNumericUpDown.Value;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_tgaTextBox.Text))
            {
                MessageBox.Show("The TGA folder you selected does not exist.",
                                "TGA folder does not exist",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            if (!Directory.Exists(_aviTextBox.Text))
            {
                MessageBox.Show("The AVI folder you selected does not exist.",
                                "AVI folder does not exist",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            if (FramesToProcess == 1 && FrameBlendingFactor > 1)
            {
                var result = MessageBox.Show("There's no purpose in having a frame blending factor higher than one while only processing one frame, it will dramatically reduce performance for no use. Do you want to change the frame blending factor to one?",
                                             "Performance Warning",
                                             MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        FrameBlendingFactor = 1;
                        break;
                    case DialogResult.No:
                        break;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void ResetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            CompressorFcc = null;
            CompressorName = null;
            CompressorState = null;
            FrameRate = ApplicationSettings.DefaultFrameRate;
            FrameBlendingFactor = ApplicationSettings.DefaultFrameBlendingFactor;
            FramesToProcess = ApplicationSettings.DefaultFramesToProcess;
            DeleteOnClose = ApplicationSettings.DefaultDeleteOnClose;
            RecordingNotifications = ApplicationSettings.DefaultRecordingNotifications;
        }
    }
}
