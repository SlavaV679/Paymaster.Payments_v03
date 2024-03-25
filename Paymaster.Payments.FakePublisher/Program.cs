using Paymaster.Payments.Domain.Config;
using Paymaster.Payments.Domain.Models;
using Paymaster.Payments.FakePublisher.Helpers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Configuration>();
builder.Services.AddTransient<RabbitMqPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", async (HttpRequest request, ILoggerFactory loggerFactory, RabbitMqPublisher rabbitMqPublisher) =>
{
    try
    {        
        var paymentRequest = new PaymentRequest()
        {
            ActId = 3.ToString(),
            Currency = "MDL",
            ExtDocId = 178644133,//(1000 + 3).ToString(),
            PaymentDate = DateTime.Now,
            PaymentTypeId = 5.ToString(),
            Purpose = "note",
            Summa = 0,
        };

        var message = JsonSerializer.Serialize(paymentRequest);
        rabbitMqPublisher.SendMessage(message);
    }
    catch (Exception ex)
    {

        throw;
    }
});

app.Run();
