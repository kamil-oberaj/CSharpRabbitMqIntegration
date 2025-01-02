Simple C# RabbitMQ integration.
How to run:
1. Please ensure, that you've installed and running docker and run the following command: `docker run -d --hostname my-rabbit --name local-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:4-management`
2. Open the solution in two separate terminals and cd into the Sender and Reciever projects in both terminals
3. Run `dotnet run` in Receiver project, wait to start
4. Run `dotnet run` in Sender project and observe how the Receiver reacts
