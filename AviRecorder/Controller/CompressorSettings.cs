using System;
using AviRecorder.KeyValues;

namespace AviRecorder.Controller
{
    public class CompressorSettings
    {
        public const string NoCompressor = "Uncompressed";
        public const string UnnamedCompressor = "Unnamed compressor";

        public const string CompressorNameKey = "Name";
        public const string CompressorFccHandlerKey = "FccHandler";
        public const string CompressorStateKey = "State";

        public CompressorSettings()
        {
            Fcc = null;
            Name = null;
            State = null;
        }

        private CompressorSettings(uint? fcc, string name, byte[] state)
        {
            Fcc = fcc;
            Name = name;
            State = state;
        }

        public uint? Fcc { get; private set; }
        public string Name { get; private set; }
        public byte[] State { get; private set; }

        public string DisplayName => Name ?? NoCompressor;

        public static CompressorSettings FromKeyValue(KeyValue kv)
        {
            var fcc = (uint?)kv?[CompressorFccHandlerKey];
            byte[] state;
            string name;

            if (fcc != null)
            {
                name = (string)kv[CompressorNameKey] ?? UnnamedCompressor;
                state = (byte[])kv[CompressorStateKey];

                if (state != null && state.Length == 0)
                    state = null;
            }
            else
            {
                state = null;
                name = null;
            }

            return new CompressorSettings(fcc, name, state);
        }

        public void Change(uint? fcc, string name, byte[] state)
        {
            if (fcc == null)
            {
                if (name != null)
                    throw new ArgumentException("The name must be null if the fcc handler is null.", nameof(name));
                if (state != null)
                    throw new ArgumentException("The state must be null if the fcc handler is null.", nameof(state));
            }
            else
            {
                if (state != null && state.Length == 0)
                    throw new ArgumentException("The state length must not be zero.", nameof(state));
            }

            Fcc = fcc;
            Name = name;
            State = state;
        }

        public KeyValue ToKeyValue()
        {
            if (Fcc == null)
                return null;

            var compressor = new KeyValue(ApplicationSettings.CompressorKey);
            compressor.AddLast(new KeyValue(CompressorNameKey, Name));
            compressor.AddLast(new KeyValue(CompressorFccHandlerKey, Fcc.Value));

            if (State != null)
                compressor.AddLast(new KeyValue(CompressorStateKey, State));

            return compressor;
        }
    }
}
