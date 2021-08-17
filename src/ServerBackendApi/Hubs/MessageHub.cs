namespace ServerBackendApi.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using ServerBackendApi.Models;

    public class MessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await this.Clients.All.SendAsync("MessageReceived", msg);
        }

    }
}
