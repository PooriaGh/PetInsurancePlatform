using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace PetInsurancePlatform.IdentityApp.Data;

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

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"), builder =>
            {
                builder
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, ApplicationDbContext.DB_SCHEMA)
                .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            options.UseOpenIddict();
        });

        services.TryAddScoped<ApplicationDbContext>();

        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
