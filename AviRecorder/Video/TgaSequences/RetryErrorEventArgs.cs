using System;
using System.IO;

namespace AviRecorder.Video.TgaSequences
{
    public class RetryErrorEventArgs : ErrorEventArgs
    {
        public RetryErrorEventArgs(Exception exception)
            : base(exception)
        {
        }

        public bool Retry { get; set; }
    }
}
