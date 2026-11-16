using FieldOps.Domain.Common;
using FieldOps.Domain.Customers;
using FieldOps.Domain.Jobs;
using FieldOps.Domain.Technicians;

namespace FieldOps.UnitTests.Domain;

public class JobTests
{
    private static readonly DateTimeOffset Monday9am = new(2026, 11, 16, 9, 0, 0, TimeSpan.Zero);

    private readonly Customer _customer = new("Acme Dental", null, null);
    private readonly Technician _technician = new("Sam Rivera", null, null, ["HVAC"]);
    private readonly Site _site;

    public JobTests()
    {
        _site = _customer.AddSite("Main office", new Address("12 Harbor Road", null, "Springfield", "IL", "62701", "US"));
    }

    private Job NewJob() => new(_customer, _site, "Replace AC filter", null);

    [Fact]
    public void New_job_starts_in_New_status()
    {
        Assert.Equal(JobStatus.New, NewJob().Status);
    }

    [Fact]
    public void Constructor_rejects_site_from_another_customer()
    {
        var other = new Customer("Other Co", null, null);
        var otherSite = other.AddSite("Warehouse", new Address("1 Dock St", null, "Springfield", "IL", "62702", "US"));

        Assert.Throws<DomainException>(() => new Job(_customer, otherSite, "Inspect", null));
    }

    [Fact]
    public void Full_lifecycle_sets_status_and_timestamps()
    {
        var job = NewJob();

        job.Schedule(_technician, Monday9am, Monday9am.AddHours(2));
        job.Start(Monday9am.AddMinutes(5));
        job.Complete(Monday9am.AddHours(1));

        Assert.Equal(JobStatus.Completed, job.Status);
        Assert.Equal(_technician.Id, job.TechnicianId);
        Assert.Equal(Monday9am.AddHours(1), job.CompletedAt);
    }

    [Fact]
    public void Schedule_rejects_end_before_start()
    {
        Assert.Throws<DomainException>(() => NewJob().Schedule(_technician, Monday9am, Monday9am.AddHours(-1)));
    }

    [Fact]
    public void Schedule_rejects_inactive_technician()
    {
        _technician.Deactivate();

        Assert.Throws<DomainException>(() => NewJob().Schedule(_technician, Monday9am, Monday9am.AddHours(1)));
    }

    [Fact]
    public void Cannot_start_unscheduled_job()
    {
        Assert.Throws<DomainException>(() => NewJob().Start(Monday9am));
    }

    [Fact]
    public void Cannot_cancel_completed_job()
    {
        var job = NewJob();
        job.Schedule(_technician, Monday9am, Monday9am.AddHours(1));
        job.Start(Monday9am);
        job.Complete(Monday9am.AddMinutes(30));

        Assert.Throws<DomainException>(() => job.Cancel("Customer changed mind"));
    }

    [Fact]
    public void Cancel_requires_a_reason()
    {
        Assert.Throws<DomainException>(() => NewJob().Cancel(" "));
    }
}
