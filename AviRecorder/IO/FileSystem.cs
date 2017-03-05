using System;
using System.IO;
using System.Security;

namespace AviRecorder.IO
{
    public static class FileSystem
    {
        public static bool CreateDirectory(string path, bool throwOnError)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                if (throwOnError)
                    throw;
            }

            return false;
        }

        public static bool MoveDirectoryFile(string sourcePath, string destPath, bool throwOnError)
        {
            try
            {
                if (File.Exists(sourcePath))
                {
                    File.Move(sourcePath, destPath);
                    return true;
                }

                if (Directory.Exists(sourcePath))
                {
                    Directory.Move(sourcePath, destPath);
                    return true;
                }
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                if (throwOnError)
                    throw;
            }

            return false;
        }

        public static bool DeleteFile(string path, bool throwOnError)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException ||
                                       ex is NotSupportedException ||
                                       ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException)
            {
                if (throwOnError)
                    throw;
            }

            return false;
        }
    }
}
