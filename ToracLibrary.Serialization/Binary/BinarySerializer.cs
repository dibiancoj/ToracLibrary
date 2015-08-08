using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.Binary
{

    /// <summary>
    /// Binary - Serialize and Deserialize Objects 
    /// </summary>
    public static class BinarySerializer
    {

        #region Serialize

        #region To File System

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="SerializeThisObject">Object to serialize</param>
        /// <param name="FilePathToSave">File Path To Save</param>
        /// <param name="thisFileMode">File Mode To Set On This File</param>
        /// <param name="thisFileAccess">File Access To This File</param>
        /// <param name="thisFileShare">File Share To Place On This File</param>
        public static void SerializeObject<T>(T SerializeThisObject, string FilePathToSave, FileMode thisFileMode, FileAccess thisFileAccess, FileShare thisFileShare)
        {
            //create the stream to write to the file system
            using (var StreamToWriteInto = new FileStream(FilePathToSave, thisFileMode, thisFileAccess, thisFileShare))
            {
                //retur the item...and save it
                new BinaryFormatter().Serialize(StreamToWriteInto, SerializeThisObject);
            }
        }

        #endregion

        #region To Byte Array

        /// <summary>
        /// Serialize Your Object To A Byte Array
        /// </summary>
        /// <param name="SerializeThisObject">Object to serialize</param>
        /// <returns>bool - Result of the save</returns>
        public static byte[] SerializeObject<T>(T SerializeThisObject)
        {
            //create the stream to write to the array of bytes
            using (var MemoryStreamToWriteInto = new MemoryStream())
            {
                //retur the item...and save it
                new BinaryFormatter().Serialize(MemoryStreamToWriteInto, SerializeThisObject);

                //return the result
                return MemoryStreamToWriteInto.ToArray();
            }
        }

        #endregion

        #endregion

        #region Deserialize

        #region To File System

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="FilePath">File Path To The Object To Be Deserialized</param>
        /// <returns>Object Of T</returns>
        /// <remarks>You will need filemode.Open permissions</remarks>
        public static T DeserializeObject<T>(string FilePath)
        {
            //create the file stream object
            using (var FileStreamToWriteInto = new FileStream(FilePath, FileMode.Open))
            {
                //cast and return the object
                return ((T)new BinaryFormatter().Deserialize(FileStreamToWriteInto));
            }
        }

        #endregion

        #region From Byte Array

        /// <summary>
        /// Deserialize An Object From A Byte Array
        /// </summary>
        /// <param name="FileData">File Data</param>
        /// <returns>Object Of T</returns>
        public static T DeserializeObject<T>(byte[] FileData)
        {
            //create the file stream object
            using (var MemoryStreamToWriteInto = new MemoryStream(FileData))
            {
                //cast and return the object
                return ((T)new BinaryFormatter().Deserialize(MemoryStreamToWriteInto));
            }
        }

        #endregion

        #endregion

    }

}
