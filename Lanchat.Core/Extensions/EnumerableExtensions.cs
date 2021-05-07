using System;
using System.Collections.Generic;

namespace Lanchat.Core.Extensions
{
    /// <summary>
    ///     Extension methods for Enumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     ForEach like in Lists
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }
    }
}