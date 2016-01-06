using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.Permutations;
using System.Linq;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to permutations
    /// </summary>
    [TestClass]
    public class PermutationTest
    {

        /// <summary>
        /// Test Permutation for a given set of characters with an exclusive character once it's used
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void PermutationTestExclusive1()
        {
            //go build the result
            var Result = PermutationBuilder.BuildPermutationListLazy(new string[] { "a", "b", "c" }, 2, true).ToArray();

            //make sure there are 6 sets
            Assert.AreEqual(6, Result.Length);

            //make sure there are 2 elements for each item
            foreach (var ResultDimension in Result)
            {
                //make sure the items in this container are 2
                Assert.AreEqual(2, ResultDimension.PermutationItems.Count());
            }

            //now look through the values
            Assert.AreEqual("a", Result[0].PermutationItems[0]);
            Assert.AreEqual("b", Result[0].PermutationItems[1]);

            Assert.AreEqual("a", Result[1].PermutationItems[0]);
            Assert.AreEqual("c", Result[1].PermutationItems[1]);

            Assert.AreEqual("b", Result[2].PermutationItems[0]);
            Assert.AreEqual("a", Result[2].PermutationItems[1]);

            Assert.AreEqual("b", Result[3].PermutationItems[0]);
            Assert.AreEqual("c", Result[3].PermutationItems[1]);

            Assert.AreEqual("c", Result[4].PermutationItems[0]);
            Assert.AreEqual("a", Result[4].PermutationItems[1]);

            Assert.AreEqual("c", Result[5].PermutationItems[0]);
            Assert.AreEqual("b", Result[5].PermutationItems[1]);
        }

        /// <summary>
        /// Test Permutation for a given set of characters that is not exclusive
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void PermutationTestNotExclusive1()
        {
            //go build the result
            var Result = PermutationBuilder.BuildPermutationListLazy(new string[] { "a", "b", "c" }, 2, false).ToArray();

            //make sure there are 6 sets
            Assert.AreEqual(9, Result.Length);

            //make sure there are 2 elements for each item
            foreach (var ResultDimension in Result)
            {
                //make sure the items in this container are 2
                Assert.AreEqual(2, ResultDimension.PermutationItems.Count());
            }

            //now look through the values
            Assert.AreEqual("a", Result[0].PermutationItems[0]);
            Assert.AreEqual("a", Result[0].PermutationItems[1]);

            Assert.AreEqual("a", Result[1].PermutationItems[0]);
            Assert.AreEqual("b", Result[1].PermutationItems[1]);

            Assert.AreEqual("a", Result[2].PermutationItems[0]);
            Assert.AreEqual("c", Result[2].PermutationItems[1]);

            Assert.AreEqual("b", Result[3].PermutationItems[0]);
            Assert.AreEqual("a", Result[3].PermutationItems[1]);

            Assert.AreEqual("b", Result[4].PermutationItems[0]);
            Assert.AreEqual("b", Result[4].PermutationItems[1]);

            Assert.AreEqual("b", Result[5].PermutationItems[0]);
            Assert.AreEqual("c", Result[5].PermutationItems[1]);

            Assert.AreEqual("c", Result[6].PermutationItems[0]);
            Assert.AreEqual("a", Result[6].PermutationItems[1]);

            Assert.AreEqual("c", Result[7].PermutationItems[0]);
            Assert.AreEqual("b", Result[7].PermutationItems[1]);

            Assert.AreEqual("c", Result[8].PermutationItems[0]);
            Assert.AreEqual("c", Result[8].PermutationItems[1]);
        }

    }

}