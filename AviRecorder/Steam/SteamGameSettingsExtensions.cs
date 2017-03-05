using System;
using System.Collections.Generic;

namespace AviRecorder.Steam
{
    public static class SteamGameSettingsExtensions
    {
        public static string GetLaunchOptions(this SteamGameSettings gameSettings)
        {
            return gameSettings?.LaunchOptions ?? SteamGameSettings.DefaultLaunchOptions;
        }

        public static IReadOnlyList<string> GetCustom(this SteamGameSettings gameSettings)
        {
            return (IReadOnlyList<string>)gameSettings?.Custom ?? Array.Empty<string>();
        }

        public static string GetStartmovieCommands(this SteamGameSettings gameSettings)
        {
            return gameSettings?.StartmovieCommands ?? SteamGameSettings.DefaultStartmovieCommands;
        }

        public static string GetEndmovieCommands(this SteamGameSettings gameSettings)
        {
            return gameSettings?.EndmovieCommands ?? SteamGameSettings.DefaultEndmovieCommands;
        }
    }
}
