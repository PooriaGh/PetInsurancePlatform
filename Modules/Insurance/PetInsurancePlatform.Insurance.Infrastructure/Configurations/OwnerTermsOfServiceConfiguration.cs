using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class OwnerTermsOfServiceConfiguration : IEntityTypeConfiguration<OwnerTermsOfService>
{
    public void Configure(EntityTypeBuilder<OwnerTermsOfService> builder)
    {
        builder
            .HasKey(ot => ot.Id);

        builder
            .Property(ot => ot.Id)
            .ValueGeneratedNever();

        builder
            .HasIndex(ot => new { ot.Owner, ot.TermsOfService })
            .IsUnique();

        builder
            .HasOne(ot => ot.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ot => ot.TermsOfService)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
