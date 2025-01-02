using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "person",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

for (int i = 0; i < 10; i++)
{
    await channel.SendAsync();
    await Task.Delay(TimeSpan.FromSeconds(1));
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


internal record Person(Guid Id, string Name, int Age)
{
    internal static Person New() => new(Guid.NewGuid(), "John Doe", 30);
    internal static string Serialize(Person person) => JsonSerializer.Serialize(person);
}

internal static class ChannelExtensions
{
    internal static async Task SendAsync(this IChannel channel)
    {
        var body = Person.Serialize(Person.New());
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "person",
            body: Encoding.UTF8.GetBytes(body));
        
        Console.WriteLine($" [x] Sent {body}");
    }
}