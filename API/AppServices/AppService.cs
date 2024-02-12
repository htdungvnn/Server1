using System.Data;
using BLL.UnitOfWork;
using DAL.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.AppService;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}