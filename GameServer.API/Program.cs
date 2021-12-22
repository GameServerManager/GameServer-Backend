using GameServer.API.Hubs;
using GameServer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string gRPCUrl = "https://localhost:7055";
var corsName = "cors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, b =>
    {
        b.WithOrigins("http://localhost:3000", "https://localhost", "http://mauderer.eu", "https://mauderer.eu")
            .AllowAnyHeader()
            .AllowCredentials();
        ;
    });
});
builder.Services.AddSingleton<ILoggerService>(s => new LoggerService(gRPCUrl))
    .AddSingleton<IServerService>(s => new ServerService(gRPCUrl))
    .AddSingleton<IHealthCheckService>(s => new HealthCheckService(gRPCUrl))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();
builder.Services.AddSignalR();

builder.Logging.ClearProviders()
    .AddConsole()
    .AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(corsName);
app.MapControllers();
app.MapHub<ConsoleHub>("/consoleHub");
app.Run();
