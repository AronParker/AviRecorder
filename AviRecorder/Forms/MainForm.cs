using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AviRecorder.Controller;
using AviRecorder.Video.TgaSequences;

namespace AviRecorder.Forms
{
    public partial class MainForm : Form
    {
        private Configuration _config;
        private GameLauncher _launcher;
        private bool _closeOnIdle;

        public MainForm(Configuration config)
        {
            InitializeComponent();

            _config = config;
            _launcher = new GameLauncher(config.Settings, this);
            _launcher.MovieStarted += Launcher_MovieStarted;
            _launcher.FrameProcessed += Launcher_FrameProcessed;
            _launcher.MovieEnded += Launcher_MovieEnded;
            _launcher.BufferOverflow += Launcher_BufferOverflow;
            _launcher.ErrorActionRequested += Launcher_ErrorActionRequested;
            _launcher.Error += Launcher_Error;
            _launcher.StateChanged += Launcher_StateChanged;

            _gameSettingsTextBox.Text = SummarizeGameSettings(_config.Settings);
            _recordingSettingsTextBox.Text = SummarizeRecordingSettings(_config.Settings);

            _startGameCheckBox.Checked = config.Settings.StartGame;
            _startGameCheckBox.CheckedChanged += StartGameCheckBox_CheckedChanged;
            _gameSettingsLabel.Enabled = _startGameCheckBox.Checked;
            _gameSettingsTextBox.Enabled = _startGameCheckBox.Checked;
            
            /*_toolTip.SetToolTip(_gameSettingsLabel, "The currently selected game and user.");
            _toolTip.SetToolTip(_recordingSettingsLabel, "Recording: The currently selected recording settings. They appar in the following format: %FPS% (%FrameBlendingFactor%, %ShutterAngle%), %VideoCompressor%.");
            _toolTip.SetToolTip(_currentMovieLabel, "Displays the file name of the current AVI file that is being recorded.");
            _toolTip.SetToolTip(_lastFrameProcessedLabel, "Displays the file name of the last processed TGA frame.");
            _toolTip.SetToolTip(_startGameCheckBox, "This option writes configs to support F9 start/stop recording. If you use a different recording tool like HLAE or Lawena, you can untick this checkbox so it will only serve as a converter from TGA frames to AVI files.");
            _toolTip.SetToolTip(_aviLinkLabel, "Opens the currently set AVI folder.");
            _toolTip.SetToolTip(_startButton, "Starts the recording process.");
            _toolTip.SetToolTip(_aboutButton, "Displays the about dialog.");*/

            ChangeState(GameLauncherState.Idle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_launcher.State != GameLauncherState.Idle)
            {
                e.Cancel = true;

                if (_closeOnIdle)
                    return;

                switch (_launcher.State)
                {
                    case GameLauncherState.Recording:
                        if (MessageBox.Show("Closing the program will cause recording to be stopped. Do you want to continue?",
                                            "Close program",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        {
                            return;
                        }
                        break;
                    case GameLauncherState.GameStarted:
                    case GameLauncherState.GameStartedAndRecording:
                        if (MessageBox.Show("Please close down the game first so that game settings can be restored. Do you want to force terminating the process of the game so that AVI Recorder can be closed immediately? (not recommended)",
                                            "Close program",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }
                
                _closeOnIdle = true;
                _launcher.ForceStop();
            }

            base.OnFormClosing(e);
        }

        private static string SummarizeGameSettings(ApplicationSettings settings)
        {
            if (settings.CurrentGame == null || settings.CurrentUser == null)
                return "No game selected.";

            return settings.CurrentGame.Name + ", " + settings.CurrentUser.Name;
        }

        private static string SummarizeRecordingSettings(ApplicationSettings settings)
        {
            if (settings.TgaDirectory.Length == 0)
                return "No TGA folder selected.";
            if (settings.AviDirectory.Length == 0)
                return "No AVI folder selected.";

            var frameRate = settings.FrameRate.ToString(NumberFormatInfo.InvariantInfo);
            var compressorName = settings.Compressor.DisplayName;

            if (settings.FrameBlendingFactor > 1)
            {
                var frameBlendingFactor = settings.FrameBlendingFactor.ToString(NumberFormatInfo.InvariantInfo);

                if (settings.FramesToProcess < settings.FrameBlendingFactor)
                {
                    var shutterAngle = ((double)settings.FramesToProcess / settings.FrameBlendingFactor * 360.0).ToString("F0", NumberFormatInfo.InvariantInfo);

                    return $"{frameRate} FPS ({frameBlendingFactor}x, {shutterAngle}°), {compressorName}";
                }

                return $"{frameRate} FPS ({frameBlendingFactor}x), {compressorName}";
            }

            return $"{frameRate} FPS, {compressorName}";
        }

        private void ChangeState(GameLauncherState state)
        {
            if (state == GameLauncherState.Idle && _closeOnIdle)
            {
                Close();
                return;
            }

            Icon icon;
            bool settingsEnabled;
            string startText;
            Image startImage;

            switch (state)
            {
                case GameLauncherState.Idle:
                    icon = Properties.Resources.hl2_blue;
                    settingsEnabled = true;
                    startText = "Start recording";
                    startImage = Properties.Resources.film_star;
                    break;
                case GameLauncherState.Recording:
                    icon = Properties.Resources.hl2_red;
                    settingsEnabled = false;
                    startText = "Stop recording";
                    startImage = Properties.Resources.film_stop;
                    break;
                case GameLauncherState.GameStarted:
                    icon = Properties.Resources.hl2_blue;
                    settingsEnabled = true;
                    startText = "Resume recording";
                    startImage = Properties.Resources.film_start;
                    break;
                case GameLauncherState.GameStartedAndRecording:
                    icon = Properties.Resources.hl2_red;
                    settingsEnabled = false;
                    startText = "Pause recording";
                    startImage = Properties.Resources.film_stop;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            Icon = icon;
            _notifyIcon.Icon = icon;
            _gameSettingsButton.Enabled = settingsEnabled;
            _gameSettingsToolStripMenuItem.Enabled = settingsEnabled;
            _recordingSettingsButton.Enabled = settingsEnabled;
            _recordingSettingsToolStripMenuItem.Enabled = settingsEnabled;
            _startButton.Text = startText;
            _startToolStripMenuItem.Text = startText;
            _startButton.Image = startImage;
            _startToolStripMenuItem.Image = startImage;
        }
        
        private void OpenTgaDirectory()
        {
            var tgaDirectory = _config.Settings.TgaDirectory;

            if (!Directory.Exists(tgaDirectory))
            {
                MessageBox.Show("Currently set TGA folder does not exist. Click \"Change...\" next to \"Recording settings\" to change it.", "TGA folder does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(tgaDirectory)?.Dispose();
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Unable to open TGA folder: " + ex.Message, "Unable to open TGA folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenAviDirectory()
        {
            var aviDirectory = _config.Settings.AviDirectory;

            if (!Directory.Exists(aviDirectory))
            {
                MessageBox.Show("Currently set AVI folder does not exist. Click \"Change...\" next to \"Recording settings\" to change it.", "AVI folder does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(aviDirectory)?.Dispose();
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Unable to open AVI folder: " + ex.Message, "Unable to open AVI folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GameSettingsButton_Click(object sender, EventArgs e)
        {
            using (var form = new GameSettingsForm(_config))
            {
                form.RecordingSettingsOnly = _launcher.State != GameLauncherState.Idle;

                if (form.ShowDialog() == DialogResult.OK)
                    _gameSettingsTextBox.Text = SummarizeGameSettings(_config.Settings);

                if (form.GameSettingsChanged)
                    _config.SaveSettings();
            }
        }

        private void RecordingSettingsButton_Click(object sender, EventArgs e)
        {
            var settings = _config.Settings;

            using (var form = new RecordingSettingsForm())
            {
                form.TgaDirectory = settings.TgaDirectory;
                form.AviDirectory = settings.AviDirectory;
                form.ChangeCompressor(settings.Compressor);
                form.FrameRate = settings.FrameRate;
                form.FrameBlendingFactor = settings.FrameBlendingFactor;
                form.FramesToProcess = settings.FramesToProcess;
                form.DeleteOnClose = settings.DeleteOnClose;
                form.RecordingNotifications = settings.RecordingNotifications;
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    settings.TgaDirectory = form.TgaDirectory;
                    settings.AviDirectory = form.AviDirectory;
                    settings.Compressor.Change(form.CompressorFcc, form.CompressorName, form.CompressorState);
                    settings.FrameRate = form.FrameRate;
                    settings.FrameBlendingFactor = form.FrameBlendingFactor;
                    settings.FramesToProcess = form.FramesToProcess;
                    settings.DeleteOnClose = form.DeleteOnClose;
                    settings.RecordingNotifications = form.RecordingNotifications;

                    _recordingSettingsTextBox.Text = SummarizeRecordingSettings(settings);

                    _config.SaveSettings();
                }
            }
        }

        private void StartGameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _config.Settings.StartGame = _startGameCheckBox.Checked;

            _gameSettingsLabel.Enabled = _startGameCheckBox.Checked;
            _gameSettingsTextBox.Enabled = _startGameCheckBox.Checked;

            _config.SaveSettings();
        }

        private async void StartButton_ClickAsync(object sender, EventArgs e)
        {
            await _launcher.StartAsync();
        }

        private void AviLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            OpenAviDirectory();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            using (var form = new AboutForm())
                form.ShowDialog();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Activate();
        }

        private void OpenTgaDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTgaDirectory();
        }

        private void OpenAviDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAviDirectory();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Launcher_MovieStarted(object sender, FileSystemEventArgs e)
        {
            _currentMovieTextBox.Text = e.Name;

            if (_config.Settings.RecordingNotifications)
                _notifyIcon.ShowBalloonTip(5000, "Recording started", $"Recording '{e.Name}'.", ToolTipIcon.Info);
        }

        private void Launcher_FrameProcessed(object sender, FileSystemEventArgs e)
        {
            _lastFrameProcessedTextBox.Text = e.Name;
        }

        private void Launcher_MovieEnded(object sender, EventArgs e)
        {
            _currentMovieTextBox.Text = "-";
            _lastFrameProcessedTextBox.Text = "-";
        }

        private void Launcher_BufferOverflow(object sender, EventArgs e)
        {
            _notifyIcon.ShowBalloonTip(5000, "Internal buffer overflow occured", "Some frames have been dropped.", ToolTipIcon.Warning);
        }

        private static void Launcher_ErrorActionRequested(object sender, RetryErrorEventArgs e)
        {
            e.Retry = MessageBox.Show(e.GetException().Message, "An error occured, action required", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry;
        }

        private void Launcher_Error(object sender, ErrorEventArgs e)
        {
            _notifyIcon.ShowBalloonTip(5000, "An error occured", e.GetException().Message, ToolTipIcon.Error);
        }

        private void Launcher_StateChanged(object sender, EventArgs e)
        {
            ChangeState(_launcher.State);
        }
    }
}
