using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis
{

    /* Low Level Command Syntax 
       * client.SendCommand("ping");
       * or
       * client.SendCommand("set a 100", Encoding.UTF8.GetString);
       * or
       * client.SendCommand("set", "FavoriteTeamKey", "Mets"); 
       */

    /* High Level Command Sytnax
     * Redis.StringSet(Key, ValueToTest)
     * or
     * Redis.StringGet(Key)
     */

    /// <summary>
    /// Raw Redis socket client. Incase you don't want to use the stackexchange or stackoverflow api's. Those api's provide more functionality. This is more of a r and d class
    /// </summary>
    /// <remarks>This uses raw sockets</remarks>
    public class RedisClient : IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor when you use the default port of 6379 and you don't want a time out
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local. You can use either Ip or Dns. ie "Fedora"</param>
        public RedisClient(string RedisHostIPAddressToSet) : this(RedisHostIPAddressToSet, DefaultRedisPort, DefaultRedisTimeout)
        {
        }

        /// <summary>
        /// Constructor when you need to change the port address
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local. You can use either Ip or Dns. ie "Fedora"</param>
        /// <param name="PortToSet">Port number of the Redis server</param>
        public RedisClient(string RedisHostIPAddressToSet, int PortToSet) : this(RedisHostIPAddressToSet, PortToSet, DefaultRedisTimeout)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local. You can use either Ip or Dns. ie "Fedora"</param>
        /// <param name="PortToSet">Port number of the Redis server. Default is 6379</param>
        /// <param name="CommandTimeOut">The time-out value, in milliseconds. If you set the property with a value between 1 and 499, the value will be changed to 500. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        public RedisClient(string RedisHostIPAddressToSet, int PortToSet, int CommandTimeOut)
        {
            RedisIPAddress = RedisHostIPAddressToSet;
            RedisPort = PortToSet;
            RedisCommandTimeOut = CommandTimeOut;

            //go try to connect now to create the socket
            Connect();
        }

        #endregion

        #region Constants

        /// <summary>
        /// Default Redis Port
        /// </summary>
        private const int DefaultRedisPort = 6379;

        /// <summary>
        /// Default redis timeout. 0 or -1 is no time out)
        /// </summary>
        private const int DefaultRedisTimeout = -1;

        /// <summary>
        /// Encoding Used In Redis
        /// </summary>
        private static readonly Encoding RedisEncoding = Encoding.UTF8;

        #region Performance Based Constants

        /// <summary>
        /// Response to a set or command
        /// </summary>
        public const string OKCommandResult = "OK";

        /// <summary>
        /// Terminate Strings 
        /// </summary>
        private const string TerminateStrings = "\r\n";

        /// <summary>
        /// Holds the terminated string in bytes so we don't have to keep computing this
        /// </summary>
        private static readonly byte[] TerminateStringsInBytes = RedisEncoding.GetBytes(TerminateStrings);

        /// <summary>
        /// Constant for r so we don't have to keep creating the same variable. Saves on memory
        /// </summary>
        private const char RChar = '\r';

        /// <summary>
        /// Constant for n so we don't have to keep creating the same variable. Saves on memory
        /// </summary>
        private const char NChar = '\n';

        /// <summary>
        /// Default buffer size
        /// </summary>
        private const int BufferSize = 16 * 1024;

        #endregion

        #endregion

        #region Properties

        #region Connection Properties

        /// <summary>
        /// Redis server ip address
        /// </summary>
        private string RedisIPAddress { get; }

        /// <summary>
        /// Port number of the Redis server
        /// </summary>
        private int RedisPort { get; }

        /// <summary>
        /// The time-out value, in milliseconds. If you set the property with a value between 1 and 499, the value will be changed to 500. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.
        /// </summary>
        private int RedisCommandTimeOut { get; }

        #endregion

        #region Response And Socket Properties

        /// <summary>
        /// Holds the socket connection to the Redis server
        /// </summary>
        private Socket SocketConnection { get; set; }

        /// <summary>
        /// Holds the response stream for the Redis Result
        /// </summary>
        private BufferedStream ResponseStream { get; set; }

        #endregion

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #endregion

        #region Enums

        /// <summary>
        /// Response type
        /// </summary>
        private enum ResponseType : byte
        {

            /// <summary>
            /// String
            /// </summary>
            SimpleStrings = (byte)'+',

            /// <summary>
            /// An error(s)
            /// </summary>
            Erorrs = (byte)'-',

            /// <summary>
            /// Int's
            /// </summary>
            Integers = (byte)':',

            /// <summary>
            /// Bulk Strings
            /// </summary>
            BulkStrings = (byte)'$',

            /// <summary>
            /// Arrays
            /// </summary>
            Arrays = (byte)'*'

        }

        #endregion

        #region Methods

        #region Low Level Send Commands

        /// <summary>
        /// Send a command to the redis server
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <returns></returns>
        public TResponse SendCommand<TResponse>(string CommandToSend)
        {
            //Send the request and return the result
            return (TResponse)SendCommand(CommandToSend, (Func<byte[], object>)null);
        }

        /// <summary>
        /// Send a command to the redis server
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="binaryDecoder"></param>
        /// <returns>Response</returns>
        public object SendCommand(string CommandToSend, Func<byte[], object> BinaryDecoder)
        {
            //Send the request
            SendRequest(RedisEncoding.GetBytes(CommandToSend + TerminateStrings));

            //Grab the response
            return FetchResponse(BinaryDecoder);
        }

        /// <summary>
        /// Send a command to the redis server
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="Arguments">Arguments</param>
        /// <returns>Response</returns>
        public object SendCommand(string CommandToSend, params string[] Arguments)
        {
            //Send the request  and return the result
            return SendCommand(CommandToSend, Arguments, null);
        }

        /// <summary>
        /// Send a command to the redis server
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="Arguments">Arguments</param>
        /// <param name="BinaryDecoder">BinaryDecoder</param>
        /// <returns>Response</returns>
        public object SendCommand(string CommandToSend, string[] Arguments, Func<byte[], object> BinaryDecoder)
        {
            //build the send command
            var SendCommand = BuildBinarySafeCommand(CommandToSend, Arguments);

            //send the request
            SendRequest(SendCommand);

            // return the result
            return FetchResponse(BinaryDecoder);
        }

        #endregion

        #region High Level - Abstracted Commands

        #region String Based

        /// <summary>
        /// Add a string based record with no expiration
        /// </summary>
        /// <param name="Key">Key to use for the record</param>
        /// <param name="Value">String value to set</param>
        /// <returns>Response command if you need it</returns>
        public string StringSet(string Key, string Value)
        {
            //use the overload
            return StringSet(Key, Value, null);
        }

        /// <summary>
        /// Add a string based record.
        /// </summary>
        /// <param name="Key">Key to use for the record</param>
        /// <param name="Value">String value to set</param>
        /// <param name="ExpirationInSeconds">Expiration in seconds</param>
        /// <returns>Response command if you need it</returns>
        public string StringSet(string Key, string Value, int? ExpirationInSeconds)
        {
            //if we have an expiration use it
            if (ExpirationInSeconds.HasValue)
            {
                //use the call with the expiration
                return (string)SendCommand("SET", Key, Value, "EX", ExpirationInSeconds.Value.ToString());
            }

            //use the low level overload
            return (string)SendCommand("SET", Key, Value);
        }

        /// <summary>
        /// Retrieve a string based record
        /// </summary>
        /// <param name="Key">Key to retrieve</param>
        /// <returns>Value of item</returns>
        public string StringGet(string Key)
        {
            //need to account if the item is not found in the cache
            var Response = SendCommand("GET", Key);

            //found in cache
            if (Response == null)
            {
                //no value...return the null
                return null;
            }

            //use the low level overload
            return ByteArrayToString((byte[])Response);
        }

        #endregion

        #region Int Based

        /// <summary>
        /// Add an int based record.
        /// </summary>
        /// <param name="Key">Key to use for the record</param>
        /// <param name="Value">String value to set</param>
        /// <returns>Response command if you need it</returns>
        public string IntSet(string Key, int Value)
        {
            //use the overload. Redis stores int in the same manner. so just call ToString(). increment will work
            return IntSet(Key, Value, null);
        }

        /// <summary>
        /// Add an int based record.
        /// </summary>
        /// <param name="Key">Key to use for the record</param>
        /// <param name="Value">String value to set</param>
        /// <param name="ExpirationInSeconds">Expiration in seconds</param>
        /// <returns>Response command if you need it</returns>
        public string IntSet(string Key, int Value, int? ExpirationInSeconds)
        {
            //use the overload. Redis stores int in the same manner. so just call ToString(). increment will work
            return StringSet(Key, Value.ToString(), ExpirationInSeconds);
        }

        /// <summary>
        /// Retrieve an int based record
        /// </summary>
        /// <param name="Key">Key to retrieve</param>
        /// <returns>Value of item</returns>
        public int? IntGet(string Key)
        {
            //try parse value
            int TryParseValue;

            //go try to parase this
            if (int.TryParse(StringGet(Key), out TryParseValue))
            {
                //return the parsed value
                return TryParseValue;
            }

            //return the null
            return null;
        }

        #endregion

        #region Increment - Decrement Int

        /// <summary>
        /// Increment an int value that is stored
        /// </summary>
        /// <param name="Key">Key to increment</param>
        /// <returns>new value. If key is not found the value will increment to 1 (redis default functionality)</returns>
        public int IncrementInt(string Key)
        {
            return Convert.ToInt32(SendCommand("INCR", Key));
        }

        /// <summary>
        /// Decrement an int value that is stored
        /// </summary>
        /// <param name="Key">Key to decrement</param>
        /// <returns>new value. If key is not found the value will increment to -1 (redis default functionality)</returns>
        public int DecrementInt(string Key)
        {
            return Convert.ToInt32(SendCommand("DECR", Key));
        }

        #endregion

        #region Key Exists

        /// <summary>
        /// Check if the key exists
        /// </summary>
        /// <param name="KeyToCheck">Key to check if it exists</param>
        /// <returns>Yes if it exists</returns>
        public bool KeyExists(string KeyToCheck)
        {
            //1 = it exists
            //0 = it does not exist
            return Convert.ToInt32(SendCommand("EXISTS", KeyToCheck)) == 1;
        }

        #endregion

        #region Remove A Cache Item

        /// <summary>
        /// Remove A Cache Item
        /// </summary>
        /// <param name="KeyToRemove">Key to remove</param>
        /// <returns>How many records it deleted. 1 if it removed the item in the cache. 0 if there were no items found for the specified key</returns>
        public int RemoveItemFromCache(string KeyToRemove)
        {
            //go delete the item
            return Convert.ToInt32(SendCommand("DEL", KeyToRemove));
        }

        #endregion

        #endregion

        #region Pipeline

        /// <summary>
        /// Go create the pipline command and return the object
        /// </summary>
        /// <returns>RedisPipelineCommand build up and ready for commands to be added</returns>
        public RedisPipelineCommand CreatePipeline()
        {
            //go create the object and return it
            return new RedisPipelineCommand(this);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to turn a byte array into a string until we abstract everything
        /// </summary>
        /// <param name="ValueToConvertToString">Value to convert to a string</param>
        /// <returns>string value of the byte array</returns>
        public string ByteArrayToString(byte[] ValueToConvertToString)
        {
            return RedisEncoding.GetString(ValueToConvertToString);
        }

        /// <summary>
        /// Helper method to turn a string into a byte array until we can abstract everything
        /// </summary>
        /// <param name="ValueToConvertToByteArray">Value to convert to a byte array</param>
        /// <returns>string value of the byte array</returns>
        public byte[] StringToByteArray(string ValueToConvertToByteArray)
        {
            return RedisEncoding.GetBytes(ValueToConvertToByteArray);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Send a request
        /// </summary>
        /// <param name="CommandToSendInBytes">Command to send in bytes</param>
        internal void SendRequest(byte[] CommandToSendInBytes)
        {
            try
            {
                SocketConnection.Send(CommandToSendInBytes);
            }
            catch (SocketException)
            {
                //clean up incase we throw an error (dispose will close the connection)
                SocketConnection.Dispose();

                //throw the error now
                throw;
            }
        }

        /// <summary>
        /// Grab the response and return it
        /// </summary>
        /// <param name="BinaryDecoder">BinaryDecoder if one</param>
        /// <returns>Response</returns>
        internal object FetchResponse(Func<byte[], object> BinaryDecoder)
        {
            //read the first byte so we can determine the response type
            var ResponseTypeBeingReturned = (ResponseType)ResponseStream.ReadByte();

            //switch response type is this call that we just made? This depends on what we get back from the socket
            switch (ResponseTypeBeingReturned)
            {
                case ResponseType.SimpleStrings:
                case ResponseType.Erorrs:
                    {
                        return ReadFirstLine();
                    }

                case ResponseType.Integers:
                    {
                        return long.Parse(ReadFirstLine());
                    }

                case ResponseType.BulkStrings:
                    {
                        var Length = int.Parse(ReadFirstLine());

                        if (Length == -1)
                        {
                            return null;
                        }

                        var Buffer = new byte[Length];

                        ResponseStream.Read(Buffer, 0, Length);

                        ReadFirstLine(); // read terminate

                        if (BinaryDecoder == null)
                        {
                            return Buffer;
                        }

                        return BinaryDecoder(Buffer);
                    }

                case ResponseType.Arrays:
                    {
                        var Length = int.Parse(ReadFirstLine());

                        if (Length == 0)
                        {
                            return Array.Empty<object>();
                        }

                        if (Length == -1)
                        {
                            return null;
                        }

                        var ArrayOfObjects = new List<object>();

                        for (int i = 0; i < Length; i++)
                        {
                            ArrayOfObjects.Add(FetchResponse(BinaryDecoder));
                        }

                        return ArrayOfObjects;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        /// <summary>
        /// Read the first line of the response
        /// </summary>
        /// <returns>Response string</returns>
        private string ReadFirstLine()
        {
            //response string to return
            var ResponseString = new StringBuilder();

            //current character
            int CurrentCharacter;

            ///previous character
            var PreviousCharacter = default(char);

            //loop until we are done
            while ((CurrentCharacter = ResponseStream.ReadByte()) != -1)
            {
                //grab the current character
                var CharacterToChar = (char)CurrentCharacter;

                // reach at TerminateLine?
                if (PreviousCharacter == RChar && CharacterToChar == NChar)
                {
                    break;
                }
                else if (PreviousCharacter == RChar && CharacterToChar == RChar)
                {
                    ResponseString.Append(PreviousCharacter); // append prev '\r'
                    continue;
                }
                else if (CharacterToChar == RChar)
                {
                    PreviousCharacter = CharacterToChar; // not append '\r'
                    continue;
                }

                //set the previous character
                PreviousCharacter = CharacterToChar;

                //append the character to the response string
                ResponseString.Append(CharacterToChar);
            }

            //return the string now
            return ResponseString.ToString();
        }

        /// <summary>
        /// Build a binary safe command
        /// </summary>
        /// <param name="CommandToSend">Command to send</param>
        /// <param name="Arguments">Argument</param>
        /// <returns>byte array for this safe command</returns>
        internal byte[] BuildBinarySafeCommand(string CommandToSend, string[] Arguments)
        {
            //grab the 3rd line
            var ThirdLine = Arguments.Select(x =>
            {
                //convert the string to a byte array
                var ByteArrayConverted = StringToByteArray(x);

                //grab the head value
                var HeadValue = RedisEncoding.GetBytes(string.Format($"{(char)ResponseType.BulkStrings}{ByteArrayConverted.Length.ToString()}{TerminateStrings}"));

                //go return after concat
                return HeadValue.Concat(ByteArrayConverted).Concat(TerminateStringsInBytes).ToArray();
            });

            //go return each of them
            return new[] {

                 //go build the first line
                 RedisEncoding.GetBytes(string.Format($"{(char)ResponseType.Arrays}{(Arguments.Length + 1).ToString()}{TerminateStrings}")),

                 //build the second line
                 RedisEncoding.GetBytes(string.Format($"{(char)ResponseType.BulkStrings}{RedisEncoding.GetBytes(CommandToSend).Length.ToString()}{TerminateStrings}{CommandToSend}{TerminateStrings}"))

            }.Concat(ThirdLine).SelectMany(xs => xs).ToArray();
        }

        /// <summary>
        /// Connect to the redis server
        /// </summary>
        private void Connect()
        {
            //create the new socket
            SocketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true, SendTimeout = RedisCommandTimeOut };

            //go connect
            SocketConnection.Connect(RedisIPAddress, RedisPort);

            //if not connected them cleanup the socket
            if (!SocketConnection.Connected)
            {
                //dispose of it
                SocketConnection.Dispose();

                //throw an error
                throw new Exception("Socket Not Able To Connect");
            }

            //go create the response stream
            ResponseStream = new BufferedStream(new NetworkStream(SocketConnection), BufferSize);
        }

        #endregion

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
                    //dispose of the stream
                    if (ResponseStream != null)
                    {
                        //dispose calls close - verified in .net source code
                        ResponseStream.Dispose();
                    }

                    //dispose of the socket
                    if (SocketConnection != null)
                    {
                        //this should close the socket as well
                        SocketConnection.Dispose();
                    }
                }
            }
            this.Disposed = true;
        }

        #endregion

    }

}
