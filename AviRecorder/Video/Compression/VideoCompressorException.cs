using System;
using System.Runtime.Serialization;

namespace AviRecorder.Video.Compression
{
    [Serializable]
    public class VideoCompressorException : Exception
    {
        public VideoCompressorException() : this(VideoCompressorError.Error)
        {
        }

        public VideoCompressorException(string message) : this(message, VideoCompressorError.Error)
        {
        }

        public VideoCompressorException(string message, Exception innerException) : this(message, innerException, VideoCompressorError.Error)
        {
        }

        public VideoCompressorException(VideoCompressorError videoCompressorError) : base()
        {
            VideoCompressorError = videoCompressorError;
        }

        public VideoCompressorException(string message, VideoCompressorError videoCompressorError) : base(message)
        {
            VideoCompressorError = videoCompressorError;
        }

        public VideoCompressorException(string message, Exception innerException, VideoCompressorError videoCompressorError) : base(message, innerException)
        {
            VideoCompressorError = videoCompressorError;
        }

        protected VideoCompressorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            VideoCompressorError = (VideoCompressorError)info.GetInt64(nameof(VideoCompressorError));
        }

        public VideoCompressorError VideoCompressorError { get; }
        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(VideoCompressorError), (long)VideoCompressorError);

            base.GetObjectData(info, context);
        }
    }
}