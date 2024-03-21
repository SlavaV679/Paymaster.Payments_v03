using Microsoft.Extensions.Configuration;

namespace Paymaster.Payments.Helpers.Config
{
    public static class Config
    {
        public static string PaymentsConnectionString { get; set; }

        public static void LoadAppsettings(ConfigurationManager configuration)
        {
            PaymentsConnectionString = configuration.GetConnectionString("PaymentsConnectionString");
        }
    }
}
