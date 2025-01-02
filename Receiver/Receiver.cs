using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "person",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (_, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("person", autoAck: true, consumer: consumer);
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();