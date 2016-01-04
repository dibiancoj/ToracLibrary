using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        #region Other Helpers

        /// <summary>
        /// Allows you to find a specific field from a JObject with less code
        /// </summary>
        /// <param name="JsonObject">JObject - JObject.Parse(JSON In String A Variable)</param>
        /// <param name="JPathQuerySelector">The query selector. ie: "field_shared_main_image_1x1", 0, "url"</param>
        /// <returns>The value of the node in a string. Cast to whatever value you would like. Will return null if it can't be found</returns>
        public static string JsonValueFromPath(JObject JsonObject, params object[] JPathQuerySelector)
        {
            //grab the root node object and put it into a tocken
            JToken CurrentNode = JsonObject;

            //let's loop through the values you are searching for
            foreach (var currentPathToSearch in JPathQuerySelector)
            {
                //try to find the node (this is a string selector)
                CurrentNode = CurrentNode[currentPathToSearch];

                //did we find a node?
                if (CurrentNode == null)
                {
                    //couldn't find a node, just return a null string
                    return null;
                }
            }

            //we went through the tree, return value as string
            return (string)CurrentNode;
        }

        #endregion

    }

}
