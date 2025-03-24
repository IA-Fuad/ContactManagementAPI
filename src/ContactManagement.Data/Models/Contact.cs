namespace ContactManagement.Data.Models;

public class Contact
{
    public Guid Id { get; private set; }
    public Name FullName { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    private Contact() {}

    public static Contact Create(string firstName, string lastName, string? email, string? phone)
    {
        return new Contact()
        {
            Id = Guid.NewGuid(),
            FullName = new(firstName, lastName),
            Email = email,
            Phone = phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    public void Update(string firstName, string lastName, string? email, string? phone)
    {
        FullName = new(firstName, lastName);
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}