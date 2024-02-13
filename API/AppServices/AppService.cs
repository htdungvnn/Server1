using System.Data;
using BLL.UnitOfWork;
using DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.AppService;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        // Add services to the container.
        services.AddTransient<IDbConnection>(sp => new SqlConnection(configuration.GetConnectionString("MoviesDb")));
        services.AddDbContextPool<MoviesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MoviesDb")));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConn");
            options.InstanceName = "MovieCache";
        });
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddControllers();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                //options.RequireHttpsMetadata = false;
                options.Audience = "movieAPI";
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Log the exception using the logger
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(context.Exception, "Authentication failed.");

                        // Customize the response sent back to the client on authentication failure
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(new
                        {
                            error = "Authentication failed",
                            details = context.Exception.Message
                        }.ToString()); // Make sure to serialize this object properly
                    }
                };
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
