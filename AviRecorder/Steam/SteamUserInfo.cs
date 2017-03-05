using System;
using System.Collections.Generic;
using System.IO;
using AviRecorder.Core;
using AviRecorder.KeyValues;
using System.Security;

namespace AviRecorder.Steam
{
    public class SteamUserInfo : IEquatable<SteamUserInfo>
    {
        private SteamUserInfo(uint id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }

        public uint Id { get; }
        public string Name { get; }
        public string Path { get; }
        
        public static IReadOnlyList<SteamUserInfo> LocateUsers(SteamLibrary library)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library));
            
            try
            {
                var userdata = new DirectoryInfo(library.InstallPath + "\\userdata");

                if (!userdata.Exists)
                    return Array.Empty<SteamUserInfo>();

                var users = new List<SteamUserInfo>();

                foreach (var dir in userdata.EnumerateDirectories())
                {
                    var userId = StringConverter.TryToUInt32(dir.Name);

                    if (!userId.HasValue || userId.Value == 0)
                        continue;

                    var localConfigPath = dir.FullName + "\\config\\localconfig.vdf";

                    if (!File.Exists(localConfigPath))
                        continue;

                    var name = (string)KeyValue.Load(localConfigPath)?["friends"]?["PersonaName"];

                    if (name != null)
                        users.Add(new SteamUserInfo(userId.Value, name, dir.FullName));
                }

                return users.ToArray();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is ParseException)
            {
                throw new SteamException("Unable to locate steam users: " + ex.Message, ex);                
            }
        }

        public bool Equals(SteamUserInfo other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((SteamUserInfo)obj);
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}
