using RestaurantAggregator.Auth.Extensions;
using RestaurantAggregator.Auth.Middlewares;
using RestaurantAggregator.Auth.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.MigrateDatabase();

if (builder.Environment.IsDevelopment())
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.ConfigureSwagger();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.UseExceptionLogging(LoggerFactory.Create(builder => builder.AddConsole()));
app.UseHttpsRedirection();

app.UseTokenValidation();
app.UseAuthorization();

app.MapControllers();

app.Run();
