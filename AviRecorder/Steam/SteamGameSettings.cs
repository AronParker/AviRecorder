using System;
using System.Collections.Generic;
using AviRecorder.KeyValues;
using System.Linq;

namespace AviRecorder.Steam
{
    public class SteamGameSettings
    {
        public const string DefaultLaunchOptions = "";
        public const string DefaultStartmovieCommands = "";
        public const string DefaultEndmovieCommands = "";

        public const string LaunchOptionsKey = "LaunchOptions";
        public const string CustomKey = "Custom";
        public const string StartmovieCommandsKey = "StartmovieCommands";
        public const string EndmovieCommandsKey = "EndmovieCommands";

        private string _launchOptions;
        private string _startmovieCommands;
        private string _endmovieCommands;

        public SteamGameSettings()
        {
            _launchOptions = string.Empty;
            Custom = new List<string>();
            _startmovieCommands = string.Empty;
            _endmovieCommands = string.Empty;
        }

        private SteamGameSettings(string launchOptions, IEnumerable<string> custom, string startmovieCommands, string endmovieCommands)
        {
            _launchOptions = launchOptions;
            Custom = new List<string>(custom);
            _startmovieCommands = startmovieCommands;
            _endmovieCommands = endmovieCommands;
        }

        public string LaunchOptions
        {
            get => _launchOptions;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _launchOptions = value;
            }
        }

        public List<string> Custom { get; }

        public string StartmovieCommands
        {
            get => _startmovieCommands;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _startmovieCommands = value;
            }
        }

        public string EndmovieCommands
        {
            get => _endmovieCommands;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _endmovieCommands = value;
            }
        }

        public static SteamGameSettings FromKeyValue(KeyValue kv)
        {
            if (kv == null)
                throw new ArgumentNullException(nameof(kv));

            var launchOptions = (string)kv[LaunchOptionsKey] ?? DefaultLaunchOptions;
            var custom = kv[CustomKey]?.Select(x => x.Key) ?? Enumerable.Empty<string>();
            var startmovieCommands = (string)kv[StartmovieCommandsKey] ?? DefaultStartmovieCommands;
            var endmovieCommands = (string)kv[EndmovieCommandsKey] ?? DefaultEndmovieCommands;

            return new SteamGameSettings(launchOptions, custom, startmovieCommands, endmovieCommands);
        }

        public KeyValue ToKeyValue(string gameDir)
        {
            if (gameDir == null)
                throw new ArgumentNullException(nameof(gameDir));

            return new KeyValue(gameDir,
                       new KeyValue(LaunchOptionsKey, LaunchOptions),
                       new KeyValue(CustomKey, Custom.Select(x => new KeyValue(x))),
                       new KeyValue(StartmovieCommandsKey, StartmovieCommands),
                       new KeyValue(EndmovieCommandsKey, EndmovieCommands));
        }
    }
}
