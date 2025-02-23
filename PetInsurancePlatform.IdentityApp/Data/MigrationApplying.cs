using Microsoft.EntityFrameworkCore;

namespace PetInsurancePlatform.IdentityApp.Data;

public static class MigrationApplying
{
    public static void ApplyInsuranceDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
