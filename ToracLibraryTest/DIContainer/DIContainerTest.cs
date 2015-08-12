using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.DIContainer;

namespace ToracLibraryTest.UnitsTest.DiContainer
{

    /// <summary>
    /// Unit test to test the Torac DI Container
    /// </summary>
    [TestClass]
    public class DiContainerTest
    {

        #region Framework For Tests

        #region Constants

        /// <summary>
        /// what to write to the log to test
        /// </summary>
        private const string WriteToLog = "Test123";

        #endregion

        private interface ILogger
        {
            StringBuilder LogFile { get; }
            void Log(string TextToLog);
        }

        private class Logger : ILogger
        {
            public StringBuilder LogFile { get; } = new StringBuilder();

            public void Log(string TextToLog)
            {
                LogFile.Append(TextToLog);
            }
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the in memory cache
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseRegistrationTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>();

            //let's grab an instance now
            ILogger LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());
        }

        #endregion

    }

}