using System.Collections.Generic;
using System.IO;
using AviRecorder.Core;
using AviRecorder.KeyValues;
using System;
using System.Security;

namespace AviRecorder.Steam
{
    public class SteamLibrary
    {
        private SteamLibrary(IReadOnlyList<string> folders)
        {
            Folders = folders;
        }

        public string InstallPath => Folders[0];

        public IReadOnlyList<string> Folders { get; }
        
        public static SteamLibrary Locate()
        {
            var installPath = SteamInstallPath.Locate();
            var folders = new List<string> { installPath };
            var libraryFoldersPath = installPath + "\\steamapps\\libraryfolders.vdf";

            if (File.Exists(libraryFoldersPath))
                ParseLibraryFolders(libraryFoldersPath, folders);
            
            return new SteamLibrary(folders.ToArray());
        }

        private static void ParseLibraryFolders(string libraryFoldersPath, List<string> folders)
        {
            KeyValue libraryFolders;

            try
            {
                libraryFolders = KeyValue.Load(libraryFoldersPath);
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is ParseException)
            {
                throw new SteamException("Unable to locate steam library folders: " + ex.Message, ex);
            }

            if (libraryFolders == null)
                return;

            for (var i = 1U; i > 0; i++)
            {
                var libraryFolder = libraryFolders[StringConverter.ToString(i)];

                if (libraryFolder == null)
                    break;

                if (libraryFolder.HasValue)
                    folders.Add(libraryFolder.Value);
            }
        }
    }
}
