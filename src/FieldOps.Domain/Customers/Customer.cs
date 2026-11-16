using FieldOps.Domain.Common;

namespace FieldOps.Domain.Customers;

public sealed class Customer : Entity
{
    private readonly List<Site> _sites = [];

    private Customer() { Name = null!; }

    public Customer(string name, string? email, string? phone)
    {
        Name = Guard.Required(name, nameof(Name));
        Email = Guard.Optional(email);
        Phone = Guard.Optional(phone);
    }

    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public IReadOnlyCollection<Site> Sites => _sites.AsReadOnly();

    public void UpdateContact(string name, string? email, string? phone)
    {
        Name = Guard.Required(name, nameof(Name));
        Email = Guard.Optional(email);
        Phone = Guard.Optional(phone);
    }

    public Site AddSite(string name, Address address, string? accessNotes = null)
    {
        if (_sites.Any(s => s.Address == address))
        {
            throw new DomainException("This customer already has a site at that address.");
        }

        var site = new Site(Id, name, address, accessNotes);
        _sites.Add(site);
        return site;
    }
}
