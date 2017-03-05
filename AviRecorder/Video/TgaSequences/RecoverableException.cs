using System;
using System.Runtime.Serialization;

namespace AviRecorder.Video.TgaSequences
{
    [Serializable]
    internal class RecoverableException : Exception
    {
        public RecoverableException()
        {
        }

        public RecoverableException(string message) : base(message)
        {
        }

        public RecoverableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecoverableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}