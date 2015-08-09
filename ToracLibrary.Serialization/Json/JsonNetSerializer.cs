using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.Json
{

    /// <summary>
    /// Serialize data types into json using json.net
    /// </summary>
    public static class JsonNetSerializer
    {

        #region Serialize

        #region Serialize With No Parameters

        /// <summary>
        /// Serialize An Object Of T
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="SerializeThisObject">Object To Serialize</param>
        /// <returns>Json String Element</returns>
        public static string Serialize<T>(T SerializeThisObject)
        {
            //use the Newtonsoft Json Serializer
            return JsonConvert.SerializeObject(SerializeThisObject);
        }

        #endregion

        #region Serialize With Parameters

        /// <summary>
        /// Serialize An Object Of T
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="SerializeThisObject">Object To Serialize</param>
        /// <param name="FormattingType">Formatting Options</param>
        /// <param name="SerializerSettings">Serializer Settings</param>
        /// <returns>Json String Element</returns>
        public static string Serialize<T>(T SerializeThisObject, Formatting FormattingType, JsonSerializerSettings SerializerSettings)
        {
            return JsonConvert.SerializeObject(SerializeThisObject, FormattingType, SerializerSettings);
        }

        #endregion

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserialize Json String Data Into An Object Of T
        /// </summary>
        /// <param name="JsonData">JsonData To Deserialize</param>
        /// <returns>Object Of T</returns>
        public static T Deserialize<T>(string JsonData)
        {
            //use the Netwonsoft Json Deserializer
            return JsonConvert.DeserializeObject<T>(JsonData);
        }

        /// <summary>
        /// Deserialize Json String Data Into An Object Of T
        /// </summary>
        /// <param name="JsonData">JsonData To Deserialize</param>
        /// <param name="SerializerSettings">Serializer Settings</param>
        /// <returns>Object Of T</returns>
        public static T Deserialize<T>(string JsonData, JsonSerializerSettings SerializerSettings)
        {
            //use the Netwonsoft Json Deserializer
            return JsonConvert.DeserializeObject<T>(JsonData, SerializerSettings);
        }

        #endregion

    }

}
