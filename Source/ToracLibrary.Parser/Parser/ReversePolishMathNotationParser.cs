using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Exceptions;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OrderTokens;

namespace ToracLibrary.Parser.Parser
{

    /// <summary>
    /// When doing mathematical parsing we need to account for multiplication and adding and order of operation. We handle this by using Reverse Polish Notation which doesn't care and account for it. Then we can continue on parsing
    /// </summary>
    /// <remarks>Also known as In Fix To Post Fix</remarks>
    public static class ReversePolishMathNotationParser
    {

        #region Public Methods

        /* Definition For Now */
        // Expression := Number {Operator Number}
        // Operator   := "+" | "-"
        // Number     := Digit{Digit}
        // Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

        /// <summary>
        /// Convert the expresssion and evaulate it to the result
        /// </summary>
        /// <param name="ReversePolishNotationTokens">Reverse tokens</param>
        /// <returns>Result</returns>
        public static double EvaluateExpression(IEnumerable<TokenBase> ReversePolishNotationTokens)
        {
            //holds the temp data structure to calculate the result
            var ResultStack = new Stack<double>();

            //loop through all the values
            foreach (var Token in ReversePolishNotationTokens)
            {
                //if this is a number then add it to the stack
                if (Token is NumberLiteralToken)
                {
                    //add the number to the stack
                    ResultStack.Push(((NumberLiteralToken)Token).Value);
                }
                else if (Token is OperatorBaseToken)
                {
                    //this is a operator. all of them deal with 2 numbers
                    if (ResultStack.Count < 2)
                    {
                        throw new ArgumentOutOfRangeException("There should be atleast 2 items in the stack. There is a parsing issue somewhere");
                    }

                    //grab the 2 numbers we need
                    var FirstNumber = ResultStack.Pop();
                    var SecondNumber = ResultStack.Pop();

                    //add the result to the stack
                    ResultStack.Push(((OperatorBaseToken)Token).Calculate(FirstNumber, SecondNumber));
                }
                else
                {
                    if (Token is LeftParenthesisOrderToken)
                    {
                        throw new MissingParenthesisException("Right ')'");
                    }
                    else if (Token is RightParenthesisOrderToken)
                    {
                        throw new MissingParenthesisException("Left ')'");
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Invalid Token Found. Token: " + Token.GetType().Name);
                    }
                }
            }

            //return the result
            return ResultStack.Pop();
        }

        /// <summary>
        /// Convert the tree into a reverse polish notation tree
        /// </summary>
        /// <param name="TokenizedTree">Tree to convert</param>
        /// <returns>The converted values</returns>
        public static IEnumerable<TokenBase> ConvertToReversePolishNotationLazy(IEnumerable<TokenBase> TokenizedTree)
        {
            //2 + 3 * 7
            //turns into 
            //2 3 7 * +

            //http://www.meta-calculator.com/learning-lab/how-to-build-scientific-calculator/infix-to-postifix-convertor.php

            //this is also known as InFix To Post Fix

            //need to keep track of the operators
            var OperatorStack = new Stack<OrderBaseToken>();

            //use an enumerator so we can peek
            using (var Reader = TokenizedTree.GetEnumerator())
            {
                //keep reading until we are the end of the stream
                while (Reader.MoveNext())
                {
                    //is this a number?
                    if (Reader.Current is NumberLiteralToken)
                    {
                        //this is a number, we want the numbers to be first.
                        yield return Reader.Current;
                    }
                    else if (Reader.Current is OperatorBaseToken)
                    {
                        //cast this into an operator
                        var CastedOperator = (OperatorBaseToken)Reader.Current;

                        //this is an operator (+,-,*,/)
                        //do we have any items (or does this item have to go before any other operator items. ie: + has to go after *)
                        if (OperatorStack.Count != 0 && NeedsToGoBeforePreviousOperator(OperatorStack.Peek(), CastedOperator))
                        {
                            //Remove the top item so we can adjust it
                            var ArrivalOfTopItem = OperatorStack.Pop();

                            //loop until we get to the correct spot
                            while (NeedsToGoBeforePreviousOperator(ArrivalOfTopItem, CastedOperator))
                            {
                                //we are ok now...return this guy
                                yield return ArrivalOfTopItem;

                                //if we have 0 items at this point then continue on
                                if (OperatorStack.Count == 0)
                                {
                                    break;
                                }

                                //remove the next guy from the top
                                ArrivalOfTopItem = OperatorStack.Pop();
                            }

                            //just add this operator to the stack
                            OperatorStack.Push(CastedOperator);
                        }
                        else
                        {
                            //just add this operator to the stack
                            OperatorStack.Push(CastedOperator);
                        }
                    }
                    else if (Reader.Current is OrderBaseToken)
                    {
                        var CastedOrderBaseToken = Reader.Current as OrderBaseToken;

                        //which order base?
                        if (CastedOrderBaseToken is LeftParenthesisOrderToken)
                        {
                            //this is a left parenthesis so we add this to the stack
                            OperatorStack.Push(CastedOrderBaseToken);
                        }
                        else if (CastedOrderBaseToken is RightParenthesisOrderToken)
                        {
                            //top order by
                            var TakeFromTop = OperatorStack.Pop();

                            //loop until we aren't at the left parenthesis
                            while (!(TakeFromTop is LeftParenthesisOrderToken))
                            {
                                //return this item
                                yield return TakeFromTop;

                                //keep popping until we get to something else
                                //if we have no items then we have issues with a missing left parenthesis
                                if (!OperatorStack.Any())
                                {
                                    throw new MissingParenthesisException("Left ')'");
                                }

                                //grab from the top again and keep going until we get to the left parenthesis
                                TakeFromTop = OperatorStack.Pop();
                            }
                        }
                    }
                }
            }

            //return all the operators at this point
            while (OperatorStack.Count > 0)
            {
                //remove and return it
                yield return OperatorStack.Pop();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Does the second operator need to go before the first operator
        /// </summary>
        /// <param name="FirstOperator">First Operator</param>
        /// <param name="SecondOperator">Second Operator</param>
        /// <returns>Does this need to go before the first operator</returns>
        private static bool NeedsToGoBeforePreviousOperator(OrderBaseToken FirstOperator, OrderBaseToken SecondOperator)
        {
            //make sure the multiple and divide go before the add
            //should go in this order +-*/
            return FirstOperator.OrderOfPresedence >= SecondOperator.OrderOfPresedence;
        }

        #endregion

    }

}
