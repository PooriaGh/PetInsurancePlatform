using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetInsurancePlatform.Insurance.Infrastructure.Data;

namespace PetInsurancePlatform.Insurance.Infrastructure.Extensions;

public static class MigrationApplying
{
    public static void ApplyInsuranceDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

        dbContext.Database.Migrate();
    }
}
