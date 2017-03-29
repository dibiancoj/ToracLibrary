using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.Json.ExtensionMethods
{

    /// <summary>
    /// Extension methods for Json related items
    /// </summary>
    public static class JsonNetExtensionMethods
    {

        /// <summary>
        /// Convert the JToken to an enum. The regular Value<EnumType> doesn't appear to work
        /// </summary>
        /// <typeparam name="TEnumType">Type to convert</typeparam>
        /// <param name="JObjectToConvert">JObject to convert</param>
        /// <returns>The enum type. Null if not able to convert</returns>
        public static TEnumType? ToEnum<TEnumType>(this JToken JObjectToConvert) where TEnumType : struct
        {
            //note: if you have a value of like 200...and your enum doesn't have a 200. it will return a 200 and not a nullable value.
            //this is how enum's work.

            //try to parse this.
            if (Enum.TryParse<TEnumType>(JObjectToConvert.Value<string>(), true, out var TryConvertResult))
            {
                //parsed correctly
                return TryConvertResult;
            }

            //return a blank enum
            return new TEnumType?();
        }

    }

}
