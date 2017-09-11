using System;
using JetBrains.Annotations;
// ReSharper disable JoinNullCheckWithUsage

namespace RouteFinderLibrary
{
    /// <inheritdoc />
    public sealed class RouteSegment : IRouteSegment
    {
        /// <inheritdoc />
        public string From { get; }
        
        /// <inheritdoc />
        public string To { get; }

        /// <summary> New route segment </summary>
        /// <param name="from">Source stop</param>
        /// <param name="to">Destination stop</param>
        public RouteSegment([NotNull] string @from, [NotNull] string to)
        {
            if (@from == null)
                throw new ArgumentNullException(nameof(@from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));
            From = @from;
            To = to;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {From} - {To}";
        }
    }
}
