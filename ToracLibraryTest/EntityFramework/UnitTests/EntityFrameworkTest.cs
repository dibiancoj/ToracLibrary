using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.Framework;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Unit test for entity framework
    /// </summary>
    [TestClass]
    public class EntityFrameworkTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(UnityContainer DIContainer)
        {
            //let's register the di container for the readonly EF Data provider
            DIContainer.RegisterType<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName, new InjectionConstructor(false, true, false));

            //let's register the di container for the editable EF data provider
            DIContainer.RegisterType<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName, new InjectionConstructor(true, true, false));
        }

        #endregion

        #region Constants

        /// <summary>
        /// holds the di container name for the readonly data provider
        /// </summary>
        private const string ReadonlyDataProviderName = "EFReadOnly";

        /// <summary>
        /// holds the di container name for the insert or update data provider
        /// </summary>
        private const string WritableDataProviderName = "EFWritableOnly";

        #endregion

        #region Untyped Methods

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void BuildConnectionStringTest1()
        {
            //the value we want to check for
            const string ValueToCheckFor = "metadata=res://*/;provider=System.Data.SqlClient;provider connection string=\"Data Source=ServerName123;Initial Catalog=Db123;Integrated Security=True\"";

            //go run the method and check the results
            Assert.AreEqual(ValueToCheckFor, EFUnTypedDP.BuildConnectionString("ServerName123", "Db123"));
        }

        #endregion

        #region Data Provider Tests

        #region Can Connect

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void CanConnect()
        {
            //make sure we can connect
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName))
            {
                Assert.AreEqual(true, DP.CanConnectToDatabase(false).Item1);
            }
        }

        #endregion

        #region Schema

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void SchemaTest1()
        {
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName))
            {
                Assert.AreEqual("dbo", DP.SchemaOfTableSelect<Ref_Test>());
            }
        }

        #endregion

        #region Auto Detect Test

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void UpdateRecordWithAutoDetectFalseTest1()
        {
            //if you have auto detect false (constructor parameter)
            //then grab a record and update. If you dont have  thisContext.ChangeTracker.DetectChanges(); in the save changes it wont update it but wont raise an error
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //description to use
            const string DescriptionTest = "New Record";

            //update string to use
            const string DescriptionToUpdateWith = "Update Description";

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                //flip the auto detect off
                DP.EFContext.Configuration.AutoDetectChangesEnabled = false;

                //add a new record
                DP.Add(new Ref_Test() { Description = DescriptionTest }, true);

                //go grab this record
                var RecordToUpdate = DP.Find<Ref_Test>(x => x.Description == DescriptionTest, true).First();

                //update the description
                RecordToUpdate.Description = DescriptionToUpdateWith;

                //save the changes now
                DP.SaveChanges();

                //go find the updated record
                var UpdatedRecord = DP.Find<Ref_Test>(x => x.Description == DescriptionToUpdateWith, true).FirstOrDefault();

                //check to make sure we have a record
                Assert.IsNotNull(UpdatedRecord);

                //check the updated description
                Assert.AreEqual(DescriptionToUpdateWith, UpdatedRecord.Description);
            }
        }

        #endregion

        #region Primary Key Is Auto Seed Lookup

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void PrimaryKeyIsAutoSeedLookupTest1()
        {
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName))
            {
                //go check the Id and make sure it comes back as true
                Assert.AreEqual(true, DP.ColumnIsAutoSeedLookup(typeof(Ref_Test).Name, nameof(Ref_Test.Id)));

                //test a column that is not an auto seed
                Assert.AreEqual(false, DP.ColumnIsAutoSeedLookup(typeof(Ref_Test).Name, nameof(Ref_Test.Description)));
            }
        }

        #endregion

        #region Regular Entity Framework Data Provider Tests

        #region Get Or Add

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void GetOrAddTest1()
        {
            //go truncate the table to get ready for the test
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //go build the record to test
            var RecordToTest = BuildRows(1).First();

            //store what the description is before we change it
            string OriginalDescriptionValue = RecordToTest.Description;

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                //let's test the "add" part when there is no record
                var InsertedRecord = DP.GetOrAdd(RecordToTest, x => x.Id == RecordToTest.Id, false);

                Assert.AreEqual(RecordToTest.Id, InsertedRecord.Id);
                Assert.AreEqual(DP.EFContext.Ref_Test.Count(), 1);

                //we are going to test the get now...so we change the local record...and then we will test what the database has
                RecordToTest.Description = "New Description";

                //now go run a get or add
                var insertedRecord2 = DP.GetOrAdd(RecordToTest, x => x.Id == RecordToTest.Id, false);

                Assert.AreEqual(OriginalDescriptionValue, insertedRecord2.Description);
                Assert.AreEqual(DP.EFContext.Ref_Test.Count(), 1);
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task GetOrAddAsyncTest1()
        {
            //go truncate the table to get ready for the test
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //go build the record to test
            var RecordToTest = BuildRows(1).First();

            //store what the description is before we change it
            string OriginalDescriptionValue = RecordToTest.Description;

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                //let's test the "add" part when there is no record
                var InsertedRecord = await DP.GetOrAddAsync(RecordToTest, x => x.Id == RecordToTest.Id, false);

                Assert.AreEqual(RecordToTest.Id, InsertedRecord.Id);
                Assert.AreEqual(DP.EFContext.Ref_Test.Count(), 1);

                //we are going to test the get now...so we change the local record...and then we will test what the database has
                RecordToTest.Description = "New Description";

                //now go run a get or add
                var insertedRecord2 = await DP.GetOrAddAsync(RecordToTest, x => x.Id == RecordToTest.Id, false);

                Assert.AreEqual(OriginalDescriptionValue, insertedRecord2.Description);
                Assert.AreEqual(DP.EFContext.Ref_Test.Count(), 1);
            }
        }

        #endregion

        #region Bulk Insert

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void BulkInsertTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                const int howManyRows = 500;

                DP.BulkInsert("dbo", BuildRows(howManyRows), System.Data.SqlClient.SqlBulkCopyOptions.Default, 100);

                Assert.AreEqual(howManyRows, DP.Fetch<Ref_Test>(false).Count());

                var firstRow = DP.EFContext.Ref_Test.First(x => x.Id == 1);

                //id is an identity seed, so whatever we put in id will start with 1, thats why description is 1 behind the id
                Assert.AreEqual(1, firstRow.Id);
                Assert.AreEqual("0", firstRow.Description);
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task BulkInsertAsyncTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                const int howManyRows = 500;

                await DP.BulkInsertAsync("dbo", BuildRows(howManyRows), System.Data.SqlClient.SqlBulkCopyOptions.Default, 100);

                Assert.AreEqual(howManyRows, DP.Fetch<Ref_Test>(false).Count());

                var firstRow = DP.EFContext.Ref_Test.First(x => x.Id == 1);

                //id is an identity seed, so whatever we put in id will start with 1, thats why description is 1 behind the id
                Assert.AreEqual(1, firstRow.Id);
                Assert.AreEqual("0", firstRow.Description);
            }
        }

        #endregion

        #region ExecuteRawSql (With And Without Parameteres)

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExecuteRawSqlWAndWithoutParamTest1()
        {
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                //go truncate the table first (this test's the no parameter)
                DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

                DP.ExecuteRawSql(@"Insert Into Ref_Test (Description,Description2) Values ({0},{1});", TransactionalBehavior.DoNotEnsureTransaction, "Ref Test 1", "Ref Test 2");

                //make sure there is 1 record
                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task ExecuteRawSqlAsyncWAndWithoutParamTest1()
        {
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                //go truncate the table first (this test's the no parameter)
                await DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment(); Async();

                await DP.ExecuteRawSqlAsync(@"Insert Into Ref_Test (Description,Description2) Values ({0},{1});", TransactionalBehavior.DoNotEnsureTransaction, "Ref Test 1", "Ref Test 2");

                //make sure there is 1 record
                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExecuteRawSqlWithResultsNoParametersTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var result = DP.ExecuteRawSqlWithResults<Ref_Test>("select * from ref_test;");

                Assert.AreEqual(HowManyRecord, result.Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExecuteRawSqlWithResultsWithParametersTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var result = DP.ExecuteRawSqlWithResults<Ref_Test>("select * from ref_test where Id={0};", 5).Single();

                Assert.AreEqual(5, result.Id);
                Assert.AreEqual("Test5", result.Description);
                Assert.AreEqual("Test_5", result.Description2);
            }
        }

        #endregion

        #region Delete

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeleteWithSqlTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                DP.Delete<Ref_Test>(x => x.Id != HowManyRecord, true);

                var records = DP.Fetch<Ref_Test>(false);

                Assert.AreEqual(1, records.Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task DeleteWithSqlAsyncTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                await DP.DeleteAsync<Ref_Test>(x => x.Id != HowManyRecord, true);

                var records = DP.Fetch<Ref_Test>(false);

                Assert.AreEqual(1, records.Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeleteMultipleTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var records = DP.Fetch<Ref_Test>(true).Where(x => x.Id == 2 || x.Id == 3).ToArray();
                Assert.AreEqual(2, records.Count());

                DP.DeleteRange(records, true);

                Assert.AreEqual(8, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task DeleteMultipleAsyncTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var records = DP.Fetch<Ref_Test>(true).Where(x => x.Id == 2 || x.Id == 3).ToArray();
                Assert.AreEqual(2, records.Count());

                await DP.DeleteRangeAsync(records, true);

                Assert.AreEqual(8, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeleteByEntityTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var record = DP.Find<Ref_Test>(x => x.Id == 5, true).Single();

                DP.Delete(record, true);

                Assert.AreEqual(HowManyRecord - 1, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task DeleteByEntityAsyncTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var record = DP.Find<Ref_Test>(x => x.Id == 5, true).Single();

                await DP.DeleteAsync(record, true);

                Assert.AreEqual(HowManyRecord - 1, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeleteByEntityTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var recordToDelete = DP.Find<Ref_Test>(x => true, true).ToList();

                recordToDelete.ForEach(x => DP.Delete(x, false));

                Assert.AreEqual(HowManyRecord, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task DeleteByEntityAsyncTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var recordToDelete = DP.Find<Ref_Test>(x => true, true).ToList();

                foreach (var thisRecord in recordToDelete)
                {
                    await DP.DeleteAsync(thisRecord, false);
                }

                Assert.AreEqual(HowManyRecord, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        #endregion

        #region Add

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Id = 1, Description = "1" };
                var newRecord2 = new Ref_Test() { Id = 2, Description = "2" };
                var newRecord3 = new Ref_Test() { Id = 3, Description = "3", Description2 = "3" };

                DP.Add(newRecord1, false);

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.Add(newRecord2, false);
                DP.Add(newRecord3, false);

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(3, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Id = 1, Description = "1" };
                var newRecord2 = new Ref_Test() { Id = 2, Description = "2" };
                var newRecord3 = new Ref_Test() { Id = 3, Description = "3", Description2 = "3" };

                DP.Add(newRecord1, true);
                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.Add(newRecord2, true);
                Assert.AreEqual(2, DP.Fetch<Ref_Test>(false).Count());

                DP.Add(newRecord3, true);
                Assert.AreEqual(3, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task AddAsyncTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Id = 1, Description = "1" };
                var newRecord2 = new Ref_Test() { Id = 2, Description = "2" };
                var newRecord3 = new Ref_Test() { Id = 3, Description = "3", Description2 = "3" };

                DP.Add(newRecord1, false);

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());

                await DP.SaveChangesAsync();

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.Add(newRecord2, false);
                DP.Add(newRecord3, false);

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                await DP.SaveChangesAsync();

                Assert.AreEqual(3, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddRangeTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Description = "1" };
                var newRecord2 = new Ref_Test() { Description = "2" };
                var newRecord3 = new Ref_Test() { Description = "3", Description2 = "3" };

                List<Ref_Test> lst = new List<Ref_Test>() { newRecord1, newRecord2, newRecord3 };

                DP.AddRange(lst, false);

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(lst.Count, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddRangeTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Description = "1" };
                var newRecord2 = new Ref_Test() { Description = "2" };
                var newRecord3 = new Ref_Test() { Description = "3", Description2 = "3" };

                List<Ref_Test> lst = new List<Ref_Test>() { newRecord1, newRecord2, newRecord3 };

                DP.AddRange(lst, true);

                Assert.AreEqual(lst.Count, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        #endregion

        #region Add Or Update

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddOrUpdateTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Id = -1, Description = "1" };
                var newRecord2 = new Ref_Test() { Id = -1, Description = "2" };

                DP.Upsert(newRecord1, false);
                Assert.AreEqual(HowManyRecord, DP.Fetch<Ref_Test>(false).Count());

                DP.Upsert(newRecord2, true);

                Assert.AreEqual(HowManyRecord + 2, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddOrUpdateTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = DP.EFContext.Ref_Test.Single(x => x.Id == 5);
                var newRecord2 = new Ref_Test() { Id = -1, Description = "2" };

                const string changestring = "jason";

                newRecord1.Description2 = changestring;

                DP.Upsert(newRecord2, true);

                Assert.AreEqual(HowManyRecord + 1, DP.Fetch<Ref_Test>(false).Count());

                Assert.AreEqual(changestring, DP.EFContext.Ref_Test.Single(x => x.Id == 5).Description2);
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddOrUpdateTest3()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var updateRecord1 = DP.EFContext.Ref_Test.Single(x => x.Id == 5);
                var updateRecord2 = DP.EFContext.Ref_Test.Single(x => x.Id == 6);

                const string changestring = "jason";

                updateRecord1.Description2 = changestring;
                updateRecord2.Description2 = changestring;

                DP.Upsert(updateRecord1, false);
                DP.Upsert(updateRecord2, false);

                DP.SaveChanges();

                Assert.AreEqual(changestring, DP.EFContext.Ref_Test.Single(x => x.Id == 5).Description2);
                Assert.AreEqual(changestring, DP.EFContext.Ref_Test.Single(x => x.Id == 6).Description2);
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddOrUpdateRangeTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var newRecord1 = new Ref_Test() { Id = -1, Description = "1" };
                var newRecord2 = new Ref_Test() { Id = -1, Description = "2" };

                var lst = new Ref_Test[] { newRecord1, newRecord2 };

                DP.UpsertRange(lst, true);

                Assert.AreEqual(HowManyRecord + lst.Count(), DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void AddOrUpdateRangeTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                const string changestring = "jason";

                var newRecord1 = DP.EFContext.Ref_Test.Single(x => x.Id == 1);
                var newRecord2 = DP.EFContext.Ref_Test.Single(x => x.Id == 2);

                newRecord1.Description = changestring;
                newRecord2.Description2 = changestring;

                var lst = new Ref_Test[] { newRecord1, newRecord2 };

                DP.UpsertRange(lst, true);

                Assert.AreEqual(changestring, DP.EFContext.Ref_Test.Single(x => x.Id == 1).Description);
                Assert.AreEqual(changestring, DP.EFContext.Ref_Test.Single(x => x.Id == 2).Description2);
            }
        }

        #endregion

        #region Find

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task FindTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName))
            {
                var records = await DP.Find<Ref_Test>(x => x.Id > 9, false).ToArrayAsync();

                Assert.AreEqual(1, records.Count());
                Assert.AreEqual(10, records.ElementAt(0).Id);
                Assert.AreEqual("Test10", records.ElementAt(0).Description);
                Assert.AreEqual("Test_10", records.ElementAt(0).Description2);
            }
        }

        #endregion

        #region Fetch

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public async Task FetchTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            Add10Rows(false, false);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(ReadonlyDataProviderName))
            {
                var query = DP.Fetch<Ref_Test>(true).Where(x => x.Id > 9);

                var records = await query.ToArrayAsync();

                Assert.AreEqual(1, records.Count());
                Assert.AreEqual(10, records.ElementAt(0).Id);
                Assert.AreEqual("Test10", records.ElementAt(0).Description);
                Assert.AreEqual("Test_10", records.ElementAt(0).Description2);
            }
        }

        #endregion

        #region Transactions

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void TransactionTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var recordToAdd = new Ref_Test() { Description = "New Record" };

                DP.StartTransaction();

                DP.Add(recordToAdd, false);

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.RollBackTransaction();

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void TransactionTest2()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(WritableDataProviderName))
            {
                var recordToAdd = new Ref_Test() { Description = "New Record" };

                DP.StartTransaction();

                DP.Add(recordToAdd, false);

                Assert.AreEqual(0, DP.Fetch<Ref_Test>(false).Count());

                DP.SaveChanges();

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());

                DP.CommitTransaction();

                Assert.AreEqual(1, DP.Fetch<Ref_Test>(false).Count());
            }
        }

        #endregion

        #endregion

        #endregion

    }

}
