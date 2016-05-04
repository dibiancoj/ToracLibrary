using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ToracLibrary.Serialization.Xml
{
    /// <summary>
    /// XML - Serialize and Deserialize Objects 
    /// </summary>
    public static class XMLSerialization
    {
        // Add [System.Xml.Serialization.XmlRoot("Transaction")] To The Top Of The Class To Control The Root Or The Node You Are Serializing
        // Also use this for properties to serialize as attribute[System.Xml.Serialization.XmlAttribute("TheAttributeName")]

        //if you have a list of interfaces or base classes. 
        //[Serializable()]
        //[XmlInclude(typeof(FormQuestionAnswerMultiStringValue))]
        //[XmlInclude(typeof(FormQuestionAnswerDateRangeValue))]
        //[XmlInclude(typeof(FormQuestionAnswerDateValue))]
        //[XmlInclude(typeof(FormQuestionAnswerNameValue))]
        //[XmlInclude(typeof(FormQuestionAnswerStringValue))]
        //public class SavedSurveyData
        //{
            //List<BaseClass> Answers { get; set; }
        //}

        #region Documentation On How To Remove Null Xml Output For Nullable Data Types

        // if you have a property that is a nullable int or a nullable enum...

        //the xml serializer will output <StartDateValue xmlns:d4p1="http://www.w3.org/2001/XMLSchema-instance" d4p1:nil="true" />...instead it doesn't output it

        //if you don't want to output add a method to your object like below

        ////method name needs to be "ShouldSerialize" then the property name (shady Microsoft)

        //        public bool ShouldSerializeTextBoxFilterType()
        //        {
        //            return TextBoxFilterType.HasValue;
        //        }

        //example with multiple

        // public StringFilters.StringFilterSearchType? TextBoxFilterType { get; set; }
        // public DateTime? StartDateValue { get; set; }

        //        public bool ShouldSerializeTextBoxFilterType()
        //        {
        //            return TextBoxFilterType.HasValue;
        //        }

        //        public bool ShouldSerializeStartDateValue()
        //        {
        //            return StartDateValue.HasValue;
        //        }

        #endregion

        #region Serialize

        //Specific Issues with the serializer with DateTime.Now
        // if you use DateTime.Now it will serialize with the time zone - 2012-01-30T00:00:00-800. Sql Server can't parse it with the time zone...
        // So to fix that please set the date time to too DateTime.UtcNow

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="SerializeThisObject">Object to serialize</param>
        /// <returns>String Representation of this object</returns>
        public static string SerializeObject<T>(T SerializeThisObject)
        {
            //create the string writer object
            using (var SerializeStringWriter = new StringWriter())
            {
                //serialize the object into the string writer
                new XmlSerializer(typeof(T)).Serialize(SerializeStringWriter, SerializeThisObject);

                //return the string writer
                return SerializeStringWriter.ToString();
            }
        }

        /// <summary>
        /// Serialize an object into an XElement Object
        /// </summary>
        /// <param name="SerializeThisObject">Object to serialize</param>
        /// <returns>String Representation of this object</returns>
        public static XElement SerializeObjectToXElement<T>(T SerializeThisObject)
        {
            //use the other method to grab the xml...this method builds it into a xelement so the user doesn't have to load it themselves for each call
            return XElement.Parse(SerializeObject(SerializeThisObject));
        }

        #endregion

        #region Deserializer

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="XmlDataToDeserialize">Serialized Xml Data</param>
        /// <returns>Object Of T</returns>
        public static T DeserializeObject<T>(XElement XmlDataToDeserialize)
        {
            //serialize the object into the string writer
            return ((T)new XmlSerializer(typeof(T)).Deserialize(XmlDataToDeserialize.CreateReader()));
        }

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="XmlDataToDeserialize">Serialized Xml Data</param>
        /// <returns>Object Of T</returns>
        public static T DeserializeObject<T>(string XmlDataToDeserialize)
        {
            //use the overload
            return DeserializeObject<T>(XElement.Parse(XmlDataToDeserialize));
        }

        #endregion
    }
}
