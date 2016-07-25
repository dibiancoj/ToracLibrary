using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.AspNet.SessionState;
using ToracLibrary.Caching;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.Email;
using ToracLibrary.Core.Security.Encryption;
using ToracLibrary.DIContainer;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;
using ToracLibrary.HtmlParsing;
using ToracLibrary.UnitTest.AspNet.AspNet;
using ToracLibrary.UnitTest.AspNet.AspNetMVC;
using ToracLibrary.UnitTest.AspNet.AspNetMVC.CustomActionResults;
using ToracLibrary.UnitTest.AspNet.AspNetMVC.HtmlHelpers;
using ToracLibrary.UnitTest.Caching;
using ToracLibrary.UnitTest.Core;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.UnitTest.EmailSMTP;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.HtmlParsing;
using static ToracLibrary.UnitTest.AspNet.AspNetMVC.CustomValueProviderFactory.JsonNetCustomValueProviderFactoryTest;
using static ToracLibrary.UnitTest.AspNet.AspNetMVC.ExtensionMethods.ControllerExtensions.ControllerExtensionTest;

namespace ToracLibrary.UnitTest.Framework
{

    /// <summary>
    /// DI Unit Test Container
    /// </summary>
    /// <remarks>Needs the TestClass attribute so the AssemblyInitialize gets called</remarks>
    public class DIUnitTestContainer
    {

        #region Constructor

        /// <summary>
        /// Constructor to init the container
        /// </summary>
        static DIUnitTestContainer()
        {
            //create the new di container
            DIContainer = new ToracDIContainer();

            //go configure the di container
            ConfigureDIContainer(DIContainer);
        }

        #endregion

        #region Properties

        /// <summary> 
        /// Declare a di container so we can build whatever we need (need the setter because we aren't setting the variable in the static constructor - we need assembly initalize)
        /// </summary>
        public static ToracDIContainer DIContainer { get; }

        #endregion

        #region Static Methods

        private static void ConfigureDIContainer(ToracDIContainer Container)
        {
            //start of entity framework configuration
            Container.Register<EntityFrameworkDP<EntityFrameworkEntityDP>>()
                .WithFactoryName(EntityFrameworkTest.ReadonlyDataProviderName)
                .WithConstructorImplementation((di) => new EntityFrameworkDP<EntityFrameworkEntityDP>(false, true, false));

            Container.Register<EntityFrameworkDP<EntityFrameworkEntityDP>>()
                .WithFactoryName(EntityFrameworkTest.WritableDataProviderName)
                .WithConstructorImplementation((di) => new EntityFrameworkDP<EntityFrameworkEntityDP>(true, true, false));
            //end of entity framework configuration

            //************************************************************

            //start of sql data provider configuration
            DIContainer.Register<IDataProvider, SQLDataProvider>()
                .WithConstructorParameters(new PrimitiveCtorParameter(SqlDataProviderTest.ConnectionStringToUse()));
            //end of sql data provider configuration

            //************************************************************

            //asp net shared mock start

            //now let's register a simple mock http response
            DIContainer.Register<MockHttpResponse>()
                .WithFactoryName(AspNetDIContainerSharedMock.AspNetMockFactoryName);

            //add a mocked request (simple mock request)
            DIContainer.Register<MockHttpRequest>()
                .WithFactoryName(AspNetDIContainerSharedMock.AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockHttpRequest(null, null, null, null, null, null));

            //add a mocked session state
            DIContainer.Register<MockHttpSessionState>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetDIContainerSharedMock.AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockHttpSessionState(null));

            //add the identity
            DIContainer.Register<MockIdentity>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetDIContainerSharedMock.AspNetMockFactoryName)
                .WithConstructorParameters(new PrimitiveCtorParameter("TestUser"));

            //add the mocked principal
            DIContainer.Register<MockPrincipal>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetDIContainerSharedMock.AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockPrincipal(di.Resolve<MockIdentity>(), new string[] { "Role1", "Role2" }));

            //end of asp net shared mock.

            //************************************************************

            //start of html helper tests

            DIContainer.Register<ViewContext>()
              .WithFactoryName(HtmlHelperTest.HtmlHelperTestDIFactoryName)
              .WithConstructorOverload();

            //register the IViewDataContainer
            DIContainer.Register<IViewDataContainer, MockIViewDataContainer>()
                .WithFactoryName(HtmlHelperTest.HtmlHelperTestDIFactoryName)
                .WithConstructorOverload();

            //now let's register the actual html helper we are going to mock
            DIContainer.Register<HtmlHelper<HtmlHelperTest.HtmlHelperTestViewModel>>()
                .WithFactoryName(HtmlHelperTest.HtmlHelperTestDIFactoryName);

            //end of html helper tests

            //************************************************************

            //start of json action result

            DIContainer.Register<JsonActionResultTest.JsonNetActionControllerTest>()
               .WithFactoryName(JsonActionResultTest.JsonActionResultFactoryName)
               .WithConstructorImplementation((di) => JsonActionResultTest.JsonNetActionControllerTest.MockController(di));

            //end of json action result

            //************************************************************

            //start of json net custom value factory

            DIContainer.Register<JsonNetCustomValueProviderFactoryControllerTest>()
              .WithFactoryName(JsonNetCustomValueProviderFactoryName)
              .WithConstructorImplementation((di) => JsonNetCustomValueProviderFactoryControllerTest.MockController(di));

            //register the mocked request
            DIContainer.Register<MockHttpRequest>()
                .WithFactoryName(JsonNetCustomValueProviderFactoryName)
                .WithConstructorImplementation((di) => JsonNetCustomValueProviderFactoryControllerTest.MockRequest(di));

            //end of json net custom value factory

            //************************************************************

            //start of asp net test

            DIContainer.Register<MockHttpRequest>()
              .WithFactoryName(AspNetTest.MockFactoryNameForSessionStateExportWithBaseHttpRequest)
              .WithConstructorImplementation((di) => new MockHttpRequest(null, null, null, new HttpCookieCollection() { new HttpCookie(ExportCurrentSessionState.SessionStateCookieName, AspNetTest.SessionIdToTest) }, null, null));

            //end of asp net test

            //************************************************************

            //start of in memory cache

            //let's register my dummy cache container
            DIContainer.Register<ICacheImplementation<IEnumerable<DummyObject>>, InMemoryCache<IEnumerable<DummyObject>>>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(InMemoryCacheTest.DIFactoryName)
                .WithConstructorImplementation((di) => new InMemoryCache<IEnumerable<DummyObject>>(InMemoryCacheTest.CacheKeyToUse, () => InMemoryCacheTest.DummyObjectCacheNoDI.BuildCacheDataSource()));

            //end of in memory cache

            //************************************************************

            //start of sql cache dep

            //let's register my dummy cache container
            DIContainer.Register<ICacheImplementation<IEnumerable<DummyObject>>, SqlCacheDependency<IEnumerable<DummyObject>>>(ToracDIContainer.DIContainerScope.Singleton)
                          .WithFactoryName(SqlCacheDependencyTest.DIFactoryName)
                          .WithConstructorImplementation((di) => new SqlCacheDependency<IEnumerable<DummyObject>>(
                                                                  SqlCacheDependencyTest.CacheKeyToUse,
                                                                  () => SqlCacheDependencyTest.DummySqlCacheObjectCacheNoDI.BuildCacheDataSource(),
                                                                  SqlDataProviderTest.ConnectionStringToUse(),
                                                                  SqlCacheDependencyTest.DatabaseSchemaUsedForCacheRefresh,
                                                                  SqlCacheDependencyTest.CacheSqlToUseToTriggerRefresh));

            //end of sql cache dep

            //************************************************************

            //start of email mock

            DIContainer.Register<ISMTPEmailServer, EmailTest.MockSMTPEmailServer>();

            //end of email mock

            //************************************************************

            //start of html parsing

            DIContainer.Register<HtmlParserWrapper>()
               .WithFactoryName(HtmlParsingTest.HtmlParserFactoryName)
               .WithConstructorImplementation((di) => new HtmlParserWrapper(string.Format($"<html><span class='{HtmlParsingTest.ClassNameInSpan}'>{HtmlParsingTest.SpanInnerTextValue}</span></html>")));

            //end of html parsing

            //************************************************************

            //start of encryption

            //let's register the di container now (md5)
            DIContainer.Register<ITwoWaySecurityEncryption, MD5HashSecurityEncryption>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(EncryptionSecurityTest.MD5DIContainerName)
                .WithConstructorParameters(new PrimitiveCtorParameter("Test"));

            //let's register the rijndael container now
            DIContainer.Register<ITwoWaySecurityEncryption, RijndaelSecurityEncryption>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(EncryptionSecurityTest.RijndaelDIContainerName)
                .WithConstructorParameters(new PrimitiveCtorParameter("1234567891123456"), new PrimitiveCtorParameter("1234567891123456"));

            //let's register the 1 way data binding
            DIContainer.Register<IOneWaySecurityEncryption, SHA256SecurityEncryption>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(EncryptionSecurityTest.SHA256ContainerName);

            //end of encryption

            //************************************************************

            //start of controller extensions tests

            DIContainer.Register<ControllerExtensionTestController>()
                .WithFactoryName(ControllerExtensionFactoryName)
                .WithConstructorImplementation((di) => ControllerExtensionTestController.MockController(di));

            //add the mock view engine
            DIContainer.Register<IViewEngine, MockIViewEngine>()
                .WithFactoryName(ControllerExtensionFactoryName)
                .WithConstructorImplementation((di) => new MockIViewEngine(new Dictionary<string, IView>() { { "_TestView", new CustomView() } }));

            //we will need to mock a view engine
            ViewEngines.Engines.Clear();

            //now add the new mock view engine
            ViewEngines.Engines.Add(DIContainer.Resolve<IViewEngine>(ControllerExtensionFactoryName));

            //end of controller extension tests
        }

        #endregion

    }

}
