using Paymaster.Payments.Helpers.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Paymaster.Payments
{
    public class PaymentConsumer : BackgroundService
    {
        private readonly ILogger<PaymentConsumer> _logger;
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public PaymentConsumer(ILogger<PaymentConsumer> logger, IConfiguration config)
        {
            _logger = logger;

            _config = config;
            var configManager = new ConfigurationManager();
            Config.LoadAppsettings(configManager);

            var factory = new ConnectionFactory
            {
                UserName = Config.RabbitMQUserName,
                Password = Config.RabbitMQPassword,
                VirtualHost = Config.RabbitMQVirtualHost
            };
            _connection = factory.CreateConnection(Config.RabbitMQServerHostNames);
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: Config.RabbitMQQueueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"ExecuteAsync of PaymentConsumer started");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // work with received message

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(Config.RabbitMQQueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
