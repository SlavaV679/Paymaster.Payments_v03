using Microsoft.Extensions.Configuration;

namespace Paymaster.Payments.Domain.Config
{
    public static class Config
    {
        public static string PaymentsConnectionString { get; set; }
        /// <summary>
        /// List endpoints for cluster RabbitMQ
        /// </summary>
        public static List<string> RabbitMQServerHostNames { get; set; }

        private static string _rabbitMQServerHostNames { get; set; }

        public static string RabbitMQUserName { get; set; }

        public static string RabbitMQPassword { get; set; }

        public static string RabbitMQVirtualHost { get; set; }

        public static string RabbitMQQueueName { get; set; }

        public static void LoadAppsettings(ConfigurationManager configuration)
        {
            PaymentsConnectionString = configuration.GetConnectionString("PaymentsConnectionString");

            _rabbitMQServerHostNames = configuration["RabbitMQServerHostNames"] ?? "localhost";
            RabbitMQServerHostNames = _rabbitMQServerHostNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim().ToLower()).ToList();

            RabbitMQUserName = configuration["RabbitMQUserName"] ?? "guest";

            RabbitMQPassword = configuration["RabbitMQPassword"] ?? "guest";

            RabbitMQVirtualHost = configuration["RabbitMQVirtualHost"] ?? "/";

            RabbitMQQueueName = configuration["RabbitMQQueueName"] ?? "paymaster";
        }
    }
}
