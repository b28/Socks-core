namespace SocksCore.Primitives
{
    public interface ISocketTimeoutsManager
    {
        /// <summary>
        /// Read timeouts from socket
        /// </summary>
        /// <param name="socket">Socket to read from</param>
        /// <returns>ISocketTimeouts structure</returns>
        ISocketTimeouts GetTimeouts(ISocketContainer socket);
        /// <summary>
        /// Set ISocketTimeouts to socket
        /// </summary>
        /// <param name="container">socket to set timeouts</param>
        /// <param name="timeouts">new ISocketTimeouts values</param>
        /// <returns>old ISocketTimeouts values</returns>
        ISocketTimeouts SetTimeouts(ISocketContainer container, ISocketTimeouts timeouts);

    }
}