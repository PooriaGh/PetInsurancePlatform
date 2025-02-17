using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
{
    public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();

        builder
           .ComplexProperty(p => p.Payment, builder =>
           {
               builder.IsRequired();
               builder.ComplexProperty(p => p.Amount);
           });

        builder
            .HasOne(p => p.Pet)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Plan)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
