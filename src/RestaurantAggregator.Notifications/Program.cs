using RestaurantAggregator.Infra;
using RestaurantAggregator.Infra.Auth;
using RestaurantAggregator.Notifications.Services;
using SignalRAuthenticationSample.Hubs;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddLogging(opts =>
    {
        opts.AddConsole();
        opts.AddDebug();
    });
}
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHostedService<NotificationReciever>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationsHub>("/notifications");

app.Run();
