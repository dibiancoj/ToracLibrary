using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.TokenFactories;
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

        #region Number Parser

        /// <summary>
        /// Reverse polish notation deals with multiplication and additional with order of operation
        /// </summary>
        /// <param name="Tokens">Tokenized expression that we will polish notation this</param>
        /// <param name="ValidTokens">Valid tokens in this expression</param>
        /// <returns>The adjusted, reversed tokens</returns>
        public static double ReversePolishNotation(IEnumerable<TokenBase> Tokens, ISet<ITokenFactory> ValidTokens)
        {
            //2 + 3 * 7
            //turns into 
            //2 3 7 * +

            //this is also known as InFix To Post Fix

            var PostFix = new List<TokenBase>();

            //stack to use
            var OperatorStack = new Stack<OperatorBaseToken>();

            //use an enumerator
            using (var Reader = Tokens.GetEnumerator())
            {
                //keep reading until we are the end of the stream
                while (Reader.MoveNext())
                {
                    //is this a number?
                    if (Reader.Current is NumberLiteralToken)
                    {
                        PostFix.Add(Reader.Current);
                    }
                    else if (Reader.Current is OperatorBaseToken)
                    {
                        if (OperatorStack.Count != 0 && Predecessor(OperatorStack.Peek(), Reader.Current as OperatorBaseToken))
                        {
                            var arrival = OperatorStack.Pop();
                            while (Predecessor(arrival, Reader.Current as OperatorBaseToken))
                            {
                                PostFix.Add(arrival);

                                if (OperatorStack.Count == 0)
                                    break;

                                arrival = OperatorStack.Pop();
                            }
                            OperatorStack.Push(Reader.Current as OperatorBaseToken);
                        }
                        else
                        {
                            OperatorStack.Push(Reader.Current as OperatorBaseToken);
                        }
                    }
                }
            }

            while (OperatorStack.Count > 0)
            {
                PostFix.Add(OperatorStack.Pop());
            }


            return -10;
            //return StackToCalculate();
        }

        private static bool Predecessor(OperatorBaseToken FirstOperator, OperatorBaseToken SecondOperator)
        {
            if (FirstOperator is MultiplyToken)
            {
                return true;
            }

            return false;

            //basically we want to ask if multiply should be before add or subtract...need to add more logic here
        }

        #endregion

        /// <summary>
        /// Parse the tokens and return the result of the expression
        /// </summary>
        /// <param name="Tokens">Tokens found in expression</param>
        /// <param name="ValidTokens">Supported Tokens for this expression type</param>
        /// <returns>the calculated value</returns>
        public static double Parse(IEnumerable<TokenBase> Tokens, ISet<ITokenFactory> ValidTokens)
        {
            //use an enumerator
            using (var Token = Tokens.GetEnumerator())
            {
                //holds the result of the expression
                double ResultOfExpression = 0;

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

                            //go calculate the operation and return the number
                            return ((OperatorBaseToken)OperationToExecute).Calculate(ResultOfExpression, SecondNumber);
                        }

                        //what are we trying to do with this token?
                        throw new ExpectingTokenException(typeof(OperatorBaseToken), Token.Current);
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
        private static double ParseEquationExpression(IEnumerator<TokenBase> TokenEnumerator)
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

                //go calculate the operation and return the number
                return ((OperatorBaseToken)OperationToExecute).Calculate(NumberLiteral, SecondNumberInExpression);
            }

            //it should have a number here
            throw new Exception("Expecting operator after number, but got " + TokenEnumerator.Current);
        }

        /// <summary>
        /// Parse the number for the given token
        /// </summary>
        /// <param name="TokenToParse">Token to parse</param>
        /// <returns>The value of the token constant</returns>
        private static double ParseNumber(TokenBase TokenToParse)
        {
            //is this a number constant token?
            if (TokenToParse is NumberLiteralToken)
            {
                //it is...return the value
                return ((NumberLiteralToken)TokenToParse).Value;
            }

            //we should have a literal..so throw an error
            throw new Exception("Expected a number constant but found " + TokenToParse);
        }

        #endregion

    }

}
