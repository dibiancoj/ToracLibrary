using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.DIContainer;
using ToracLibrary.DIContainer.Exceptions;

namespace ToracLibraryTest.UnitsTest.DiContainer
{

    /// <summary>
    /// Unit test to test the Torac DI Container
    /// </summary>
    [TestClass]
    public class DIContainerTest
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
        /// Let's make sure if we don't register an item, then when we go to resolve it, it will fail
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        public void NoRegistrationErrorTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //let's grab an instance now, but we never registered it...so it should raise an error
            ILogger LoggerToUse = DIContainer.Resolve<ILogger>();
        }

        /// <summary>
        /// Test the interface base transient for the DI container works
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseTransientRegistrationTest1()
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

            //let's ensure it's not using a singleton
            Assert.AreEqual(string.Empty, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        /// <summary>
        /// Test the interface base singleton for the DI container works
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseSingletonRegistrationTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(ToracDIContainer.DIContainerScope.Singleton);

            //let's grab an instance now
            ILogger LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //its a singleton, so it should return the same instance which already has the test we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        #endregion

    }

}