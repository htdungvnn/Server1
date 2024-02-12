namespace API.AppBuilder;

public static class AppBuilder
{
    public static void AddAppBuilder(this IApplicationBuilder app, bool isDev)
    {
        if (isDev)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }
}