using System.Linq;
using NUnit.Framework;

namespace RouteFinderLibrary.Tests
{
    public class ShuffleTests
    {
        [Test]
        public void ShuffleUsingTestGenerator()
        {
            var testGenerator = new DelegateRandomGenerator(maxValue =>
            {
                Assert.IsTrue(maxValue > 0);
                // shifting to one position
                var nextValue = maxValue - 2;
                return nextValue >= 0
                    ? nextValue 
                    : 0;
            });

            var items = Enumerable.Range(0, 100).ToList();
            var shuffledItems = items.Shuffle(testGenerator).ToList();
            CollectionAssert.AreEquivalent(items, shuffledItems);
            CollectionAssert.AreNotEqual(items, shuffledItems);
        }

        [Test]
        public void ShuffleUsingDefaultGenerator()
        {
            var items = Enumerable.Range(0, 100).ToList();
            var shuffledItems = items.Shuffle().ToList();
            CollectionAssert.AreEquivalent(items, shuffledItems);
            // There is a chance for shuffled collection to be equal to the sorted one, so can't check AreNotEqual
        }
    }
}
