using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static ToracLibrary.Core.CharacterMapping.Characters;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to test the character mapping functionality
    /// </summary>
    public class CharacterMappingTest
    {

        /// <summary>
        /// Make sure the numeric numbers return are correct
        /// </summary>
        [Fact]
        public void NumericCharactersTest1()
        {
            //going to hard code this test
            var DigitsToTest = AllNumberCharactersLazy().OrderBy(x => x).ToArray();

            //start testing this. going to do this manually to ensure everything is correct
            for (int i = 0; i < 10; i++)
            {
                //test the result
                Assert.Equal(i, DigitsToTest[i]);
            }
        }

        /// <summary>
        /// Make sure the alphabet numbers return are correct
        /// </summary>
        [Fact]
        public void AlphabetCharactersTest1()
        {
            //loop through all the characters and test it
            var ResultOfCall = new HashSet<char>(AllAlphaBetCharacters());

            //holds an independent string incase characters.constant gets modified by accident
            const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

            //we are first going to test the constant in the characters.cs module to make sure we are in sync. 
            Assert.Equal(Alphabet, AlphabetCharacters);

            //test all the characters now (we are not going to call Characters
            foreach (var RequiredCharacter in Alphabet)
            {
                //make sure its there
                Assert.True(ResultOfCall.Contains(RequiredCharacter));
            }
        }

    }

}