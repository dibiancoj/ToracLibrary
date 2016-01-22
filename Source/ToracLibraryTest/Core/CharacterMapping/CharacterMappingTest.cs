using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ToracLibrary.Core.CharacterMapping.Characters;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test the character mapping functionality
    /// </summary>
    [TestClass]
    public class CharacterMappingTest
    {

        /// <summary>
        /// Make sure the numeric numbers return are correct
        /// </summary>
        [TestCategory("Core.CharacterMapping")]
        [TestCategory("Core")]
        [TestMethod]
        public void NumericCharactersTest1()
        {
            //going to hard code this test
            var DigitsToTest = AllNumberCharactersLazy().OrderBy(x => x).ToArray();

            //start testing this. going to do this manually to ensure everything is correct
            for (int i = 0; i < 10; i++)
            {
                //test the result
                Assert.AreEqual(i, DigitsToTest[i]);
            }

        }

        /// <summary>
        /// Make sure the alphabet numbers return are correct
        /// </summary>
        [TestCategory("Core.CharacterMapping")]
        [TestCategory("Core")]
        [TestMethod]
        public void AlphabetCharactersTest1()
        {
            //loop through all the characters and test it
            var ResultOfCall = AllAlphaBetCharactersLazy().ToArray();

            //characters to test
            var CharactersInAlphaBet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            //test all the characters now
            foreach(var RequiredCharacter in CharactersInAlphaBet)
            {
                //make sure its there
                Assert.IsTrue(ResultOfCall.Any(y => y == RequiredCharacter));
            }
        }

    }

}