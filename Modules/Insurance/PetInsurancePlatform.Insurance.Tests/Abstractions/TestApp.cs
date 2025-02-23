using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetInsurancePlatform.Insurance.Infrastructure.Data;
using Testcontainers.PostgreSql;

namespace PetInsurancePlatform.Insurance.Tests.Abstractions;

public class TestApp : AppFixture<Program>
{
    private PostgreSqlContainer _dbContainer = null!;

    protected override ValueTask PreSetupAsync()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("insurance")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        return base.PreSetupAsync();
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.RemoveAll(typeof(DbContextOptions<InsuranceDbContext>));

        s.AddDbContext<InsuranceDbContext>(options =>
        {
            options
            .UseNpgsql(_dbContainer.GetConnectionString())
            .UseSnakeCaseNamingConvention();
        });
    }

    protected override async ValueTask SetupAsync()
    {
        await _dbContainer.StartAsync();
    }

    protected override async ValueTask TearDownAsync()
    {
        await _dbContainer.StopAsync();
    }
}
