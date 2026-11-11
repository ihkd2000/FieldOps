using FieldOps.Domain.Common;
using FieldOps.Domain.Customers;
using FieldOps.Domain.Technicians;

namespace FieldOps.Domain.Jobs;

/// <summary>
/// A unit of work at a customer site. Moves New → Scheduled → InProgress → Completed,
/// and can be cancelled at any point before completion.
/// </summary>
public sealed class Job : Entity
{
    private Job() { Title = null!; }

    public Job(Customer customer, Site site, string title, string? description, JobPriority priority = JobPriority.Normal)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(site);

        if (site.CustomerId != customer.Id)
        {
            throw new DomainException("The site does not belong to this customer.");
        }

        CustomerId = customer.Id;
        SiteId = site.Id;
        Title = Guard.Required(title, nameof(Title));
        Description = Guard.Optional(description);
        Priority = priority;
    }

    public Guid CustomerId { get; private set; }
    public Guid SiteId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public JobPriority Priority { get; private set; }
    public JobStatus Status { get; private set; } = JobStatus.New;
    public Guid? TechnicianId { get; private set; }
    public DateTimeOffset? ScheduledStart { get; private set; }
    public DateTimeOffset? ScheduledEnd { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public void Schedule(Technician technician, DateTimeOffset start, DateTimeOffset end)
    {
        ArgumentNullException.ThrowIfNull(technician);
        EnsureStatus(JobStatus.New, JobStatus.Scheduled);

        if (!technician.IsActive)
        {
            throw new DomainException("Cannot schedule an inactive technician.");
        }

        if (end <= start)
        {
            throw new DomainException("Scheduled end must be after the start.");
        }

        TechnicianId = technician.Id;
        ScheduledStart = start;
        ScheduledEnd = end;
        Status = JobStatus.Scheduled;
    }

    public void Start(DateTimeOffset at)
    {
        EnsureStatus(JobStatus.Scheduled);
        StartedAt = at;
        Status = JobStatus.InProgress;
    }

    public void Complete(DateTimeOffset at)
    {
        EnsureStatus(JobStatus.InProgress);

        if (at < StartedAt)
        {
            throw new DomainException("Completion time cannot be before the start time.");
        }

        CompletedAt = at;
        Status = JobStatus.Completed;
    }

    public void Cancel(string reason)
    {
        EnsureStatus(JobStatus.New, JobStatus.Scheduled, JobStatus.InProgress);
        CancellationReason = Guard.Required(reason, "Cancellation reason");
        Status = JobStatus.Cancelled;
    }

    private void EnsureStatus(params JobStatus[] allowed)
    {
        if (!allowed.Contains(Status))
        {
            throw new DomainException($"Job cannot change from {Status} in this way.");
        }
    }
}
