using RfidBarcode.Application;
using RfidBarcode.Application.Common;
using RfidBarcode.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Crm.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages().AddNewtonsoftJson();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAntiforgery();

builder.Services.AddSingleton<IMqttClientService, MqttClientService>();
builder.Services.AddSingleton<IHostedService, IMqttClientService>(serviceProvider =>
{
    return serviceProvider.GetService<IMqttClientService>()!;
});

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.MapAreaControllerRoute(
    name: "Setup",
    areaName: "Setup",
    pattern: "Setup/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Settings",
    areaName: "Settings",
    pattern: "Settings/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "ValetSettings",
    areaName: "ValetSettings",
    pattern: "ValetSettings/{controller=Home}/{action=Index}/{id?}");

var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});


app.UseSerilogRequestLogging();

app.Run();
