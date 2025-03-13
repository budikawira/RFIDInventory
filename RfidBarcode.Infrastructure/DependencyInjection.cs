using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Domain.Entities.Identities;
using RfidBarcode.Infrastructure.Services;
using RfidBarcode.Infrastructure.Services.Identities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Quartz;
using RfidBarcode.Infrastructure.Services.Jobs;
using Microsoft.AspNetCore.Localization;

namespace RfidBarcode.Infrastructure
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var name = typeof(ApplicationDbContext).Assembly.FullName;
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //SQL Server
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString,
            //    b => b.MigrationsAssembly(name)
            //));

            //Postgres
            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseNpgsql(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            //MySQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("DefaultConnection")!,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                    options => {
                        options.SignIn.RequireConfirmedAccount = false;
                    })
                    .AddDefaultTokenProviders()
                    .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
                        AdditionalUserClaimsPrincipalFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.AddAuthentication(x =>
            {
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //x.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Custom/CustomLogout";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserMgmt", policy => policy.RequireClaim("UserMgmt"));
            });

            services.AddTransient<IUserResolverService, UserResolverService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHttpClient();

            services.AddQuartz(q =>
            {
                //Sync to Tracking App
                //var job1 = new JobKey("SyncTrackingItemJob");
                //q.AddJob<SyncTrackingItemJob>(opts => opts.WithIdentity(job1));
                //var cron = configuration["Cron:SyncTrackingItemJob"];
                //if (cron != null)
                //{
                //    q.AddTrigger(opts => opts
                //        .ForJob(job1)
                //        .WithIdentity("SyncTrackingItemJob-trigger")
                //        .WithCronSchedule(cron)
                //    );
                //}
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
