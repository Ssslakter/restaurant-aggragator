using RestaurantAggregator.Auth.Extensions;
using RestaurantAggregator.Auth.Middlewares;
using RestaurantAggregator.Auth.Swagger;
using RestaurantAggregator.Core.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabases(builder.Configuration);
builder.Services.AddUserServices();
builder.Services.AddConfiguration();

builder.Services.AddControllers();
builder.Services.AddJwtAuthentification();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.ConfigureSwagger();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionLogging(LoggerFactory.Create(builder => builder.AddConsole()));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
