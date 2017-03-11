using System;
using System.Collections.Generic;
using AviRecorder.KeyValues;
using AviRecorder.Steam;
using AviRecorder.Extensions;
using AviRecorder.Video.TgaSequences;

namespace AviRecorder.Controller
{
    internal class ApplicationSettings
    {
        public const bool DefaultStartGame = true;
        public const string DefaultTgaDirectory = "";
        public const string DefaultAviDirectory = "";
        public const int DefaultFrameRate = 60;
        public const int DefaultFrameBlendingFactor = 1;
        public const int DefaultFramesToProcess = 1;
        public const bool DefaultDeleteOnClose = true;
        public const bool DefaultRecordingNotifications = true;

        public const string SettingsKey = "Settings";
        public const string CurrentGameKey = "CurrentGame";
        public const string CurrentUserKey = "CurrentUser";
        public const string StartGameKey = "StartGame";
        public const string GameSettingsKey = "GameSettings";
        public const string RecordingSettingsKey = "RecordingSettings";
        public const string TgaDirectoryKey = "TgaDirectory";
        public const string AviDirectoryKey = "AviDirectory";
        public const string CompressorKey = "Compressor";
        public const string FrameRateKey = "FrameRate";
        public const string FrameBlendingFactorKey = "FrameBlendingFactor";
        public const string FramesToProcessKey = "FramesToProcess";
        public const string DeleteOnCloseKey = "DeleteOnClose";
        public const string RecordingNotificationsKey = "RecordingNotifications";

        private string _tgaDirectory;
        private string _aviDirectory;
        private Dictionary<string, SteamGameSettings> _gameSettings;

        public SteamGameInfo CurrentGame { get; set; }
        public SteamUserInfo CurrentUser { get; set; }
        public bool StartGame { get; set; }

        public string TgaDirectory
        {
            get => _tgaDirectory;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _tgaDirectory = value;
            }
        }
        public string AviDirectory
        {
            get => _aviDirectory;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _aviDirectory = value;
            }
        }
        public CompressorSettings Compressor { get; private set; }
        public int FrameRate { get; set; }
        public int FrameBlendingFactor { get; set; }
        public int FramesToProcess { get; set; }
        public bool DeleteOnClose { get; set; }
        public bool RecordingNotifications { get; set; }

        private ApplicationSettings()
        {
        }

        public static ApplicationSettings FromKeyValue(KeyValue kv, IReadOnlyList<SteamGameInfo> games, IReadOnlyList<SteamUserInfo> users)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games));
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            var settings = new ApplicationSettings();
            var recordingSettings = kv?[RecordingSettingsKey];

            settings.CurrentGame = ParseGame((string)kv?[CurrentGameKey], games);
            settings.CurrentUser = ParseUser((uint?)kv?[CurrentUserKey] ?? 0, users);
            settings.StartGame = (bool?)kv?[StartGameKey] ?? DefaultStartGame;

            settings._gameSettings = ParseGameSettings(kv?[GameSettingsKey], games);

            settings._tgaDirectory = (string)recordingSettings?[TgaDirectoryKey] ?? DefaultTgaDirectory;
            settings._aviDirectory = (string)recordingSettings?[AviDirectoryKey] ?? DefaultAviDirectory;
            settings.Compressor = CompressorSettings.FromKeyValue(recordingSettings?[CompressorKey]);
            settings.FrameRate = MathEx.Constrain((int?)recordingSettings?[FrameRateKey] ?? DefaultFrameRate, 1, TgaSequenceRecorder.MaxSupportedFrameRate);
            settings.FrameBlendingFactor = Math.Max((int?)recordingSettings?[FrameBlendingFactorKey] ?? DefaultFrameBlendingFactor, 1);
            settings.FramesToProcess = MathEx.Constrain((int?)recordingSettings?[FramesToProcessKey] ?? DefaultFramesToProcess, 1, settings.FrameBlendingFactor);
            settings.DeleteOnClose = (bool?)recordingSettings?[DeleteOnCloseKey] ?? DefaultDeleteOnClose;
            settings.RecordingNotifications = (bool?)recordingSettings?[RecordingNotificationsKey] ?? DefaultRecordingNotifications;
            
            return settings;
        }

        public SteamGameSettings GetCurrentGameSettings()
        {
            if (CurrentGame == null)
                return null;

            if (_gameSettings.TryGetValue(CurrentGame.GameDir, out var settings))
                return settings;

            return null;
        }

        public SteamGameSettings GetGameSettings(SteamGameInfo game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (_gameSettings.TryGetValue(game.GameDir, out var settings))
                return settings;

            return null;
        }

        public void SetGameSettings(SteamGameInfo game, SteamGameSettings settings)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            _gameSettings[game.GameDir] = settings;
        }

        public KeyValue ToKeyValue(IReadOnlyList<SteamGameInfo> games)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games));

            var settingsData = new KeyValue(SettingsKey);

            if (CurrentGame != null)
                settingsData.AddLast(new KeyValue(CurrentGameKey, CurrentGame.GameDir));

            if (CurrentUser != null)
                settingsData.AddLast(new KeyValue(CurrentUserKey, CurrentUser.Id));

            settingsData.AddLast(new KeyValue(StartGameKey, StartGame));

            var gameSettings = new KeyValue(GameSettingsKey);

            foreach (var settings in _gameSettings)
            {
                if (settings.Value == null)
                    continue;

                gameSettings.AddLast(settings.Value.ToKeyValue(settings.Key));
            }

            settingsData.AddLast(gameSettings);

            var recordingSettings = new KeyValue(RecordingSettingsKey);

            recordingSettings.AddLast(new KeyValue(TgaDirectoryKey, TgaDirectory));
            recordingSettings.AddLast(new KeyValue(AviDirectoryKey, AviDirectory));

            var compressor = Compressor.ToKeyValue();

            if (compressor != null)
                recordingSettings.AddLast(compressor);

            recordingSettings.AddLast(new KeyValue(FrameRateKey, FrameRate));
            recordingSettings.AddLast(new KeyValue(FrameBlendingFactorKey, FrameBlendingFactor));
            recordingSettings.AddLast(new KeyValue(FramesToProcessKey, FramesToProcess));
            recordingSettings.AddLast(new KeyValue(DeleteOnCloseKey, DeleteOnClose));
            recordingSettings.AddLast(new KeyValue(RecordingNotificationsKey, RecordingNotifications));

            settingsData.AddLast(recordingSettings);

            return settingsData;
        }

        private static SteamGameInfo ParseGame(string key, IReadOnlyList<SteamGameInfo> games)
        {
            if (key != null)
                foreach (var game in games)
                    if (game.GameDir == key)
                        return game;

            return null;
        }

        private static SteamUserInfo ParseUser(uint key, IReadOnlyList<SteamUserInfo> users)
        {
            if (key != 0)
                foreach (var user in users)
                    if (user.Id == key)
                        return user;

            return null;
        }

        private static Dictionary<string, SteamGameSettings> ParseGameSettings(KeyValue kv, IReadOnlyList<SteamGameInfo> games)
        {
            var dict = new Dictionary<string, SteamGameSettings>(StringComparer.OrdinalIgnoreCase);

            foreach (var game in games)
                dict.Add(game.GameDir, null);

            if (kv != null)
                foreach (var gameSetting in kv)
                    if (dict.ContainsKey(gameSetting.Key))
                        dict[gameSetting.Key] = SteamGameSettings.FromKeyValue(gameSetting);

            return dict;
        }
    }
}
