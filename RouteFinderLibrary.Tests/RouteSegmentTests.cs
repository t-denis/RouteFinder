using System;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace RouteFinderLibrary.Tests
{
    public class RouteSegmentTests
    {
        [Test]
        public void NullStops()
        {
            Assert.DoesNotThrow(() => new RouteSegment("A", "B"));
            Assert.Throws<ArgumentNullException>(() => new RouteSegment(null, "abc"));
            Assert.Throws<ArgumentNullException>(() => new RouteSegment("abc", null));
            Assert.Throws<ArgumentNullException>(() => new RouteSegment(null, null));
        }

        [Test]
        public void OverridenToString()
        {
            var segmentA = new RouteSegment("ABC", "XYZ");
            var segmentB = new RouteSegment("XYZ", "ABC");
            var segmentAString = segmentA.ToString();
            var segmentBString = segmentB.ToString();
            Assert.IsNotNull(segmentAString);
            Assert.IsNotNull(segmentBString);
            StringAssert.AreNotEqualIgnoringCase(segmentAString, segmentBString);
            StringAssert.Contains(segmentA.From, segmentAString);
            StringAssert.Contains(segmentA.To, segmentAString);
            StringAssert.Contains(segmentB.From, segmentBString);
            StringAssert.Contains(segmentB.To, segmentBString);
        }
    }
}
