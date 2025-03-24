using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Data.Infrastructure.EFModelConfigurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contact");
        builder.HasKey(c => c.Id);
        builder.ComplexProperty(c => c.FullName)
            .Property(c => c.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
        builder.ComplexProperty(c => c.FullName)
            .Property(c => c.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
        builder.Property(c => c.DeletedAt);
    }
}