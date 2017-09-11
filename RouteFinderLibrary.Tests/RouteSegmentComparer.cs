using System;
using System.Collections.Generic;

namespace RouteFinderLibrary.Tests
{
    internal sealed class RouteSegmentComparer : Comparer<RouteSegment>
    {
        public override int Compare(RouteSegment x, RouteSegment y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var fromComparison = string.Compare(x.From, y.From, StringComparison.Ordinal);
            if (fromComparison != 0) return fromComparison;
            return string.Compare(x.To, y.To, StringComparison.Ordinal);
        }
    }
}