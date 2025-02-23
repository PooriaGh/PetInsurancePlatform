using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder
            .HasKey(o => o.Id);

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();

        builder
           .Ignore(o => o.FullName);

        builder
           .HasMany(o => o.Pets)
           .WithOne()
           .OnDelete(DeleteBehavior.Restrict);

        builder
           .HasMany<OwnerTermsOfService>()
           .WithOne(ot => ot.Owner)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
