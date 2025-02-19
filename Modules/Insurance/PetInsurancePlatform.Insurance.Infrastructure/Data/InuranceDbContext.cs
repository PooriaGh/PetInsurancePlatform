using Microsoft.EntityFrameworkCore;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Domain.Models;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;
using PetInsurancePlatform.SharedKernel.Data;
using System.Reflection;

namespace PetInsurancePlatform.Insurance.Infrastructure.Data;

internal sealed class InuranceDbContext : BaseDbContext, IInsuranceDbContext
{
    public const string DB_SCHEMA = "insurance";

    public DbSet<PetType> PetTypes => Set<PetType>();

    public DbSet<PetTypeDisease> PetTypeDiseases => Set<PetTypeDisease>();

    public DbSet<Disease> Diseases => Set<Disease>();

    public DbSet<Pet> Pets => Set<Pet>();

    public DbSet<InsurancePlan> InsurancePlans => Set<InsurancePlan>();

    public DbSet<InsuranceCoverage> InsuranceCoverages => Set<InsuranceCoverage>();

    public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();

    public DbSet<Owner> Owners => Set<Owner>();

    public DbSet<Province> Provinces => Set<Province>();

    public DbSet<City> Cities => Set<City>();

    public DbSet<TermsOfService> TermsOfServices => Set<TermsOfService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(DB_SCHEMA);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
