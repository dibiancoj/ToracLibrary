using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis.Result
{

    /// <summary>
    /// Subscribe result for redis
    /// </summary>
    public class SubscribeResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CommandToSet">Command Sent</param>
        /// <param name="ChannelSubscribedToSet">Channel subscribed to</param>
        /// <param name="ResultOfCommandToSet">Result of command (1 = successful)</param>
        public SubscribeResult(string CommandToSet, string ChannelSubscribedToSet, int ResultOfCommandToSet)
        {
            Command = CommandToSet;
            ChannelSubscribedTo = ChannelSubscribedToSet;
            ResultOfCommand = ResultOfCommandToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Command Sent
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Channel subscribed to
        /// </summary>
        public string ChannelSubscribedTo { get; }

        /// <summary>
        /// Result of command (1 = successful)
        /// </summary>
        public int ResultOfCommand { get; set; }

        #endregion

        #region Constants

        /// <summary>
        /// Succesfull command number
        /// </summary>
        public const int SuccessfulCommand = 1;

        #endregion

    }

}
