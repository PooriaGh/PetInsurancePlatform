using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Infrastructure.Data;
using PetInsurancePlatform.Insurance.Infrastructure.ModuleInstaller;
using Testcontainers.PostgreSql;

namespace PetInsurancePlatform.Insurance.Tests;

public class Sut : AppFixture<Program>
{
    private PostgreSqlContainer _dbContainer = null!;

    protected override async ValueTask PreSetupAsync()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("insurance")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _dbContainer.StartAsync();
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        var descriptor = s.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<InsuranceDbContext>));

        if (descriptor is not null)
        {
            s.Remove(descriptor);
        }

        s.AddDbContext<InsuranceDbContext>(options =>
        {
            options
            .UseNpgsql(_dbContainer.GetConnectionString(), builder =>
            {
                builder
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, InsuranceDbContext.DB_SCHEMA)
                .MigrationsAssembly(InsuranceModule.Assembly.FullName);
            })
            .UseSnakeCaseNamingConvention();
        });

        s.AddScoped<IInsuranceDbContext>(sp => sp.GetRequiredService<InsuranceDbContext>());
    }

    protected override async ValueTask SetupAsync()
    {
        using var scope = Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    protected override async ValueTask TearDownAsync()
    {
        await _dbContainer.StopAsync();
    }
}
