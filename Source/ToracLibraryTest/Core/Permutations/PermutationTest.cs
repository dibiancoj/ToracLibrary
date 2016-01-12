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

        #region Permutation List

        /// <summary>
        /// Test Permutation for a given set of characters with an exclusive character once it's used
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void PermutationTestExclusive1()
        {
            //choices we can use
            var Choices = new string[] { "a", "b", "c" };

            //length we are going to use
            const int LengthToTest = 2;

            //is exclusive
            const bool IsExclusive = true;

            //go build the result
            var Result = PermutationBuilder.BuildPermutationListLazy(Choices, LengthToTest, IsExclusive).ToArray();

            //make sure there are 6 sets
            Assert.AreEqual(6, Result.Length);

            //make sure there are 2 elements for each item
            foreach (var ResultDimension in Result)
            {
                //make sure the items in this container are 2
                Assert.AreEqual(2, ResultDimension.PermutationItems.Count());
            }

            //we are going to test this against the total number of choices method to make sure everything is in synch. what is returned here matches the total number of choices in the other method
            Assert.AreEqual(Result.LongCount(), PermutationBuilder.TotalNumberOfPermutationCombinations(Choices, LengthToTest, IsExclusive));

            //check the overload with the count
            Assert.AreEqual(Result.LongCount(), PermutationBuilder.TotalNumberOfPermutationCombinations(Choices.Length, LengthToTest, IsExclusive));

            //now look through the values
            Assert.AreEqual("ab", Result[0].PermutationItemsTogether());
            Assert.AreEqual("ac", Result[1].PermutationItemsTogether());
            Assert.AreEqual("ba", Result[2].PermutationItemsTogether());
            Assert.AreEqual("bc", Result[3].PermutationItemsTogether());
            Assert.AreEqual("ca", Result[4].PermutationItemsTogether());
            Assert.AreEqual("cb", Result[5].PermutationItemsTogether());
        }

        /// <summary>
        /// Test Permutation for a given set of characters that is not exclusive
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void PermutationTestNotExclusive1()
        {
            //choices we can use
            var Choices = new string[] { "a", "b", "c" };

            //length we are going to use
            const int LengthToTest = 2;

            //is exclusive
            const bool IsExclusive = false;

            //go build the result
            var Result = PermutationBuilder.BuildPermutationListLazy(Choices, LengthToTest, IsExclusive).ToArray();

            //make sure there are 6 sets
            Assert.AreEqual(9, Result.Length);

            //make sure there are 2 elements for each item
            foreach (var ResultDimension in Result)
            {
                //make sure the items in this container are 2
                Assert.AreEqual(2, ResultDimension.PermutationItems.Count());
            }

            //we are going to test this against the total number of choices method to make sure everything is in synch. what is returned here matches the total number of choices in the other method
            Assert.AreEqual(Result.LongCount(), PermutationBuilder.TotalNumberOfPermutationCombinations(Choices, LengthToTest, IsExclusive));

            //check the overload with the count
            Assert.AreEqual(Result.LongCount(), PermutationBuilder.TotalNumberOfPermutationCombinations(Choices.Length, LengthToTest, IsExclusive));

            //now look through the values
            Assert.AreEqual("aa", Result[0].PermutationItemsTogether());
            Assert.AreEqual("ab", Result[1].PermutationItemsTogether());
            Assert.AreEqual("ac", Result[2].PermutationItemsTogether());
            Assert.AreEqual("ba", Result[3].PermutationItemsTogether());
            Assert.AreEqual("bb", Result[4].PermutationItemsTogether());
            Assert.AreEqual("bc", Result[5].PermutationItemsTogether());
            Assert.AreEqual("ca", Result[6].PermutationItemsTogether());
            Assert.AreEqual("cb", Result[7].PermutationItemsTogether());
            Assert.AreEqual("cc", Result[8].PermutationItemsTogether());
        }

        #endregion

        #region Permutation Total Choices

        /// <summary>
        /// Test total number of permutation 
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void TotalNumberOfPermutationsNotExclusive1()
        {
            //parameters for test 1
            var Test1Params = new
            {
                Choices = new string[] { "a", "b", "c" },
                Length = 2,
                IsExclusive = false,
                TestResultShouldBe = 3 * 3
            };

            //parameters for test 2
            var Test2Params = new
            {
                Choices = new string[] { "a", "b", "c", "d" },
                Length = 3,
                IsExclusive = false,
                TestResultShouldBe = 4 * 4 * 4
            };

            //go test different scenario. basic formula = (number of characters) * (number of characters) * (number of characters) keep multiplying until you get the length which is the 2nd parameter in the method
            //test 1
            Assert.AreEqual(Test1Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test1Params.Choices, Test1Params.Length, Test1Params.IsExclusive));

            //test 1 - check overload
            Assert.AreEqual(Test1Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test1Params.Choices.Length, Test1Params.Length, Test1Params.IsExclusive));


            //test 2
            Assert.AreEqual(Test2Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test2Params.Choices, Test2Params.Length, Test2Params.IsExclusive));

            //test 2 - check overload
            Assert.AreEqual(Test2Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test2Params.Choices.Length, Test2Params.Length, Test2Params.IsExclusive));
        }

        /// <summary>
        /// Test total number of permutation 
        /// </summary>
        [TestCategory("Core.Permutation")]
        [TestCategory("Core")]
        [TestMethod]
        public void TotalNumberOfPermutationsExclusive1()
        {
            //parameters for test 1
            var Test1Params = new
            {
                Choices = new string[] { "a", "b", "c" },
                Length = 2,
                IsExclusive = true,
                TestResultShouldBe = 3 * 2
            };

            //parameters for test 2
            var Test2Params = new
            {
                Choices = new string[] { "a", "b", "c", "d" },
                Length = 3,
                IsExclusive = true,
                TestResultShouldBe = 4 * 3 * 2
            };

            //go test different scenario. basic formula = (number of characters) * (number of characters) * (number of characters) keep multiplying until you get the length which is the 2nd parameter in the method
            //test 1
            Assert.AreEqual(Test1Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test1Params.Choices, Test1Params.Length, Test1Params.IsExclusive));

            //test 1 - check overload
            Assert.AreEqual(Test1Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test1Params.Choices.Length, Test1Params.Length, Test1Params.IsExclusive));


            //test 2
            Assert.AreEqual(Test2Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test2Params.Choices, Test2Params.Length, Test2Params.IsExclusive));

            //test 2 - check overload
            Assert.AreEqual(Test2Params.TestResultShouldBe, PermutationBuilder.TotalNumberOfPermutationCombinations(Test2Params.Choices.Length, Test2Params.Length, Test2Params.IsExclusive));
        }

        #endregion

    }

}
