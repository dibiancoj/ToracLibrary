using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibrary.Core.DataProviders.SqlBuilder;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Generic Data Provider For Entity Framework
    /// </summary>
    /// <remarks>Works with Entity Framework 6.0 DbContext. Properties are immutable</remarks>
    /// <typeparam name="TEfContextType">Is the specific context type. Mainly used so we can type the specific entity model if you want to call stored procedures</typeparam>
    public class EntityFrameworkDP<TEfContextType> : IDisposable, IEntityFrameworkDataRepository
        where TEfContextType : DbContext, new()
    {

        #region Example on how to create the Data Provider

        //using (var DP = new EntityFrameworkDP<SandboxEntities5>(new SandboxEntities5()))

        //**Notes About Configuration**
        //thisContext.Configuration
        //AutoDetectChangesEnabled (on my default) used if you know you want to track changes for a saving. turn off for faster calls doenst need to track the changes of everything received
        //ProxyCreationEnabled (off by default) is used if you want to use the model after the context has been disposed
        //LazyLoadingEnabled (on by default) is used if you want to enable it. you can turn it off on the context map instead of through code

        //difference between "AsNoTracking" And AutoDetectChanges
        //Setting AutoDetectChangesEnabled to false stops automatic detection of changes for objects that are being tracked. 
        //so it will tell if you changed the object. AsNoTracking Doesnt track the item at all

        //to call stored procedures
        //DP.thisContext.UserLogInTry(parameters of this stored procedure)

        //*** Why we are using Expression<Func In Find, Fetch
        //The overload in the where clause for a func is returning IEnumerable...An expression in iqueryable.
        //so if you did a func, it would go get every record in the db...then filter it in memory...if you do expression then it transform the query
        //You can declare the expression like this, or just pass it into the method because it excepts an expression 
        //System.Linq.Expressions.Expression<Func<Ref_ClaimTree, bool>> f = (x => x.TreeId == 5);

        #endregion

        #region Example Of Projection With Map To Ref Table without includes

        //you will need to put the Context into a variable, won't work if you do DP.thisContext

        //var context = DP.thisContext

        //var sql = (from myData in RefThisTable
        //select new Class(){
        //id = myData.Id,
        //txt = context.RefLookup.where(x => x.id == myData.Id)
        //}).AsQueryable()

        //this will work and when you do reflection with txt..it will map back to ref_lookup and everything will work

        #endregion

        #region Constructor

        #region Pass In Context Constructors

        /// <summary>
        /// Constructor. Used when you currently have a context that you want to share. Or when you want to create the context and then pass it in.
        /// </summary>
        /// <param name="EntityFrameworkContext">Object Context Of Entity Framework</param>
        public EntityFrameworkDP(TEfContextType EntityFrameworkContext)
            : this(EntityFrameworkContext, null, null, null)
        {
        }

        /// <summary>
        /// Constructor. Used when you currently have a context that you want to share. Or when you want to create the context and then pass it in.
        /// </summary>
        /// <param name="EntityFrameworkContext">Object Context Of Entity Framework</param>
        /// <param name="AutoDetectChangesEnabled">AutoDetectChangesEnabled is on by default. It's used if you don't need to track changes. Or if you are going to save a large amount of records. It removes the overhead of keeping track one record at a time. Save changes will automatically call detect changes if this is false so your records will be saved. This is different then as no tracking. As no tracking will not let you track at all. This by calling Context.DetectChanges() you can recover the changes.</param>
        /// <param name="ProxyCreationEnabled">ProxyCreationEnabled is used if you want to use the model after the context has been disposed. Off By Default</param>
        /// <param name="EnableLazyLoading">LazyLoadingEnabled is used if you want to enable it. you can turn it off on the context map instead of through code. On By Default</param>
        /// <remarks>Don't want user changing the properties after queries have been launched (if you call multiple queries with the same DP) so just adding it in constructor and making the context property private</remarks>
        public EntityFrameworkDP(TEfContextType EntityFrameworkContext,
                                 bool? AutoDetectChangesEnabled,
                                 bool? ProxyCreationEnabled,
                                 bool? EnableLazyLoading)
        {
            //set the property of the entity framework
            EFContext = EntityFrameworkContext;

            //let's add all the configuration options now

            //if they want to set auto detech changes then do it now
            if (AutoDetectChangesEnabled.HasValue)
            {
                EFContext.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled.Value;
            }

            //if they want to set proxy creation then set it now
            if (ProxyCreationEnabled.HasValue)
            {
                EFContext.Configuration.ProxyCreationEnabled = ProxyCreationEnabled.Value;
            }

            //if they want to enable lazy loading then set it now
            if (EnableLazyLoading.HasValue)
            {
                EFContext.Configuration.LazyLoadingEnabled = EnableLazyLoading.Value;
            }
        }

        #endregion

        #region Data Provider Automatically Creates A Context Constructors

        /// <summary>
        /// Constructor. Used when you currently have a context that you want to share. Or when you want to create the context and then pass it in.
        /// </summary>
        public EntityFrameworkDP()
            : this(new TEfContextType(), null, null, null)
        {
        }

        /// <summary>
        /// Constructor. Used when you currently have a context that you want to share. Or when you want to create the context and then pass it in.
        /// </summary>
        /// <param name="AutoDetectChangesEnabled">AutoDetectChangesEnabled is on by default. It's used if you don't need to track changes. Or if you are going to save a large amount of records. It removes the overhead of keeping track one record at a time. Save changes will automatically call detect changes if this is false so your records will be saved. This is different then as no tracking. As no tracking will not let you track at all. This by calling Context.DetectChanges() you can recover the changes.</param>
        /// <param name="ProxyCreationEnabled">ProxyCreationEnabled is used if you want to use the model after the context has been disposed. Off By Default</param>
        /// <param name="EnableLazyLoading">LazyLoadingEnabled is used if you want to enable it. you can turn it off on the context map instead of through code. On By Default</param>
        /// <remarks>Don't want user changing the properties after queries have been launched (if you call multiple queries with the same DP) so just adding it in constructor and making the context property private</remarks>
        public EntityFrameworkDP(bool? AutoDetectChangesEnabled,
                                 bool? ProxyCreationEnabled,
                                 bool? EnableLazyLoading)
            : this(new TEfContextType(), AutoDetectChangesEnabled, ProxyCreationEnabled, EnableLazyLoading)
        {
        }

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// The Entity Framework context object for the database
        /// </summary>
        public TEfContextType EFContext { get; }

        /// <summary>
        /// Command Timeout In Seconds. A Null Value Indicates That The Default Value Of The Underlying Provider Will Be Used.
        /// </summary>
        public int? Timeout
        {
            get { return EFContext.Database.CommandTimeout; }
            set { EFContext.Database.CommandTimeout = value; }
        }

        /// <summary>
        /// Sets the method to write the sql and sql actions. Sets the Context.Log automatically when setting this property
        /// </summary>
        public Action<string> SqlLogWriter
        {
            get { return EFContext.Database.Log; }
            set { EFContext.Database.Log = value; }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Holds the transaction if the user calls Start Transaction
        /// </summary>
        private TransactionScope TransactionToRun { get; set; }

        #endregion

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #endregion

        #region Transaction

        #region Public Methods

        #region Start Transaction Overload

        /// <summary>
        /// Starts A Transaction. Overload where you can pass in options
        /// </summary>
        /// <remarks>If you want to read the uncomitted data (data added or edited before scope transaction then use ReadUncommitted)</remarks>
        public void StartTransaction(TransactionOptions Options)
        {
            //create the new transaction code
            TransactionToRun = new TransactionScope(TransactionScopeOption.RequiresNew, Options);
        }

        /// <summary>
        /// Starts A Transaction With Default Options
        /// </summary>
        /// <remarks>If you want to read the uncomitted data (data added or edited before scope transaction then use ReadUncommitted)</remarks>
        public void StartTransaction()
        {
            //Default Options
            TransactionOptions DefaultOption = new TransactionOptions()
            {
                //read uncommited means anything that was inserted during the transaction will be run if you run a query before committing this transaction.
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            //use the overload now
            StartTransaction(DefaultOption);
        }

        #endregion

        /// <summary>
        /// Commits An Active Transaction (Start Transaction Must Be Called Before Calling This Method [Error Will Be Raised Otherwise])
        /// </summary>
        public void CommitTransaction()
        {
            //if we haven't hit Start Transaction then throw an error
            if (TransactionToRun == null)
            {
                throw new NullReferenceException("Please Call Start Transaction Before Calling Commit, Because There Is No Active Transaction To Commit");
            }

            //we have a transaction...let's go commit it
            TransactionToRun.Complete();

            //go cleanup the transaction
            CleanupTransaction();
        }

        /// <summary>
        /// Rollback An Active Transaction (Start Transaction Must Be Called Before Calling This Method [Error Will Be Raised Otherwise])
        /// </summary>
        public void RollBackTransaction()
        {
            //there really is no rollback..if dispose is called before txScope.Complete() the TransactionScope will tell the connections to rollback their transactions
            //the cleanup transaction will also check for a null transaction...so we don't need to that here
            CleanupTransaction();
        }

        #endregion

        #region Internal Helpers Methods

        /// <summary>
        /// Internal Method To Clean Up The Transaction After It Has Been Committed Or Rollbacked. This Way We Can Get It Setup If You Want To Run Another Transaction After The Last One You Just Committed Or Rolled Back
        /// </summary>
        private void CleanupTransaction()
        {
            //** Note - there really is no rollback..if dispose is called before txScope.Complete() the TransactionScope will tell the connections to rollback their transactions
            if (TransactionToRun == null)
            {
                throw new NullReferenceException("Please Call Start Transaction Before Calling Internal Private Method CleanupTransaction, Because There Is No Active Transaction To Rollback");
            }

            //let's dispose of this transaction now
            TransactionToRun.Dispose();

            //clear it out too
            TransactionToRun = null;
        }

        #endregion

        #endregion

        #region Main Helper Methods

        /// <summary>
        /// Builds the object you want to query. Basis of all the calls. At the method level so we don't have to declare a new data provider for each different type of data we want to query (if we want to make 3 queries while only creating one data provider)
        /// </summary>
        /// <typeparam name="T">Type Of T You Want To Query</typeparam>
        /// <returns>Db Set Of T That You Can Query</returns>
        private DbSet<T> BuildObjectSet<T>() where T : class
        {
            //returning dbset instead of IDbSet because add range and remove range arent off of the interface. Since we know what T is
            //we can just return DbSet<T>

            //create the object set and return it
            return EFContext.Set<T>();
        }

        /// <summary>
        /// Sets the properties needed in a common method to implement tracking on DbSet object. Ie. AsNoTracking
        /// </summary>
        /// <typeparam name="T">Type of record in the dbset</typeparam>
        /// <param name="DbSetToSetTracking">DbSet to set the tracking on</param>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>IQueryable Of T.</returns>
        private static IQueryable<T> SetTrackRecords<T>(IDbSet<T> DbSetToSetTracking, bool TrackRecords) where T : class
        {
            //do we want to track the records?
            if (TrackRecords)
            {
                //we want to track the records so just return IQueryable<T>
                return DbSetToSetTracking.AsQueryable();
            }

            //we don't want to track the records, so set the AsNoTrackingFlag and return the IQueryable
            return DbSetToSetTracking.AsNoTracking();
        }

        #endregion

        #region Querying Methods

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<T> Fetch<T>(bool TrackRecords) where T : class
        {
            //Lookup is my database model
            //var data = (from mydata in DP.Fetch<Lookup>()
            //            where mydata.Id == 2
            //            select mydata).ToArray();

            //complex example with an include and projection
            //return (from myData in DP.Fetch<Ref_Courses>(false).Include(x => x.Ref_CourseStats)
            //        where myData.StateId == StateId && myData.IsActive
            //        select new CourseRoundInfo()
            //        {
            //            CourseId = myData.CourseId,
            //            Name = myData.Name,
            //            Location = myData.Location,
            //            TeeBoxData = from myTeeBoxData in myData.Ref_CourseStats
            //                         select new CourseBaseInfo.TeeBoxBaseInfo()
            //                         {
            //                             TeeLevelId = myTeeBoxData.TeeId,
            //                             Description = myTeeBoxData.Description
            //                         }
            //        }).ToArray();

            //let's go set the dbSet, set tracking, and return IQueryable
            return SetTrackRecords(BuildObjectSet<T>(), TrackRecords);
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="WherePredicate">Criteria to match on</param>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<T> Find<T>(Expression<Func<T, bool>> WherePredicate, bool TrackRecords) where T : class
        {
            //var data = DP.Find<Lookup>(x => x.Id == 2,true).ToArray();
            //or 
            //var data = DP.Find((Lookup x) => x.Id == 2,true).ToArray();

            //let's go set the dbSet, set tracking, set the where and return IQueryable
            return SetTrackRecords(BuildObjectSet<T>(), TrackRecords).Where(WherePredicate);
        }

        #endregion

        #region Get or Add

        /// <summary>
        /// Tries to get a record where we have a match on record selector. If it can't find it, it will add it. Method looks for the first record that matches
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="RecordToInsertIfNotFound">Record to insert if we can't find it</param>
        /// <param name="RecordSelector">Query to find the record that was passed in. Looks for the first record</param>
        /// <param name="TrackRecords">Track records? Do you plan on save this record later on?</param>
        /// <returns>Record to retrieve or the inserted record</returns>
        public T GetOrAdd<T>(T RecordToInsertIfNotFound, Expression<Func<T, bool>> RecordSelector, bool TrackRecords) where T : class
        {
            //cache th object set
            IDbSet<T> ObjectSet = BuildObjectSet<T>();

            //let's go to try to find a record in the db
            T RecordToFindItDbAttempt = SetTrackRecords(ObjectSet, TrackRecords).FirstOrDefault(RecordSelector);

            //if we didn't find a record, then add it
            if (RecordToFindItDbAttempt == null)
            {
                //we didn't find the record, so insert it
                RecordToFindItDbAttempt = ObjectSet.Add(RecordToInsertIfNotFound);

                //save the record
                SaveChanges();
            }

            //now return the record
            return RecordToFindItDbAttempt;
        }

        /// <summary>
        /// Tries to get a record where we have a match on record selector. If it can't find it, it will add it. Method looks for the first record that matches
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="RecordToInsertIfNotFound">Record to insert if we can't find it</param>
        /// <param name="RecordSelector">Query to find the record that was passed in. Looks for the first record</param>
        /// <param name="TrackRecords">Track records? Do you plan on save this record later on?</param>
        /// <returns>Task of record to retrieve or the inserted record</returns>
        public async Task<T> GetOrAddAsync<T>(T RecordToInsertIfNotFound, Expression<Func<T, bool>> RecordSelector, bool TrackRecords) where T : class
        {
            //cache th object set
            IDbSet<T> ObjectSet = BuildObjectSet<T>();

            //let's go to try to find a record in the db
            T RecordToFindItDbAttempt = await SetTrackRecords(ObjectSet, TrackRecords).FirstOrDefaultAsync(RecordSelector).ConfigureAwait(false);

            //if we didn't find a record, then add it
            if (RecordToFindItDbAttempt == null)
            {
                //we didn't find the record, so insert it
                RecordToFindItDbAttempt = ObjectSet.Add(RecordToInsertIfNotFound);

                //save the record
                await SaveChangesAsync().ConfigureAwait(false);
            }

            //now return the record
            return RecordToFindItDbAttempt;
        }

        #endregion

        #region Delete Methods

        #region Delete When You Have The Entity

        #region Regular

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void Delete<T>(T EntityToDelete, bool CommitChanges) where T : class
        {
            //remove the item
            BuildObjectSet<T>().Remove(EntityToDelete);

            //if they want to save the changes then save them
            if (CommitChanges)
            {
                //go save the changes now
                SaveChanges();
            }
        }

        #endregion

        #region Async

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public async Task DeleteAsync<T>(T EntityToDelete, bool CommitChanges) where T : class
        {
            //remove the item
            BuildObjectSet<T>().Remove(EntityToDelete);

            //if they want to save the changes then save them
            if (CommitChanges)
            {
                //go save the changes now
                await SaveChangesAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #endregion

        #region Delete From An Expression. Will Select Elements Then Delete Them.

        #region Regular

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        public void Delete<T>(Expression<Func<T, bool>> Predicate, bool CommitChanges) where T : class
        {
            //grab the records to delete, then pass it into the helper method
            DeleteRange(BuildObjectSet<T>().Where(Predicate).ToArray(), CommitChanges);
        }

        #endregion

        #region Async

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        public async Task DeleteAsync<T>(Expression<Func<T, bool>> Predicate, bool CommitChanges) where T : class
        {
            //grab the records to delete, then pass it into the helper method
            DeleteRange(await BuildObjectSet<T>().Where(Predicate).ToArrayAsync().ConfigureAwait(false), false);

            //if we want to commit the changes then go save it now
            if (CommitChanges)
            {
                //go save the changes
                await SaveChangesAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #endregion

        #region Delete Range

        #region Regular

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void DeleteRange<T>(IEnumerable<T> EntitiesToDelete, bool CommitChanges) where T : class
        {
            //remove the item
            BuildObjectSet<T>().RemoveRange(EntitiesToDelete);

            //if they want to save the changes then save them
            if (CommitChanges)
            {
                //go save the changes now
                SaveChanges();
            }
        }

        #endregion

        #region Async

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public async Task DeleteRangeAsync<T>(IEnumerable<T> EntitiesToDelete, bool CommitChanges) where T : class
        {
            //remove the item
            BuildObjectSet<T>().RemoveRange(EntitiesToDelete);

            //if they want to save the changes then save them
            if (CommitChanges)
            {
                //go save the changes now
                await SaveChangesAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Add - Add Or Update Methods

        #region Straight Add

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAdd">Entity To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void Add<T>(T EntityToAdd, bool CommitChanges) where T : class
        {
            //add the entity now
            BuildObjectSet<T>().Add(EntityToAdd);

            //if they want to save the changes then save them
            if (CommitChanges)
            {
                //go commit the changes
                SaveChanges();
            }
        }

        /// <summary>
        /// Adds the list of records passed in to the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToAdd">List Of Entities To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void AddRange<T>(IEnumerable<T> EntitiesToAdd, bool CommitChanges) where T : class
        {
            //make sure we have entities to add first
            if (EntitiesToAdd.AnyWithNullCheck())
            {
                //add the range of items
                BuildObjectSet<T>().AddRange(EntitiesToAdd);

                //if they wanted to commit the changes, then do it now
                if (CommitChanges)
                {
                    //all done, go save the database now
                    SaveChanges();
                }
            }
        }

        #endregion

        #region Add Or Update - Upsert

        /// <summary>
        /// Either Add Or Update An Entity Based On If The Record Exists In The Db Already
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAddOrUpdate">Entity To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void Upsert<T>(T EntityToAddOrUpdate, bool CommitChanges) where T : class
        {
            //*** Note On Recursive Table structure: if you have a recursive table structure the add method will blow up on save. You will get the following error
            //"The changes to the database were committed successfully, but an error occurred while updating the object context. The ObjectContext might be in an inconsistent state. Inner exception message: A circular relationship path has been detected while enforcing a referential integrity constraints. Referential integrity cannot be enforced on circular relationships."
            //for the add just run an Add method and that will save. The update will work in this method

            //going to use the UpsertRange helper method. creating an array is a little overhead more then we need, but it prevents duplicate code.
            UpsertRange(new T[] { EntityToAddOrUpdate }, CommitChanges);
        }

        /// <summary>
        /// Either Add Or Update An IEnumerable Of Entity Based On The Func You Pass in
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToAddOrUpdate">Entities To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void UpsertRange<T>(IEnumerable<T> EntitiesToAddOrUpdate, bool CommitChanges) where T : class
        {
            //go add the entities
            BuildObjectSet<T>().AddOrUpdate(EntitiesToAddOrUpdate.ToArray());

            //if they wanted to commit the changes, then do it now
            if (CommitChanges)
            {
                //all done, go save the database now
                SaveChanges();
            }
        }

        #endregion

        #endregion

        #region Execute Raw Sql (Non Results Back & Results Back)

        #region Non Results Back

        #region Regular

        /// <summary>
        /// Go Execute Raw Sql With No Results Coming Back And No Parameters. Example Would Be Update Or Insert.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <param name="TransactionType">Transaction Type To Use. Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present. So do you want to run this in a transaction or not</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteRawSql(string RawSqlToExecute, TransactionalBehavior TransactionType)
        {
            //go run the sql with no parameters
            return ExecuteRawSql(RawSqlToExecute, TransactionType, null);
        }

        /// <summary>
        /// Go Execute Raw Sql With No Results Coming Back And With Parameters. Example Would Be Update Or Insert.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <param name="Parameters">Params Of Parameters To Pass In</param>
        /// <param name="TransactionType">Transaction Type To Use. Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present. So do you want to run this in a transaction or not</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteRawSql(string RawSqlToExecute, TransactionalBehavior TransactionType, params object[] Parameters)
        {
            //example on how to call this
            //DP.ExecuteRawSql(@"Insert Into Ref_Test (Description,Description2) Values ({0},{1});", "Ref Test 1", "Ref Test 2");

            //Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present.
            //so if you are running a backup db command then it will fail because you can't wrap that in a transaction (sql server constraint)
            //so allow the developer to pass in the transaction mode


            //go run the query - if there are no parameter then create a blank array so it doesnt bomb out, if you pass in null it would error out
            return EFContext.Database.ExecuteSqlCommand(TransactionType, RawSqlToExecute, Parameters ?? Array.Empty<object>());
        }

        #endregion

        #region Async

        /// <summary>
        /// Go Execute Raw Sql With No Results Coming Back And No Parameters. Example Would Be Update Or Insert.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <param name="TransactionType">Transaction Type To Use. Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present. So do you want to run this in a transaction or not</param>
        /// <returns>Number of rows affected</returns>
        public Task<int> ExecuteRawSqlAsync(string RawSqlToExecute, TransactionalBehavior TransactionType)
        {
            //go run the sql with no parameters
            return ExecuteRawSqlAsync(RawSqlToExecute, TransactionType, null);
        }

        /// <summary>
        /// Go Execute Raw Sql With No Results Coming Back And With Parameters. Example Would Be Update Or Insert.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <param name="TransactionType">Transaction Type To Use. Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present. So do you want to run this in a transaction or not</param>
        /// <param name="Parameters">Params Of Parameters To Pass In</param>
        /// <returns>Number of rows affected</returns>
        public Task<int> ExecuteRawSqlAsync(string RawSqlToExecute, TransactionalBehavior TransactionType, params object[] Parameters)
        {
            //example on how to call this
            //DP.ExecuteRawSql(@"Insert Into Ref_Test (Description,Description2) Values ({0},{1});", "Ref Test 1", "Ref Test 2");

            //Starting with EF6 Database.ExecuteSqlCommand() by default will wrap the command in a transaction if one was not already present.
            //so if you are running a backup db command then it will fail because you can't wrap that in a transaction (sql server constraint)
            //so allow the developer to pass in the transaction mode

            //go run the query - if there are no parameter then create a blank array so it doesnt bomb out, if you pass in null it would error out
            return EFContext.Database.ExecuteSqlCommandAsync(TransactionType, RawSqlToExecute, Parameters ?? Array.Empty<object>());
        }

        #endregion

        #endregion

        #region With Results Back

        #region Regular

        /// <summary>
        /// Go Execute Raw Sql With Results Coming Back With No Parameters.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <returns>IEumerable Of T</returns>
        public DbRawSqlQuery<T> ExecuteRawSqlWithResults<T>(string RawSqlToExecute)
        {
            //use the overload and go execute and return the values using no parameters
            return ExecuteRawSqlWithResults<T>(RawSqlToExecute, null);
        }

        /// <summary>
        /// Go Execute Raw Sql With Results Coming Back And With Parameters.
        /// </summary>
        /// <param name="RawSqlToExecute">Raw Sql To Execute</param>
        /// <param name="Parameters">Params Of Parameters To Pass In</param>
        /// <returns>IEumerable Of T</returns>
        public DbRawSqlQuery<T> ExecuteRawSqlWithResults<T>(string RawSqlToExecute, params object[] Parameters)
        {
            //example on how to call this
            //var result = DP.ExecuteRawSqlWithResults<Ref_Test>("select * from ref_test where Id={0};", 5).Single();

            //go grab the data now - if there are no parameter then create a blank array so it doesnt bomb out, if you pass in null it would error out
            return EFContext.Database.SqlQuery<T>(RawSqlToExecute, Parameters ?? Array.Empty<object>());
        }

        #endregion

        #endregion

        #endregion

        #region Bulk Insert - Uses Ado.Net Bulk Insert

        #region Regular Methods

        /// <summary>
        /// Inserts a list of items using ado.net bulk insert. The method uses reflection to build up the data table. If the reflection is too slow, then use regular ado.net bulk insert method instead of this generic method
        /// </summary>
        /// <typeparam name="T">Type Of T</typeparam>
        /// <param name="TableSchemaOfT">Schema of the table we are writing too. The table should be typeof(T)</param>
        /// <param name="ItemsToInsert">Items To Insert</param>
        /// <param name="BulkCopyOptions">Bulk Copy Options To Use</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <returns>result of save</returns>
        public bool BulkInsert<T>(string TableSchemaOfT, IEnumerable<T> ItemsToInsert, SqlBulkCopyOptions BulkCopyOptions, int BatchSize) where T : class
        {
            //let's go create the sql data provider
            using (var DP = new SQLDataProvider(EFContext.Database.Connection.ConnectionString))
            {
                //go run the sql data provider bulk insert
                return DP.BulkInsert(TableSchemaOfT, DataTableHelpers.ToDataTable.BuildDataTableFromListOfObjects(ItemsToInsert, typeof(T).Name), BulkCopyOptions, BatchSize);
            }
        }

        /// <summary>
        /// Inserts a list of items using ado.net bulk insert. The method uses reflection to build up the data table. If the reflection is too slow, then use regular ado.net bulk insert method instead of this generic method
        /// </summary>
        /// <typeparam name="T">Type Of T</typeparam>
        /// <param name="TableSchemaOfT">Schema of the table we are writing too. The table should be typeof(T)</param>
        /// <param name="ItemsToInsert">Items To Insert</param>
        /// <param name="BulkCopyOptions">Bulk Copy Options To Use</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <param name="CommandTimeOutInSeconds">Command Time Out In Seconds</param>
        /// <returns>result of save</returns>
        public bool BulkInsert<T>(string TableSchemaOfT, IEnumerable<T> ItemsToInsert, SqlBulkCopyOptions BulkCopyOptions, int BatchSize, int CommandTimeOutInSeconds) where T : class
        {
            //let's go create the sql data provider
            using (var DP = new SQLDataProvider(EFContext.Database.Connection.ConnectionString))
            {
                //go run the sql data provider bulk insert
                return DP.BulkInsert(TableSchemaOfT, DataTableHelpers.ToDataTable.BuildDataTableFromObject(ItemsToInsert, typeof(T).Name), BulkCopyOptions, BatchSize, CommandTimeOutInSeconds);
            }
        }

        #endregion

        #region Task Based Methods

        /// <summary>
        /// Inserts a list of items using ado.net bulk insert. The method uses reflection to build up the data table. If the reflection is too slow, then use regular ado.net bulk insert method instead of this generic method
        /// </summary>
        /// <typeparam name="T">Type Of T</typeparam>
        /// <param name="TableSchemaOfT">Schema of the table we are writing too. The table should be typeof(T)</param>
        /// <param name="ItemsToInsert">Items To Insert</param>
        /// <param name="BulkCopyOptions">Bulk Copy Options To Use</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <returns>Task Result Of Save</returns>
        public Task<bool> BulkInsertAsync<T>(string TableSchemaOfT, IEnumerable<T> ItemsToInsert, SqlBulkCopyOptions BulkCopyOptions, int BatchSize) where T : class
        {
            //go start the task and return it
            return Task<bool>.Factory.StartNew(() =>
            {
                //let's go create the sql data provider
                using (var DP = new SQLDataProvider(EFContext.Database.Connection.ConnectionString))
                {
                    //go run the sql data provider bulk insert
                    return DP.BulkInsert(TableSchemaOfT, DataTableHelpers.ToDataTable.BuildDataTableFromListOfObjects(ItemsToInsert, typeof(T).Name), BulkCopyOptions, BatchSize);
                }
            });
        }

        /// <summary>
        /// Inserts a list of items using ado.net bulk insert. The method uses reflection to build up the data table. If the reflection is too slow, then use regular ado.net bulk insert method instead of this generic method
        /// </summary>
        /// <typeparam name="T">Type Of T</typeparam>
        /// <param name="TableSchemaOfT">Schema of the table we are writing too. The table should be typeof(T)</param>
        /// <param name="ItemsToInsert">Items To Insert</param>
        /// <param name="BulkCopyOptions">Bulk Copy Options To Use</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <param name="CommandTimeOutInSeconds">Command Time Out In Seconds</param>
        /// <returns>Task Result Of Save</returns>
        public Task<bool> BulkInsertAsync<T>(string TableSchemaOfT, IEnumerable<T> ItemsToInsert, SqlBulkCopyOptions BulkCopyOptions, int BatchSize, int CommandTimeOutInSeconds) where T : class
        {
            //go start the task and return it
            return Task<bool>.Factory.StartNew(() =>
            {
                //let's go create the sql data provider
                using (var DP = new SQLDataProvider(EFContext.Database.Connection.ConnectionString))
                {
                    //go run the sql data provider bulk insert
                    return DP.BulkInsert(TableSchemaOfT, DataTableHelpers.ToDataTable.BuildDataTableFromObject(ItemsToInsert, typeof(T).Name), BulkCopyOptions, BatchSize, CommandTimeOutInSeconds);
                }
            });
        }

        #endregion

        #endregion

        #region Attach

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAttach">Entity To Attach</param>
        public void Attach<T>(T EntityToAttach) where T : class
        {
            //build the object set and attach the entity
            BuildObjectSet<T>().Attach(EntityToAttach);
        }

        #endregion

        #region Save

        #region Save Helper Methods

        /// <summary>
        /// If you have AutoDetectChangesEnabled = false and you try to update a record, it won't save correctly unless you call DetectChanges. This method will check the context AutoDetectChangesEnabled setting and will either call the auto detect or just return
        /// </summary>
        private void PreSaveAutoDetectCheck()
        {
            //if the check is true we are coming from the public method and we need to check the flag.
            if (!EFContext.Configuration.AutoDetectChangesEnabled)
            {
                //if we aren't deteching changes then we need to check changes
                EFContext.ChangeTracker.DetectChanges();
            }
        }

        #endregion

        /// <summary>
        /// Saves all context changes
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public int SaveChanges()
        {
            //If you have AutoDetectChangesEnabled = false and you try to update a record, it won't save correctly unless you call DetectChanges. This method will check the context AutoDetectChangesEnabled setting and will either call the auto detect or just return
            PreSaveAutoDetectCheck();

            //we are ready to save now. Go ahead and have the context save the changes
            return EFContext.SaveChanges();
        }

        /// <summary>
        /// Saves all context changes async
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <remarks>You can call await SaveChangesAsync in the calling method</remarks>
        public Task<int> SaveChangesAsync()
        {
            //If you have AutoDetectChangesEnabled = false and you try to update a record, it won't save correctly unless you call DetectChanges. This method will check the context AutoDetectChangesEnabled setting and will either call the auto detect or just return
            PreSaveAutoDetectCheck();

            //we are ready to save now. Go ahead and have the context save the changes and return the task
            return EFContext.SaveChangesAsync();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Tries to connect to the database. Used for troubleshooting connection issues
        /// </summary>
        /// <param name="ThrowException">If you want to throw the exception it won't return with the tuple. It will just raise the exception</param>
        /// <returns>Tuple. Item is is the result yes or no you can connect. Item 2 is the exception that was raised if it raised an exception</returns>
        public Tuple<bool, Exception> CanConnectToDatabase(bool ThrowException)
        {
            try
            {
                //go try to just run a simple select without touching able tables
                ExecuteRawSql("SELECT 1", TransactionalBehavior.DoNotEnsureTransaction);

                //we were able to connect so return true
                return new Tuple<bool, Exception>(true, null);
            }
            catch (Exception ex)
            {
                //if you want to throw the exception then do it now
                if (ThrowException)
                {
                    throw;
                }

                //you want to return the actual respose without raising an error then return the tuple.
                return new Tuple<bool, Exception>(false, ex);
            }
        }

        /// <summary>
        /// Determines if the field has identity on. Ie. primary key has auto seed on.
        /// </summary>
        /// <param name="TableName">Table Name Where The Column Exists.</param>
        /// <param name="ColumnName">Column Name To Check If Auto Seed Is On</param>
        /// <returns>Result If The Column Has Auto Seed On. Will Throw An Error If Column Name Is Not Found In Table</returns>
        public bool ColumnIsAutoSeedLookup(string TableName, string ColumnName)
        {
            //go run the sql and return the result (will return null if we can't find the column in the table)
            bool? ColumnIsAutoSeed = ExecuteRawSqlWithResults<bool?>(SharedSqlHelpers.ColumnIsAutoSeedSql(TableName, ColumnName)).FirstOrDefault();

            //did we find the column in the table
            if (!ColumnIsAutoSeed.HasValue)
            {
                //we didn't find the column...throw an error 
                throw new NullReferenceException($"Can't Find Column '{ColumnName}' In The Table '{TableName}'. Please Correct The Parameters Of Method");
            }

            //we found the column and we have a result...so just return it
            return ColumnIsAutoSeed.Value;
        }

        #region Table Settings

        /// <summary>
        /// Get a list of all the items that contain the settings for the type passed in. This is the table
        /// </summary>
        /// <typeparam name="TEntityType">Entity Type To Lookup. Must Be Part Of The Entity Model In This Context</typeparam>
        /// <returns>Collection of all the meta data properties for this table</returns>
        public ReadOnlyMetadataCollection<MetadataProperty> TableSettingsSelect<TEntityType>() where TEntityType : class
        {
            //grab the context's workspace which contains a bunch of settings
            var WorkSpace = ((IObjectContextAdapter)EFContext).ObjectContext.MetadataWorkspace;

            //grab the collection of the "SSpace", then grab the items off of that
            var EntityContainerInfo = WorkSpace.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>()[0];

            //grab the entity information name
            return EntityContainerInfo.GetEntitySetByName(typeof(TEntityType).Name, true).MetadataProperties;
        }

        /// <summary>
        /// Grab the databaes schema for the entity passed in.
        /// </summary>
        /// <typeparam name="TEntityType">Entity Type To Lookup. Must Be Part Of The Entity Model In This Context</typeparam>
        /// <returns>Table schema of TEntityType</returns>
        public string SchemaOfTableSelect<TEntityType>() where TEntityType : class
        {
            //try to grab the meta property fetch
            MetadataProperty MetaPropertyFetch;

            //go grab the table settings and return the schema now...then return the schema if we find a match
            if (TableSettingsSelect<TEntityType>().TryGetValue("Schema", true, out MetaPropertyFetch))
            {
                //we found the schema, try to return it
                return MetaPropertyFetch.Value.ToString();
            }

            //if we get here then error out.
            throw new ArgumentNullException("Can't Find A Schema");
        }

        #endregion

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    //if we have an active transaction then we need to do clean it up
                    if (TransactionToRun != null)
                    {
                        //rollback cleans up the transaction...so i don't need to call cleanup after this
                        RollBackTransaction();
                    }

                    //dispose of the context
                    EFContext.Dispose();
                }
            }
            this.Disposed = true;
        }

        #endregion

    }

}
