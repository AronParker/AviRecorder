using System;
using System.Collections.Generic;

namespace AviRecorder.Steam
{
    public class SteamCustomCollection
    {
        public SteamCustomCollection(IReadOnlyList<string> active, IReadOnlyList<string> inactive)
        {
            if (active == null)
                throw new ArgumentNullException(nameof(active));
            if (inactive == null)
                throw new ArgumentNullException(nameof(inactive));

            Active = active;
            Inactive = inactive;
        }

        public IReadOnlyList<string> Active { get; }
        public IReadOnlyList<string> Inactive { get; }
    }
}
