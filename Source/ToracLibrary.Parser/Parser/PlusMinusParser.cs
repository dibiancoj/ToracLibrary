using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Parser
{

    /// <summary>
    /// Takes the tokens and parses the values
    /// </summary>
    public static class PlusMinusParser
    {

        #region Public Methods

        /// <summary>
        /// Parse the tokens and return the result of the expression
        /// </summary>
        /// <param name="Tokens">Tokens found in expression</param>
        /// <returns>the calculated value</returns>
        public static int Parse(IEnumerable<TokenBase> Tokens)
        {
            //use an enumerator
            using (var Token = Tokens.GetEnumerator())
            {
                //holds the result of the expression
                int ResultOfExpression = 0;

                //loop through all the tokens
                while (Token.MoveNext())
                {
                    //parse this value
                    ResultOfExpression = ParseEquationExpression(Token);

                    //can we move next?
                    if (Token.MoveNext())
                    {
                        //is this an operator token?
                        if (Token.Current is OperatorBaseToken)
                        {
                            //grab the current token
                            var OperationToExecute = Token.Current;

                            //we have the operator token...move to the next item now so we can grab the value to calcualte
                            Token.MoveNext();

                            //parse the second number
                            var SecondNumber = ParseEquationExpression(Token);

                            //are we adding?
                            if (OperationToExecute is PlusToken)
                            {
                                return ResultOfExpression + SecondNumber;
                            }

                            //are we subtracting
                            if (OperationToExecute is MinusToken)
                            {
                                return ResultOfExpression - SecondNumber;
                            }

                            //unsupported token..throw an error
                            throw new UnsupportedOperatorException((OperatorBaseToken)OperationToExecute, new OperatorBaseToken[] { new PlusToken(), new MinusToken() });
                        }

                        //what are we trying to do with this token?
                        throw new ExpectingTokenException(new OperatorBaseToken(), Token.Current);
                    }
                }

                //return the result
                return ResultOfExpression;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a specific equation. {{Number}} {{Operator}} {{Number}}
        /// </summary>
        /// <param name="TokenEnumerator">Token enumerator with the continue enumerator from base method</param>
        /// <returns>The result of the equation</returns>
        private static int ParseEquationExpression(IEnumerator<TokenBase> TokenEnumerator)
        {
            //this is the start of the equation. This should be the start of the literal
            var NumberLiteral = ParseNumber(TokenEnumerator.Current);

            //if we have no more items..then just return the number
            if (!TokenEnumerator.MoveNext())
            {
                return NumberLiteral;
            }

            //if we need to some operations
            if (TokenEnumerator.Current is OperatorBaseToken)
            {
                //which operation should we execute?
                var OperationToExecute = TokenEnumerator.Current;

                //move to the next item in the reader
                TokenEnumerator.MoveNext();

                //this should be the 2nd number now
                var SecondNumberInExpression = ParseNumber(TokenEnumerator.Current);

                //are we adding it
                if (OperationToExecute is PlusToken)
                {
                    //add them and return the expression
                    return NumberLiteral + SecondNumberInExpression;
                }

                //is its subtraction?
                if (OperationToExecute is MinusToken)
                {
                    //subtract the numbers and return it
                    return NumberLiteral - SecondNumberInExpression;
                }

                //we have an operator token that is not supported
                throw new Exception("Unsupported operator: " + OperationToExecute);
            }

            //it should have a number here
            throw new Exception("Expecting operator after number, but got " + TokenEnumerator.Current);
        }

        /// <summary>
        /// Parse the number for the given token
        /// </summary>
        /// <param name="TokenToParse">Token to parse</param>
        /// <returns>The value of the token constant</returns>
        private static int ParseNumber(TokenBase TokenToParse)
        {
            //is this a number constant token?
            if (TokenToParse is NumberConstantToken)
            {
                //it is...return the value
                return ((NumberConstantToken)TokenToParse).Value;
            }

            //we should have a literal..so throw an error
            throw new Exception("Expected a number constant but found " + TokenToParse);
        }

        #endregion

    }

}
