using System;
using System.Runtime.InteropServices;

namespace AviRecorder.Video.Compression
{
    public class VideoCompressorHandle : SafeHandle
    {
        private VideoCompressorHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            return NativeMethods.ICClose(handle) == IntPtr.Zero;
        }
    }
}
