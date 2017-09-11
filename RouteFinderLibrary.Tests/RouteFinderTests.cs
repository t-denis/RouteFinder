using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RouteFinderLibrary.Extensions;
using RouteFinderLibrary.Properties;

namespace RouteFinderLibrary.Tests
{
    public class RouteFinderTests
    {
        private List<IRouteSegment> _orderedTestSegments;

        [SetUp]
        public void Init()
        {
            // Explicit creation without cycles is for readability purposes.
            // This should be a correct data, sorted in the required order.
            _orderedTestSegments = new List<IRouteSegment>
            {
                new RouteSegment("1", "2"),
                new RouteSegment("2", "3"),
                new RouteSegment("3", "4"),
                new RouteSegment("4", "5"),
                new RouteSegment("5", "6"),
                new RouteSegment("6", "7"),
                new RouteSegment("7", "8"),
                new RouteSegment("8", "9"),
                new RouteSegment("9", "10"),
            };
        }

        [Test]
        public void ProposedTest()
        {
            var segments = new[]
            {
                new RouteSegment("Melbourne", "Cologne"),
                new RouteSegment("Moscow", "Paris"),
                new RouteSegment("Cologne", "Moscow"),
            };

            var route = segments.OrderByStops().ToList();

            var expected = new[]
            {
                new RouteSegment("Melbourne", "Cologne"),
                new RouteSegment("Cologne", "Moscow"),
                new RouteSegment("Moscow", "Paris"),
            };
            CollectionAssert.AreEqual(expected, route, new RouteSegmentComparer());

            foreach (var item in route)
            {
                Console.WriteLine(item);
            }
        }

        [Test]
        public void CorrectData()
        {
            var shuffledData = _orderedTestSegments.Shuffle();
            var orderedData = shuffledData.OrderByStops().ToList();
            CollectionAssert.AreEqual(_orderedTestSegments, orderedData, new RouteSegmentComparer());
        }

        [Test]
        public void CorrectData_SingleItem()
        {
            var segments = new[] { new RouteSegment("1", "2") };
            var route = segments.OrderByStops().ToList();
            var expected = new[] { new RouteSegment("1", "2") };
            CollectionAssert.AreEqual(expected, route, new RouteSegmentComparer());
        }

        [Test]
        [Description("Framework Design Guidelines for Collections. " +
                     "DO NOT return null values from collection properties or from methods returning " +
                     "collections. Return an empty collection or an empty array instead." +
                     "https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/guidelines-for-collections")]
        public void EmptyInputCollectionProducesEmptyOutputCollection()
        {
            var route = new RouteSegment[0].OrderByStops().ToList();
            Assert.IsNotNull(route);
            CollectionAssert.IsEmpty(route);
        }

        [Test]
        public void NullInput()
        {
            Assert.Throws<ArgumentNullException>(
                // ReSharper disable once AssignNullToNotNullAttribute
                () => ((IEnumerable<RouteSegment>)null).OrderByStops());
        }

        [Test]
        public void NullSegment()
        {
            var segments = new[]
            {
                new RouteSegment("1", "2"),
                null,
                new RouteSegment("2", "3"),
            };

            var ex = Assert.Throws<ArgumentException>(() => segments.OrderByStops());
            Assert.AreEqual(ErrorMessages.NullSegment, ex.Message);
        }

        [Test]
        public void DanglingStartSegment()
        {
            _orderedTestSegments.Add(new RouteSegment("test1", "5"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void DanglingEndSegment()
        {
            _orderedTestSegments.Add(new RouteSegment("5", "test1"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void Repeat()
        {
            _orderedTestSegments.Add(new RouteSegment("2", "3"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void Bypass()
        {
            _orderedTestSegments.Add(new RouteSegment("1", "3"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void ReverseStart()
        {
            _orderedTestSegments.Add(new RouteSegment("2", "1"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void ReverseMiddle()
        {
            _orderedTestSegments.Add(new RouteSegment("5", "4"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void ReverseEnd()
        {
            _orderedTestSegments.Add(new RouteSegment("10", "9"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void JumpBackFromMiddleToStart()
        {
            _orderedTestSegments.Add(new RouteSegment("5", "1"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void JumpBackFromMiddleToMiddle()
        {
            _orderedTestSegments.Add(new RouteSegment("7", "4"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void JumpBackFromEndToMiddle()
        {
            _orderedTestSegments.Add(new RouteSegment("10", "5"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void DanglingCycle()
        {
            _orderedTestSegments.AddRange(new[]
            {
                new RouteSegment("5", "test1"),
                new RouteSegment("test1", "test2"),
                new RouteSegment("test2", "5")
            });
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void SmallCycle()
        {
            var segments = new[]
            {
                new RouteSegment("1", "2"),
                new RouteSegment("2", "1"),
            };
            var ex = Assert.Throws<ArgumentException>(() => segments.OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }
        
        [Test]
        public void Cycle()
        {
            _orderedTestSegments.Add(new RouteSegment("10", "1"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void StandaloneCycle()
        {
            _orderedTestSegments.AddRange(new[]
            {
                new RouteSegment("test1", "test2"),
                new RouteSegment("test2", "test3"),
                new RouteSegment("test3", "test1"),
            });
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesOrGaps, ex.Message);
        }

        [Test]
        public void SelfReferencingSegment()
        {
            _orderedTestSegments.Add(new RouteSegment("5", "5"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void StandaloneSelfReferencingSegment()
        {
            var segments = new[] { new RouteSegment("test1", "test1") };
            var ex = Assert.Throws<ArgumentException>(() => segments.OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesBypassesRepeatsOrDanglingSegments, ex.Message);
        }

        [Test]
        public void Gaps()
        {
            _orderedTestSegments.Add(new RouteSegment("test1", "test2"));
            var ex = Assert.Throws<ArgumentException>(() => _orderedTestSegments.Shuffle().OrderByStops());
            Assert.AreEqual(ErrorMessages.CyclesOrGaps, ex.Message);
        }
    }
}
