using System.Text.Json.Serialization;
using RestaurantAggregator.BL;
using RestaurantAggregator.Infra.Config;
using RestaurantAggregator.Infra.Middlewares;
using RestaurantAggregator.Infra.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterBLServices(builder.Configuration);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
     options.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAggregator API"));
}
app.UseExceptionLogging(LoggerFactory.Create(builder => builder.AddConsole()));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
