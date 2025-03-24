using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Data.Infrastructure.EFModelConfigurations;

public class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("Fund");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Name).HasMaxLength(100).IsRequired();
        builder.Property(f => f.IsDeleted).HasDefaultValue(false).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.UpdatedAt).IsRequired();
        builder.Property(f => f.DeletedAt);
    }
}