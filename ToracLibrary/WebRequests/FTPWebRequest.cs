using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DiskIO;

namespace ToracLibrary.Core.WebRequests
{

    /// <summary>
    /// Connect To FTP Server And Upload / Download Files
    /// </summary>
    /// <remarks>Constructor - If No User Name Or Password Is Sent In Then Your Using Anonymous Access. Class properties are immutable</remarks>
    public class FTPWebRequest
    {

        #region Constructor

        /// <summary>
        /// Constructor. Overloaded - Use For Anonymous Access
        /// </summary>
        /// <param name="FTPserverURL">Address of FTP Server</param>
        /// <param name="FTPServerUsesPassiveMode">Does The FTP User Passive FTP Or Active (True = Passive)</param>
        /// <param name="FTPServerConnectsUsingSSL">FTP server must connect using SSL</param>
        public FTPWebRequest(string FTPserverURL, bool FTPServerUsesPassiveMode, bool FTPServerConnectsUsingSSL)
        {
            //Validate
            if (new Uri(FTPserverURL).Scheme != Uri.UriSchemeFtp)
            {
                throw new InvalidDataException("The Server URI parameter should start with the ftp:// scheme.");
            }

            //Create the instance of the web client
            FTPserverURI = new Uri(FTPserverURL);

            //Set the use anonymous access
            UseAnonymousAccess = true;

            //Set the passive Mode
            UsePassiveFTPMode = FTPServerUsesPassiveMode;

            //Set the SSL
            UseSSL = FTPServerConnectsUsingSSL;
        }

        /// <summary>
        /// Constructor. Overloaded - If No User Name Or Password Is Sent In Then Your Using Anonymous Access
        /// </summary>
        /// <param name="FTPserverURL">Address of FTP Server</param>
        /// <param name="UserName">User Name To LogIn With</param>
        /// <param name="UserPW">User's Password To LogIn With</param>
        /// <param name="FTPServerUsesPassiveMode">Does The FTP User Passive FTP Or Active (True = Passive)</param>
        /// <param name="FTPServerConnectsUsingSSL">FTP server must connect using SSL</param>
        public FTPWebRequest(string FTPserverURL, string UserName, string UserPW, bool FTPServerUsesPassiveMode, bool FTPServerConnectsUsingSSL)
        {
            //Validate
            if (new Uri(FTPserverURL).Scheme != Uri.UriSchemeFtp)
            {
                throw new InvalidDataException("The Server URI parameter should start with the ftp:// scheme.");
            }

            //Validate the user name and pw
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPW))
            {
                throw new ArgumentNullException("Missing User Name Or Password. If You Are Using Anonymous Access Please Use The Other Constructor Overload.");
            }
            //End Of Validation

            //Create the instance of the web client
            FTPserverURI = new Uri(FTPserverURL);

            //Add the credentials
            LogInInfo = new NetworkCredential(UserName, UserPW);

            //Set the passive Mode
            UsePassiveFTPMode = FTPServerUsesPassiveMode;

            //Set the SSL
            UseSSL = FTPServerConnectsUsingSSL;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the ftp client to connect and run any transactions you need to
        /// </summary>
        private Uri FTPserverURI { get; }

        /// <summary>
        /// Holds the user name and password you should log in with if your not using Anonymous Access
        /// </summary>
        private NetworkCredential LogInInfo { get; }

        /// <summary>
        /// Holds the flag if you should use AnonymousAccess Or Not
        /// </summary>
        private bool UseAnonymousAccess { get; }

        /// <summary>
        /// Does The FTP Use Passive Mode.
        /// </summary>
        private bool UsePassiveFTPMode { get; }

        /// <summary>
        /// Should We Connect Using SSL
        /// </summary>
        private bool UseSSL { get; }

        #endregion

        #region Main Methods

        /// <summary>
        /// Delete A File On The FTP Server
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <returns>FTP Web Response. Call StatusDescription For Description Or Status Code</returns>
        public FtpWebResponse DeleteFile(string FileName)
        {
            //Create the FTP request.
            var Request = (FtpWebRequest)WebRequest.Create(FTPserverURI + FileName);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.DeleteFile;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                //set the log in credentials
                Request.Credentials = LogInInfo;
            }

            //Return the response
            return (FtpWebResponse)Request.GetResponse();
        }

        /// <summary>
        /// Get The Size Info Of A File On The FTP Server (In Bytes)
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <returns>Long - Size Of File in Bytes</returns>
        public long FileSizeInfo(string FileName)
        {
            //Create the FTP request.
            var Request = (FtpWebRequest)WebRequest.Create(FTPserverURI + FileName);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.GetFileSize;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                //set the log in credentials
                Request.Credentials = LogInInfo;
            }

            //Return the response
            using (FtpWebResponse Response = (FtpWebResponse)Request.GetResponse())
            {
                return Response.ContentLength;
            }
        }

        /// <summary>
        /// Get The Date Time Stamp Of A File On The FTP Server
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <returns>DateTime - of the file timestamp</returns>
        public DateTime FileDateTimeStamp(string FileName)
        {
            //Create the FTP request.
            var Request = (FtpWebRequest)WebRequest.Create(FTPserverURI + FileName);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                Request.Credentials = LogInInfo;
            }

            //Return the response
            using (FtpWebResponse Response = (FtpWebResponse)Request.GetResponse())
            {
                return Response.LastModified;
            }
        }

        /// <summary>
        /// Get The Listing Of The Directory On Server
        /// </summary>
        /// <param name="PathOffOfBaseDirectory">Null If You Just Want The Base Directory. Otherwise /ThisFolder/ThatFolder</param>
        /// <returns>IEnumerable of string - List Of Files</returns>
        public IEnumerable<string> FileListingLazy(string PathOffOfBaseDirectory)
        {
            //path to use
            Uri FTPServerPath = FTPserverURI;

            //if we want something off of the base path, then add it
            if (!string.IsNullOrEmpty(PathOffOfBaseDirectory))
            {
                //add the 2 paths together
                FTPServerPath = new Uri(FTPServerPath + PathOffOfBaseDirectory);
            }

            //create the request
            var Request = (FtpWebRequest)WebRequest.Create(FTPServerPath);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.ListDirectory;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                Request.Credentials = LogInInfo;
            }

            //Declare and dispose of response
            using (var Response = (FtpWebResponse)Request.GetResponse())
            {
                //Pass the stream reader to the file reader and return it to a list
                return FileReader.ReadFileToIEnumerableLazy(new StreamReader(Response.GetResponseStream()));
            }
        }

        /// <summary>
        /// Upload A File To A FTP Server
        /// </summary>
        /// <param name="FilePathAndName">File Path Including Name</param>
        /// <returns>FTP Web Response. Call StatusDescription For Description Or Status Code</returns>
        public FtpWebResponse UploadFile(string FilePathAndName)
        {
            //make sure the file is located
            if (!File.Exists(FilePathAndName))
            {
                //can't find file, throw the exception
                throw new FileNotFoundException("Can't Find File At: " + FilePathAndName);
            }

            //Create the FTP Web request.
            var Request = (FtpWebRequest)WebRequest.Create(FTPserverURI + FilePathAndName);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.UploadFile;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                Request.Credentials = LogInInfo;
            }

            //Set the file data by reading the bytes
            byte[] FileContents = File.ReadAllBytes(FilePathAndName);

            //Set the Content Length
            Request.ContentLength = FileContents.Length;

            //Declare the request Stream and Set the Request Stream From The FTP Server
            using (var requestStream = Request.GetRequestStream())
            {
                //Write The Stream From The File Contents
                requestStream.Write(FileContents, 0, FileContents.Length);
            }

            //Return the response
            return (FtpWebResponse)Request.GetResponse();
        }

        /// <summary>
        /// Download A File To A FTP Server
        /// </summary>
        /// <param name="FilePathAndNameToDownload">File And Path Of The File We Want To Download</param>
        /// <param name="SavePath">The Path To Save The File On The User's Local Machine</param>
        /// <returns>Task Of FTP Web Response. Call StatusDescription For Description Or Status Code</returns>
        /// <example>Task-bool- f1 = ftp.DownloadFile("1.bak", "C:\1.bak");</example>
        public bool DownloadFile(string FilePathAndNameToDownload, string SavePath)
        {
            //Create the web request.
            var Request = (FtpWebRequest)WebRequest.Create(FTPserverURI + FilePathAndNameToDownload);

            //Set the type of action
            Request.Method = WebRequestMethods.Ftp.DownloadFile;

            //Set The Passive Mode
            Request.UsePassive = UsePassiveFTPMode;

            //Use SSL
            Request.EnableSsl = UseSSL;

            //if it's the permissions is set to anonymous access then don't set the credentials
            if (!UseAnonymousAccess)
            {
                Request.Credentials = LogInInfo;
            }

            //Get the response
            //Response = (FtpWebResponse)Response.GetResponse();
            using (FtpWebResponse Response = (FtpWebResponse)Request.GetResponse())
            {
                //create the response stream
                using (Stream ResponseStream = Response.GetResponseStream())
                {
                    //create the file stream
                    using (FileStream FileStreamToRead = new FileStream(SavePath, FileMode.Create))
                    {
                        //Declare the buffer and the tally of how many bytes we read
                        byte[] Buffer = new byte[2048];

                        //how many have we read?
                        int SizeRead = 0;

                        //loop and write the file
                        do
                        {
                            //go read the stream
                            SizeRead = ResponseStream.Read(Buffer, 0, Buffer.Length);

                            //write a chunk of the file
                            FileStreamToRead.Write(Buffer, 0, SizeRead);

                        } while (SizeRead != 0);

                        //Flush out the file stream then just in case
                        FileStreamToRead.Flush();
                    }
                }

                //Return the flag to let you know it downloaded successfully.
                return true;
            }
        }

        #endregion

    }

}
