using System;
using System.IO;
using System.Security;
using Microsoft.Win32;

namespace AviRecorder.Steam
{
    public static class SteamInstallPath
    {
        public static string Locate()
        {
            try
            {
                return GetLocalMachineInstallPath();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                throw new SteamException("Unable to locate steam install path: " + ex.Message, ex);
            }
        }

        private static string GetLocalMachineInstallPath()
        {
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            using (var steam = hklm.OpenSubKey(@"SOFTWARE\Valve\Steam"))
            {
                var installPath = (string)steam?.GetValue("InstallPath", null, RegistryValueOptions.None);

                if (Directory.Exists(installPath))
                    return installPath;
            }

            throw new SteamException("No Steam installation path found.");
        }
    }
}
