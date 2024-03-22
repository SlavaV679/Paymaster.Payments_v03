using Paymaster.Payments.Domain.Config;
using Paymaster.Payments.Logic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Paymaster.Payments
{
    public class PaymentConsumer : BackgroundService // : IConsumer
    {
        private readonly ILogger<PaymentConsumer> _logger;
        private readonly IPaymentsLogic _paymentsLogic;
        private readonly IPaymentsRepository _paymentsRepository;
        private IConnection _connection;
        private IModel _channel;

        public PaymentConsumer(ILogger<PaymentConsumer> logger, 
                            IPaymentsLogic paymentsLogic,
                            IPaymentsRepository paymentsRepository)
        {
            _logger = logger;
            _paymentsLogic = paymentsLogic;
            _paymentsRepository = paymentsRepository;

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
                var requestMessage = Encoding.UTF8.GetString(ea.Body.ToArray());

                // work with received message
                var v2 = _paymentsLogic.MakePayment(requestMessage);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(Config.RabbitMQQueueName, false, consumer);

            return Task.CompletedTask;
        }   
        
        //public override string HandleRequestFromQueue(MessageFromRabbit message)
        //{
        //    // валидация сообщения
        //    if(message == null)
        //    {
        //        //logger;
        //    }

        //    paymentsLogic.MakePayment();
        //    // handle request logic
        //    // здесь может быть использование класса логики
        //    return base.ToString();
        //}
    }
}
