using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TarkovBot.Database.Data;

public class CreateDBContext:IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Default connection string is not configured in appsettings.json for design-time DbContext.");
        }

        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseSqlServer(connectionString);
        return new DatabaseContext(builder.Options);
    }
}