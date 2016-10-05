﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Parser
{

    /// <summary>
    /// When doing mathematical parsing we need to account for multiplication and adding and order of operation. We handle this by using Reverse Polish Notation which doesn't care and account for it. Then we can continue on parsing
    /// </summary>
    public static class ReversePolishNotationParser
    {

        #region Public Methods

        /// <summary>
        /// Convert the expresssion and evaulate it to the result
        /// </summary>
        /// <param name="ReversePolishNotationTokens">Reverse tokens</param>
        /// <returns>Result</returns>
        public static double EvaluateExpression(IEnumerable<TokenBase> ReversePolishNotationTokens)
        {

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

            //this is also known as InFix To Post Fix

            //need to keep track of the operators
            var OperatorStack = new Stack<OperatorBaseToken>();

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
        private static bool NeedsToGoBeforePreviousOperator(OperatorBaseToken FirstOperator, OperatorBaseToken SecondOperator)
        {
            //make sure the multiple and divide go before the add
            //should go in this order +-*/
            return FirstOperator.OrderOfPresedence > SecondOperator.OrderOfPresedence;
        }

        #endregion

    }

}
