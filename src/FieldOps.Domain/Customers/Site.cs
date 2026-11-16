using FieldOps.Domain.Common;

namespace FieldOps.Domain.Customers;

/// <summary>A physical location where work is performed for a customer.</summary>
public sealed class Site : Entity
{
    private Site() { Name = null!; Address = null!; }

    internal Site(Guid customerId, string name, Address address, string? accessNotes)
    {
        CustomerId = customerId;
        Name = Guard.Required(name, nameof(Name));
        Address = address ?? throw new DomainException("Address is required.");
        AccessNotes = Guard.Optional(accessNotes);
    }

    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public string? AccessNotes { get; private set; }

    public void UpdateAccessNotes(string? accessNotes) => AccessNotes = Guard.Optional(accessNotes);
}
