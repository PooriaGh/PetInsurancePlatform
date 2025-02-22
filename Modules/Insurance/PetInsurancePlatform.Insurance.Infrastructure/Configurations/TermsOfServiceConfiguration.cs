using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class TermsOfServiceConfiguration : IEntityTypeConfiguration<TermsOfService>
{
    public void Configure(EntityTypeBuilder<TermsOfService> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .ValueGeneratedNever();

        builder
            .HasIndex(t => t.Id)
            .IsUnique();

        builder
            .HasMany<OwnerTermsOfService>()
            .WithOne(ot => ot.TermsOfService)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
