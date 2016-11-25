using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis
{

    /// <summary>
    /// Holds a pipeline command.
    /// </summary>
    public class RedisPipelineCommand
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RedisPipelineCommand(RedisClient ClientToSet)
        {
            //create a new command to run
            CommandToRun = new List<byte[]>();

            //set the client
            Client = ClientToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Redis client
        /// </summary>
        private RedisClient Client { get; }

        /// <summary>
        /// Commands to run in a byte array
        /// </summary>
        private List<byte[]> CommandToRun { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Adds a command to run
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="Arguments">arguments</param>
        public void AddCommandToRun(string CommandToSend, params string[] Arguments)
        {
            //go build the binary safe
            CommandToRun.Add(Client.BuildBinarySafeCommand(CommandToSend, Arguments));
        }

        /// <summary>
        /// Go save the pipeline
        /// </summary>
        /// <returns>The list of responses. This can't be an interator because we need to ensure all the responses get brought down. Otherwise the next call won't return the correct response</returns>
        public IEnumerable<object> SavePipeLine()
        {
            //go start running the command
            Client.SendRequest(CommandToRun.SelectMany(x => x).ToArray());

            //responses
            var Responses = new List<object>();

            //now loop through each of the commands and return the results
            foreach (var Response in CommandToRun)
            {
                //go fetch the response
                Responses.Add(RedisClient.FetchResponse(null, Client.ResponseStream));
            }

            //return the list
            return Responses;
        }

        #endregion

    }

}
