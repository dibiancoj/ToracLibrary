using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNetMVC.CustomValueProviderFactory
{

    //****there is an known issue with enums. If you pass the enum in as an int (not a string value) it wont deserialize correctly.
    // JsonSerializerToUse.Deserialize<ExpandoObject>(JsonReader); will convert the number to int64. It wont be able to cast it to the enum which uses int32.
    // so write a custom model binder or don't use json.net to deserialize items

    //in Application_Start
    //ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().Single());
    //ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());

    /// <summary>
    /// Allows you to use JsonNet as the default serializer. When incoming request is sent to the controller, json.net will deserialize this guy.
    /// </summary>
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {

        #region Override Methods

        /// <summary>
        /// Override Method
        /// </summary>
        /// <param name="ControllerContext">Controller Context To Use</param>
        /// <returns>IValueProvider</returns>
        public override IValueProvider GetValueProvider(ControllerContext ControllerContext)
        {
            // first make sure we have a valid context
            if (ControllerContext == null)
            {
                throw new ArgumentNullException("ControllerContext");
            }

            // now make sure we are dealing with a json request
            if (!ControllerContext.HttpContext.Request.ContentType.StartsWith(AspNetConstants.JsonContentType, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // get a generic stream reader (get reader for the http stream)
            using (var StreamReaderToUse = new StreamReader(ControllerContext.HttpContext.Request.InputStream))
            {
                // convert stream reader to a JSON Text Reader
                using (var JsonReader = new JsonTextReader(StreamReaderToUse))
                {
                    // tell JSON to read
                    if (!JsonReader.Read())
                    {
                        return null;
                    }

                    // make a new Json serializer (not sure if i can cache this...do i need a new object each time)?
                    var JsonSerializerToUse = new JsonSerializer();

                    // add the dyamic object converter to our serializer
                    JsonSerializerToUse.Converters.Add(new ExpandoObjectConverter());

                    // use JSON.NET to deserialize object to a dynamic (expando) object
                    object JsonObject;

                    // if we start with a "[", treat this as an array
                    if (JsonReader.TokenType == JsonToken.StartArray)
                    {
                        JsonObject = JsonSerializerToUse.Deserialize<List<ExpandoObject>>(JsonReader);
                    }
                    else
                    {
                        JsonObject = JsonSerializerToUse.Deserialize<ExpandoObject>(JsonReader);
                    }

                    // create a backing store to hold all properties for this deserialization
                    var BackingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                    // add all properties to this backing store
                    AddToBackingStore(BackingStore, string.Empty, JsonObject);

                    // return the object in a dictionary value provider so the MVC understands it
                    return new DictionaryValueProvider<object>(BackingStore, CultureInfo.CurrentCulture);
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Add a backing store to the dictionary
        /// </summary>
        /// <param name="BackingStore">Dictionary to set with the backing properties</param>
        /// <param name="Prefix">prefix</param>
        /// <param name="Value">value of the item</param>
        private static void AddToBackingStore(Dictionary<string, object> BackingStore, string Prefix, object Value)
        {
            //let's try to conver this thing to a dictionary first
            var DictionaryTryCast = Value as IDictionary<string, object>;

            //is it a dictionary?
            if (DictionaryTryCast != null)
            {
                //it is a dictionary, just loop through everything and add it
                foreach (var DictionaryKeyValuePair in DictionaryTryCast)
                {
                    AddToBackingStore(BackingStore, MakePropertyKey(Prefix, DictionaryKeyValuePair.Key), DictionaryKeyValuePair.Value);
                }

                //exit the method
                return;
            }

            //let's try to cast it as a list
            var ListTryCast = Value as IList;

            //is it a list?
            if (ListTryCast != null)
            {
                //it is a list, let's loop through and add the items
                for (int i = 0; i < ListTryCast.Count; i++)
                {
                    AddToBackingStore(BackingStore, MakeArrayKey(Prefix, i), ListTryCast[i]);
                }

                //exit the method
                return;
            }

            //its a primitive type, just add it
            BackingStore[Prefix] = Value;
        }

        /// <summary>
        /// Makes a json array key
        /// </summary>
        /// <param name="Prefix">Prefix to use</param>
        /// <param name="Index">Index to use</param>
        /// <returns>Array key to use</returns>
        private static string MakeArrayKey(string Prefix, int Index)
        {
            return $"{Prefix}[{Index.ToString(CultureInfo.InvariantCulture)}]";
        }

        /// <summary>
        /// Makes a property key
        /// </summary>
        /// <param name="Prefix">Prefix to use</param>
        /// <param name="PropertyName">Property name to use</param>
        /// <returns>Property Key</returns>
        private static string MakePropertyKey(string Prefix, string PropertyName)
        {
            //is the prefix null?
            if (string.IsNullOrEmpty(Prefix))
            {
                //no prefix, just return the property name
                return PropertyName;
            }

            //we have a prefix...add it and the property name
            return $"{Prefix}.{PropertyName}";
        }

        #endregion

    }

}
