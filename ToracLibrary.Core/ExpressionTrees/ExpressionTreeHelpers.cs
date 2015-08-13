using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExpressionTrees
{

    /// <summary>
    /// Holds expression tree helpers
    /// </summary>
    public class ExpressionTreeHelpers
    {

        #region New Item From Expression Tree

        /// <summary>
        /// Builds a new instance using an expression tree
        /// </summary>
        /// <param name="ConstructorInfoOfNewType">Constructor info for the new type. Using typeof(T).GetConstructors().First()</param>
        /// <param name="ConstructorParameter">Parameters of the contructor. So the parameters you would pass into the constructor</param>
        /// <returns>Func which creates the new object</returns>
        /// <remarks>Be sure to cache this if you are going to need it again. Call .Compile() to grab the func</remarks>
        public static Expression<Func<object[], object>> BuildNewObject(ConstructorInfo ConstructorInfoOfNewType, IEnumerable<ParameterInfo> ConstructorParameter)
        {
            //so instead of having to modify the expression you get back. We will use func(params object[] ConstructorParameters)
            //so we basically say for ConstructorParameters[0], ConstructorParameters[1], etc. 
            //this allows us to always pass back the same signature for the funct. Otherwise we would have to spit out which parmeters to get back

            //build the constructor parameter
            var ConstructorParameterName = Expression.Parameter(typeof(object[]), "args");

            //We are going build up all the types that the constructor takes
            var ConstructorParameterTypes = ConstructorParameter.Select(x => x.ParameterType).Select((t, i) => Expression.Convert(Expression.ArrayIndex(ConstructorParameterName, Expression.Constant(i)), t)).ToArray();

            //now build the "New Object" expression
            var NewObjectExpression = Expression.New(ConstructorInfoOfNewType, ConstructorParameterTypes);

            //now let's build the lambda and return it
            return Expression.Lambda<Func<object[], object>>(NewObjectExpression, ConstructorParameterName);
        }

        #endregion

    }

}
