using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using AviRecorder.IO;
using AviRecorder.Steam;
using AviRecorder.Video.Compression;
using AviRecorder.Video.TgaSequences;

namespace AviRecorder.Controller
{
    internal class GameLauncher : IDisposable
    {
        private const string StartupConfig = "avi_recorder_startup.cfg";
        private const string StartConfig = "avi_recorder_start.cfg";
        private const string StopConfig = "avi_recorder_stop.cfg";

        private ApplicationSettings _settings;
        private TgaSequenceRecorder _recorder;
        private Process _process;
        
        private IReadOnlyList<string> _lastActiveCustom;
        private GameLauncherState _state;

        private bool _disposed = false;

        public GameLauncher(ApplicationSettings settings, ISynchronizeInvoke synchronizingObject)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (synchronizingObject == null)
                throw new ArgumentNullException(nameof(synchronizingObject));

            _settings = settings;
            _recorder = new TgaSequenceRecorder { SynchronizingObject = synchronizingObject };
            _process = new Process { EnableRaisingEvents = true, SynchronizingObject = synchronizingObject };
            _process.Exited += ProcessExited;

            _lastActiveCustom = null;
            _state = GameLauncherState.Idle;
        }

        public event FileSystemEventHandler MovieStarted
        {
            add => _recorder.MovieStarted += value;
            remove => _recorder.MovieStarted -= value;
        }

        public event FileSystemEventHandler FrameProcessed
        {
            add => _recorder.FrameProcessed += value;
            remove => _recorder.FrameProcessed -= value;
        }

        public event EventHandler MovieEnded
        {
            add => _recorder.MovieEnded += value;
            remove => _recorder.MovieEnded -= value;
        }

        public event EventHandler BufferOverflow
        {
            add => _recorder.BufferOverflow += value;
            remove => _recorder.BufferOverflow -= value;
        }

        public event ErrorActionHandler ErrorActionRequested
        {
            add => _recorder.ErrorActionRequested += value;
            remove => _recorder.ErrorActionRequested -= value;
        }

        public event ErrorEventHandler Error
        {
            add => _recorder.Error += value;
            remove => _recorder.Error -= value;
        }

        public event EventHandler StateChanged;

        public GameLauncherState State
        {
            get => _state;
            private set
            {
                if (value == _state)
                    return;

                _state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task StartAsync()
        {
            SteamGameSettings gameSettings;

            switch (_state)
            {
                case GameLauncherState.Idle:
                    if (!ValidateSettings())
                        return;

                    if (_settings.StartGame)
                    {
                        gameSettings = _settings.GetCurrentGameSettings();

                        if (!StartGame(gameSettings))
                            return;

                        await StartRecordingWithGame(gameSettings);
                    }
                    else
                    {
                        await StartRecordingWithoutGame();
                    }

                    break;
                case GameLauncherState.Recording:
                case GameLauncherState.GameStartedAndRecording:
                    _recorder.Stop();
                    break;
                case GameLauncherState.GameStarted:
                    await StartRecordingWithGame(_settings.GetCurrentGameSettings());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool ValidateSettings()
        {
            if (_settings.TgaDirectory.Length == 0)
            {
                MessageBox.Show("No TGA folder selected. Click \"Change...\" next to \"Recording settings\" to select one.", "No TGA folder selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.AviDirectory.Length == 0)
            {
                MessageBox.Show("No AVI folder selected. Click \"Change...\" next to \"Recording settings\" to select one.", "No AVI folder selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Directory.Exists(_settings.TgaDirectory))
            {
                MessageBox.Show("Currently set TGA folder does not exist. Click \"Change...\" next to \"Recording settings\" to change it.", "TGA folder does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Directory.Exists(_settings.AviDirectory))
            {
                MessageBox.Show("Currently set AVI folder does not exist. Click \"Change...\" next to \"Recording settings\" to change it.", "AVI folder does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.StartGame)
            {
                if (_settings.CurrentGame == null)
                {
                    MessageBox.Show("No game selected. Click \"Change...\" next to \"Game settings\" to select one.", "No game selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (_settings.CurrentUser == null)
                {
                    MessageBox.Show("No user selected. Click \"Change...\" next to \"Game settings\" to select one.", "No user selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (_settings.CurrentGame.IsRunning)
                {
                    MessageBox.Show("Game is already running. Please close it down first before starting a recording session.", "Game is already running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (File.Exists(_settings.CurrentGame.GameDir + "\\cfg\\" + StartupConfig) ||
                    File.Exists(_settings.CurrentGame.GameDir + "\\cfg\\" + StartConfig) ||
                    File.Exists(_settings.CurrentGame.GameDir + "\\cfg\\" + StopConfig))
                {
                    if (MessageBox.Show("The config files \"" + StartupConfig + "\", \"" + StartConfig + "\" and \"" + StopConfig + "\" will be overwritten by this program, are you sure you want to continue?",
                                        "Confirm overwrite",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task StartRecordingWithGame(SteamGameSettings gameSettings)
        {
            try
            {
                ApplyRecorderSettings();

                try
                {
                    WriteStartAndStopConfigs(gameSettings);
                }
                catch (Exception ex) when (ex is IOException ||
                                           ex is UnauthorizedAccessException ||
                                           ex is SecurityException)
                {
                    throw new GameLauncherException("Unable to write recording configs: " + ex.Message, ex);
                }
            }
            catch (GameLauncherException ex)
            {
                MessageBox.Show(ex.Message, "Unable to apply recorder settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            State = GameLauncherState.GameStartedAndRecording;
            await StartRecording();

            var hasExited = GetProcessHasExited();

            if (hasExited)
                EndGame(gameSettings);
            else
                State = GameLauncherState.GameStarted;
        }

        private async Task StartRecordingWithoutGame()
        {
            try
            {
                ApplyRecorderSettings();
            }
            catch (GameLauncherException ex)
            {
                MessageBox.Show(ex.Message, "Unable to apply recorder settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            State = GameLauncherState.Recording;
            await StartRecording();

            EndRecording();
            State = GameLauncherState.Idle;
        }

        private async Task StartRecording()
        {
            try
            {
                await _recorder.Start();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                MessageBox.Show("Failed to start recording: " + ex.Message,
                                "Failed to start recording",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void ForceStop()
        {
            switch (_state)
            {
                case GameLauncherState.Idle:
                    break;
                case GameLauncherState.Recording:
                    _recorder.Stop();
                    break;
                case GameLauncherState.GameStarted:
                case GameLauncherState.GameStartedAndRecording:
                    try
                    {
                        if (!_process.HasExited)
                            _process.Kill();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("Unable to kill game process: " + ex.Message, "Unable to kill game process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (InvalidOperationException)
                    {
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EndRecording()
        {
            _recorder.Compressor?.Dispose();
            _recorder.Compressor = null;
        }

        private bool StartGame(SteamGameSettings gameSettings)
        {
            try
            {
                _lastActiveCustom = _settings.CurrentGame.GetActiveCustom();
            }
            catch (SteamException ex)
            {
                MessageBox.Show(ex.Message, "Unable to detect user customizations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                ApplyGameSettings(gameSettings);
                StartProcess(gameSettings);
                State = GameLauncherState.GameStarted;
                return true;
            }
            catch (GameLauncherException ex)
            {
                MessageBox.Show(ex.Message, "Unable to start game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RevertGameSettings(gameSettings);
                return false;
            }
        }

        private void EndGame(SteamGameSettings gameSettings)
        {
            EndRecording();
            RevertGameSettings(gameSettings);
            _lastActiveCustom = null;

            State = GameLauncherState.Idle;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            switch (_state)
            {
                case GameLauncherState.GameStartedAndRecording:
                    _recorder.Stop();
                    break;
                case GameLauncherState.GameStarted:
                    EndGame(_settings.GetCurrentGameSettings());
                    break;
            }
        }

        private void ApplyGameSettings(SteamGameSettings gameSettings)
        {
            try
            {
                WriteStartupConfig();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new GameLauncherException("Unable to write recording configs: " + ex.Message, ex);
            }

            try
            {
                _settings.CurrentGame.ApplyCustom(gameSettings.GetCustom(), _lastActiveCustom, true);
            }
            catch (SteamException ex)
            {
                throw new GameLauncherException(ex.Message, ex);
            }
        }

        private void RevertGameSettings(SteamGameSettings gameSettings)
        {
            var game = _settings.CurrentGame;

            while (true)
            {
                try
                {
                    DeleteStartupStartAndStopConfigs(game);

                    game.RevertCustom(gameSettings.GetCustom(),
                                      _lastActiveCustom,
                                      true);
                    break;
                }
                catch (Exception ex) when (ex is GameLauncherException ||
                                           ex is SteamException)
                {
                    if (MessageBox.Show(ex.Message,
                                        "Unable to restore game settings",
                                        MessageBoxButtons.RetryCancel,
                                        MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        break;
                    }
                }
            }
        }

        private static void DeleteStartupStartAndStopConfigs(SteamGameInfo game)
        {
            try
            {
                FileSystem.DeleteFile(game.GameDir + "\\cfg\\" + StartupConfig, true);
                FileSystem.DeleteFile(game.GameDir + "\\cfg\\" + StartConfig, true);
                FileSystem.DeleteFile(game.GameDir + "\\cfg\\" + StopConfig, true);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new GameLauncherException("Unable to delete recording config: " + ex.Message, ex);
            }
        }

        private void ApplyRecorderSettings()
        {
            try
            {
                _recorder.TgaDirectory = _settings.TgaDirectory;
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new GameLauncherException("Unable to set TGA folder: " + ex.Message, ex);
            }

            try
            {
                _recorder.AviDirectory = _settings.AviDirectory;
            }
            catch (ArgumentException ex)
            {
                throw new GameLauncherException("Unable to set AVI folder: " + ex.Message, ex);
            }

            try
            {
                if (_recorder.Compressor != null && _settings.Compressor.Fcc != null && _recorder.Compressor.Info.FccHandler == _settings.Compressor.Fcc)
                {
                    _recorder.Compressor.SetState(_settings.Compressor.State);
                }
                else
                {
                    _recorder.Compressor?.Dispose();

                    if (_settings.Compressor.Fcc != null)
                    {
                        var compressor = VideoCompressor.Open((uint)_settings.Compressor.Fcc);

                        if (_settings.Compressor.State != null)
                            compressor.SetState(_settings.Compressor.State);

                        _recorder.Compressor = compressor;
                    }
                }
            }
            catch (VideoCompressorException ex)
            {
                throw new GameLauncherException("Unable to set video compressor: " + ex.Message, ex);
            }

            _recorder.FrameRate = _settings.FrameRate;
            _recorder.FrameBlendingFactor = _settings.FrameBlendingFactor;
            _recorder.FramesToProcess = _settings.FramesToProcess;
            _recorder.DeleteOnClose = _settings.DeleteOnClose;
        }

        private void StartProcess(SteamGameSettings gameSettings)
        {
            _process.StartInfo.FileName = _settings.CurrentGame.Executable;
            _process.StartInfo.Arguments = string.Join(" ",
                                                       _settings.CurrentGame.Arguments,
                                                       "+exec " + StartupConfig,
                                                       gameSettings.GetLaunchOptions());
            try
            {
                _process.Start();
            }
            catch (Win32Exception ex)
            {
                throw new GameLauncherException("Unable to start game: " + ex.Message, ex);
            }
        }

        private bool GetProcessHasExited()
        {
            try
            {
                return _process.HasExited;
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Unable to detect exit code for the game process: " + ex.Message, "Unable to detect exit code for the game process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        private void WriteStartupConfig()
        {
            FileSystem.CreateDirectory(_settings.CurrentGame.GameDir + "\\cfg", true);

            using (var startup = CreateConfig(_settings.CurrentGame.GameDir + "\\cfg\\" + StartupConfig))
            {
                startup.WriteLine("alias avi_recorder_startstop avi_recorder_start");
                startup.WriteLine("alias avi_recorder_start \"alias avi_recorder_startstop avi_recorder_stop; exec " + StartConfig + "\"");
                startup.WriteLine("alias avi_recorder_stop \"endmovie; host_framerate 0; host_timescale 1; exec " + StopConfig + "; alias avi_recorder_startstop avi_recorder_start\"");
                startup.WriteLine("bind F9 avi_recorder_startstop");
                startup.WriteLine("host_writeconfig");
            }
        }

        private void WriteStartAndStopConfigs(SteamGameSettings gameSettings)
        {
            var startmovieComannds = gameSettings.GetStartmovieCommands();
            var endmovieCommands = gameSettings.GetEndmovieCommands();

            FileSystem.CreateDirectory(_settings.CurrentGame.GameDir + "\\cfg", true);

            using (var start = CreateConfig(_settings.CurrentGame.GameDir + "\\cfg\\" + StartConfig))
            {
                var hostFramerate = (long)_settings.FrameRate * _settings.FrameBlendingFactor;
                var hostTimescale = 1.0 / hostFramerate;
                
                start.WriteLine(FormattableString.Invariant($"host_framerate {hostFramerate:F0}"));

                if (!string.IsNullOrEmpty(startmovieComannds))
                    start.WriteLine(startmovieComannds);

                start.WriteLine($"startmovie \"{_settings.TgaDirectory}\\rec_\"");
            }

            using (var stop = CreateConfig(_settings.CurrentGame.GameDir + "\\cfg\\" + StopConfig))
            {
                if (!string.IsNullOrEmpty(endmovieCommands))
                    stop.WriteLine(endmovieCommands);
            }
        }

        private static StreamWriter CreateConfig(string path)
        {
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan);

            try
            {
                return new StreamWriter(fs, Core.StringConverter.Utf8, 1024, false);
            }
            catch (Exception)
            {
                fs.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            _recorder.Dispose();
            _process?.Dispose();

            _disposed = true;
        }
    }
}
