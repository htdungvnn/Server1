using API.AppBuilder;
using API.AppService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddAppBuilder(app.Environment.IsDevelopment());

app.MapControllers();

app.Run();