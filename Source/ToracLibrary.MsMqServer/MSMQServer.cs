using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.MsMqServer
{

    //To install msmq on a machine.
    //1. Control Panel
    //2. Turn Windows Features On / Off
    //3. Microsoft Message Queue (MSMQ) Server

    //To launch GUI
    //1. Computer Management
    //2. Services and Applications Tab On Screen
    //3. Message Queuing

    //*** This handles local message queues. If you need a public one or what not, you will need to the BuildMessageQueuePath method and whatever else you need***

    /// <summary>
    /// Generic Class To Handles Microsofts Message Queuing or MSMQ
    /// </summary>
    public static class MSMQ
    {

        #region Public Static Methods

        #region Queue Methods

        /// <summary>
        /// Check to see if a queue exists on the server already
        /// </summary>
        /// <param name="QueueName">Queue name to check if it exists.</param>
        /// <returns>If it exists</returns>
        public static bool QueueExists(string QueueName)
        {
            //go build the queue name and return if it exists
            return MessageQueue.Exists(BuildMessageQueuePath(QueueName));
        }

        /// <summary>
        /// Delete a queue from the server
        /// </summary>
        /// <param name="QueueName">Queue name to check if it exists.</param>
        /// <returns>Did it delete</returns>
        public static bool DeleteQueue(string QueueName)
        {
            //go delete the queue
            MessageQueue.Delete(BuildMessageQueuePath(QueueName));

            //it deleted the queue return the positive result
            return true;
        }

        /// <summary>
        /// Creates a queue on the server
        /// </summary>
        /// <param name="QueueName">Queue name to check if it exists.</param>>
        /// <param name="DescriptionOfQueue">Description of the queue which will be the label</param>
        /// <param name="Transactional">Do we want this queue to be transactional. They will be written to disk if it is. Otherwise they are stored in memory. Memory is faster but disk can be recovered if server crashes and needs to be rebooted</param>
        /// <returns>Was the queue created</returns>
        public static bool CreateAQueue(string QueueName, string DescriptionOfQueue, bool Transactional)
        {
            //the queue returned from the method implements idisposable...so we need to catch it and dispose of it
            using (var QueueToCreate = MessageQueue.Create(BuildMessageQueuePath(QueueName), Transactional))
            {
                //set the label now
                QueueToCreate.Label = DescriptionOfQueue;

                //return that we have created the queue 
                return true;
            }
        }

        /// <summary>
        /// Gets all the messages in a queue
        /// </summary>
        /// <typeparam name="T">T is the type of body that the message has. So in send message whatever the type of ObjectToStoreInMessage is</typeparam>
        /// <param name="QueueName">Queue name to get all the messages for</param>
        /// <returns>List of messages. Is lazy returned by using yield return.</returns>
        public static IEnumerable<ToracMsMqMessage<T>> RetrieveAllMessagesInQueueLazy<T>(string QueueName)
        {
            //grab the messsage queue
            using (var MessageQueueServer = new MessageQueue(BuildMessageQueuePath(QueueName)))
            {
                //loop through the messages now
                foreach (var MessageToRetrieve in MessageQueueServer.GetAllMessages())
                {
                    //set the formatter on this message so we can get the message
                    MessageToRetrieve.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });

                    //let's return the message now
                    yield return new ToracMsMqMessage<T>(MessageToRetrieve.Id, (T)MessageToRetrieve.Body);

                    //dispose of this message now
                    MessageToRetrieve.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes all the messages in a queue
        /// </summary>
        /// <param name="QueueName">Queue name to purge</param>
        /// <returns>Did it delete all the message</returns>
        public static bool PurgeAllMessagesInQueue(string QueueName)
        {
            //grab the messsage queue
            using (var MessageQueueServer = new MessageQueue(BuildMessageQueuePath(QueueName)))
            {
                //go delete all the messages
                MessageQueueServer.Purge();

                //return the positive result
                return true;
            }
        }

        #endregion

        #region Message Methods

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="QueueName">Queue Name To Put The Message In</param>
        /// <param name="ObjectToStoreInMessage">Object to store in the message. Could be a string or an object. Object will get serialized.</param>
        /// <param name="IsMessageRecoverable">Should this message be written to disk? This way if the server needs to be rebooted it will be saved. Otherwise it will be stored in memory which is faster but can't be recovered.</param>
        /// <returns>If the message was sent</returns>
        public static bool SendMessage<T>(string QueueName, T ObjectToStoreInMessage, bool IsMessageRecoverable)
        {
            //grab the messsage queue
            using (var MessageQueueServer = new MessageQueue(BuildMessageQueuePath(QueueName)))
            {
                //create the new message object (throw the message in the constructor)
                using (var MessageToSend = new Message(ObjectToStoreInMessage))
                {
                    //set the recoverable flag
                    MessageToSend.Recoverable = IsMessageRecoverable;

                    //send the message now
                    MessageQueueServer.Send(MessageToSend);

                    //return the positive result
                    return true;
                }
            }
        }

        #endregion

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Builds the fully formatted queue path needed to pass into the api
        /// </summary>
        /// <param name="QueueName">Queue name to build on</param>
        /// <returns>The full message queue path</returns>
        private static string BuildMessageQueuePath(string QueueName)
        {
            //just go build the path and return
            return string.Format(@".\Private$\{0}", QueueName);
        }

        #endregion

    }

}
