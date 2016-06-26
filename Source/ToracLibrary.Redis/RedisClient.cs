using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Redis
{

    /* Command Syntax 
       * client.SendCommand("ping");
       * or
       * client.SendCommand("set a 100", Encoding.UTF8.GetString);
       * or
       * client.SendCommand("set", new[] { Encoding.UTF8.GetBytes("test"), Encoding.UTF8.GetBytes("abcde") }, Encoding.UTF8.GetString);
       * or
       * result = Encoding.UTF8.GetString((byte[])client.SendCommand("get", new[] { Encoding.UTF8.GetBytes("test") }));
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
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local</param>
        public RedisClient(string RedisHostIPAddressToSet) : this(RedisHostIPAddressToSet, DefaultRedisPort, DefaultRedisTimeout)
        {
        }

        /// <summary>
        /// Constructor when you need to change the port address
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local</param>
        /// <param name="PortToSet">Port number of the Redis server</param>
        public RedisClient(string RedisHostIPAddressToSet, int PortToSet) : this(RedisHostIPAddressToSet, PortToSet, DefaultRedisTimeout)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RedisHostIPAddressToSet">Redis server ip address. "127.0.0.1" for local</param>
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
        /// Terminate Strings 
        /// </summary>
        private const string TerminateStrings = "\r\n";

        /// <summary>
        /// Default Redis Port
        /// </summary>
        private const int DefaultRedisPort = 6379;

        /// <summary>
        /// Default redis timeout. Which is no time out
        /// </summary>
        private const int DefaultRedisTimeout = -1;

        /// <summary>
        /// Encoding Used In Redis
        /// </summary>
        private static readonly Encoding RedisEncoding = Encoding.UTF8;

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
        public enum ResponseType : byte
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

        #region Send Commands

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
        /// <returns></returns>
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
        /// <returns></returns>
        public object SendCommand(string CommandToSend, params byte[][] Arguments)
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
        /// <returns></returns>
        public object SendCommand(string CommandToSend, byte[][] Arguments, Func<byte[], object> BinaryDecoder)
        {
            //build the send command
            var SendCommand = BuildBinarySafeCommand(CommandToSend, Arguments);

            //send the request
            SendRequest(SendCommand);

            // return the result
            return FetchResponse(BinaryDecoder);
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
        private void SendRequest(byte[] CommandToSendInBytes)
        {
            try
            {
                SocketConnection.Send(CommandToSendInBytes);
            }
            catch (SocketException)
            {
                //clean up incase we throw an error
                SocketConnection.Close();
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
        private object FetchResponse(Func<byte[], object> BinaryDecoder)
        {
            //read the first byte so we can determine the response type
            var ResponseTypeBeingReturned = (ResponseType)ResponseStream.ReadByte();

            switch (ResponseTypeBeingReturned)
            {
                case ResponseType.SimpleStrings:
                    {
                        return ReadFirstLine();
                    }
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
                        var length = int.Parse(ReadFirstLine());

                        if (length == -1)
                        {
                            return null;
                        }

                        var buffer = new byte[length];

                        ResponseStream.Read(buffer, 0, length);

                        ReadFirstLine(); // read terminate

                        if (BinaryDecoder == null)
                        {
                            return buffer;
                        }
                        else
                        {
                            return BinaryDecoder(buffer);
                        }
                    }
                case ResponseType.Arrays:
                    {
                        var length = int.Parse(ReadFirstLine());

                        if (length == 0)
                        {
                            return new object[0];
                        }
                        if (length == -1)
                        {
                            return null;
                        }

                        var objects = new object[length];

                        for (int i = 0; i < length; i++)
                        {
                            objects[i] = FetchResponse(BinaryDecoder);
                        }

                        return objects;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
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

                if (PreviousCharacter == '\r' && CharacterToChar == '\n') // reach at TerminateLine
                {
                    break;
                }
                else if (PreviousCharacter == '\r' && CharacterToChar == '\r')
                {
                    ResponseString.Append(PreviousCharacter); // append prev '\r'
                    continue;
                }
                else if (CharacterToChar == '\r')
                {
                    PreviousCharacter = CharacterToChar; // not append '\r'
                    continue;
                }

                PreviousCharacter = CharacterToChar;
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
        private byte[] BuildBinarySafeCommand(string CommandToSend, byte[][] Arguments)
        {
            //grab the first line
            var FirstLine = RedisEncoding.GetBytes((char)ResponseType.Arrays + (Arguments.Length + 1).ToString() + TerminateStrings);

            //Grab the second line
            var SecondLine = RedisEncoding.GetBytes((char)ResponseType.BulkStrings + RedisEncoding.GetBytes(CommandToSend).Length.ToString() + TerminateStrings + CommandToSend + TerminateStrings);

            //grab the 3rd line
            var ThirdLine = Arguments.Select(x =>
            {
                //grab the head value
                var HeadValue = RedisEncoding.GetBytes((char)ResponseType.BulkStrings + x.Length.ToString() + TerminateStrings);

                //go return after concat
                return HeadValue.Concat(x).Concat(RedisEncoding.GetBytes(TerminateStrings)).ToArray();

            }).ToArray();

            //go return each of them
            return new[] { FirstLine, SecondLine }.Concat(ThirdLine).SelectMany(xs => xs).ToArray();
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
                //close the socket
                SocketConnection.Close();

                //dispose of it
                SocketConnection.Dispose();

                //exit the method
                return;
            }

            //go create the response stream
            ResponseStream = new BufferedStream(new NetworkStream(SocketConnection), 16 * 1024);
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
                        ResponseStream.Dispose();
                    }

                    //dispose of the socket
                    if (SocketConnection != null)
                    {
                        SocketConnection.Dispose();
                    }
                }
            }
            this.Disposed = true;
        }

        #endregion

    }

}
