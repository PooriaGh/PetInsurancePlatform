using Microsoft.EntityFrameworkCore;
using PetInsurancePlatform.Insurance.Domain.Models;
using PetInsurancePlatform.SharedKernel.Data;

namespace PetInsurancePlatform.Insurance.Application.Data;

public interface IInsuranceDbContext : IBaseDbContext
{
    DbSet<PetType> PetTypes { get; }

    DbSet<PetTypeDisease> PetTypeDiseases { get; }

    DbSet<Disease> Diseases { get; }

    DbSet<Pet> Pets { get; }

    DbSet<InsurancePlan> InsurancePlans { get; }

    DbSet<InsurancePolicy> InsurancePolicies { get; }

    DbSet<Owner> Owners { get; }

    DbSet<Province> Provinces { get; }

    DbSet<City> Cities { get; }

    DbSet<TermsOfService> TermsOfServices { get; }
}
