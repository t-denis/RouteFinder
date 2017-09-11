using NUnit.Framework;

namespace RouteFinderLibrary.Tests
{
    public class RouteSegmentComparerTests
    {
        private readonly RouteSegmentComparer _comparer = new RouteSegmentComparer();

        [Test]
        public void Equality()
        {
            var segmentA = new RouteSegment("Moscow", "Saint Petersburg");
            var segmentB = new RouteSegment("Saint Petersburg", "Moscow");
            var segmentACopy = new RouteSegment(segmentA.From, segmentA.To);

            Assert.IsTrue(_comparer.Compare(segmentA, segmentACopy) == 0);
            Assert.IsTrue(_comparer.Compare(segmentA, segmentB) != 0);
            Assert.IsTrue(_comparer.Compare(segmentA, null) != 0);
            Assert.IsTrue(_comparer.Compare(null, segmentA) != 0);
            Assert.IsTrue(_comparer.Compare(null, null) == 0);
        }
    }
}
