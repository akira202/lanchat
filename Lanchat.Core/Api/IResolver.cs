namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Handling received data.
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        void RegisterHandler(IApiHandler apiHandler);

        /// <summary>
        ///     Handle incoming data.
        /// </summary>
        /// <param name="item">Json string</param>
        void CallHandler(string item);
    }
}