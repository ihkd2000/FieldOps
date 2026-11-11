using FieldOps.Domain.Common;

namespace FieldOps.Domain.Customers;

public sealed record Address
{
    public Address(string line1, string? line2, string city, string? region, string postalCode, string country)
    {
        Line1 = Guard.Required(line1, nameof(Line1));
        Line2 = Guard.Optional(line2);
        City = Guard.Required(city, nameof(City));
        Region = Guard.Optional(region);
        PostalCode = Guard.Required(postalCode, nameof(PostalCode));
        Country = Guard.Required(country, nameof(Country));
    }

    public string Line1 { get; }
    public string? Line2 { get; }
    public string City { get; }
    public string? Region { get; }
    public string PostalCode { get; }
    public string Country { get; }
}
