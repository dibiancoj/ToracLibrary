using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis.PubSub
{

    //for asap.net put this calls in the global asax or wherever

    /// <summary>
    /// Seperate client for pub sub operations.
    /// </summary>
    /// <remarks>Currently we have it set for only 1 channel per instance.</remarks>
    public class RedisPubSubClient : RedisClient
    {

        #region Constructor

        /// <summary>
        /// Constructor when you use the default port of 6379 and you don't want a time out
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local. You can use either Ip or Dns. ie "Fedora"</param>
        /// <param name="ChannelsToSubscribeToSet">Channels to subscribe to. This is case sensitive!!!</param>
        /// <param name="CallBackToSet">Callback when the publish was made will invoke the callback method with the response</param>
        public RedisPubSubClient(string RedisHostIPAddressToSet, IEnumerable<string> ChannelsToSubscribeToSet, Action<PubSubPublishResult> CallBackToSet) : this(RedisHostIPAddressToSet, DefaultRedisPort, ChannelsToSubscribeToSet, CallBackToSet)
        {
        }

        /// <summary>
        /// Constructor when you need to change the port address
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local. You can use either Ip or Dns. ie "Fedora"</param>
        /// <param name="PortToSet">Port number of the Redis server</param>
        /// <param name="ChannelsToSubscribeToSet">Channels to subscribe to. This is case sensitive!!!</param>
        /// <param name="CallBackToSet">Callback when the publish was made will invoke the callback method with the response</param>
        public RedisPubSubClient(string RedisHostIPAddressToSet, int PortToSet, IEnumerable<string> ChannelsToSubscribeToSet, Action<PubSubPublishResult> CallBackToSet) : base(RedisHostIPAddressToSet, PortToSet, DefaultRedisTimeout)
        {
            //set the channel
            ChannelsSubscribedTo = ChannelsToSubscribeToSet;

            //set the callback
            CallBack = CallBackToSet;

            //go init the pub sub logic
            InitPubSub();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channels to subscribe to. This is case sensitive!!!
        /// </summary>
        private IEnumerable<string> ChannelsSubscribedTo { get; }

        /// <summary>
        /// Callback when the publish was made will invoke the callback method with the response
        /// </summary>
        private Action<PubSubPublishResult> CallBack { get; }

        #endregion

        #region Enum

        /// <summary>
        /// Holds the response to a pub sub publish
        /// </summary>
        private enum PubSubResponse
        {

            /// <summary>
            /// Channel Name
            /// </summary>
            ChannelName = 1,

            /// <summary>
            /// Message Result sent from the server
            /// </summary>
            MessageResult = 2

        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Create the pub sub socket
        /// </summary>
        private void InitPubSub()
        {
            //go create the subscription
            foreach (var ChannelToSubscribeTo in ChannelsSubscribedTo)
            {
                //subscribe to this channel
                Subscribe(ChannelToSubscribeTo);
            }

            //go create the receive sock
            InitReceiveCallBack(SocketConnection, BufferSize, CallBack);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Init the receive callback
        /// </summary>
        /// <param name="SocketConnectionToUse">Socket Connection to use</param>
        /// <param name="BufferSizeToUse">Buffer size to use</param>
        /// <param name="CallBackToUse">Callback when the publish was made will invoke the callback method with the response</param>
        private static void InitReceiveCallBack(Socket SocketConnectionToUse, int BufferSizeToUse, Action<PubSubPublishResult> CallBackToUse)
        {
            //pass to the async method whatever we need
            var StatePassThrough = new PubSubCallbackState(SocketConnectionToUse, BufferSizeToUse, CallBackToUse);

            // Begin receiving the data from the remote device.
            SocketConnectionToUse.BeginReceive(StatePassThrough.Buffer, 0, BufferSizeToUse, 0, new AsyncCallback(ReceiveCallback), StatePassThrough);
        }

        /// <summary>
        /// On Receive content callback
        /// </summary>
        /// <param name="AsyncResultToUse">Result of the async call</param>
        private static void ReceiveCallback(IAsyncResult AsyncResultToUse)
        {
            // Retrieve the state object and the client socket 
            PubSubCallbackState PassedThroughState = (PubSubCallbackState)AsyncResultToUse.AsyncState;

            //Read data from the remote device.
            int BytesReadFromCall = PassedThroughState.SocketConnection.EndReceive(AsyncResultToUse);

            //using this so we can reuse the redis client. otherwise we should just do: Encoding.ASCII.GetString(PassedThroughState.Buffer, 0, BytesReadFromCall))
            using (var StreamToRead = new BufferedStream(new MemoryStream(PassedThroughState.Buffer)))
            {
                //grab the raw result
                var RawResult = ((IList<object>)FetchResponse(null, StreamToRead));

                //go invoke the callback
                PassedThroughState.CallBack(new PubSubPublishResult(ByteArrayToString((byte[])RawResult[(int)PubSubResponse.ChannelName]), ByteArrayToString((byte[])RawResult[(int)PubSubResponse.MessageResult])));
            }

            //we need to create the connection so we can get the next publish.
            InitReceiveCallBack(PassedThroughState.SocketConnection, PassedThroughState.BufferSize, PassedThroughState.CallBack);
        }

        #endregion

    }

}
