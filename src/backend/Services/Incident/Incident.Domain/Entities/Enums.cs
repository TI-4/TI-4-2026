namespace Incident.Domain.Entities;

public enum Objectenum {
    Pending = 0,
    In_Process = 1,
    Resolved = 2,
    Canceled = 3,
}

public enum Complainenum
{
    Pending,
    In_Process,
    Resolved,
    Dismissed,
}

public enum Tickets
{
    Claim,
    Found,
    Match,
    Pickup
}
