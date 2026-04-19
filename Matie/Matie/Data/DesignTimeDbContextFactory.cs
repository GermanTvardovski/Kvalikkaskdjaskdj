using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Matie.Data;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DBCon>
{
    public DBCon CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("Matie")
            ?? "Host=localhost;Port=5432;Database=Matie;Username=postgres;Password=123";

        var optionsBuilder = new DbContextOptionsBuilder<DBCon>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DBCon(optionsBuilder.Options);
    }
}
