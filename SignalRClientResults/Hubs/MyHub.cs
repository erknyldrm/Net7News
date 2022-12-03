using Microsoft.AspNetCore.SignalR;

namespace SignalRClientResults.Hubs
{
    public interface IMyHub
    {
        Task<string> ReceiveMessage(string message);
    }
    public class MyHub : Hub
    {
        public async Task TestBeforeInvokeResults(string message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
        }

        public async Task TestAfterInvokeResults(string message, string connectionId)
        {
            string logMessage = await Clients.Client(connectionId).InvokeAsync<string>("receiveMessage", message, new());

            Console.WriteLine($"ConnectionId:{connectionId} - message: {logMessage}");
        }

        public async Task LogAsync(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }

    public class MyStronglyHub : Hub<IMyHub>
    {
        public async Task InvokeResults(string message, string connectionId)
        {
            string logMessage = await Clients.Client(connectionId).ReceiveMessage(message);
            Console.WriteLine($"ConnectionId:{connectionId} - message: {logMessage}");
        }

        public async Task LogAsync(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
