/*
This really wasn't done for the purpose of building a json serializer. More for just playing around with expression tree's.
The serializer is really limited. I stopped once i got 1 object to serialize. However, using expression tree's this is faster then json.net by 50%!
*/


using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PerfSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var timingsMe = new List<double>();
            var timingsJson = new List<double>();

            var data = Tester.Build(100).ToArray();

            /*
                [
                    {"Id":0,"Txt":"0"},
                    {"Id":1,"Txt":"1"},
                    {"Id":2,"Txt":"2"},
                    {"Id":3,"Txt":"3"},
                    {"Id":4,"Txt":"4"}
                ]
            */

            var z = new Jason();

            var result = z.SerializeJson(data.ElementAt(0));

            //warm up json .net
            var jsonNetResultWarmUp = JsonConvert.SerializeObject(data.ElementAt(0));

            //Func<Tester, string> x = t =>
            // {
            //     var sb = new StringBuilder();

            //     sb.Append("{");

            //     sb.AppendFormat("{0}", "Id").Append(':').Append(t.Id);
            //     sb.Append(',');

            //     sb.Append("}");

            //     return sb.ToString();
            // };

            var testObject = data;

            Console.WriteLine("Starting Json.Net");

            for (int i = 0; i < 100; i++)
            {
                var sw = Stopwatch.StartNew();

                var jsonNetResult = JsonConvert.SerializeObject(testObject);

                sw.Stop();
                timingsJson.Add(sw.ElapsedTicks);
            }

            Console.WriteLine("Starting Mine");


            for (int i = 0; i < 100; i++)
            {
                var sb = new StringBuilder();
                var sw = Stopwatch.StartNew();

                var serializer = z.Cache.ElementAt(0).Value as Func<Tester, string>;

                foreach(var t in testObject)
                {
                    sb.Append(serializer(t));
                }

                sw.Stop();
                timingsMe.Add(sw.ElapsedTicks);
            }

            Console.WriteLine("Json.Net - Min: " + timingsJson.Min().ToString("n1"));
            Console.WriteLine("Me - Min: " + timingsMe.Min().ToString("n1"));
            Console.WriteLine();

            Console.ReadLine();
        }
    }

    public class Jason
    {

        #region Constructor

        public Jason()
        {
            Cache = new Dictionary<string, object>();
        }

        #endregion

        #region Instance Properties

        public Dictionary<string, object> Cache { get; set; }

        #endregion

        #region Instance Methods

        public string SerializeJson<T>(T objectToSerialize) where T : class
        {
            //get the func 
            object cacheSerializer;

            if (!Cache.TryGetValue(typeof(T).Name, out cacheSerializer))
            {
                cacheSerializer = SerializeBuilder().Compile();

                Cache.Add(typeof(T).Name, cacheSerializer);
            }

            var castedFunc = ((Func<T, string>)cacheSerializer);

            //if (!(objectToSerialize is IEnumerable))
           // {
                return castedFunc(objectToSerialize);
           // }

            //is an array?


            //var sb = new StringBuilder();
            //sb.Append("[");

            //var enumerator = (objectToSerialize as IEnumerable).GetEnumerator();

            //while (enumerator.MoveNext())
            //{
            //    sb.Append(castedFunc(enumerator.Current as T));

            //    sb.Append(",");
            //}

            //sb.Append("]");

            //return sb.ToString();
        }

        #endregion

        #region Private Immutable Static Variables

        private static readonly ConstantExpression StartObject = Expression.Constant("{");
        private static readonly ConstantExpression EndObject = Expression.Constant("}");
        private static readonly ConstantExpression QuoteLiteral = Expression.Constant(@"""");
        private static readonly ConstantExpression Colon = Expression.Constant(":");
        private static readonly ConstantExpression Comma = Expression.Constant(",");

        #endregion

        #region Private Static Methods

        private static Expression<Func<Tester, string>> SerializeBuilder()
        {
            //declare the parameter into the lambda
            var lambdaArgument = Expression.Parameter(typeof(Tester), "x");
            //var lambdaArgumentStringBuilder = Expression.Parameter(typeof(StringBuilder), "sb");

            //grab the append methods off of the string builder
            var appendInt = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(int) });
            var appendString = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(string) });

            var sb = Expression.New(typeof(StringBuilder));

            var workingExpression = Expression.Call(sb, appendString, StartObject);

            //properties
            var properties = typeof(Tester).GetProperties();

            //grab the properties
            for (int i = 0; i < properties.Length; i++)
            {
                //grab the getter
                //var propertyGetter = typeof(Jason).GetMethod("GetPropertyFunc", new Type[] { prop.PropertyType }).Invoke(null, new object[] { prop.Name });

                var prop = properties[i];

                //grab the property
                var propertyGet = Expression.Property(lambdaArgument, typeof(Tester).GetProperty(prop.Name));

                //add the first quote for the property name
                workingExpression = Expression.Call(workingExpression, appendString, QuoteLiteral);

                //add the property name
                workingExpression = Expression.Call(workingExpression, appendString, Expression.Constant(prop.Name));

                //add the 2nd quote for the property name
                workingExpression = Expression.Call(workingExpression, appendString, QuoteLiteral);

                //add the :
                workingExpression = Expression.Call(workingExpression, appendString, Colon);

                //is string
                var isStringValue = prop.PropertyType == typeof(string);

                //append method to use
                var appendMethodToUse = isStringValue ? appendString : appendInt;

                //if a string we need to add a quote
                if (isStringValue)
                {
                    workingExpression = Expression.Call(workingExpression, appendString, QuoteLiteral);
                }

                //append the property name to the string builder
                workingExpression = Expression.Call(workingExpression, appendMethodToUse, propertyGet);

                //if string add the end quote
                if (isStringValue)
                {
                    workingExpression = Expression.Call(workingExpression, appendString, QuoteLiteral);
                }

                //add the comma
                if (i < properties.Length - 1)
                {
                    workingExpression = Expression.Call(workingExpression, appendString, Comma);
                }
            }

            workingExpression = Expression.Call(workingExpression, appendString, EndObject);

            var ToString = Expression.Call(workingExpression, typeof(StringBuilder).GetMethod("ToString", new Type[0] { }));

            return Expression.Lambda<Func<Tester, string>>(ToString, new ParameterExpression[] { lambdaArgument });
        }

        private static Expression<Func<Tester, TPropertyType>> GetPropertyFunc<TPropertyType>(string propertyName)
        {
            //declare the lambda argument
            ParameterExpression LambdaArgument = Expression.Parameter(typeof(Tester), "x");

            var z = Expression.Property(LambdaArgument, typeof(Tester).GetProperty(propertyName));

            return Expression.Lambda<Func<Tester, TPropertyType>>(z, LambdaArgument);
        }

        #endregion

    }

    public class Tester
    {
        public int Id { get; set; }
        public string Txt { get; set; }

        public static IEnumerable<Tester> Build(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                yield return new Tester { Id = i, Txt = i.ToString() };
            }
        }
    }

}
