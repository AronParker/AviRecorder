using System.Collections.Generic;

namespace AviRecorder.Extensions
{
    internal static class ListEx
    {
        public static void Replace<T>(this List<T> list, IEnumerable<T> collection)
        {
            list.Clear();
            list.AddRange(collection);
        }
    }
}
