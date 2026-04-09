using System;
using System.Collections.Generic;

namespace GameCore.Utility
{
    public static class CollectionExtensions
    {
        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
            where TKey : IComparable<TKey>
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext()) return default;

            var min = e.Current;
            var minKey = selector(min);

            while (e.MoveNext())
            {
                var currentKey = selector(e.Current);
                if (currentKey.CompareTo(minKey) < 0)
                {
                    min = e.Current;
                    minKey = currentKey;
                }
            }

            return min;
        }
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
            where TKey : IComparable<TKey>
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext()) return default;

            var max = e.Current;
            var maxKey = selector(max);

            while (e.MoveNext())
            {
                var currentKey = selector(e.Current);
                if (currentKey.CompareTo(maxKey) > 0) 
                {
                    max = e.Current;
                    maxKey = currentKey;
                }
            }

            return max;
        }
    }
}