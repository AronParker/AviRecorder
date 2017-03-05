using System;
using System.Runtime.Serialization;

namespace AviRecorder.Core
{
    [Serializable]
    public class ParseException : Exception
    {
        public ParseException(string message, int startIndex, int length) : base(message)
        {
            StartIndex = startIndex;
            Length = length;
        }

        protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StartIndex = info.GetInt32(nameof(StartIndex));
            Length = info.GetInt32(nameof(Length));
        }
        
        public int StartIndex { get; }
        public int Length { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StartIndex), StartIndex);
            info.AddValue(nameof(Length), Length);

            base.GetObjectData(info, context);
        }
    }
}
