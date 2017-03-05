namespace AviRecorder.Video.Compression
{
    public enum VideoCompressorError
    {
        Ok = 0,
        Unsupported = -1,
        BadFormat = -2,
        Memory = -3,
        Internal = -4,
        BadFlags = -5,
        BadParam = -6,
        BadSize = -7,
        BadHandle = -8,
        CantUpdate = -9,
        Abort = -10,
        Error = -100,
        BadBitDepth = -200,
        BadImageSize = -201
    }
}
