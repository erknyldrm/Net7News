
using Microsoft.AspNetCore.SignalR.Client;

HubConnection connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7195/myhub")
    .Build();

await connection.StartAsync();

Console.WriteLine($"ConnectionID: {connection.ConnectionId}");

//#before invokeResults
//connection.On<string>("receiveMessage", async message =>
//{
//    Console.WriteLine($"Message: {message}");

//    await connection.InvokeAsync("LogAsync", "Ok");
//});

//#after invokeResults
connection.On<string, string>("receiveMessage", async message =>
{
    Console.WriteLine($"Message: {message}");

    //await connection.InvokeAsync("LogAsync", "Ok");
    return "Ok- Client Results";
});


while (true)
{
   // await Message.TestBeforeInvokeResults(connection);
   
    await Message.TestAfterInvokeResults(connection);

}
 
static class Message
{
    public static async Task TestBeforeInvokeResults(HubConnection connection)
    {
        if (Console.ReadKey().Key == ConsoleKey.M)
        {
            Console.WriteLine();
            Console.WriteLine("Please write your message.");
            Console.Write("Message: ");
            string message = Console.ReadLine();
            Console.WriteLine();
            await connection.InvokeAsync("TestBeforeInvokeResults", message);
        }
    }

    public static async Task TestAfterInvokeResults(HubConnection connection)
    {
        if (Console.ReadKey().Key == ConsoleKey.K)
        {
            Console.WriteLine();
            Console.WriteLine("Please write connectionId.");
            Console.Write("connectionId: ");
            string connectionId = Console.ReadLine();
            Console.WriteLine("Please write your message.");
            Console.Write("Message: ");
            string message = Console.ReadLine();
            Console.WriteLine();
            await connection.InvokeAsync("TestAfterInvokeResults", message, connectionId);
        }
    }
}
