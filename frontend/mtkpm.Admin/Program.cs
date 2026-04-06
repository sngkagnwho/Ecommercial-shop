using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using mtkpm.Admin.Configuration;
using mtkpm.Admin.Services;
using mtkpm.Admin.Infrastructure.Http;
using mtkpm.Admin.Infrastructure.Caching;
using mtkpm.Admin.Features.Dashboard.Services;
using mtkpm.Admin.Features.Analytics.Services;
using mtkpm.Admin.Features.Reports.Services;

namespace mtkpm.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/admin-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add memory cache
            builder.Services.AddMemoryCache();

            // Add services
            builder.Services.AddControllersWithViews();
            
            // Configure Razor View Engine to support feature-based view locations
            builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/{1}/Views/Shared/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Shared/Views/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{1}/Views/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Features/Shared/Views/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Features/Shared/Views/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });
            
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddHttpContextAccessor();

            // Configure API Settings from appsettings.json
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>() ?? new ApiSettings();
            builder.Services.AddSingleton(apiSettings);

            // Configure HTTP Client Settings
            var httpClientConfig = new mtkpm.Admin.Infrastructure.Http.HttpClientConfiguration
            {
                BaseUrl = apiSettings.BaseUrl,
                TimeoutSeconds = apiSettings.RequestTimeoutSeconds,
                MaxRetries = 3,
                EnableLogging = true
            };
            builder.Services.AddSingleton(httpClientConfig);

            // Register Infrastructure Services
            builder.Services.AddScoped<ICacheService, MemoryCacheService>();
            builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            // Add HttpClient for backward compatibility
            builder.Services.AddHttpClient<IApiService, ApiService>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(apiSettings.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(apiSettings.RequestTimeoutSeconds);
                });

            // Register BackendApiClient for direct API communication
            builder.Services.AddHttpClient<BackendApiClient>()
                .ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(apiSettings.RequestTimeoutSeconds);
                });

            // Register Core Application Services
            builder.Services.AddScoped<ITokenManager, TokenManager>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAdminDiscountService, AdminDiscountService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAdminPaymentService, AdminPaymentService>();
            builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();
            builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();

            // Register Orders Feature Services
            builder.Services.AddScoped<mtkpm.Admin.Features.Orders.Services.IUserAddressService, mtkpm.Admin.Features.Orders.Services.UserAddressService>();

            // Register Feature Services
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IReportService, ReportService>();

            // Configure JWT Settings (for validation if needed)
            var jwtSettings = new JwtSettings();
            builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            // Add Cookie Authentication for session management
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login"; // Works with feature-based routing
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/Unauthorized";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
            });

            // Add session for storing tokens
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
