using Paymaster.Payments.Domain.Config;
using RabbitMQ.Client;
using System.Text;

namespace Paymaster.Payments.FakePublisher.Helpers
{
    public static class RabbitMqPublisher
    {
        public static void SendMessage(string message)
        {
            var factory = new ConnectionFactory
            {
                UserName = Config.RabbitMQUserName,
                Password = Config.RabbitMQPassword,
                VirtualHost = Config.RabbitMQVirtualHost
            };
            var connection = factory.CreateConnection(Config.RabbitMQServerHostNames);
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: Config.RabbitMQQueueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: Config.RabbitMQQueueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
