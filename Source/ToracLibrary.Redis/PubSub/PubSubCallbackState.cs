using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis.PubSub
{

    /// <summary>
    /// Holds the state which we need to pass to the async callback
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class PubSubCallbackState
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SocketConnectionToSet">Socket used in call</param>
        /// <param name="BufferSizeToSet">Buffer size to use</param>
        /// <param name="CallBackToSet">Callback when publish was made will invoke this callback</param>
        public PubSubCallbackState(Socket SocketConnectionToSet, int BufferSizeToSet, Action<PubSubPublishResult> CallBackToSet)
        {
            //set the socket
            SocketConnection = SocketConnectionToSet;

            //set the buffer size
            BufferSize = BufferSizeToSet;

            //set the buffer
            Buffer = new byte[BufferSizeToSet];

            //set the callback
            CallBack = CallBackToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Socket used in call
        /// </summary>
        public Socket SocketConnection { get; }

        /// <summary>
        /// Buffer size to use
        /// </summary>
        public int BufferSize { get; }

        /// <summary>
        /// Buffer to use
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// Callback when the publish was made will invoke the callback method with the response
        /// </summary>
        public Action<PubSubPublishResult> CallBack { get; }

        #endregion

    }

}
