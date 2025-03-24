namespace ContactManagement.Data.Models;

public record Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public Name(string firstName, string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
        
        FirstName = firstName;
        LastName = lastName;
    }
}