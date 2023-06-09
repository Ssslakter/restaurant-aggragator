using RestaurantAggregator.Auth.Extensions;
using RestaurantAggregator.Infra.Middlewares;
using RestaurantAggregator.Infra.Swagger;
using System.Text.Json.Serialization;
using System.Reflection;
using RestaurantAggregator.Infra.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserServices(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAdmins(builder.Configuration);
    builder.Services.ConfigureSwagger(Assembly.GetExecutingAssembly());
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
     options.SwaggerEndpoint("v1/swagger.yaml", "RestaurantAggregator API"));
}

app.UseExceptionLogging(LoggerFactory.Create(builder => builder.AddConsole()));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
