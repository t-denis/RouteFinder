using System.Collections.Generic;
using JetBrains.Annotations;

namespace RouteFinderLibrary.Extensions
{
    /// <summary> Extension methods for <see cref="IEnumerable{IRouteSegment}"/> </summary>
    public static class RouteSegmentCollectionEx
    {
        [NotNull, ItemNotNull]
        public static IEnumerable<IRouteSegment> OrderByStops([NotNull, ItemNotNull] this IEnumerable<IRouteSegment> source)
        {
            var routeFinder = new RouteFinder();
            return routeFinder.FindRoute(source);
        }
    }
}