namespace blazor_client_app.Chat
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides all methods need for chatting with other app clients.
    /// </summary>
    public interface IChatService : IAsyncDisposable
    {
        /// <summary>
        /// Gets the auto generated client id of this app client.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Initializes the service by trying to establish a server connection.
        /// </summary>
        /// <param name="onMessageReceived">
        /// Action, which will be executed every time a message is received.
        /// </param>
        /// <returns>The task of the current execution.</returns>
        Task InitializeAsync(Action<ChatMessage> onMessageReceived);

        /// <summary>
        /// Sends a message to the other chat app clients.
        /// </summary>
        /// <param name="message">The message content to be sent.</param>
        /// <returns>The task of the current execution.</returns>
        Task SendAsync(string message);
    }
}