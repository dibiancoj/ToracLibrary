using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataTypes;

namespace ToracLibrary.Core.DataProviders.SqlBuilder
{

    /// <summary>
    /// If you have an object and you want to build a generic sql statement, this class will build up sql using reflection on the properties of the object passed in
    /// </summary>
    public static class GenericSqlBuilder
    {

        #region Public Static Methods

        /// <summary>
        /// Builds an insert sql statement for the object passed in using reflection on the properties of the object passed in
        /// </summary>
        /// <typeparam name="T">Type Of The Record To Add</typeparam>
        /// <param name="RecordToAdd">Record To Add</param>
        /// <param name="TableSchema">Schema Of The Table You Are Inserting Into</param>
        /// <param name="TableName">Table Name To Insert Into</param>
        /// <param name="InferredPrimaryKeyFieldName">Primary Key, If PrimaryKeyIsIdentityColumn Is True, Then Primary Key Will Not Be Set</param>
        /// <param name="PrimaryKeyIsIdentityColumn">Is The Primary Key An Auto Seed Item. If So, Then Item Won't Be Outputted In The Sql. EntityFrameworkDP.ColumnAutoSeedLookup Can Look This Up For You</param>
        /// <returns>Sql To Run On The Db</returns>
        public static string BuildInsertSql<T>(T RecordToAdd, string TableSchema, string TableName, string InferredPrimaryKeyFieldName, bool PrimaryKeyIsIdentityColumn)
        {
            //create the field list sql
            var InsertFieldSql = new StringBuilder();

            //create the field value list sql
            var InsertFieldValues = new StringBuilder();

            //loop through all the properties in the object to set the fields we are going to insert into
            foreach (PropertyInfo thisProperty in PropertiesToLoopThroughLazy(RecordToAdd.GetType()))
            {
                //if its the primary key and the identity column is true then ignore this guy
                if (!(PrimaryKeyIsIdentityColumn && string.Equals(InferredPrimaryKeyFieldName, thisProperty.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    //let's add the field name
                    InsertFieldSql.Append(thisProperty.Name);

                    //add the comma now
                    InsertFieldSql.Append(",");

                    //check to see if this value requires a quote
                    string QuoteCheck = DataTypeQuoteCheck(thisProperty.PropertyType);

                    //do we need a quote
                    bool NeedsAQuote = !string.IsNullOrEmpty(QuoteCheck);

                    //do we need a quote?
                    if (NeedsAQuote)
                    {
                        //add the quote
                        InsertFieldValues.Append(QuoteCheck);
                    }

                    //grab the value of the item
                    object ValueOfField = thisProperty.GetValue(RecordToAdd);

                    //if the field is null then output null else output the item
                    if (ValueOfField == null || string.IsNullOrEmpty(ValueOfField.ToString()))
                    {
                        //field is null, add the null value
                        InsertFieldValues.Append("Null");
                    }
                    else
                    {
                        //set the field values now
                        InsertFieldValues.Append(ValueOfField);
                    }

                    //do we need a quote?
                    if (NeedsAQuote)
                    {
                        //add the quote
                        InsertFieldValues.Append(QuoteCheck);
                    }

                    //add the comma now
                    InsertFieldValues.Append(",");
                }
            }

            //let's remove the last comma
            InsertFieldSql.Remove(InsertFieldSql.Length - 1, 1);

            //let's remove the last comma
            InsertFieldValues.Remove(InsertFieldValues.Length - 1, 1);

            //return the sql
            return string.Format("INSERT INTO {0}.{1}({2}) VALUES({3});", TableSchema, TableName, InsertFieldSql, InsertFieldValues);
        }

        /// <summary>
        /// Builds an update sql statement for the object passed in using reflection on the properties of the object passed in
        /// </summary>
        /// <typeparam name="T">Type Of The Record To Add</typeparam>
        /// <param name="RecordToUpdate">Record To Update</param>
        /// <param name="TableSchema">Schema Of The Table You Are Inserting Into</param>
        /// <param name="TableName">Table Name To Insert Into</param>
        /// <param name="InferredPrimaryKeyFieldName">Inferred Primary Key Used To Do Where PrimaryKeyId = </param>
        /// <returns>Sql To Run On The Db</returns>
        public static string BuildUpdateSql<T>(T RecordToUpdate, string TableSchema, string TableName, string InferredPrimaryKeyFieldName)
        {
            //create the field list sql
            var UpdateFieldAndValuesSql = new StringBuilder();

            //holds the primary key inferred value so we can run a where
            object PrimaryKeyValue = null;

            //loop through all the properties in the object to set the fields we are going to insert into
            foreach (PropertyInfo thisProperty in PropertiesToLoopThroughLazy(RecordToUpdate.GetType()))
            {
                //if this is the primary key then we need to grab the value and store it
                if (string.Equals(thisProperty.Name, InferredPrimaryKeyFieldName, StringComparison.OrdinalIgnoreCase))
                {
                    //set the value
                    PrimaryKeyValue = thisProperty.GetValue(RecordToUpdate);

                    //make sure its not null
                    if (PrimaryKeyValue == null)
                    {
                        throw new ArgumentNullException("Primary Key Value Can't Be Null");
                    }
                }
                else
                {
                    //let's add the field name
                    UpdateFieldAndValuesSql.Append(thisProperty.Name);

                    //add the equals
                    UpdateFieldAndValuesSql.Append("=");

                    //check to see if this value requires a quote
                    string QuoteCheck = DataTypeQuoteCheck(thisProperty.PropertyType);

                    //do we need a quote
                    bool NeedsAQuote = !string.IsNullOrEmpty(QuoteCheck);

                    //do we need a quote?
                    if (NeedsAQuote)
                    {
                        //add the quote
                        UpdateFieldAndValuesSql.Append(QuoteCheck);
                    }

                    //grab the value of the item
                    object ValueOfField = thisProperty.GetValue(RecordToUpdate);

                    //if the field is null then output null else output the item
                    if (ValueOfField == null || string.IsNullOrEmpty(ValueOfField.ToString()))
                    {
                        //field is null, add the null value
                        UpdateFieldAndValuesSql.Append("Null");
                    }
                    else
                    {
                        //set the field values now
                        UpdateFieldAndValuesSql.Append(ValueOfField);
                    }

                    //do we need a quote?
                    if (NeedsAQuote)
                    {
                        //add the quote
                        UpdateFieldAndValuesSql.Append(QuoteCheck);
                    }

                    //add the comma now
                    UpdateFieldAndValuesSql.Append(",");
                }
            }

            //let's remove the last comma
            UpdateFieldAndValuesSql.Remove(UpdateFieldAndValuesSql.Length - 1, 1);

            //return the sql
            return $"UPDATE {TableSchema}.{TableName} SET {UpdateFieldAndValuesSql} WHERE {InferredPrimaryKeyFieldName} = {PrimaryKeyValue};";
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Get the properties to build the sql off of. Need to filter out any collections ie foreign keys
        /// </summary>
        /// <param name="TypeToGetProperties">Type To Get</param>
        /// <returns>list of property into to add to the sql</returns>
        private static IEnumerable<PropertyInfo> PropertiesToLoopThroughLazy(Type TypeToGetProperties)
        {
            //grab the primitives types we care about
            IEnumerable<Type> PrimitiveTypesToLookFor = PrimitiveTypes.PrimitiveTypesSelect();

            //loop through all the properties
            foreach (var PropertyToCheckFor in TypeToGetProperties.GetProperties())
            {
                //we just want string ,ints, etc...
                if (PrimitiveTypesToLookFor.Contains(PropertyToCheckFor.PropertyType))
                {
                    //this is a primitive type...return it
                    yield return PropertyToCheckFor;
                }
            }
        }

        /// <summary>
        /// Checks the property type and if it requires a quote then it returns a quote. else returns string.empty
        /// </summary>
        /// <param name="PropertyDataType">Property Data Type</param>
        /// <returns>quote or string.empty if it doesn't need a quote</returns>
        private static string DataTypeQuoteCheck(Type PropertyDataType)
        {
            //does this data type need a quote?
            if (SharedSqlHelpers.DataTypeNeedsQuoteInSql(PropertyDataType))
            {
                //it needs a quote return the quote
                return "'";
            }

            //doens't need quote return string.empty
            return string.Empty;
        }

        #endregion

    }

}
