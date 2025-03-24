using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Data.Infrastructure.EFModelConfigurations;

public class ContactFundConfiguration : IEntityTypeConfiguration<FundContact>
{
    public void Configure(EntityTypeBuilder<FundContact> builder)
    {
        builder.ToTable("FundContacts");
        builder.HasKey(f => new { f.FundId, f.ContactId });
        builder.HasOne(f => f.Fund)
            .WithMany()
            .HasForeignKey(f => f.FundId)
            .IsRequired();
        builder.HasOne(f => f.Contact)
            .WithMany()
            .HasForeignKey(f => f.ContactId)
            .IsRequired();
        builder.Property(f => f.AssignedAt).IsRequired();
    }
}