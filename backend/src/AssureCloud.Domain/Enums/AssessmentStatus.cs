namespace AssureCloud.Domain.Enums;

public sealed class AssessmentStatus : Enumeration
{
    public static readonly AssessmentStatus Draft = new(0, "Draft");
    public static readonly AssessmentStatus InProgress = new(1, "InProgress");
    public static readonly AssessmentStatus Completed = new(2, "Completed");
    public static readonly AssessmentStatus Cancelled = new(3, "Cancelled");
    public static readonly AssessmentStatus OnHold = new(4, "OnHold");

    private AssessmentStatus(int value, string name) : base(value, name) { }
}
