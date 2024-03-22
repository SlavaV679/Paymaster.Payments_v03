using Paymaster.Payments.Data.Payments;
using Paymaster.Payments.FakePublisher.Helpers;
using Paymaster.Payments.Helpers.Config;
using Paymaster.Payments.Helpers.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Config.LoadAppsettings(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", async (HttpRequest request, ILoggerFactory loggerFactory) =>
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

        var message = Newtonsoft.Json.JsonConvert.SerializeObject(paymentRequest);
        RabbitMqPublisher.SendMessage(message);
    }
    catch (Exception ex)
    {

        throw;
    }
});

app.Run();
