using System;
using System.Runtime.Serialization;

namespace AviRecorder.Controller
{
    [Serializable]
    internal class GameLauncherException : Exception
    {
        public GameLauncherException()
        {
        }

        public GameLauncherException(string message) : base(message)
        {
        }

        public GameLauncherException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GameLauncherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}