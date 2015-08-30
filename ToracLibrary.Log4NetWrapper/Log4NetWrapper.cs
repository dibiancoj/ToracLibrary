using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Log4NetAPI
{

    /// <summary>
    /// Wrapper Class For Log 4 Net. Provides Ability To Use Log 4 Net. (http://logging.apache.org/log4net/)
    /// </summary>
    public class Log4NetWrapper
    {

        //this uses features from .net 4.5 (System.Runtime.CompilerServices -- > CallerMemberName, & CallerLineNumber

        #region Web.Config Documentation

        //with the following web.config it will throw the log into a folder named "Log"

        //** put this off of the <configuration> node

        //<configSections>
        //    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
        //</configSections>

        //<log4net>
        //    <root>
        //        <level value="DEBUG" />
        //        <appender-ref ref="LogFileAppender" />
        //    </root>
        //    <appender name="LogFileAppender" type="log4net.Appender.RollingFileAppender" >
        //        <param name="File" value="Log/log.txt" />
        //        <param name="AppendToFile" value="true" />
        //        <rollingStyle value="Size" />
        //        <maxSizeRollBackups value="10" />
        //        <maximumFileSize value="10MB" />
        //        <staticLogFileName value="true" />
        //        <layout type="log4net.Layout.PatternLayout">
        //            <param name="ConversionPattern" value="%-5p - %d{yyyy-MM-dd hh:mm:ss} | %m%n - %C{1}.%M " />
        //        </layout>
        //    </appender>
        //</log4net>

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - Set up the log file
        /// </summary>
        /// <remarks>Class has immutable propeties</remarks>
        public Log4NetWrapper(bool thisUseCallerMemberInfoInOutputText)
        {
            //set the logger (when re-writting this was a static property. if this blows up move it below the XmlConfigurator.Configure() call)
            Logger = LogManager.GetLogger(typeof(Log4NetWrapper));

            //go configure the log file
            XmlConfigurator.Configure();

            //set the boolean property
            UseCallerMemberInfoInOutputText = thisUseCallerMemberInfoInOutputText;        
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the logging mechanism to write to the log file
        /// </summary>
        public ILog Logger { get; }

        /// <summary>
        /// If true will use the caller member info and append your output text to show you calling method name and line number
        /// </summary>
        public bool UseCallerMemberInfoInOutputText { get; set; }

        #endregion

        #region Methods

        #region Debug

        /// <summary>
        /// Writes to the Log 4 Net Log. (Debug Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void DebugMessageToLog(string MessageToWrite,
                                     [CallerMemberName]string SourceMemberName = "",
                                     [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Debug(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber));
        }

        /// <summary>
        /// Writes to the Log 4 Net Log. (Debug Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="ExceptionToWrite">Exception To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void DebugMessageToLog(string MessageToWrite, Exception ExceptionToWrite,
                                      [CallerMemberName]string SourceMemberName = "",
                                      [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Debug(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber), ExceptionToWrite);
        }

        #endregion

        #region Error

        /// <summary>
        /// Writes to the Log 4 Net Log. (Error Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void ErrorMessageToLog(string MessageToWrite,
                                     [CallerMemberName]string SourceMemberName = "",
                                     [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Error(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber));
        }

        /// <summary>
        /// Writes to the Log 4 Net Log. (Error Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="ExceptionToWrite">Exception To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void ErrorMessageToLog(string MessageToWrite, Exception ExceptionToWrite,
                                     [CallerMemberName]string SourceMemberName = "",
                                     [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Error(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber), ExceptionToWrite);
        }

        #endregion

        #region Warning

        /// <summary>
        /// Writes to the Log 4 Net Log. (Warn Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void WarnMessageToLog(string MessageToWrite,
                                    [CallerMemberName]string SourceMemberName = "",
                                    [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Warn(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber));
        }

        /// <summary>
        /// Writes to the Log 4 Net Log. (Warn Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="ExceptionToWrite">Exception To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void WarnMessageToLog(string MessageToWrite, Exception ExceptionToWrite,
                                    [CallerMemberName]string SourceMemberName = "",
                                    [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Warn(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber), ExceptionToWrite);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Writes to the Log 4 Net Log. (Fatal Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void FatalMessageToLog(string MessageToWrite,
                                     [CallerMemberName]string SourceMemberName = "",
                                     [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Fatal(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber));
        }

        /// <summary>
        /// Writes to the Log 4 Net Log. (Fatal Notification)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="ExceptionToWrite">Exception To Write</param>
        /// <param name="SourceMemberName">Source Member Name - Don't Pass In, Compiler Will Set It At Run Time</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        public void FatalMessageToLog(string MessageToWrite, Exception ExceptionToWrite,
                                     [CallerMemberName]string SourceMemberName = "",
                                     [CallerLineNumber]int SourceLineNumber = -1)
        {
            Logger.Fatal(BuildOutputHeader(MessageToWrite, SourceMemberName, SourceLineNumber), ExceptionToWrite);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Builds the output text which contains more informaiton (method name, source line number)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        /// <returns>Output Message</returns>
        private string BuildOutputHeader(string MessageToWrite, string SourceMemberName, int SourceLineNumber)
        {
            //do we want to use the calling member info 
            if (UseCallerMemberInfoInOutputText)
            {
                //return the formatted string
                return $"MethodInfo: {SourceMemberName} [{SourceLineNumber}] - {MessageToWrite}";
            }

            //don't want to format, just output whatever the end developer wanted
            return MessageToWrite;
        }

        #endregion

        #endregion
    }

}
