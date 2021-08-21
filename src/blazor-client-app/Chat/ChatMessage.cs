namespace blazor_client_app.Chat
{
    using System;

    public class ChatMessage
    {
        public ChatMessage(string clientUniqueId, string message, string type)
        {
            this.ClientUniqueId = clientUniqueId;
            this.Message = message;
            this.Type = type;
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// Do not change the property names, as it is defined by contract.
        /// </summary>
        public string ClientUniqueId { get; }

        /// <summary>
        /// Do not change the property names, as it is defined by contract.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Do not change the property names, as it is defined by contract.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Do not change the property names, as it is defined by contract.
        /// </summary>
        public string Type { get; }

        public bool IsReceived => this.Type == ChatConstants.ReceivedType;

        public bool IsSent => this.Type == ChatConstants.SentType;

        public string DateAsText => this.Date.ToString("dd. MMM, HH:mm:sss");
    }
}