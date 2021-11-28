using GameServer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string gRPCUrl = "https://localhost:7055";
builder.Services.AddSingleton<ILoggerService>( s => new LoggerService(gRPCUrl));
builder.Services.AddSingleton<IServerService>( s => new ServerService(gRPCUrl));
builder.Services.AddSingleton<IHealthCheckService>( s => new HealthCheckService(gRPCUrl));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
