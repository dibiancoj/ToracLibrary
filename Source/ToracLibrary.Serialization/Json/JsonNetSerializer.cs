using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.IO;
using System.Text;

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

        public static string SerializeWithArrayPool<T>(T SerializeThisObject, ArrayPool<char> ArrayPoolToUse)
        {
            using (var WriterToUse = new StringWriter())
            {
                using (var JsonWriter = new JsonTextWriter(WriterToUse))
                {
                    JsonWriter.ArrayPool = new JsonNetArrayPool<char>(ArrayPoolToUse);
                    JsonWriter.CloseOutput = true;
                    JsonWriter.AutoCompleteOnClose = true;

                    JsonSerializer.Create().Serialize(JsonWriter, SerializeThisObject);

                    return WriterToUse.ToString();
                }
            }
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

        #region Deserialize From Stream

        /// <summary>
        /// Deserializes an item from a stream to an object. This is a little faster based on documentation because we don't need to load the entire string into memory. We can just read from the stream. This is great when we make web request calls
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="StreamToReadFrom">Stream to read from</param>
        /// <returns>the deserialized object</returns>
        public static T DeserializeFromStream<T>(Stream StreamToReadFrom)
        {
            return DeserializeFromStreamWithArrayPool<T>(StreamToReadFrom, null);
        }

        /// <summary>
        /// Deserializes an item from a stream to an object. This is a little faster based on documentation because we don't need to load the entire string into memory. We can just read from the stream. This is great when we make web request calls
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="StreamToReadFrom">Stream to read from</param>
        /// <param name="ArrayPoolToUse">Array pool to use. Pass in ArrayPool<char>.Shared</param>
        /// <returns>the deserialized object</returns>
        public static T DeserializeFromStreamWithArrayPool<T>(Stream StreamToReadFrom, ArrayPool<char> ArrayPoolToUse)
        {
            //this is great if you make a web request and you get a stream back.
            using (StreamReader StreamReaderToUse = new StreamReader(StreamToReadFrom))
            {
                using (var JsonReaderToUse = new JsonTextReader(StreamReaderToUse))
                {
                    //set the array pool (handle the overload)
                    if (ArrayPoolToUse != null)
                    {
                        JsonReaderToUse.ArrayPool = new JsonNetArrayPool<char>(ArrayPoolToUse);
                    }

                    //create the json.net engine
                    var SerializerEngine = new JsonSerializer();

                    //read the json from a stream - json size doesn't matter because only a small piece is read at a time from the HTTP request
                    return SerializerEngine.Deserialize<T>(JsonReaderToUse);
                }
            }
        }

        #endregion

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
                return default;
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
