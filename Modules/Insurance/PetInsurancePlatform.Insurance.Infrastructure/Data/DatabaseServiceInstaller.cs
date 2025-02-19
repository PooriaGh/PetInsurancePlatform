using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PetInsurancePlatform.Insurance.Infrastructure.Data;

internal static class DatabaseInstaller
{
    public static void AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("[ConnectionStrings:Database] has null value.");
        }

        services.AddDbContext<InuranceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"), builder =>
            {
                builder
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, InuranceDbContext.DB_SCHEMA)
                .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
        });

        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
