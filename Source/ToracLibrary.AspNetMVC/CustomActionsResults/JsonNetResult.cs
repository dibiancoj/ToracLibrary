using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.Serialization.Json;

namespace ToracLibrary.AspNet.AspNetMVC.CustomActionsResults
{

    /// <summary>
    /// Class Used To Return Json.Net Serialized Data Back From The Controller
    /// </summary>
    /// <remarks>Specific Fields Are Immutable</remarks>
    public class JsonNetResult : ActionResult
    {

        #region Static Constructors

        /// <summary>
        /// Static constructor to cache everything we can
        /// </summary>
        static JsonNetResult()
        {
            //let's create a new instance of the cached properties
            CachedJsonSerializerSettings = new JsonSerializerSettings();
            CachedIsoDateTimeConverter = new IsoDateTimeConverter();
            JsonContentType = AspNetConstants.JsonContentType;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor Where You Want To Serialize An Object
        /// </summary>
        /// <param name="DataToSerialize">Data To Serialize</param>
        public JsonNetResult(object DataToSerialize)
            : this(DataToSerialize, CachedJsonSerializerSettings, CachedIsoDateTimeConverter)
        {
        }

        /// <summary>
        /// No Data To Serialize Constructor
        /// </summary>
        public JsonNetResult()
            : this(null, CachedJsonSerializerSettings, CachedIsoDateTimeConverter)
        {
        }

        #region Constructor Helper

        /// <summary>
        /// Constructor Helper
        /// </summary>
        /// <param name="DataToSerialize">Data To Serialize (Pass in null for nothing)</param>
        /// <param name="JsonSerializerSettingsToSet">Json serializer settings to be used. Passing it in incase we ever want to add a constructor overload</param>
        /// <param name="DateTimeConverterToSet">Date time converter to be used. Passing it in incase we ever want to add a constructor overload</param>
        private JsonNetResult(object DataToSerialize, JsonSerializerSettings JsonSerializerSettingsToSet, IsoDateTimeConverter DateTimeConverterToSet)
        {
            // create serializer settings
            SerializerSettings = JsonSerializerSettingsToSet;

            // setup default serializer settings (use the date time serializer which parses dates that look like dates
            //instead of UTC or whatever dates they are
            SerializerSettings.Converters.Add(DateTimeConverterToSet);

            //if we passed in data then go set the property
            if (DataToSerialize != null)
            {
                //set the data to serialize
                Data = DataToSerialize;
            }
        }

        #endregion

        #endregion

        #region Static Properties

        /// <summary>
        /// Cached JsonSerializerSettings. This way we don't have to keep creating a new object. Helps GC
        /// </summary>
        private static JsonSerializerSettings CachedJsonSerializerSettings { get; }

        /// <summary>
        /// Cached IsoDateTimeConverter. This way we don't have to keep creating a new object. Helps GC
        /// </summary>
        private static IsoDateTimeConverter CachedIsoDateTimeConverter { get; }

        /// <summary>
        /// Holds the json content type
        /// </summary>
        private static string JsonContentType { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Data To Return
        /// </summary>
        private object Data { get; }

        /// <summary>
        /// Serializer Settings
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; }

        /// <summary>
        /// Encoding
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Content TYpe
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Formatting To Format The Json With
        /// </summary>
        public Formatting Formatting { get; set; }

        #endregion

        #region Action Results Override Methods

        /// <summary>
        /// Execute Results
        /// </summary>
        /// <param name="context">Controller Context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            //if the context is null then throw an error
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            //if we set a content type, then set the value with that property
            if (string.IsNullOrEmpty(ContentType))
            {
                //we didn't set a content type...set it with the json type
                context.HttpContext.Response.ContentType = JsonContentType;
            }
            else
            {
                //set the content type
                context.HttpContext.Response.ContentType = ContentType;
            }

            //if the content encoding property is set then set the response content encoding
            if (ContentEncoding != null)
            {
                //set the content encoding
                context.HttpContext.Response.ContentEncoding = ContentEncoding;
            }

            //if we have something to serialize, then serialize it now
            if (Data != null)
            {
                // If you need special handling, you can call another form of SerializeObject below
                context.HttpContext.Response.Write(JsonNetSerializer.Serialize(Data, Formatting, SerializerSettings));
            }
        }

        #endregion

        #region Static Factory Methods

        /// <summary>
        /// Factory Method To Create A New JsonNetResult
        /// </summary>
        /// <param name="DataToSerialize">Data To Serialize</param>
        /// <returns>Json Net Result</returns>
        public static JsonNetResult JsonNet(object DataToSerialize)
        {
            //Example To Call This From The Controller
            //return JsonNet(thisObject);

            //go build up the object and return the Action Result
            return new JsonNetResult(DataToSerialize);
        }

        #endregion

    }

}
