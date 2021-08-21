namespace ServerBackendApi.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using ServerBackendApi.Models;

    public class MessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            Console.WriteLine($"NewMessage: from={msg.clientuniqueid}, msg={msg.message}, type={msg.type}, date={msg.date}");
            await this.Clients.All.SendAsync("MessageReceived", msg);
        }

    }
}
