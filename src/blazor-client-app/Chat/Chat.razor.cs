using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blazor_client_app.Chat
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.SignalR.Client;

    public partial class Chat
    {
        // This code was created with the help of the microsoft MSDN documentation:
        // https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-tutorial-build-blazor-server-chat-app

        private readonly string clientUniqueId = Guid.NewGuid().ToString();
        private readonly List<ChatMessage> messages = new List<ChatMessage>();

        private string newMessage;
        private string status;
        private string hubUrl;

        private HubConnection hubConnection;

        public async void EnterAsnyc(KeyboardEventArgs e)
        {
            Console.WriteLine("Enter: " + e.Code);
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                Console.WriteLine("Sending... " + (this.newMessage ?? "<null>"));
                await this.SendAsync(this.newMessage);
            }
        }

        public async Task StartChatAsync()
        {
            try
            {
                status = "...";

                await Task.Delay(3);

                messages.Clear();

                // Create the chat client
                //string baseUrl = navigationManager.BaseUri;

                hubUrl = ChatConstants.HubUrl; // baseUrl.TrimEnd('/') + HubUrl;

                hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .Build();

                hubConnection.On<ChatMessage>("MessageReceived", BroadcastMessage);

                await hubConnection.StartAsync();

                status = string.Empty;
            }
            catch (Exception e)
            {
                newMessage = $"ERROR: Failed to start chat client: {e.Message}";

                status = e.ToString();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await this.StartChatAsync();
        }

        private void BroadcastMessage(ChatMessage chatMessage)
        {
            bool isMine = chatMessage.ClientUniqueId.Equals(clientUniqueId, StringComparison.OrdinalIgnoreCase);
            string type = isMine ? ChatConstants.SentType : ChatConstants.ReceivedType;

            messages.Add(new ChatMessage(this.clientUniqueId, chatMessage.Message, type));

            // Inform blazor the cUI needs updating
            this.StateHasChanged();
        }

        private async Task SendAsync(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var newMessage = new ChatMessage(clientUniqueId, message, ChatConstants.SentType);

                await hubConnection.SendAsync("NewMessage", newMessage);

                this.newMessage = string.Empty;
            }
        }

        private async Task DisconnectAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.StopAsync();
                await hubConnection.DisposeAsync();

                hubConnection = null;
            }
        }
    }
}
