using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Data.Infrastructure.EFDbContext;

public class ContactManagementDbContext : DbContext
{
    public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Fund> Funds { get; set; }
    public DbSet<FundContact> FundContacts { get; set; }
}