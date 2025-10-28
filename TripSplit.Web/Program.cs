using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using TripSplit.Application;
using TripSplit.Application.Abstractions;
using TripSplit.Infrastructure;
using TripSplit.Infrastructure.Identity;
using TripSplit.Infrastructure.Persistence;
using TripSplit.Infrastructure.Seed;
using TripSplit.Web.Mapping;
using TripSplit.Web.Services;
using Microsoft.EntityFrameworkCore;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
        var app = builder.Build();
        ConfigurePipeline(app);
        MapEndpoints(app);
        await SeedAsync(app);
        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        // Infra + App
        services.AddInfrastructure(config);
        services.AddApplication();
        services.AddAutoMapper(cfg => cfg.AddProfile(new WebMappingProfile()));

        // Identity
        services.AddIdentity<AppUser, AppRole>(opt =>
        {
            opt.Password.RequiredLength = 8;
            opt.Password.RequireDigit = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireNonAlphanumeric = false;

            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedAccount = !env.IsDevelopment();
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // COOKIE
        services.ConfigureApplicationCookie(o =>
        {
            o.Cookie.Name = "TripSplit.Auth";
            o.Cookie.HttpOnly = true;
            o.Cookie.SameSite = SameSiteMode.Lax;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            o.SlidingExpiration = true;

            o.LoginPath = "/Identity/Account/Login";
            o.LogoutPath = "/Identity/Account/Logout";
            o.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });

        services.ConfigureExternalCookie(o =>
        {
            o.Cookie.Name = "TripSplit.External";
            o.Cookie.HttpOnly = true;
            o.Cookie.SameSite = SameSiteMode.Lax;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        // Provider
        var auth = config.GetSection("Authentication");

        var googleId = auth["Google:ClientId"];
        var googleSecret = auth["Google:ClientSecret"];
        if (!string.IsNullOrWhiteSpace(googleId) && !string.IsNullOrWhiteSpace(googleSecret))
        {
            services.AddAuthentication().AddGoogle(o =>
            {
                o.ClientId = googleId!;
                o.ClientSecret = googleSecret!;
                o.SaveTokens = true;

                o.CorrelationCookie.SameSite = SameSiteMode.Lax;
                o.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }

        var msId = auth["Microsoft:ClientId"];
        var msSecret = auth["Microsoft:ClientSecret"];
        if (!string.IsNullOrWhiteSpace(msId) && !string.IsNullOrWhiteSpace(msSecret))
        {
            services.AddAuthentication().AddMicrosoftAccount(o =>
            {
                o.ClientId = msId!;
                o.ClientSecret = msSecret!;
                o.SaveTokens = true;

                o.CorrelationCookie.SameSite = SameSiteMode.Lax;
                o.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }

        var fbId = auth["Facebook:AppId"];
        var fbSecret = auth["Facebook:AppSecret"];

        if (!string.IsNullOrWhiteSpace(fbId) && !string.IsNullOrWhiteSpace(fbSecret))
        {
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = fbId!;
                options.AppSecret = fbSecret!;
                options.SaveTokens = true;

                options.CallbackPath = "/signin-facebook";

                options.Scope.Clear();
                options.Scope.Add("public_profile");
                options.Scope.Add("email");

                options.Fields.Add("id");
                options.Fields.Add("name");
                options.Fields.Add("first_name");
                options.Fields.Add("last_name");
                options.Fields.Add("email");

                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.GivenName, "first_name");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Surname, "last_name");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");

                options.AccessDeniedPath = "/Identity/Account/Login";
                options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                {
                    OnRemoteFailure = ctx =>
                    {
                        ctx.Response.Redirect("/Identity/Account/Login?error=fb_remote_failure");
                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }


        // Current user + e-mail
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IEmailSender, TripSplit.Web.Services.NoOpEmailSender>();

        // MVC + Razor Pages
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddControllersWithViews();
        services.AddRazorPages();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseMigrationsEndPoint();
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = r => r.Tags.Contains("ready") });
    }

    private static void MapEndpoints(WebApplication app)
    {
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Trips}/{action=Index}/{id?}");

        app.MapRazorPages();
    }

    private static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        await DbSeeder.SeedAsync(users, roles);
    }
}
