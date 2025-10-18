using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using RfidBarcode.Application;
using RfidBarcode.Application.Common;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Crm.Services;
using RfidBarcode.Infrastructure;
using Serilog;
using System.Globalization;
using System.Text;

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
builder.Services.AddControllers();
builder.Services.AddQuartz(q =>
{
    var jobDailyReportKey = new JobKey("DailyReportJob");
    q.AddJob<DailyReportJob>(opts => opts.WithIdentity(jobDailyReportKey));
    var cron = builder.Configuration["Cron:DailyReportJob"];
    if (cron != null)
    {
        q.AddTrigger(opts => opts
            .ForJob(jobDailyReportKey)
            .WithIdentity("DailyReportJob-trigger")
            .WithCronSchedule(cron)
        );
    }

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Remove("ASPNETCORE-BROWSER-TOKEN");
        await next();
    });
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
    name: "Reports",
    areaName: "Reports",
    pattern: "Reports/{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});


//app.UseSerilogRequestLogging();

app.Run();
