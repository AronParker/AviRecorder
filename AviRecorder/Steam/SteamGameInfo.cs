using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using AviRecorder.Core;
using AviRecorder.IO;
using AviRecorder.KeyValues;

namespace AviRecorder.Steam
{
    public class SteamGameInfo
    {
        private const string NameKey = "Name";
        private const string AppIdKey = "AppId";
        private const string GameDirKey = "GameDir";
        private const string ExecutableKey = "Executable";
        private const string ArgumentsKey = "Arguments";

        private SteamGameInfo(string name, uint appId, string gameDir, string executable, string arguments)
        {
            Name = name;
            AppId = appId;
            GameDir = gameDir;
            Executable = executable;
            Arguments = arguments;
        }

        public string Name { get; }
        public uint AppId { get; }
        public string GameDir { get; }
        public string Executable { get; }
        public string Arguments { get; }

        public static IReadOnlyList<SteamGameInfo> LocateGames(KeyValue gameData, SteamLibrary library)
        {
            if (gameData == null)
                throw new ArgumentNullException(nameof(gameData));
            if (library == null)
                throw new ArgumentNullException(nameof(library));

            if (!gameData.HasChildren)
                return Array.Empty<SteamGameInfo>();

            return gameData.Select(kv => Parse(kv, library)).Where(game => game != null).ToArray();
        }

        public bool IsRunning
        {
            get
            {
                var instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Executable));

                foreach (var process in instances)
                    process.Dispose();

                return instances.Length > 0;
            }
        }

        public string GetLaunchOptions(SteamUserInfo user)
        {
            if (AppId == 0)
                return null;

            var localConfigPath = user.Path + "\\config\\localconfig.vdf";

            if (!File.Exists(localConfigPath))
                return null;

            KeyValue localConfig;

            try
            {
                localConfig = KeyValue.Load(localConfigPath);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is ParseException)
            {
                throw new SteamException("Unable to detect launch options: ", ex);
            }

            var apps = localConfig?["Software"]?["Valve"]?["Steam"]?["apps"];

            if (apps == null)
                return null;

            return (string)apps[StringConverter.ToString(AppId)]?["LaunchOptions"];
        }

        public IReadOnlyList<string> GetActiveCustom()
        {
            try
            {
                return EnumerateVpkFilesAndDirectories(GameDir + "\\custom").ToArray();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Unable to detect user customizations: " + ex.Message, ex);
            }
        }

        public SteamCustomCollection GetCustom()
        {
            try
            {
                var activeList = new List<string>();

                foreach (var active in EnumerateVpkFilesAndDirectories(GameDir + "\\custom"))
                    activeList.Add(active);

                var inactiveList = new List<string>();

                foreach (var inactive in EnumerateVpkFilesAndDirectories(GameDir + "\\custom_store"))
                    if (!activeList.Contains(inactive, StringComparer.OrdinalIgnoreCase))
                        inactiveList.Add(inactive);

                return new SteamCustomCollection(activeList.ToArray(), inactiveList.ToArray());
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Unable to detect user customizations: " + ex.Message, ex);
            }
        }

        public void ApplyCustom(IEnumerable<string> custom, IEnumerable<string> active, bool throwOnError)
        {
            if (custom == null)
                throw new ArgumentNullException(nameof(custom));
            if (active == null)
                throw new ArgumentNullException(nameof(active));

            try
            {
                SetCustom(active, custom, throwOnError);
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Unable to apply user customizations: " + ex.Message);
            }
        }

        public void RevertCustom(IEnumerable<string> custom, IEnumerable<string> activeCustom, bool throwOnError)
        {
            if (custom == null)
                throw new ArgumentNullException(nameof(custom));
            if (activeCustom == null)
                throw new ArgumentNullException(nameof(activeCustom));

            try
            {
                SetCustom(custom, activeCustom, throwOnError);
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Unable to revert user customizations: " + ex.Message);
            }
        }

        public bool Equals(SteamGameInfo other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((SteamGameInfo)obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        private static IEnumerable<string> EnumerateVpkFilesAndDirectories(string path)
        {
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
                return Enumerable.Empty<string>();

            return dir.EnumerateFileSystemInfos()
                      .Where(x => x is DirectoryInfo || x.Name.EndsWith(".vpk", StringComparison.OrdinalIgnoreCase))
                      .Select(x => x.Name);
        }

        private static SteamGameInfo Parse(KeyValue kv, SteamLibrary library)
        {
            var gameName = kv.Key;

            if (gameName.Length == 0)
                throw new SteamException("The name of the game is missing.");

            var gameDir = (string)kv[GameDirKey];

            if (gameDir == null)
                throw new SteamException("The path to the game directory of game \"" + gameName + "\" is missing.");

            var executable = (string)kv[ExecutableKey];

            if (executable == null)
                throw new SteamException("The path to the executable of game \"" + gameName + "\" is missing.");

            var parser = new GameDirAndExecutableParser(gameName);

            if (!parser.Parse(gameDir, executable, library))
                return null;

            var arguments = ((string)kv[ArgumentsKey])?.Replace("<steam>", library.InstallPath) ?? string.Empty;
            var appId = (uint?)kv[AppIdKey] ?? 0;

            return new SteamGameInfo(gameName, appId, parser.ValidatedGameDir, parser.ValidatedExecutable, arguments);
        }

        private struct GameDirAndExecutableParser
        {
            private string _gameName;
            private string _validatedExecutable;
            private string _validatedGameDir;

            public GameDirAndExecutableParser(string gameName)
            {
                _gameName = gameName;
                _validatedExecutable = null;
                _validatedGameDir = null;
            }

            public string ValidatedGameDir => _validatedGameDir;

            public string ValidatedExecutable => _validatedExecutable;

            public bool Parse(string gameDir, string executable, SteamLibrary library)
            {
                gameDir = gameDir.Replace("<steam>", library.InstallPath);
                executable = executable.Replace("<steam>", library.InstallPath);

                if (!gameDir.Contains("<library>"))
                    _validatedGameDir = ValidateGameDir(gameDir);

                if (!executable.Contains("<library>"))
                    _validatedExecutable = ValidateExecutable(executable);

                if (_validatedGameDir != null && _validatedExecutable != null)
                    return true;

                foreach (var folder in library.Folders)
                {
                    var newGameDir = _validatedGameDir ??
                                     ValidateGameDir(gameDir.Replace("<library>", folder));

                    var newExecutable = _validatedExecutable ??
                                        ValidateExecutable(executable.Replace("<library>", folder));

                    if (newGameDir != null && newExecutable != null)
                    {
                        _validatedGameDir = newGameDir;
                        _validatedExecutable = newExecutable;
                        return true;
                    }
                }

                return false;
            }

            private string ValidateGameDir(string path)
            {
                try
                {
                    path = Path.GetFullPath(path);
                }
                catch (Exception ex) when (ex is ArgumentException ||
                                           ex is NotSupportedException ||
                                           ex is IOException ||
                                           ex is UnauthorizedAccessException ||
                                           ex is SecurityException)
                {
                    throw new SteamException("The path to game directory of game \"" + _gameName + "\" is invalid: " + ex.Message, ex);
                }

                if (Directory.Exists(path))
                    return path;

                return null;
            }

            private string ValidateExecutable(string path)
            {
                try
                {
                    path = Path.GetFullPath(path);
                }
                catch (Exception ex) when (ex is ArgumentException ||
                                           ex is NotSupportedException ||
                                           ex is IOException ||
                                           ex is UnauthorizedAccessException ||
                                           ex is SecurityException)
                {
                    throw new SteamException("Invalid game data, the path to executable of game \"" + _gameName + "\" is invalid: " + ex.Message, ex);
                }

                if (File.Exists(path))
                    return path;

                return null;
            }
        }

        private static string ValidateGameDir(string path, string gameName)
        {
            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("The path to game directory of game \"" + gameName + "\" is invalid: " + ex.Message, ex);
            }

            if (Directory.Exists(path))
                return path;

            return null;
        }

        private static string ValidateExecutable(string path, string gameName)
        {
            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Invalid game data, the path to executable of game \"" + gameName + "\" is invalid: " + ex.Message, ex);
            }

            if (File.Exists(path))
                return path;

            return null;
        }

        private void SetCustom(IEnumerable<string> from, IEnumerable<string> to, bool throwOnError)
        {
            var needed = new HashSet<string>(to, StringComparer.OrdinalIgnoreCase);
            var ensureCustomStoreExistance = false;

            foreach (var fileName in from)
            {
                var itemRemains = needed.Remove(fileName);

                if (!itemRemains)
                {
                    if (!ensureCustomStoreExistance)
                    {
                        FileSystem.CreateDirectory(GameDir + "\\custom_store", throwOnError);
                        ensureCustomStoreExistance = true;
                    }

                    FileSystem.MoveDirectoryFile(GameDir + "\\custom\\" + fileName, GameDir + "\\custom_store\\" + fileName, throwOnError);
                }
            }

            var itemsMissing = needed.Count > 0;

            if (itemsMissing)
            {
                FileSystem.CreateDirectory(GameDir + "\\custom", throwOnError);

                foreach (var fileName in needed)
                    FileSystem.MoveDirectoryFile(GameDir + "\\custom_store\\" + fileName, GameDir + "\\custom\\" + fileName, throwOnError);
            }
        }
    }
}
