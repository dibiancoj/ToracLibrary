using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Interace so entity framework data provider can be mocked. This is a universal where you pass in a type of T to each method. Use the ITypedRepository for a specific type throughout the repository
    /// </summary>
    /// <remarks>Not all methods are implemented. Will add as we need them</remarks>
    public interface IEntityFrameworkRepository
    {

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        IQueryable<T> Fetch<T>(bool TrackRecords) where T : class;

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="WherePredicate">Criteria to match on</param>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        IQueryable<T> Find<T>(Expression<Func<T, bool>> WherePredicate, bool TrackRecords) where T : class;

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        void Delete<T>(T EntityToDelete, bool CommitChanges) where T : class;

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        Task DeleteAsync<T>(T EntityToDelete, bool CommitChanges) where T : class;

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        void Delete<T>(Expression<Func<T, bool>> Predicate, bool CommitChanges) where T : class;

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        Task DeleteAsync<T>(Expression<Func<T, bool>> Predicate, bool CommitChanges) where T : class;

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        Task DeleteRangeAsync<T>(IEnumerable<T> EntitiesToDelete, bool CommitChanges) where T : class;

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAdd">Entity To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void Add<T>(T EntityToAdd, bool CommitChanges) where T : class;

        /// <summary>
        /// Adds the list of records passed in to the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToAdd">List Of Entities To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void AddRange<T>(IEnumerable<T> EntitiesToAdd, bool CommitChanges) where T : class;

        /// <summary>
        /// Either Add Or Update An Entity Based On If The Record Exists In The Db Already
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAddOrUpdate">Entity To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void Upsert<T>(T EntityToAddOrUpdate, bool CommitChanges) where T : class;

        /// <summary>
        /// Either Add Or Update An IEnumerable Of Entity Based On The Func You Pass in
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntitiesToAddOrUpdate">Entities To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void UpsertRange<T>(IEnumerable<T> EntitiesToAddOrUpdate, bool CommitChanges) where T : class;

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <typeparam name="T">Type Of Item To Query</typeparam>
        /// <param name="EntityToAttach">Entity To Attach</param>
        void Attach<T>(T EntityToAttach) where T : class;

        /// <summary>
        /// Saves all context changes
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves all context changes async
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <remarks>You can call await SaveChangesAsync in the calling method</remarks>
        Task<int> SaveChangesAsync();

    }

}
