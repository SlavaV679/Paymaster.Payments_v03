using NLog.Extensions.Logging;
using NLog;
using Paymaster.Payments;
using Paymaster.Payments.Logic.Repository;
using Paymaster.Payments.Logic;
using Paymaster.Payments.Logic.Interfaces;
using Paymaster.Payments.Data.Payments;
using Paymaster.Payments.Domain.Config;

var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<PaymentConsumer>();

builder.Services.AddSingleton<PaymentsContext>();
builder.Services.AddSingleton<Configuration>();
builder.Services.AddTransient<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddTransient<IPaymentsLogic, PaymentsLogic>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    loggingBuilder.AddNLog();
});

var host = builder.Build();
host.Run();
