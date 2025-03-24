namespace ContactManagement.Data.Models;

public class FundContact
{
    public Guid ContactId { get; private set; }
    public Guid FundId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    
    public Contact Contact { get; private set; } = null!;
    public Fund Fund { get; private set; } = null!;

    private FundContact() {}

    public static FundContact Create(Guid contactId, Guid fundId)
    {
        return new()
        {
            ContactId = contactId,
            FundId = fundId,
            AssignedAt = DateTime.UtcNow
        };
    }
}