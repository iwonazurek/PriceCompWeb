using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Data;
using PriceCompWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PriceCompDbContext>(options =>
{
    var provider = builder.Configuration["DatabaseProvider"];
    if (string.Equals(provider, "SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
    }
    else
    {
        var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDirectory);
        var databasePath = Path.Combine(dataDirectory, "pricecomp.db");
        options.UseSqlite($"Data Source={databasePath}");
    }
});
builder.Services.AddScoped<ShoppingBasketService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.Name = "PriceCompAdmin";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PriceCompDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
