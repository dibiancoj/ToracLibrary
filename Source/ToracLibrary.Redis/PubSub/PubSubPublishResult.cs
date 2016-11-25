using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis.PubSub
{

    /// <summary>
    /// Holds the result of a pub sub when the server calls a publish.
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class PubSubPublishResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ChannelToSet">Channel that was published</param>
        /// <param name="MessageToSet">Message that was sent</param>
        public PubSubPublishResult(string ChannelToSet, string MessageToSet)
        {
            Channel = ChannelToSet;
            Message = MessageToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channel that was published
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Message that was sent
        /// </summary>
        public string Message { get; set; }

        #endregion

    }

}
