using GameServer.API.Helper;
using GameServer.API.Hubs;
using GameServer.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Xml;

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
    .AddSingleton<IHubClientManager, HubClientManager>()
    .AddSingleton<IDatabaseService, DatabaseService>()
    .AddSingleton<IUserService, UserService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();
builder.Services.AddSignalR();


var doc = new XmlDocument();
doc.Load("secret.xml");
var key = doc.DocumentElement?.SelectSingleNode("/secret/key")?.InnerText;


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.SecurityTokenValidators.Clear();
    x.SecurityTokenValidators.Add(new CustomJwtTokenValidator());
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Headers.Authorization.ToString();

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;

        },
        OnChallenge = context =>
        {
            return Task.CompletedTask;

        },
        OnForbidden = context =>
        {
            return Task.CompletedTask;

        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;

        }
    };


    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});


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
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(corsName);
app.MapControllers();
app.MapHub<ConsoleHub>("/consoleHub");
app.Run();
