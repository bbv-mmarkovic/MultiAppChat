namespace blazor_client_app.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;

    public partial class Chat
    {
        [Inject]
        private IChatService chatService { get; set; }

        private readonly List<ChatMessage> messages = new List<ChatMessage>();

        private string newMessage;
        private string status;

        public async void EnterAsnyc(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await this.SendAsync(this.newMessage);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.status = "...";
                await this.chatService.InitializeAsync((msg) => BroadcastMessage(msg));
                this.status = string.Empty;
            }
            catch (Exception e)
            {
                status = "ERROR - " + e.ToString();
            }
        }

        private void BroadcastMessage(ChatMessage chatMessage)
        {
            bool isMine = chatMessage.ClientUniqueId.Equals(this.chatService.ClientId, StringComparison.OrdinalIgnoreCase);
            string type = isMine ? ChatConstants.SentType : ChatConstants.ReceivedType;

            var msg = new ChatMessage(this.chatService.ClientId, chatMessage.Message, type);
            messages.Add(msg);

            // Inform blazor the cUI needs updating
            this.StateHasChanged();
        }

        private async Task SendAsync(string message)
        {
            await this.chatService.SendAsync(message);
            this.newMessage = string.Empty;
        }
    }
}
