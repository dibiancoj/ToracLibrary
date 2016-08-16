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

        /*if you want to archive the files by date use (see rolling style and date pattern below
         <log4net>
            <root>
              <level value = "DEBUG" />
              < appender -ref ref="LogFileAppender" />
            </root>
            <appender name = "LogFileAppender" type="log4net.Appender.RollingFileAppender">
              <param name = "File" value="C:\PortalTestWebUIErrors\log.txt" />
              <param name = "AppendToFile" value="true" />
              <rollingStyle value = "Date" />
              < datePattern value="yyyyMMdd" />
              <!--<rollingStyle value = "Size" />
              < maxSizeRollBackups value="10" />
              <maximumFileSize value = "1MB" /> -->
              < staticLogFileName value="true" />
              <layout type = "log4net.Layout.PatternLayout" >
                < param name="ConversionPattern" value="%-5p - %d{yyyy-MM-dd hh:mm:ss} | %m%n - %C{1}.%M " />
              </layout>
            </appender>
            </log4net>
        */

        /* you can set the property of the path or file name at run time using run time parameters...
          <appender name="LogFileAppender" type="log4net.Appender.RollingFileAppender">
            <file type="log4net.Util.PatternString" value="%property{Bla}/log.txt" />     <------ notice the %property{Bla}....
            <!--<param name="File" value="%property{Bla}/log.txt" />-->
            <param name="AppendToFile" value="true" />
            <rollingStyle value="Size" />
            <maxSizeRollBackups value="10" />
            <maximumFileSize value="10MB" />
            <staticLogFileName value="true" />
            <layout type="log4net.Layout.PatternLayout">
            <param name="ConversionPattern" value="%-5p - %d{yyyy-MM-dd hh:mm:ss} | %m%n - %C{1}.%M " />
            </layout>
            </appender>

            then in code before you call anything
            log4net.GlobalContext.Properties["Bla"] = "Jason";
            var Logger = LogManager.GetLogger(typeof(Program));
            XmlConfigurator.Configure();


            you can also output session variables or session id by using ([%aspnet-request....
            <layout type="log4net.Layout.PatternLayout">
                <param name="ConversionPattern" value="%-5p - %d{yyyy-MM-dd hh:mm:ss} | %m%n - %C{1}.%M [%aspnet-request{ASP.NET_SessionId}]" />
            </layout>


            ***a pretty good pattern i used      <param name="ConversionPattern" value="%newline%-%level - %d{yyyy-MM-dd hh:mm:ss}%newline     SessionId = [%aspnet-request{ASP.NET_SessionId}] %newline     %message %newline" />
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

        /// <summary>
        /// log4net has Level but I don't like how they did it. I want us to spell out which level
        /// </summary>
        public enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

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
        /// Write to the log. This method checks to ensure the log level is going to write into log 4 net. This way we don't need to call the method if log4net isn't going to write it.
        /// </summary>
        /// <param name="Level">Level to write</param>
        /// <param name="MessageToWrite">Message to write</param>
        public static void WriteToLog(LogLevel Level, string MessageToWrite)
        {
            //just avoids having to call the method if the level we want to write into is off
            if (Logger.IsDebugEnabled && Level == LogLevel.Debug)
            {
                Logger.Debug(MessageToWrite);
            }

            else if (Logger.IsInfoEnabled && Level == LogLevel.Info)
            {
                Logger.Info(MessageToWrite);
            }

            else if (Logger.IsWarnEnabled && Level == LogLevel.Warn)
            {
                Logger.Warn(MessageToWrite);
            }

            else if (Logger.IsErrorEnabled && Level == LogLevel.Error)
            {
                Logger.Error(MessageToWrite);
            }

            else if (Logger.IsFatalEnabled && Level == LogLevel.Fatal)
            {
                Logger.Fatal(MessageToWrite);
            }
        }

        /// <summary>
        /// Write to the log. This method checks to ensure the log level is going to write into log 4 net. We pass in a func to avoid the string.format which is expensive. Ran test in BenchmarkDotNet. If you have the debug level set to debug (which outputs everything its a little more expensive). If you have it on error. It is better on memory and faster because you only run string.format every once in a while.
        /// </summary>
        /// <param name="Level">Level to write</param>
        /// <param name="MessageToWrite">Message to write in a lambda to avoid string.format which is heavy if the log level is not even set</param>
        public static void WriteToLog(LogLevel Level, Func<string> MessageToWrite)
        {
            //just avoids having to call the method if the level we want to write into is off
            if (Logger.IsDebugEnabled && Level == LogLevel.Debug)
            {
                Logger.Debug(MessageToWrite());
            }

            else if (Logger.IsInfoEnabled && Level == LogLevel.Info)
            {
                Logger.Info(MessageToWrite());
            }

            else if (Logger.IsWarnEnabled && Level == LogLevel.Warn)
            {
                Logger.Warn(MessageToWrite());
            }

            else if (Logger.IsErrorEnabled && Level == LogLevel.Error)
            {
                Logger.Error(MessageToWrite());
            }

            else if (Logger.IsFatalEnabled && Level == LogLevel.Fatal)
            {
                Logger.Fatal(MessageToWrite());
            }
        }

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
