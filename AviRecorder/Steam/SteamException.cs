using System;
using System.Runtime.Serialization;

namespace AviRecorder.Steam
{
    [Serializable]
    public class SteamException : Exception
    {
        public SteamException()
        {
        }

        public SteamException(string message) : base(message)
        {
        }

        public SteamException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SteamException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
