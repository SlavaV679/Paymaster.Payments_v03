using Paymaster.Payments.FakePublisher.Helpers;
using Paymaster.Payments.Helpers.Config;

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
        RabbitMqPublisher.Sending();
    }
    catch (Exception ex)
    {

        throw;
    }
});

app.Run();
