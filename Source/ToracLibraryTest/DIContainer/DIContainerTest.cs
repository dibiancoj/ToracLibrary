using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.DIContainer;
using ToracLibrary.DIContainer.Exceptions;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;
using static ToracLibrary.DIContainer.ToracDIContainer;

namespace ToracLibraryTest.UnitsTest.DIContainer
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

        /// <summary>
        /// Used to make sure resolving with multiple levels deep to resolve
        /// </summary>
        private interface IBaseLogger
        {
            ILogger Logger { get; set; }
        }

        /// <summary>
        /// Used to make sure resolving with multiple levels deep to resolve
        /// </summary>
        private class BaseLogger : IBaseLogger
        {

            #region Constructor

            public BaseLogger(ILogger LoggerToSet)
            {
                Logger = LoggerToSet;
            }

            #endregion

            #region Properties

            public ILogger Logger { get; set; }

            #endregion

        }

        /// <summary>
        /// Multiple level test
        /// </summary>
        private class SqlDIProviderWithBaseLogger
        {
            public SqlDIProviderWithBaseLogger(IBaseLogger BaseLoggerToSet)
            {
                BaseLogger = BaseLoggerToSet;
            }

            internal IBaseLogger BaseLogger { get; }
        }

        /// <summary>
        /// Interface Of ILogger to retrieve
        /// </summary>
        private interface ILogger
        {
            StringBuilder LogFile { get; }
            void Log(string TextToLog);
        }

        /// <summary>
        /// Implementation of ILogger
        /// </summary>
        private class Logger : ILogger
        {
            public StringBuilder LogFile { get; } = new StringBuilder();

            public void Log(string TextToLog)
            {
                LogFile.Append(TextToLog);
            }
        }

        /// <summary>
        /// Implementation to test with a constructor parameter
        /// </summary>
        private class SectionFactoryWithConstructorParameter
        {
            public SectionFactoryWithConstructorParameter(ILogger logger)
            {
                Log = logger;
            }

            public ILogger Log { get; }
        }

        /// <summary>
        /// Data provider that we will pass parameters. ILogger needs to get resolved from the container. The connection string is just a primitive type
        /// </summary>
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

        /// <summary>
        /// Implementation with a generic type
        /// </summary>
        /// <typeparam name="T">Type of the data provider</typeparam>
        private class SqlDIWithTypeProvider<T>
        {
            public Type GetTypeOfT()
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Implementation with multiple constructors
        /// </summary>
        private class OverloadedConstructor
        {

            #region Constructor

            public OverloadedConstructor()
            {
            }

            public OverloadedConstructor(string DescriptionToSet) : this(DescriptionToSet, null)
            {
            }

            public OverloadedConstructor(string DescriptionToSet, ILogger LoggerToSet)
            {
                Description = DescriptionToSet;
                Logger = LoggerToSet;
            }

            #endregion

            #region Properties

            public string Description { get; }

            public ILogger Logger { get; }

            #endregion

        }

        #endregion

        #region Unit Tests

        #region Exception Tests

        /// <summary>
        /// Let's make sure if we don't register an item, then when we go to resolve it, it will fail
        /// </summary>        
        [TestCategory("DIContainer")]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        [TestMethod]
        public void NoRegistrationErrorTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //let's grab an instance now, but we never registered it...so it should raise an error
            var LoggerToUse = DIContainer.Resolve<ILogger>();
        }

        /// <summary>
        /// Test multiple types with no factory name.This should blow up with a MultipleTypesFoundException error
        /// </summary>
        [TestCategory("DIContainer")]
        [ExpectedException(typeof(MultipleTypesFoundException))]
        [TestMethod]
        public void MultipleTypeFoundErrorTest1()
        {
            //this example would be for factories
            //PolicyFactory implements SectionFactory
            //ClaimFactory implements SectionFactory

            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //register a second instance
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //now try to resolve it would should lead to an error
            DIContainer.Resolve<ILogger>();
        }

        #endregion

        #region Resolve All

        /// <summary>
        /// Test resolve all when you have multiple factories
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ResolveAllTest1()
        {
            //this example would be for factories
            //PolicyFactory implements SectionFactory
            //ClaimFactory implements SectionFactory

            //declare my container
            var DIContainer = new ToracDIContainer();

            //prefix for factory
            const string FactoryNamePrefix = "Factory";

            //store the factory names
            const string FactoryName1 = FactoryNamePrefix + "0";
            const string FactoryName2 = FactoryNamePrefix + "1";

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName2);

            //count how many resolve all items we have
            int ResolveAllResultCount = 0;

            //lets loop through the resolve all
            foreach (var FactoryToResolve in DIContainer.ResolveAllLazy<ILogger>().OrderBy(x => x.Key))
            {
                //let's resolve based on the index (so index 0 is the first factory)
                Assert.AreEqual($"{FactoryNamePrefix}{ResolveAllResultCount}", FactoryToResolve.Key);

                //and just make sure the actual instance is not null
                Assert.IsNotNull(FactoryToResolve.Value);

                //increase the count
                ResolveAllResultCount++;
            }

            //make sure we have 2 factories that resolved
            Assert.AreEqual(2, ResolveAllResultCount);
        }

        #endregion

        #region Clear All

        /// <summary>
        /// Test clearing of all the registrations for a specific type
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ClearAllRegistrationsForSpecificTypeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //we are mixing up the order below to make sure the removal works correctly

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>().WithFactoryName(Guid.NewGuid().ToString());

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                 .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

            //register a second instance
            DIContainer.Register<ILogger, Logger>().WithFactoryName(Guid.NewGuid().ToString());

            //let's make sure we have 2 instances of ilogger
            Assert.AreEqual(2, DIContainer.AllRegistrationSelectLazy(typeof(ILogger)).Count());

            //clear all the registrations
            DIContainer.ClearAllRegistrationsForSpecificType<ILogger>();

            //make sure we have 1 item left in the container, we can't resolve sql di provider because ilogger is a dependency!!!!!
            Assert.AreEqual(1, DIContainer.AllRegistrationSelectLazy().Count(x => x.ConcreteType == typeof(SqlDIProvider)));
        }

        /// <summary>
        /// Test clearing of all the registrations
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ClearAllRegistrationsTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>().WithFactoryName(Guid.NewGuid().ToString());

            //register a second instance
            DIContainer.Register<ILogger, Logger>().WithFactoryName(Guid.NewGuid().ToString());

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

            //clear all the registrations
            DIContainer.ClearAllRegistrations();

            //make sure we have 0 items in the container
            Assert.AreEqual(0, DIContainer.AllRegistrationSelectLazy().Count());
        }

        #endregion

        #region Get All Items In The Container

        /// <summary>
        /// Get all the registered items in the container
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void AllRegistrationsInContainerTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //store the factory names
            const string FactoryName1 = "FactoryName1";
            const string FactoryName2 = "FactoryName2";

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName2);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

            //now let's check what items we have registered
            var ItemsRegistered = DIContainer.AllRegistrationSelectLazy().ToArray();

            //make sure we have 3 items
            Assert.AreEqual(3, ItemsRegistered.Length);

            //make sure we have factory 1
            Assert.IsTrue(ItemsRegistered.Any(x => x.FactoryName == FactoryName1));

            //make sure the logger is a transient
            Assert.AreEqual(DIContainerScope.Transient, ItemsRegistered.First(x => x.FactoryName == FactoryName1).ObjectScope);

            //make sure the second factory is a transient
            Assert.AreEqual(DIContainerScope.Transient, ItemsRegistered.First(x => x.FactoryName == FactoryName2).ObjectScope);

            //make sure the sql di provider is a singleton
            Assert.AreEqual(DIContainerScope.Singleton, ItemsRegistered.First(x => x.TypeToResolve == typeof(SqlDIProvider)).ObjectScope);

            //make sure we have factory 2
            Assert.IsTrue(ItemsRegistered.Any(x => x.FactoryName == FactoryName2));

            //make sure we have the sql di provider now
            Assert.IsTrue(ItemsRegistered.Any(x => x.ConcreteType == typeof(SqlDIProvider)));
        }

        #endregion

        #region Default Scope

        /// <summary>
        /// Test that the default scope is a transient
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void DefaultScopeTest1()
        {
            //what is the default scope
            Assert.AreEqual(DIContainerScope.Transient, ToracDIContainer.DefaultScope);
        }

        #endregion

        #region Per Thread Lifetime

        /// <summary>
        /// Test that we can resolve a per thread lifetime
        /// </summary>
        [TestCategory("DIContainer")]
        [TestMethod]
        public void PerThreadLifeTimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure the log has the value we wrote to it.
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());

            //let's resolve another logger...this should be the same logger
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());

            //let's kill the reference to the logger...so this way we can cleanup the memory and have gc collect it
            LoggerToUse = null;

            //run gc a few times
            GC.Collect();

            //let's try to grab a new logger...and see if we still have the same log text we wrote into it....we most likely shouldn't
            var NewLoggerReference = DIContainer.Resolve<ILogger>();

            //we should be expecting a blank string because this should be a new logger
            Assert.AreEqual(string.Empty, DIContainer.Resolve<ILogger>().LogFile.ToString());

            //let's try to gc collect and see if the new logger reference is gone
            GC.Collect();

            //make sure we still have a logger
            Assert.IsNotNull(NewLoggerReference);
        }

        #endregion

        #region Resolve

        //1. Simple Interface To Concrete
        //2. Simple Concrete To Concrete
        //3. Concrete With Every Constructor Parameter Is In Container
        //4. Concrete With Constructor Implementation
        //5. Test Multi Level Resolve. Parent To Child To Child
        //6. Generic Types
        //7. Factory Names
        //8. Overload Constructors
        //9. Constructor with object set parameters
        //10. Constructor With ResolveCtorParameter And Factory Name

        #region Simple Interface To Concrete

        [TestCategory("DIContainer.SimpleInterfaceToConcete.Transient")]
        [TestCategory("DIContainer.SimpleInterfaceToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void InterfaceToConcreteTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

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

        [TestCategory("DIContainer.SimpleInterfaceToConcete.Singleton")]
        [TestCategory("DIContainer.SimpleInterfaceToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void InterfaceToConcreteSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure the singleton still has the text value
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        [TestCategory("DIContainer.SimpleInterfaceToConcete.PerThreadLifetime")]
        [TestCategory("DIContainer.SimpleInterfaceToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void InterfaceToConcretePerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<ILogger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure the ThreadLifeTime still has the text value
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<ILogger>().LogFile.ToString());

            //clear out the variable
            LoggerToUse = null;

            //run gc to cleanup
            GC.Collect();

            //Logger should be null now
            Assert.AreEqual(string.Empty, DIContainer.Resolve<ILogger>().LogFile.ToString());
        }

        #endregion

        #region Simple Concrete To Concrete

        [TestCategory("DIContainer.SimpleConcreteToConcete.Transient")]
        [TestCategory("DIContainer.SimpleConcreteToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteToConceteTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<Logger>(DIContainerScope.Transient);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<Logger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure it's not using a singleton. Grab a new instance of ILogger, and make sure the result is empty
            Assert.AreEqual(string.Empty, DIContainer.Resolve<Logger>().LogFile.ToString());
        }

        [TestCategory("DIContainer.SimpleConcreteToConcete.Singleton")]
        [TestCategory("DIContainer.SimpleConcreteToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteToConceteSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<Logger>(DIContainerScope.Singleton);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<Logger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure the singleton still has the text value
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<Logger>().LogFile.ToString());
        }

        [TestCategory("DIContainer.SimpleConcreteToConcete.PerThreadLifetime")]
        [TestCategory("DIContainer.SimpleConcreteToConcete")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteToConcetePerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<Logger>(DIContainerScope.PerThreadLifetime);

            //let's grab an instance now
            var LoggerToUse = DIContainer.Resolve<Logger>();

            //make sure the logger is not null
            Assert.IsNotNull(LoggerToUse);

            //write test to the log
            LoggerToUse.Log(WriteToLog);

            //now let's check the log
            Assert.AreEqual(WriteToLog, LoggerToUse.LogFile.ToString());

            //let's ensure the ThreadLifeTime still has the text value
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<Logger>().LogFile.ToString());

            //clear out the variable
            LoggerToUse = null;

            //run gc to cleanup
            GC.Collect();

            //Logger should be null now
            Assert.AreEqual(string.Empty, DIContainer.Resolve<Logger>().LogFile.ToString());
        }

        #endregion

        #region Concrete With All Constructor Params Are In Container

        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer.Transient")]
        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteAllConstParamsInContainerTransientTest1()
        {
            //basically want to test a transient with a dependency in the constructor passed in

            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SectionFactoryWithConstructorParameter>(DIContainerScope.Transient);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SectionFactoryWithConstructorParameter>());
        }

        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer.Singleton")]
        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteAllConstParamsInContainerSingletonTest1()
        {
            //basically want to test a transient with a dependency in the constructor passed in

            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SectionFactoryWithConstructorParameter>(DIContainerScope.Singleton);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SectionFactoryWithConstructorParameter>());
        }

        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer.PerThreadLifetime")]
        [TestCategory("DIContainer.ConcreteAllConstParamsInContainer")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteAllConstParamsInContainerPerThreadLifetimeTest1()
        {
            //basically want to test a transient with a dependency in the constructor passed in

            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SectionFactoryWithConstructorParameter>(DIContainerScope.PerThreadLifetime);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SectionFactoryWithConstructorParameter>());
        }

        #endregion

        #region Concrete With Constructor Implementation

        [TestCategory("DIContainer.ConcreteWithConstructorImplementation.Transient")]
        [TestCategory("DIContainer.ConcreteWithConstructorImplementation")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteWithConstructorImplementationTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Transient)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

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

            //logger is a transient...it should be empty
            Assert.AreEqual(string.Empty, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConcreteWithConstructorImplementation.Singleton")]
        [TestCategory("DIContainer.ConcreteWithConstructorImplementation")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteWithConstructorImplementationSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

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

            //logger is a singleton...it should be filled
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConcreteWithConstructorImplementation.PerThreadLifetime")]
        [TestCategory("DIContainer.ConcreteWithConstructorImplementation")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteWithConstructorImplementationPerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.PerThreadLifetime)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

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

            //logger is a thread per lifetime...it should be filled
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConcreteWithConstructorImplementation.SingletonWithPerThreadLifetime")]
        [TestCategory("DIContainer.ConcreteWithConstructorImplementation")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConcreteWithConstructorImplementationSingletonWithPerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorImplementation((di) => new SqlDIProvider(ConnectionStringToUse, di.Resolve<ILogger>()));

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

            //logger is a transient, it should be a new instance and blank
            Assert.AreEqual(string.Empty, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        #endregion

        #region Multi Level Resolve. Parent To Child To Child

        [TestCategory("DIContainer.MultiLevelResolve.Transient")]
        [TestCategory("DIContainer.MultiLevelResolve")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void MultiLevelResolveTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //add the regular logger now
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

            //add the base logger now
            DIContainer.Register<IBaseLogger, BaseLogger>(DIContainerScope.Transient);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProviderWithBaseLogger>(DIContainerScope.Transient);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SqlDIProviderWithBaseLogger>());
        }

        [TestCategory("DIContainer.MultiLevelResolve.Singleton")]
        [TestCategory("DIContainer.MultiLevelResolve")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void MultiLevelResolveSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //add the regular logger now
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //add the base logger now
            DIContainer.Register<IBaseLogger, BaseLogger>(DIContainerScope.Singleton);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProviderWithBaseLogger>(DIContainerScope.Singleton);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SqlDIProviderWithBaseLogger>());
        }

        [TestCategory("DIContainer.MultiLevelResolve.PerThreadLifetime")]
        [TestCategory("DIContainer.MultiLevelResolve")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void MultiLevelResolvePerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //add the regular logger now
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //add the base logger now
            DIContainer.Register<IBaseLogger, BaseLogger>(DIContainerScope.PerThreadLifetime);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProviderWithBaseLogger>(DIContainerScope.PerThreadLifetime);

            //let's grab an the data provide rnow
            Assert.IsNotNull(DIContainer.Resolve<SqlDIProviderWithBaseLogger>());
        }

        #endregion

        #region Generic Types

        [TestCategory("DIContainer.GenericTypes.Transient")]
        [TestCategory("DIContainer.GenericTypes")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void GenericTypesTransientTest1()
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

        [TestCategory("DIContainer.GenericTypes.Singleton")]
        [TestCategory("DIContainer.GenericTypes")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void GenericTypesTSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<SqlDIWithTypeProvider<string>>(DIContainerScope.Singleton);

            //let's grab an instance now
            var GenericTypeToUse = DIContainer.Resolve<SqlDIWithTypeProvider<string>>();

            //make sure the logger is not null
            Assert.IsNotNull(GenericTypeToUse);

            //grab the type of T
            Assert.AreEqual(typeof(string), GenericTypeToUse.GetTypeOfT());
        }

        [TestCategory("DIContainer.GenericTypes.PerThreadLifetime")]
        [TestCategory("DIContainer.GenericTypes")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void GenericTypesTPerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<SqlDIWithTypeProvider<string>>(DIContainerScope.PerThreadLifetime);

            //let's grab an instance now
            var GenericTypeToUse = DIContainer.Resolve<SqlDIWithTypeProvider<string>>();

            //make sure the logger is not null
            Assert.IsNotNull(GenericTypeToUse);

            //grab the type of T
            Assert.AreEqual(typeof(string), GenericTypeToUse.GetTypeOfT());
        }

        #endregion

        #region Factory Names

        [TestCategory("DIContainer.FactoryNames.Transient")]
        [TestCategory("DIContainer.FactoryNames")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void FactoryNamesTransientTest1()
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
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>().WithFactoryName(FactoryName2);

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

        [TestCategory("DIContainer.FactoryNames.Singleton")]
        [TestCategory("DIContainer.FactoryNames")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void FactoryNamesSingletonTest1()
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
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton).WithFactoryName(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton).WithFactoryName(FactoryName2);

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

        [TestCategory("DIContainer.FactoryNames.PerThreadLifetime")]
        [TestCategory("DIContainer.FactoryNames")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void FactoryNamesPerThreadLifeTimeTest1()
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
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime).WithFactoryName(FactoryName1);

            //register a second instance
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime).WithFactoryName(FactoryName2);

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

        #endregion

        #region Overload Constructors

        [TestCategory("DIContainer.OverloadConstructors.Transient")]
        [TestCategory("DIContainer.OverloadConstructors")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void OverloadedConstructorTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //the 3 factory names so we can test this in 1 method
            const string NoParameterOverload = "NoParameterOverload";
            const string ParameterWithStringOverload = "ParameterWithStringOverload";
            const string ParameterWithStringAndLoggerOverload = "ParameterWithStringAndLoggerOverload";

            //the string we pass in to test the overload constructor
            const string ConstructorParameterValue = "Jason 123";

            //let's register ILogger so we have it for the last register
            DIContainer.Register<ILogger, Logger>();

            //register my item now with no overloads
            DIContainer.Register<OverloadedConstructor>()
                .WithFactoryName(NoParameterOverload);

            //register the one with the string overload
            DIContainer.Register<OverloadedConstructor>()
                .WithFactoryName(ParameterWithStringOverload)
                .WithConstructorOverload(typeof(string))
                .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue));

            //register the one with the string and logger overload
            DIContainer.Register<OverloadedConstructor>()
             .WithFactoryName(ParameterWithStringAndLoggerOverload)
             .WithConstructorOverload(typeof(string), typeof(ILogger))
             .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

            //let's grab an instance now with no parameters
            var OverloadWithNoParameters = DIContainer.Resolve<OverloadedConstructor>(NoParameterOverload);

            //the string should be null
            Assert.IsNull(OverloadWithNoParameters.Description);

            //--------------------------------------------------------

            //now grab the item with the string only overload
            var OverloadWithStringParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringParameter.Description);

            //--------------------------------------------------------

            //now grab the item with the string and ilogger overload
            var OverloadWithStringAndILoggerOverloadParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringAndLoggerOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringAndILoggerOverloadParameter.Description);

            //make sure ILogger is not null
            Assert.IsNotNull(OverloadWithStringAndILoggerOverloadParameter.Logger);
        }

        [TestCategory("DIContainer.OverloadConstructors.Singleton")]
        [TestCategory("DIContainer.OverloadConstructors")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void OverloadedConstructorSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //the 3 factory names so we can test this in 1 method
            const string NoParameterOverload = "NoParameterOverload";
            const string ParameterWithStringOverload = "ParameterWithStringOverload";
            const string ParameterWithStringAndLoggerOverload = "ParameterWithStringAndLoggerOverload";

            //the string we pass in to test the overload constructor
            const string ConstructorParameterValue = "Jason 123";

            //let's register ILogger so we have it for the last register
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //register my item now with no overloads
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.Singleton).WithFactoryName(NoParameterOverload);

            //register the one with the string overload
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.Singleton)
                .WithFactoryName(ParameterWithStringOverload)
                .WithConstructorOverload(typeof(string))
                .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue));

            //register the one with the string and logger overload
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.Singleton)
             .WithFactoryName(ParameterWithStringAndLoggerOverload)
             .WithConstructorOverload(typeof(string), typeof(ILogger))
             .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

            //let's grab an instance now with no parameters
            var OverloadWithNoParameters = DIContainer.Resolve<OverloadedConstructor>(NoParameterOverload);

            //the string should be null
            Assert.IsNull(OverloadWithNoParameters.Description);

            //--------------------------------------------------------

            //now grab the item with the string only overload
            var OverloadWithStringParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringParameter.Description);

            //--------------------------------------------------------

            //now grab the item with the string and ilogger overload
            var OverloadWithStringAndILoggerOverloadParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringAndLoggerOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringAndILoggerOverloadParameter.Description);

            //make sure ILogger is not null
            Assert.IsNotNull(OverloadWithStringAndILoggerOverloadParameter.Logger);
        }

        [TestCategory("DIContainer.OverloadConstructors.PerThreadLifetime")]
        [TestCategory("DIContainer.OverloadConstructors")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void OverloadedConstructorPerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //the 3 factory names so we can test this in 1 method
            const string NoParameterOverload = "NoParameterOverload";
            const string ParameterWithStringOverload = "ParameterWithStringOverload";
            const string ParameterWithStringAndLoggerOverload = "ParameterWithStringAndLoggerOverload";

            //the string we pass in to test the overload constructor
            const string ConstructorParameterValue = "Jason 123";

            //let's register ILogger so we have it for the last register
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //register my item now with no overloads
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.PerThreadLifetime).WithFactoryName(NoParameterOverload);

            //register the one with the string overload
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.PerThreadLifetime)
                .WithFactoryName(ParameterWithStringOverload)
                .WithConstructorOverload(typeof(string))
                .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue));

            //register the one with the string and logger overload
            DIContainer.Register<OverloadedConstructor>(DIContainerScope.PerThreadLifetime)
             .WithFactoryName(ParameterWithStringAndLoggerOverload)
             .WithConstructorOverload(typeof(string), typeof(ILogger))
             .WithConstructorParameters(new PrimitiveCtorParameter(ConstructorParameterValue), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

            //let's grab an instance now with no parameters
            var OverloadWithNoParameters = DIContainer.Resolve<OverloadedConstructor>(NoParameterOverload);

            //the string should be null
            Assert.IsNull(OverloadWithNoParameters.Description);

            //--------------------------------------------------------

            //now grab the item with the string only overload
            var OverloadWithStringParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringParameter.Description);

            //--------------------------------------------------------

            //now grab the item with the string and ilogger overload
            var OverloadWithStringAndILoggerOverloadParameter = DIContainer.Resolve<OverloadedConstructor>(ParameterWithStringAndLoggerOverload);

            //make sure the value is correct
            Assert.AreEqual(ConstructorParameterValue, OverloadWithStringAndILoggerOverloadParameter.Description);

            //make sure ILogger is not null
            Assert.IsNotNull(OverloadWithStringAndILoggerOverloadParameter.Logger);
        }

        #endregion

        #region Constructor With Object Set Parameters

        [TestCategory("DIContainer.ConstructorWithObjectSetParameters.Transient")]
        [TestCategory("DIContainer.ConstructorWithObjectSetParameters")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithObjectSetParametersTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Transient)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(string.Empty, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConstructorWithObjectSetParameters.Singleton")]
        [TestCategory("DIContainer.ConstructorWithObjectSetParameters")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithObjectSetParametersSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConstructorWithObjectSetParameters.PerThreadLifetime")]
        [TestCategory("DIContainer.ConstructorWithObjectSetParameters")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithObjectSetParametersPerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime);

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.PerThreadLifetime)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>()));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        #endregion

        #region Constructor With ResolveCtorParameter And Factory Name

        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName.Transient")]
        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithResolveWithFactoryNameTransientTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //factory name
            var FactoryName = Guid.NewGuid().ToString();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient)
                .WithFactoryName(FactoryName);

            DIContainer.Register<ILogger, Logger>(DIContainerScope.Transient)
                .WithFactoryName("LoggerVersion2");

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Transient)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>(FactoryName)));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(string.Empty, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName.Singleton")]
        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithResolveWithFactoryNameSingletonTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //factory name
            var FactoryName = Guid.NewGuid().ToString();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton)
                .WithFactoryName(FactoryName);

            DIContainer.Register<ILogger, Logger>(DIContainerScope.Singleton)
             .WithFactoryName("LoggerVersion2");

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.Singleton)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>(FactoryName)));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName.PerThreadLifetime")]
        [TestCategory("DIContainer.ConstructorWithResolveWithFactoryName")]
        [TestCategory("DIContainer")]
        [TestMethod]
        public void ConstructorWithResolveWithFactoryNamePerThreadLifetimeTest1()
        {
            //declare my container
            var DIContainer = new ToracDIContainer();

            //factory name
            var FactoryName = Guid.NewGuid().ToString();

            //register my item now with no overloads
            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime)
                .WithFactoryName(FactoryName);

            DIContainer.Register<ILogger, Logger>(DIContainerScope.PerThreadLifetime)
                .WithFactoryName("LoggerVersion2");

            //let's register the data provider (since a string get's passed in, we need to specify how to create this guy)
            DIContainer.Register<SqlDIProvider>(DIContainerScope.PerThreadLifetime)
                .WithConstructorParameters(new PrimitiveCtorParameter(ConnectionStringToUse), new ResolveCtorParameter(x => x.Resolve<ILogger>(FactoryName)));

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

            //its a singleton, so it should return the same instance which already has the text we wrote into it
            Assert.AreEqual(WriteToLog, DIContainer.Resolve<SqlDIProvider>().LoggerToUse.LogFile.ToString());
        }

        #endregion

        #endregion

        #endregion

    }

}
