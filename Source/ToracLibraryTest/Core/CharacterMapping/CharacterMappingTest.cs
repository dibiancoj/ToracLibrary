using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var ResultOfCall = new HashSet<char>(AllAlphaBetCharactersLazy());

            //holds an independent string incase characters.constant gets modified by accident
            const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

            //we are first going to test the constant in the characters.cs module to make sure we are in sync. 
            Assert.AreEqual(Alphabet, AlphabetCharacters);

            //test all the characters now (we are not going to call Characters
            foreach (var RequiredCharacter in Alphabet)
            {
                //make sure its there
                Assert.IsTrue(ResultOfCall.Contains(RequiredCharacter));
            }
        }

    }

}