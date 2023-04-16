using RestaurantAggregator.Auth.Extensions;
using RestaurantAggregator.Infra.Middlewares;
using RestaurantAggregator.Infra.Swagger;
using RestaurantAggregator.Infra.Config;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDbContext(builder.Configuration);
builder.Services.AddUserServices();

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddJwtAuthentication(builder.Configuration);
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
