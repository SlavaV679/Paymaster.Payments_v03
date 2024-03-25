using Microsoft.Extensions.Configuration;

namespace Paymaster.Payments.Domain.Config
{
    public class Configuration // TODO VDV нужно ли здесь наследоваться от интерфейса? IConfiguration
    {
        private readonly IConfiguration configuration;
        public Configuration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string PaymentsConnectionString => configuration.GetConnectionString("PaymentsConnectionString");

        private string _rabbitMQServerHostNames => configuration["RabbitMQServerHostNames"] ?? "localhost";
        /// <summary>
        /// List endpoints for cluster RabbitMQ
        /// </summary>
        public List<string> RabbitMQServerHostNames => _rabbitMQServerHostNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim().ToLower()).ToList();

        public string RabbitMQUserName => configuration["RabbitMQUserName"] ?? "guest";

        public string RabbitMQPassword => configuration["RabbitMQPassword"] ?? "guest";

        public string RabbitMQVirtualHost => configuration["RabbitMQVirtualHost"] ?? "/";

        public string RabbitMQQueueName => configuration["RabbitMQQueueName"] ?? "paymaster";
    }
}
