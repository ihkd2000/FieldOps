namespace FieldOps.Domain.Jobs;

public enum JobStatus
{
    New,
    Scheduled,
    InProgress,
    Completed,
    Cancelled,
}

public enum JobPriority
{
    Low,
    Normal,
    High,
    Emergency,
}
