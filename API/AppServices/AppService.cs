using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace API.AppService;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services,IConfiguration configuration)
    {
        // Add services to the container.
        services.AddDbContext<MoviesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MoviesDb")));
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}