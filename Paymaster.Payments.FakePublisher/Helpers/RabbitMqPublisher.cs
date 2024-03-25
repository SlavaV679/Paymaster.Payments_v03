using Paymaster.Payments.Domain.Config;
using RabbitMQ.Client;
using System.Text;

namespace Paymaster.Payments.FakePublisher.Helpers
{
    public class RabbitMqPublisher
    {
        private readonly Configuration _config;

        public RabbitMqPublisher(Configuration configuration)
        {
            _config= configuration;
        }
        
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory
            {
                UserName = _config.RabbitMQUserName,
                Password = _config.RabbitMQPassword,
                VirtualHost = _config.RabbitMQVirtualHost
            };
            var connection = factory.CreateConnection(_config.RabbitMQServerHostNames);
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _config.RabbitMQQueueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _config.RabbitMQQueueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
