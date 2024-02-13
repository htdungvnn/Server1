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
        app.UseCors("AllowAll"); // Use the CORS policy
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        // app.Use(async (context, next) =>
        // {
        //     await next();
        //
        //     if (context.Response.StatusCode == 401)
        //     {
        //         // Log additional details here
        //         Console.WriteLine("Unauthorized request to " + context.Request.Path);
        //     }
        // });
    }
}