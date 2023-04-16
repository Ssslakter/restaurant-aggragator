using System.Text.Json.Serialization;
using RestaurantAggregator.BL;
using RestaurantAggregator.Infra.Config;
using RestaurantAggregator.Infra.Middlewares;
using RestaurantAggregator.Infra.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.RegisterBLServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
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
