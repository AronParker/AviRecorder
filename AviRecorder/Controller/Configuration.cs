using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Forms;
using AviRecorder.KeyValues;
using AviRecorder.Steam;
using AviRecorder.Core;

namespace AviRecorder.Controller
{
    public class Configuration
    {
        private const string SettingsFile = "settings.kv";
        private const string GamesFile = "games.kv";

        private Configuration(IReadOnlyList<SteamGameInfo> games, IReadOnlyList<SteamUserInfo> users, ApplicationSettings settings)
        {
            Games = games;
            Users = users;
            Settings = settings;
        }

        public IReadOnlyList<SteamGameInfo> Games { get; }
        public IReadOnlyList<SteamUserInfo> Users { get; }
        public ApplicationSettings Settings { get; }

        public static Configuration Load()
        {
            if (!File.Exists(GamesFile))
                SaveDefaultGameData();

            var gameData = LoadGameData();
            var settingsData = LoadSettingsData();

            var library = SteamLibrary.Locate();
            var games = SteamGameInfo.LocateGames(gameData, library);

            if (games.Count == 0)
                throw new SteamException("No supported steam games found on this system.");

            var users = SteamUserInfo.LocateUsers(library);

            if (users.Count == 0)
                throw new SteamException("No steam users found on this system.");

            var settings = ApplicationSettings.FromKeyValue(settingsData, games, users);

            return new Configuration(games, users, settings);
        }

        private static KeyValue LoadGameData()
        {
            KeyValue gameData;
            try
            {
                gameData = KeyValue.Load(GamesFile);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is ParseException)
            {
                throw new SteamException("An error occured while attempting to load the supported games: " + ex.Message, ex);
            }

            if (gameData == null || !gameData.HasChildren)
                throw new SteamException("No supported games found.");

            return gameData;
        }

        private static KeyValue LoadSettingsData()
        {
            try
            {
                if (File.Exists(SettingsFile))
                    return KeyValue.Load(SettingsFile);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is ParseException)
            {
                throw new SteamException("An error occured while attempting to load settings: " + ex.Message);
            }

            return null;
        }

        private static void SaveDefaultGameData()
        {
            try
            {
                File.WriteAllBytes(GamesFile, Properties.Resources.games);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is SecurityException ||
                                       ex is UnauthorizedAccessException)
            {
                throw new SteamException("An error occured while attempting to save the supported games: " + ex.Message);
            }
        }

        public void SaveSettings()
        {
            try
            {
                Settings.ToKeyValue(Games).Save(SettingsFile);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                MessageBox.Show("An error occured while attempting to save game settings: " + ex.Message,
                                "Failed to save game settings",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
