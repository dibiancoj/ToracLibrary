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
    public static class Log4NetWrapper
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

        /*
        You can also add dynamic runtime properties such as
         <param name="File" value="{LogDirectory}/log.txt" />

            Then call the following to set it
            
            //set the logger
            Logger = LogManager.GetLogger(typeof(LoggingService));

            //reuse the web ui errors folder to write to the log. We know this is writable
            log4net.GlobalContext.Properties["LogDirectory"] = System.Configuration.ConfigurationManager.AppSettings["WebUIErrors"];

            //go grab the configuration
            XmlConfigurator.Configure();


        */

        #endregion

        #region Static Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static Log4NetWrapper()
        {
            //set the logger
            Logger = LogManager.GetLogger(typeof(Log4NetWrapper));

            //go grab the configuration
            XmlConfigurator.Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the logging mechanism to write to the log file
        /// </summary>
        public static ILog Logger { get; }

        #endregion

        //Level's you can write. 
        //ALL
        //DEBUG
        //INFO
        //WARN
        //ERROR
        //FATAL
        //OFF

        #region Public Helper Methods

        /// <summary>
        /// Builds the output text which contains more informaiton (method name, source line number)
        /// </summary>
        /// <param name="MessageToWrite">Message To Write</param>
        /// <param name="SourceMemberName">Source Member Name</param>
        /// <param name="SourceLineNumber">Source Line Number</param>
        /// <returns>Output Message</returns>
        public static string BuildOutputHeader(string MessageToWrite, [CallerMemberName]string SourceMemberName = "", [CallerLineNumber]int SourceLineNumber = -1)
        {
            //return the formatted string
            return $"MethodInfo: {SourceMemberName} [{SourceLineNumber}] - {MessageToWrite}";
        }

        #endregion

    }

}
