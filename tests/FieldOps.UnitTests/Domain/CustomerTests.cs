using FieldOps.Domain.Common;
using FieldOps.Domain.Customers;

namespace FieldOps.UnitTests.Domain;

public class CustomerTests
{
    private static Address SampleAddress() =>
        new("12 Harbor Road", null, "Springfield", "IL", "62701", "US");

    [Fact]
    public void Constructor_trims_values_and_blanks_become_null()
    {
        var customer = new Customer("  Acme Dental  ", " ", "555-0100");

        Assert.Equal("Acme Dental", customer.Name);
        Assert.Null(customer.Email);
        Assert.Equal("555-0100", customer.Phone);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_requires_a_name(string? name)
    {
        Assert.Throws<DomainException>(() => new Customer(name!, null, null));
    }

    [Fact]
    public void AddSite_links_site_to_customer()
    {
        var customer = new Customer("Acme Dental", null, null);

        var site = customer.AddSite("Main office", SampleAddress());

        Assert.Equal(customer.Id, site.CustomerId);
        Assert.Single(customer.Sites);
    }

    [Fact]
    public void AddSite_rejects_duplicate_address()
    {
        var customer = new Customer("Acme Dental", null, null);
        customer.AddSite("Main office", SampleAddress());

        Assert.Throws<DomainException>(() => customer.AddSite("Duplicate", SampleAddress()));
    }
}
