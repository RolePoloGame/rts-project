using System.Collections.Generic;

namespace RTS.Core
{
    public static class ListExtensions
    {
        public static T Dequeue<T>(this List<T> list)
        {
            if (list == null || list.Count == 0) return default;
            var item = list[0];
            list.Remove(item);
            return item;
        }
    }
}
