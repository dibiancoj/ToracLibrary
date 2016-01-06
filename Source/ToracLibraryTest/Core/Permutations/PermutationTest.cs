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
        /// Test Permutation for a given set of characters
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void PermutationTest1()
        {
            //go build the result
            var Result = PermutationBuilder.BuildPermutationListLazy(new string[] { "a", "b", "c" }, 2).ToArray();

            //make sure there are 6 sets
            Assert.AreEqual(6, Result.Length);

            //make sure there are 2 elements for each item
            foreach(var ResultDimension in Result)
            {
                //make sure the items in this container are 2
                Assert.AreEqual(2, ResultDimension.Count());
            }

            //now look through the values
            Assert.AreEqual("a", Result[0].ElementAt(0));
            Assert.AreEqual("b", Result[0].ElementAt(1));

            Assert.AreEqual("a", Result[1].ElementAt(0));
            Assert.AreEqual("c", Result[1].ElementAt(1));

            Assert.AreEqual("b", Result[2].ElementAt(0));
            Assert.AreEqual("a", Result[2].ElementAt(1));

            Assert.AreEqual("b", Result[3].ElementAt(0));
            Assert.AreEqual("c", Result[3].ElementAt(1));

            Assert.AreEqual("c", Result[4].ElementAt(0));
            Assert.AreEqual("a", Result[4].ElementAt(1));

            Assert.AreEqual("c", Result[5].ElementAt(0));
            Assert.AreEqual("b", Result[5].ElementAt(1));
        }

    }

}