using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetInsurancePlatform.Insurance.Application.Data;
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
            return;
        }

        services.AddDbContext<InsuranceDbContext>(options =>
        {
            options
            .UseNpgsql(configuration.GetConnectionString("Database"), builder =>
            {
                builder
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, InsuranceDbContext.DB_SCHEMA)
                .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            })
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IInsuranceDbContext>(sp => sp.GetRequiredService<InsuranceDbContext>());

        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
