using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class InsurancePlanConfiguration : IEntityTypeConfiguration<InsurancePlan>
{
    public void Configure(EntityTypeBuilder<InsurancePlan> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();

        builder
           .ComplexProperty(p => p.Price);

        builder
            .ComplexProperty(p => p.Coverages);

        builder
            .HasMany(p => p.Policies)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(p => p.Coverages)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
