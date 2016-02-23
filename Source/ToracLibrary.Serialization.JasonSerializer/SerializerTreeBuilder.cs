using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes;

namespace ToracLibrary.Serialization.JasonSerializer
{

    /// <summary>
    /// Tree Builder Which is used to create the expression tree to serialize an object
    /// </summary>
    internal class SerializerTreeBuilder
    {

        #region Private Immutable Static Variables

        private static readonly ConstantExpression StartObject = Expression.Constant("{");
        private static readonly ConstantExpression EndObject = Expression.Constant("}");
        private static readonly ConstantExpression QuoteLiteral = Expression.Constant(@"""");
        //private static readonly ConstantExpression Colon = Expression.Constant(":");
        private static readonly ConstantExpression Comma = Expression.Constant(",");
        private static readonly ConstantExpression NullCheckExpression = Expression.Constant(null);
        private static readonly ConstantExpression NullOutputExpression = Expression.Constant("null");

        private static readonly string QuoteStringLiteral = @"""";
        private static readonly string QuoteAndColonStringLiteral = @""":";

        /// <summary>
        /// Used for the common writes since a static readonly property will be faster then the dictionary lookup
        /// </summary>
        private static readonly MethodInfo AppendString = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) });

        #endregion

        #region Static Internal Methods

        /// <summary>
        /// This is the entry point for the recursion through each object. BuildSingleItemTree will actually build the object
        /// </summary>
        /// <typeparam name="TClass">Class to serialize</typeparam>
        /// <param name="TypeLookup">Lookup values for each primitive type</param>
        /// <returns>Expression tree to use to serialize the object</returns>
        internal static Expression<Func<TClass, JasonSerializerContainer, string>> SerializeBuilder<TClass>(Dictionary<Type, BasePrimitiveTypeOutput> TypeLookup) where TClass : class
        {
            //grab the type we want to serliaze
            var TypeOfT = typeof(TClass);

            //declare the parameter into the lambda
            var TypeToSerialize = Expression.Parameter(TypeOfT, "x");

            //declare the serializer so we can call it for the ienumerable objects
            var SerializerArgument = Expression.Parameter(typeof(JasonSerializerContainer), "serializer");

            //create the string builder which will write the json
            var StringBuilderVariable = Expression.New(typeof(StringBuilder));

            //brab the properties for this type...so we can start the recursion
            var PropertiesOfFirstObject = TypeOfT.GetProperties();

            //go start building the first tree node
            var WorkingExpression = BuildSingleItemTree(TypeLookup, PropertiesOfFirstObject, StringBuilderVariable, null, TypeToSerialize, SerializerArgument);

            //when we are done we are going to call ToString() so we can return the json
            WorkingExpression = Expression.Call(WorkingExpression, typeof(StringBuilder).GetMethod("ToString", new Type[0] { }));

            //go create the lambda and return it
            return Expression.Lambda<Func<TClass, JasonSerializerContainer, string>>(WorkingExpression, new ParameterExpression[] { TypeToSerialize, SerializerArgument });
        }

        #endregion

        #region Single Item Tree Builder

        /// <summary>
        /// Builds a single tree item and returns it to build off of
        /// </summary>
        /// <param name="TypeLookup">Lookup values for each primitive type</param>
        /// <param name="Properties">properties of the object to serialize</param>
        /// <param name="StringBuilderVariable">string builder expression to write too</param>
        /// <param name="WorkingExpression">working expression. Which will change from the recursion. null of first call as it starts the tree building</param>
        /// <param name="TypeToSerialize">Type to serialize</param>
        /// <param name="SerializerArgument">The JsonSerializer object as a parameter into the func</param>
        /// <returns>Expression to build off of</returns>
        private static Expression BuildSingleItemTree(Dictionary<Type, BasePrimitiveTypeOutput> TypeLookup, PropertyInfo[] Properties, NewExpression StringBuilderVariable, Expression WorkingExpression, Expression TypeToSerialize, ParameterExpression SerializerArgument)
        {
            //which expression do we want to build off of. Is this the first call?
            Expression expressionToUse = WorkingExpression ?? StringBuilderVariable;

            //go append the start object "{"
            WorkingExpression = Expression.Call(expressionToUse, AppendString, StartObject);

            //loop through the properties and add them to the tree
            for (int i = 0; i < Properties.Length; i++)
            {
                //throw this into it's own variable
                var CurrentPropertyToSerialize = Properties[i];

                //grab the property into a property expression
                var PropertyGetter = Expression.Property(TypeToSerialize, CurrentPropertyToSerialize);

                //combine the property name with the quotes and the colon. This way we make less calls at runtime. This is much faster then doing 3 expression.calls
                var PropertyNameLeftHandInJson = Expression.Constant(QuoteStringLiteral + CurrentPropertyToSerialize.Name + QuoteAndColonStringLiteral);

                //add the 2nd quote for the property name...and the : in 1 call to make it less calls
                WorkingExpression = Expression.Call(WorkingExpression, AppendString, PropertyNameLeftHandInJson);

                //write the value. Is this a primitive type?
                if (TypeLookup.ContainsKey(CurrentPropertyToSerialize.PropertyType))
                {
                    //go grab the type lookup
                    var TypeReference = TypeLookup[CurrentPropertyToSerialize.PropertyType];

                    //append method to use
                    var appendMethodToUse = TypeReference.StringBuilderWriteMethod;

                    //if a string we need to add a quote
                    if (TypeReference.NeedsQuotesAroundValue)
                    {
                        WorkingExpression = Expression.Call(WorkingExpression, AppendString, QuoteLiteral);
                    }

                    //go build the value expression we want to output
                    Expression ValueToOutput = TypeReference.OutputValue(PropertyGetter);

                    //append the property name to the string builder
                    WorkingExpression = Expression.Call(WorkingExpression, appendMethodToUse, ValueToOutput);

                    //if string add the end quote
                    if (TypeReference.NeedsQuotesAroundValue)
                    {
                        WorkingExpression = Expression.Call(WorkingExpression, AppendString, QuoteLiteral);
                    }
                }
                else if (IsEnumerableLikeType(CurrentPropertyToSerialize.PropertyType))
                {
                    //is array
                    var TypeOfElements = CurrentPropertyToSerialize.PropertyType.IsGenericType ? CurrentPropertyToSerialize.PropertyType.GetGenericArguments()[0] : CurrentPropertyToSerialize.PropertyType.GetElementType();

                    //go call the ienumerable overload (add the null check overload)
                    WorkingExpression = Expression.Condition(Expression.Equal(PropertyGetter, NullCheckExpression),
                        Expression.Call(WorkingExpression, AppendString, NullOutputExpression),
                        Expression.Call(WorkingExpression, AppendString, Expression.Call(SerializerArgument, typeof(JasonSerializerContainer).GetMethod(nameof(JasonSerializerContainer.SerializeIEnumerable), BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(TypeOfElements), PropertyGetter)));
                }
                else
                {
                    //we are at an object here...we need to keep building out of this object recursively

                    //expression condition returns the actual result of the if statement. ExpressionIfThen.. returns a void which we don't want to use

                    //basically
                    //if (property == null)
                    //{
                    //  Add "null"
                    //}
                    //else
                    //{
                    //  Add the rest of the object's properties to serialize
                    //}

                    WorkingExpression = Expression.Condition(Expression.Equal(PropertyGetter, NullCheckExpression),
                        Expression.Call(WorkingExpression, AppendString, NullOutputExpression),
                        BuildSingleItemTree(TypeLookup, CurrentPropertyToSerialize.PropertyType.GetProperties(), StringBuilderVariable, WorkingExpression, PropertyGetter, SerializerArgument));
                }

                //add the comma (only if it's not the last property)
                if (i < Properties.Length - 1)
                {
                    WorkingExpression = Expression.Call(WorkingExpression, AppendString, Comma);
                }
            }

            //add the end object string "}"
            WorkingExpression = Expression.Call(WorkingExpression, AppendString, EndObject);

            //return the expression
            return WorkingExpression;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Is this an Enumerable. Handles arrays which are a little bit different in how they are stored
        /// </summary>
        /// <param name="TypeToCheck">type to check</param>
        /// <returns>is enumerable</returns>
        internal static bool IsEnumerableLikeType(Type TypeToCheck)
        {
            return TypeToCheck.IsArray || (TypeToCheck.IsGenericType && TypeToCheck.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        /// <summary>
        /// what is the T of each element in the enumerable
        /// </summary>
        /// <param name="EnumerableTypeToCheck">type of the enumerable</param>
        /// <returns>Type of each element in the enumerable type</returns>
        internal static Type ElementTypeInEnumerable(Type EnumerableTypeToCheck)
        {
            return EnumerableTypeToCheck.IsArray ? EnumerableTypeToCheck.GetElementType() : EnumerableTypeToCheck.GetGenericArguments()[0];
        }

        /// <summary>
        /// is this a primitive type. Does it get outputted in a non object in json?
        /// </summary>
        /// <param name="type">type to check.</param>
        /// <returns>is primitive type</returns>
        private static bool IsPrimitiveType(Type TypeToCheck)
        {
            //this is only what we handle now
            return TypeToCheck == typeof(string) || TypeToCheck == typeof(int) || TypeToCheck == typeof(DateTime);
        }

        #endregion

    }


}
