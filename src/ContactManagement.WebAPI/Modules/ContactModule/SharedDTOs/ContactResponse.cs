using ContactManagement.Data.Models;

namespace ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;

public record ContactResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }

    public ContactResponse(Guid id, string firstName, string lastName, string? email, string? phone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}

internal static class ContactResponseExtensions
{
    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse(contact.Id, contact.FullName.FirstName, contact.FullName.LastName, contact.Email, contact.Phone);
    }
}