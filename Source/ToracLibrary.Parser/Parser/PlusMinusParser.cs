using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Parser.Tokenizer.Tokens;
using ToracLibrary.Parser.Tokenizer.Tokens.OperatorTokens;

namespace ToracLibrary.Parser.Parser
{

    /// <summary>
    /// Takes the tokens and parses the values
    /// </summary>
    public class PlusMinusParser
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TokensToSet">Tokens found in expression</param>
        public PlusMinusParser(IEnumerable<TokenBase> TokensToSet)
        {
            Tokens = TokensToSet.GetEnumerator();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the tokens in the expression
        /// </summary>
        private IEnumerator<TokenBase> Tokens { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parse the tokens and return the result of the expression
        /// </summary>
        /// <returns>the calculated value</returns>
        public int Parse()
        {
            //holds the result of the expression
            int ResultOfExpression = 0;

            //loop through all the tokens
            while (Tokens.MoveNext())
            {
                //parse this value
                ResultOfExpression = ParseExpression();

                //can we move next?
                if (Tokens.MoveNext())
                {

                    //is this an operator token?
                    if (Tokens.Current is OperatorBaseToken)
                    {
                        //grab the current token
                        var OperationToExecute = Tokens.Current;

                        //parse the second number
                        var SecondNumber = Parse();

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

                        //we shouldn't get here
                        throw new ArgumentOutOfRangeException("Unsupported Operator: " + OperationToExecute);
                    }

                    //what are we trying to do with this token?
                    throw new ArgumentOutOfRangeException("Expecting Operator After Expression, But Got " + Tokens.Current);
                }
            }

            //return the result
            return ResultOfExpression;
        }

        #endregion

        #region Private Methods

        private int ParseExpression()
        {
            var number = ParseNumber();
            if (!Tokens.MoveNext())
                return number;

            if (Tokens.Current is OperatorBaseToken)
            {
                var op = Tokens.Current;
                Tokens.MoveNext();

                var secondNumber = ParseNumber();
                if (op is PlusToken)
                {
                    return number + secondNumber;
                }
                if (op is MinusToken)
                    return number - secondNumber;

                throw new Exception("Unsupported operator: " + op);
            }

            throw new Exception("Expecting operator after number, but got " + Tokens.Current);
        }

        /// <summary>
        /// Parse the number for the given token
        /// </summary>
        /// <returns></returns>
        private int ParseNumber()
        {
            if (Tokens.Current is NumberConstantToken)
            {
                return (Tokens.Current as NumberConstantToken).Value;
            }

            throw new Exception("Expected a number constant but found " + Tokens.Current);
        }

        #endregion

    }

}
