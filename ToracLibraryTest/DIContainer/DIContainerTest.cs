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

        /// <summary>
        /// Holds the connection string to use when you pass it in to the SqlDIProvider
        /// </summary>
        private const string ConnectionStringToUse = "TestConn";

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

        private class SqlDIProvider
        {
            public SqlDIProvider(string ConnectionStringToSet, ILogger LoggerToSet)
            {
                ConnectionString = ConnectionStringToSet;
                LoggerToUse = LoggerToSet;
            }

            internal string ConnectionString { get; }
            internal ILogger LoggerToUse { get; }
        }

        private class SqlDIWithTypeProvider<T>
        {
            public Type GetTypeOfT()
            {
                return typeof(T);
            }
        }

        #endregion

        #region Unit Tests

        #region Exception Tests

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
            var LoggerToUse = DIContainer.Resolve<ILogger>();
        }

        /// <summary>
        /// Test multiple types with no factory name. This should blow up with a MultipleTypesFoundException error
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        [ExpectedException(typeof(MultipleTypesFoundException))]
        public void MultipleTypeFoundErrorTest1()
        {
            //this example would be for factories
            //PolicyFactory implements SectionFactory
            //ClaimFactory implements SectionFactory

            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(ToracDIContainer.DIContainerScope.Singleton);

            //register a second instance
            DIContainer.Register<ILogger, Logger>(ToracDIContainer.DIContainerScope.Singleton);
        }

        #endregion

        /// <summary>
        /// Test the interface base transient for the DI container works
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>();

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure it's not using a singleton. Grab a new instance of ILogger, and make sure the result is empty
            Assert.AreEqual(string.Empty, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        /// <summary>
        /// Test the interface base singleton for the DI container works
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(ToracDIContainer.DIContainerScope.Singleton);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //its a singleton, so it should return the same instance which already has the test we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        /// <summary>
        /// Test a concrete class to concrete class
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void ConcreteToConcreteWithConstructorParameterTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(ToracDIContainer.DIContainerScope.Singleton);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register(ToracDIContainer.DIContainerScope.Singleton, () => new SqlDIProvider(ConnectionStringToUse, DIContainer.Resolve<ILogger>()));

            //let's grab an the data provide rnow
            var DataProviderToUse = DIContainer.Resolve<SqlDIProvider>();

            //make sure the logger is not null
            Assert.IsNotNull(DataProviderToUse);

            //make sure the logger is not null
            Assert.IsNotNull(DataProviderToUse.LoggerToUse);

            //make sure the connection string is not null
            Assert.IsFalse(string.IsNullOrEmpty(DataProviderToUse.ConnectionString));

            //make sure the connection string is correct
            Assert.AreEqual(ConnectionStringToUse, DataProviderToUse.ConnectionString);

            //write test to the log
            DataProviderToUse.LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, DataProviderToUse.LoggerToUse.LogFile.ToString());

            //its a singleton, so it should return the same instance which already has the test we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        /// <summary>
        /// Test multiple types with a factory name
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void MultipleFactoriesWithFactoryNameTest1()
        {
            //this example would be for factories
            //PolicyFactory implements SectionFactory
            //ClaimFactory implements SectionFactory

            //declare my container
            var DIContainer = new ToracDIContainer();

            //store the factory names
            const string FactoryName1 = "FactoryName1";
            const string FactoryName2 = "FactoryName2";

            //what the 2nd factory will write
            const string Factory2LoggerTestString = "WriteToFactory2";

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>(FactoryName2);

            //let's try to resolve the first guy
            var FactoryLogger1 = DIContainer.Resolve<ILogger>(FactoryName1);

            //grab the 2nd ILogger
            var FactoryLogger2 = DIContainer.Resolve<ILogger>(FactoryName2);

            //make sure the logger is not null
            Assert.IsNotNull(FactoryLogger1);
            Assert.IsNotNull(FactoryLogger2);

            //let's test both loggers now
            //write test to the log
            FactoryLogger1.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, FactoryLogger1.LogFile.ToString());

            //let's write to the 2nd logger
            FactoryLogger2.Log(Factory2LoggerTestString);

            //check the log
            Assert.AreEqual(Factory2LoggerTestString, FactoryLogger2.LogFile.ToString());
        }

        /// <summary>
        /// Let's make sure generic types work. Where the method has a generic parameter
        /// </summary>
        [TestMethod]
        [TestCategory("ToracLibrary.DIContainer")]
        [TestCategory("DIContainer")]
        public void InterfaceBaseGenericTypeTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<SqlDIWithTypeProvider<string>>();

            //let's grab an instance now
            var GenericTypeToUse = DIContainer.Resolve<SqlDIWithTypeProvider<string>>();

            //make sure the logger is not null
            Assert.IsNotNull(GenericTypeToUse);

            //grab the type of T
            Assert.AreEqual(typeof(string), GenericTypeToUse.GetTypeOfT());
        }

        #endregion

    }

}