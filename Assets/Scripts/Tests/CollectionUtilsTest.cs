using Jerre.Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class CollectionUtilsTest
    {
        [Test]
        public void ExtractRandomValues()
        {
            var list = new List<int>
            {
                1,
                2,
                3
            };
            var extractedValues = CollectionUtils.ExtractRandomValues(list, 2);
            Assert.AreEqual(extractedValues.Length, 2);
            Assert.IsTrue(ArrayUtils.CountOccurrences(1, extractedValues) <= 1);
            Assert.IsTrue(ArrayUtils.CountOccurrences(2, extractedValues) <= 1);
            Assert.IsTrue(ArrayUtils.CountOccurrences(3, extractedValues) <= 1);
        }

        [Test]
        public void ExtractRandomValues_empty_input()
        {
            var list = new List<int>
            {
                1,
                2,
                3
            };
            var extractedValues = CollectionUtils.ExtractRandomValues(list, 0);
            Assert.AreEqual(extractedValues.Length, 0);
        }

        [Test]
        public void ExtractRandomValues_all_values()
        {
            var list = new List<int>
            {
                1,
                2,
                3
            };
            var extractedValues = CollectionUtils.ExtractRandomValues(list, 3);
            Assert.AreEqual(extractedValues.Length, 3);
            Assert.AreEqual(1, ArrayUtils.CountOccurrences(1, extractedValues));
            Assert.AreEqual(1, ArrayUtils.CountOccurrences(2, extractedValues));
            Assert.AreEqual(1, ArrayUtils.CountOccurrences(3, extractedValues));
        }
        
    }
}
