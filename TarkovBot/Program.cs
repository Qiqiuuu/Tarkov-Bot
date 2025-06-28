using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using TarkovBot.Database.Controllers;
using TarkovBot.Database.Data;
using TarkovBot.Database.Services;

string rootPath = Path.Combine(AppContext.BaseDirectory, "../../../");
string dotenvPath = Path.Combine(rootPath, ".env");
DotNetEnv.Env.Load(dotenvPath); 


var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddDiscordGateway(options =>
    {
        options.Intents = GatewayIntents.All;
        options.Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
    })
    .AddApplicationCommands()
    .AddDbContext<DatabaseContext>(options =>
    {
        string connectionString = builder.Configuration.GetConnectionString("Default");
        options.UseSqlServer(connectionString); 
    })
    .AddSingleton<TeamKillsController>()
    .AddScoped<ITeamKills,TeamKills>();

var host = builder.Build();

host.AddModules(typeof(Program).Assembly);
host.UseGatewayHandlers();

await host.RunAsync();