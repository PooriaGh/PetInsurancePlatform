using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class PetTypeConfiguration : IEntityTypeConfiguration<PetType>
{
    public void Configure(EntityTypeBuilder<PetType> builder)
    {
        builder
            .HasKey(pt => pt.Id);

        builder
            .Property(pt => pt.Id)
            .ValueGeneratedNever();

        builder
            .Ignore(pt => pt.Diseases);

        builder
            .HasMany(pt => pt.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(pt => pt.PetTypeDiseases)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
