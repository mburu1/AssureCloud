namespace AssureCloud.Domain.Enums;

public sealed class AuditStatus : Enumeration
{
    public static readonly AuditStatus Planned = new(0, "Planned");
    public static readonly AuditStatus InProgress = new(1, "InProgress");
    public static readonly AuditStatus Completed = new(2, "Completed");
    public static readonly AuditStatus Cancelled = new(3, "Cancelled");
    public static readonly AuditStatus OnHold = new(4, "OnHold");

    private AuditStatus(int value, string name) : base(value, name) { }
}
