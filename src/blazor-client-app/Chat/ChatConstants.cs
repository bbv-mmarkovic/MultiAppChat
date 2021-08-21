namespace blazor_client_app.Chat
{
    public class ChatConstants
    {
        // private const string HubUrl = "https://bbv-multichat-backend.azurewebsites.net/MessageHub";
        public const string HubUrl = "http://localhost:5000/MessageHub";
        public const string ReceivedType = "received";
        public const string SentType = "sent";
    }
}