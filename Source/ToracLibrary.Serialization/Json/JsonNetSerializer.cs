using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

        #region Static Properties

        /// <summary>
        /// Abstract class type handling. Pass in settings into serialize and deserialize to handle abstract class
        /// </summary>
        public static readonly JsonSerializerSettings AbstractClassSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        #endregion

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

        #region JObject and JToken

        // notes 
        //To get a node to a specific data type
        //Call myJTokenNode.Value<string>();
        //To deserialize it into an object call myJTokenNode.ToObject<Person>();
        //For enums - call myJokenNode.ToObject<MyEnumType>();

        #region Public Methods

        /// <summary>
        /// Load a JObject from a stream
        /// </summary>
        /// <param name="StreamToLoadfrom">stream to load from</param>
        /// <returns>The loaded JObject</returns>
        public static JObject JObjectFromStream(Stream StreamToLoadfrom)
        {
            //In Asp.net Core --> Stream = bindingContext.ActionContext.HttpContext.Request.Body
            return JObject.Load(new JsonTextReader(new StreamReader(StreamToLoadfrom, Encoding.UTF8)));
        }

        /// <summary>
        /// Allows you to find a specific field from a JObject with less code
        /// </summary>
        /// <param name="JsonObject">JObject - JObject.Parse(JSON In String A Variable)</param>
        /// <param name="JPathQuerySelector">The query selector. ie: "field_shared_main_image_1x1", 0, "url"</param>
        /// <returns>The node. Call .ToObject for object deserialization. or .Value(string) to convert it to a string, etc.</returns>
        public static JToken JsonValueFromPath(JObject JsonObject, params object[] JPathQuerySelector)
        {
            //go parse the data and return the jtoken
            return JTokenValueFromPath(JsonObject, JPathQuerySelector);
        }

        /// <summary>
        /// Allows you to find a specific field from a JObject with less code
        /// </summary>
        /// <typeparam name="T">Type of the value you want to return. Will do a json deserialize after it finds the value</typeparam>
        /// <param name="JsonObject">JObject - JObject.Parse(JSON In String A Variable)</param>
        /// <param name="JPathQuerySelector">The query selector. ie: "field_shared_main_image_1x1", 0, "url"</param>
        /// <returns>The value of the node in a type of t. Will return default(T) if it can't be found</returns>
        public static T JsonValueFromPath<T>(JObject JsonObject, params object[] JPathQuerySelector)
        {
            //use the overload then convert it
            var ResultOfParse = JsonValueFromPath(JsonObject, JPathQuerySelector);

            //if its null return right away
            if (ResultOfParse == null)
            {
                //can't find node. Returning default T
                return default(T);
            }

            //otherwise try to convert it
            return ResultOfParse.ToObject<T>();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Allows you to find a specific field from a JObject with less code
        /// </summary>
        /// <typeparam name="T">Type of the value you want to return. Will do a json deserialize after it finds the value</typeparam>
        /// <param name="JsonObject">JObject - JObject.Parse(JSON In String A Variable)</param>
        /// <param name="JPathQuerySelector">The query selector. ie: "field_shared_main_image_1x1", 0, "url"</param>
        /// <returns>JToken - null if not found</returns>
        private static JToken JTokenValueFromPath(JObject JsonObject, params object[] JPathQuerySelector)
        {
            //grab the root node object and put it into a tocken
            JToken CurrentNode = JsonObject;

            //let's loop through the values you are searching for
            foreach (var PathToSearch in JPathQuerySelector)
            {
                //try to find the node (this is a string selector)
                CurrentNode = CurrentNode[PathToSearch];

                //did we find a node?
                if (CurrentNode == null)
                {
                    //couldn't find a node, just return a null string
                    return null;
                }
            }

            //we went through the tree, return the token
            return CurrentNode;
        }

        #endregion

        #endregion

    }

}
