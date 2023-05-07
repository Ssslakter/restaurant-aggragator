using RestaurantAggregator.Auth.Extensions;
using RestaurantAggregator.BL;
using RestaurantAggregator.Infra.Config;
using RestaurantAggregator.MVC.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddUserServices(builder.Configuration);
builder.Services.RegisterBLServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseErrorDisplayPage(LoggerFactory.Create(builder => builder.AddConsole()));
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
