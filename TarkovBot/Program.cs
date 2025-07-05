using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using TarkovBot.Database.Controllers;
using TarkovBot.Database.Data;
using TarkovBot.Database.Services;
using TarkovBot.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddDiscordGateway(options =>
    {
        options.Intents = GatewayIntents.All;
        options.Token = builder.Configuration["Discord:Token"];
    })
    .AddApplicationCommands()
    .AddDbContext<DatabaseContext>(options =>
    {
        string connectionString = builder.Configuration.GetConnectionString("Default");
        options.UseSqlServer(connectionString);
    })
    .AddSingleton(sp => sp.GetRequiredService<IOptions<TwitterAccount>>().Value)
    .AddSingleton<TeamKillsController>()
    .AddHostedService<TwitterMonitoringService>()
    .AddScoped<ITeamKillsService, TeamKillsService>()
    .AddSingleton<TwitterRetrieverController>()
    .AddSingleton<TwitterMonitoringService>()
    .AddScoped<ITwitterRetrieverService, TwitterRetrieverService>()
    .Configure<TwitterAccount>(builder.Configuration.GetSection("Twitter"))
    .AddHttpClient<TwitterHTTPClient>("TwitterV2Client", client =>
    {
        client.BaseAddress = new Uri("https://api.twitter.com/");
        var twitterLogin = builder.Configuration.GetSection("Twitter").Get<TwitterAccount>();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", twitterLogin.BearerToken);
    });

var host = builder.Build();

host.AddModules(typeof(Program).Assembly);
host.UseGatewayHandlers();

await host.RunAsync();