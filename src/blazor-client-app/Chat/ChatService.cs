namespace blazor_client_app.Chat
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR.Client;

    public class ChatService : IAsyncDisposable
    {
        private static readonly string ClientGuid = Guid.NewGuid().ToString("N");


        private bool initialized;

        private HubConnection hubConnection;

        public string ClientId => ClientGuid;

        public async Task InitializeAsync(Action<ChatMessage> OnMessageReceived)
        {
            if (this.initialized)
            {
                return;
            }

            var hubUrl = ChatConstants.HubUrl; // baseUrl.TrimEnd('/') + HubUrl;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            hubConnection.On<ChatMessage>("MessageReceived", OnMessageReceived);

            await this.hubConnection.StartAsync();

            this.initialized = true;
        }

        public async Task SendAsync(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var newMessage = new ChatMessage(this.ClientId, message, ChatConstants.SentType);

                await hubConnection.SendAsync("NewMessage", newMessage);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (this.hubConnection != null)
            {
                await this.hubConnection.StopAsync();
                await this.hubConnection.DisposeAsync();
                this.hubConnection = null;
            }
        }
    }
}