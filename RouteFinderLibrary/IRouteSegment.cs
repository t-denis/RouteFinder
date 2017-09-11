using JetBrains.Annotations;

namespace RouteFinderLibrary
{
    /// <summary> Route segment, from one stop to another. </summary>
    /// <remarks>The interface <see cref="IRouteSegment"/> is extracted to have the ability 
    /// to use custom route segment classes that contain some other related data - 
    /// length of the segment, duration, cost, etc.</remarks>
    public interface IRouteSegment
    {
        /// <summary> Start/source stop </summary>
        [NotNull]
        string From { get; }

        /// <summary> Finish/destination stop </summary>
        [NotNull]
        string To { get; }
    }
}