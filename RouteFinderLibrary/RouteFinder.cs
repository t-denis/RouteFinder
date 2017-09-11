using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RouteFinderLibrary.Extensions;
using RouteFinderLibrary.Properties;

namespace RouteFinderLibrary
{
    /// <summary> Implements ordering of route segments </summary>
    public sealed class RouteFinder
    {
        /// <summary> Reorder segments to build a route. </summary>
        /// <param name="source">Collection of segments.</param>
        /// <returns>A route (ordered segments)</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Incorrect data. <paramref name="source"/> countains gaps, repeats, bypasses, 
        /// cycles or dangling segments.
        /// </exception>
        [NotNull, ItemNotNull]
        public IEnumerable<IRouteSegment> FindRoute([NotNull, ItemNotNull] IEnumerable<IRouteSegment> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var segments = source as ICollection<IRouteSegment> ?? source.ToList();
            if (segments.Any(x => x == null))
                throw new ArgumentException(ErrorMessages.NullSegment);
            if (segments.Any(x => string.Equals(x.From, x.To, StringComparison.Ordinal)))
                throw new ArgumentException(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments);
            if (segments.Select(x => x.From).HasDuplacates())
                throw new ArgumentException(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments);
            if (segments.Select(x => x.To).HasDuplacates())
                throw new ArgumentException(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments);

            if (segments.Count < 2) // nothing to sort
                return segments.ToList();

            var startSegments = FindStartSegments(segments).ToList();
            if (!startSegments.Any())
                throw new ArgumentException(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments);
            if (startSegments.Count > 1)
                throw new ArgumentException(ErrorMessages.CyclesOrGaps);
            var startSegment = startSegments.Single();
            var route = FindRouteIterator(segments, startSegment)
                .ToList();
            if (route.Count != segments.Count)
                throw new ArgumentException(ErrorMessages.CyclesOrGaps);
            return route;
        }
        
        private IEnumerable<IRouteSegment> FindRouteIterator(ICollection<IRouteSegment> source,
            IRouteSegment startSegment)
        {
            // .ToDictionary() LINQ method current implementation does not use a well-known capacity.
            var segmentsDictionary = new Dictionary<string, IRouteSegment>(source.Count);
            foreach (var segment in source)
                segmentsDictionary.Add(segment.From, segment);

            var currentSegment = startSegment;
            while (currentSegment != null)
            {
                segmentsDictionary.Remove(currentSegment.From);
                yield return currentSegment;
                segmentsDictionary.TryGetValue(currentSegment.To, out currentSegment);
            }
        }
        
        private IEnumerable<IRouteSegment> FindStartSegments(ICollection<IRouteSegment> segments)
        {
            // Since we have neither cycles, nor gaps, there should be a segment with a "from"-stop
            // that is not used as "to"-stop in a current or any other segments.
            var toStops = new HashSet<string>(segments.Select(segment => segment.To));
            return segments.Where(x => !toStops.Contains(x.From));
        }
    }
}