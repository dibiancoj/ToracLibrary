using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.WebRequests
{

    /// <summary>
    /// Make a web request to download a file
    /// </summary>
    public static class FileDownloadWebRequest
    {

        /// <summary>
        /// Make a web request to download a file (byte array) - Async
        /// </summary>
        /// <param name="Url">url to download</param>
        /// <param name="UseDefaultCredentials">Use Default Credentials</param>
        /// <returns>byte[]</returns>
        public static async Task<byte[]> GetFileAsync(string Url, bool UseDefaultCredentials)
        {
            //var t = await WebRequestHelper.GetFileAsync(@"http://localhost:35234/ToPDF/8533/Unset/Unset/true/A4/30d54da1-dbd1-4fe8-adc1-bedf991ff160");
            //to save the bytes to the file system IO.File.WriteAllBytes(@"C:\Users\jdibianco.CORP\Desktop\jason.pdf", t);

            //create the web request
            var WebRequestToMake = WebRequest.Create(Url);

            //set the credentials if we want to use default credentials
            if (UseDefaultCredentials)
            {
                WebRequestToMake.UseDefaultCredentials = true;
                WebRequestToMake.PreAuthenticate = true;
            }

            //create the web request and go grab the data
            using (var WebResponse = (HttpWebResponse)await WebRequestToMake.GetResponseAsync())
            {
                //grab the reader so we can grab the file
                using (var BinaryReaderFromResponse = new BinaryReader(WebResponse.GetResponseStream()))
                {
                    //set the bytes of the file
                    return BinaryReaderFromResponse.ReadBytes(Convert.ToInt32(WebResponse.ContentLength));
                }
            }
        }

        /// <summary>
        /// Make a web request to download a file (byte array)
        /// </summary>
        /// <param name="Url">url to download</param>
        /// <param name="UseDefaultCredentials">Use Default Credentials</param>
        /// <returns>byte[]</returns>
        public static byte[] GetFile(string Url, bool UseDefaultCredentials)
        {
            //var t = WebRequestHelper.GetFile(@"http://localhost:35234/ToPDF/8533/Unset/Unset/true/A4/30d54da1-dbd1-4fe8-adc1-bedf991ff160");
            //to save the bytes to the file system IO.File.WriteAllBytes(@"C:\Users\jdibianco.CORP\Desktop\jason.pdf", t);

            //create the web request
            var WebRequestToMake = WebRequest.Create(Url);

            //set the credentials if we want to use default credentials
            if (UseDefaultCredentials)
            {
                WebRequestToMake.UseDefaultCredentials = true;
                WebRequestToMake.PreAuthenticate = true;
            }

            //create the web request and go grab the data
            using (var WebResponse = (HttpWebResponse)WebRequestToMake.GetResponse())
            {
                //grab the reader so we can grab the file
                using (var BinaryReaderFromResponse = new BinaryReader(WebResponse.GetResponseStream()))
                {
                    //set the bytes of the file
                    return BinaryReaderFromResponse.ReadBytes(Convert.ToInt32(WebResponse.ContentLength));
                }
            }
        }

    }

}
