using System;
using System.Windows.Forms;
using AviRecorder.Controller;
using AviRecorder.Video.Avi;
using AviRecorder.Video.Compression;

namespace AviRecorder.Forms
{
    public partial class VideoCompressorForm : Form
    {
        private VideoCompressorInfo[] _compressors;
        private VideoCompressor _compressor;

        private bool _suspendUpdate;

        public VideoCompressorForm()
        {
            InitializeComponent();
            InitializeCompressors();
        }

        public VideoCompressor Compressor
        {
            get => _compressor;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                var index = FindIndex(value.Info.FccHandler);

                if (index == -1)
                    throw new VideoCompressorException("Failed to locate the given video compressor.");

                _suspendUpdate = true;
                _compressorListBox.SelectedIndex = index;
                _suspendUpdate = false;

                ChangeCompressor(value);
                DisplayCompressor(value);
            }
        }

        private void InitializeCompressors()
        {
            _compressorListBox.Items.Add(CompressorSettings.NoCompressor);

            foreach (var item in _compressors = VideoCompressorInfo.GetCompressorInfos())
                _compressorListBox.Items.Add(item.Description);

            _compressorListBox.SelectedIndex = 0;
        }

        private int FindIndex(uint? fcc)
        {
            if (fcc == null)
                return 0;

            for (var i = 0; i < _compressors.Length; i++)
                if (fcc == _compressors[i].FccHandler)
                    return i + 1;

            return -1;
        }

        private void ChangeCompressor(VideoCompressor compressor)
        {
            _compressor?.Dispose();
            _compressor = compressor;
        }

        private void DisplayCompressor(VideoCompressor compressor)
        {
            if (compressor == null)
            {
                _nameTextBox.Text = CompressorSettings.NoCompressor;
                _fccTextBox.Text = "DIB ";
                _driverTextBox.Text = "None";

                _configureButton.Enabled = false;
                _aboutButton.Enabled = false;
            }
            else
            {
                _nameTextBox.Text = compressor.Info.Description;
                _fccTextBox.Text = FourCC.ToString(compressor.Info.FccHandler);
                _driverTextBox.Text = compressor.Info.Driver;

                _configureButton.Enabled = compressor.HasConfigureDialog;
                _aboutButton.Enabled = compressor.HasAboutDialog;
            }
        }

        private void CompressorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspendUpdate)
                return;

            var index = _compressorListBox.SelectedIndex;

            if (index == 0)
            {
                ChangeCompressor(null);
                DisplayCompressor(null);
                return;
            }

            var compressor = VideoCompressor.TryOpen(_compressors[index - 1]);

            if (compressor == null)
            {
                MessageBox.Show($"Failed to open video compressor \"{_compressors[index - 1].Description}\".",
                                "Failed to open compressor.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                _compressorListBox.SelectedIndex = 0;
                return;
            }

            ChangeCompressor(compressor);
            DisplayCompressor(compressor);
        }

        private void ConfigureButton_Click(object sender, EventArgs e)
        {
            try
            {
                _compressor.ShowConfigureDialog(this);
            }
            catch (VideoCompressorException ex)
            {
                MessageBox.Show(ex.Message, "Failed to open configure dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            try
            {
                _compressor.ShowAboutDialog(this);
            }
            catch (VideoCompressorException ex)
            {
                MessageBox.Show(ex.Message, "Failed to open about dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
