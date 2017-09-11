using System.Collections.Generic;
using System.Linq;

namespace RouteFinderLibrary.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    internal static class EnumerableEx
    {
        internal static bool HasDuplacates<T>(this IEnumerable<T> source)
        {
            var hashset = new HashSet<T>();
            return source.Any(x => !hashset.Add(x));
        }
    }
}