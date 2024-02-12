using Duende.IdentityServer;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        // builder.Services.AddIdentityServer()
        //     .AddConfigurationStore(options =>
        //     {
        //         options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
        //             sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
        //     })
        //     .AddOperationalStore(options =>
        //     {
        //         options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
        //             sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
        //     });

        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Assuming you have a connection string in your appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(migrationsAssembly)));
        //builder.Services.InitializeDatabase();
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            // .AddInMemoryIdentityResources(Config.IdentityResources)
            // .AddInMemoryApiScopes(Config.ApiScopes)
            // .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>();

        // builder.Services.AddDbContext<PersistedGrantDbContext>(options =>
        //     options.UseSqlServer(connectionString, 
        //         b => b.MigrationsAssembly(migrationsAssembly)));

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}