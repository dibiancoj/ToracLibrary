using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.MsMqServer
{

    /// <summary>
    /// Torac Msmq Message. Only The Data We Need
    /// </summary>
    /// <typeparam name="T">Type of the body of the message. The real data you set with the message</typeparam>
    /// <remarks>Class is immutable</remarks>
    public class ToracMsMqMessage<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IdOfMessage">Message Id</param>
        /// <param name="BodyOfMessage">Data In The Message</param>
        public ToracMsMqMessage(string IdOfMessage, T BodyOfMessage)
        {
            //set the id
            MessageId = IdOfMessage;

            //set the body
            MessageBodyOfData = BodyOfMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data In The Message
        /// </summary>
        public T MessageBodyOfData { get; }

        /// <summary>
        /// Holds the message id
        /// </summary>
        public string MessageId { get; }

        #endregion

    }

}
