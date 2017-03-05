using System;
using System.IO;

namespace AviRecorder.Video.TgaSequences
{
    public class TgaSequence
    {
        private int _currentFrame;

        private TgaSequence(string name, string directory)
        {
            Name = name;
            Directory = directory;
            _currentFrame = 0;
        }

        public string Name { get; }

        public string Directory { get; }

        public int CurrentFrame
        {
            get { return _currentFrame; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _currentFrame = value;
            }
        }

        public string CurrentFrameName => FormattableString.Invariant($"{Name}{CurrentFrame:D4}.tga");

        public string CurrentFrameFullName => FormattableString.Invariant($"{Directory}{Name}{CurrentFrame:D4}.tga");

        public string WavPath => Directory + Name + ".wav";

        public static TgaSequence FromPath(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (path.EndsWith("0000.tga", StringComparison.OrdinalIgnoreCase))
            {
                for (var i = path.Length; --i >= 0;)
                {
                    var c = path[i];

                    if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
                    {
                        var name = path.Substring(i + 1, path.Length - i - 1 - 8);
                        var dir = path.Substring(0, i + 1);

                        return new TgaSequence(name, dir);
                    }
                }
            }

            return null;
        }

        public bool IsCurrentFrame(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            return IsFrameNumber(fileName, _currentFrame);
        }

        public bool IsNextFrame(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            return IsFrameNumber(fileName, _currentFrame + 1);
        }

        private bool IsFrameNumber(string fileName, int number)
        {
            if (string.Compare(fileName, 0, Name, 0, Name.Length, StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            if (!fileName.EndsWith(".tga", StringComparison.OrdinalIgnoreCase))
                return false;

            var numLength = fileName.Length - Name.Length - 4;

            if (numLength < 1 || numLength > 10)
                return false;

            var lastDigit = fileName.Length - 5;
            var divisor = 1;

            while (fileName[lastDigit] - '0' == number / divisor % 10)
            {
                if (--numLength == 0)
                    return true;

                lastDigit--;
                divisor *= 10;
            }

            return false;
        }
    }
}
