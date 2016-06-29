using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis
{

    /// <summary>
    /// Represents a redis transaction
    /// </summary>
    public class RedisTransaction : IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RedisTransaction(RedisClient ClientToSet)
        {
            //set the client
            Client = ClientToSet;

            //go flip hte flag saying this transaction is active
            TransactionIsActive = true;

            //go start the transaction
            Client.SendCommand("MULTI");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Redis client
        /// </summary>
        private RedisClient Client { get; }

        /// <summary>
        /// Did committed ever get called?
        /// </summary>
        private bool TransactionIsActive { get; set; }

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #endregion

        #region Method

        /// <summary>
        /// Run a command into the transaction group
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="Arguments">arguments</param>
        public void AddCommandToRun(string CommandToSend, params string[] Arguments)
        {
            //we just just use the client method to call the command
            Client.SendCommand(CommandToSend, Arguments);
        }

        /// <summary>
        /// Go save the transaction
        /// </summary>
        /// <returns>The list of responses. This can't be an interator because we need to ensure all the responses get brought down. Otherwise the next call won't return the correct response</returns>
        public IEnumerable<object> CommitTheTransaction()
        {
            //go start running the command...exec returns an array of objects
            var Result = (IEnumerable<object>)Client.SendCommand("EXEC");

            //now flip the flag because the last command worked
            TransactionIsActive = false;

            //return the results now
            return Result;
        }

        /// <summary>
        /// Discard the transaction
        /// </summary>
        public string DiscardTransaction()
        {
            //go kill the transaction
            var Result = Client.SendCommand("DISCARD");

            //now flip the flag because the last command worked
            TransactionIsActive = false;

            //return the result
            return Result.ToString();
        }

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
                    //is the transaction still pending?
                    if (TransactionIsActive)
                    {
                        //go discard the transaction
                        DiscardTransaction();

                        //let the user know about this
                        throw new Exception("Redis.Transaction Has Not Been Committed Or Discarded. Transaction Being Disposed.");
                    }
                }
            }
            this.Disposed = true;
        }

        #endregion

    }

}
