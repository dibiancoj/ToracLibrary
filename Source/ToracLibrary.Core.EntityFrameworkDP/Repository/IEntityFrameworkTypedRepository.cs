using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP.Repository
{

    /// <summary>
    /// Provides a typed repository for a single type of T.
    /// </summary>
    /// <remarks>Not all methods are added. Add as needed</remarks>
    public interface IEntityFrameworkTypedRepository<T>
        where T : class
    {

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        IQueryable<T> Fetch(bool TrackRecords);

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="WherePredicate">Criteria to match on</param>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> WherePredicate, bool TrackRecords);

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        void Delete(T EntityToDelete, bool CommitChanges);

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        Task DeleteAsync(T EntityToDelete, bool CommitChanges);

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        void Delete(Expression<Func<T, bool>> Predicate, bool CommitChanges);

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        Task DeleteAsync(Expression<Func<T, bool>> Predicate, bool CommitChanges);

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        Task DeleteRangeAsync(IEnumerable<T> EntitiesToDelete, bool CommitChanges);

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="EntityToAdd">Entity To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void Add(T EntityToAdd, bool CommitChanges);

        /// <summary>
        /// Adds the list of records passed in to the specified entity
        /// </summary>
        /// <param name="EntitiesToAdd">List Of Entities To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void AddRange(IEnumerable<T> EntitiesToAdd, bool CommitChanges);

        /// <summary>
        /// Either Add Or Update An Entity Based On If The Record Exists In The Db Already
        /// </summary>
        /// <param name="EntityToAddOrUpdate">Entity To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void Upsert(T EntityToAddOrUpdate, bool CommitChanges);

        /// <summary>
        /// Either Add Or Update An IEnumerable Of Entity Based On The Func You Pass in
        /// </summary>
        /// <param name="EntitiesToAddOrUpdate">Entities To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        void UpsertRange(IEnumerable<T> EntitiesToAddOrUpdate, bool CommitChanges);

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="EntityToAttach">Entity To Attach</param>
        void Attach(T EntityToAttach);

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
