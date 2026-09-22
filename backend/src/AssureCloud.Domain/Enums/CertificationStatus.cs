namespace AssureCloud.Domain.Enums;

public sealed class CertificationStatus : Enumeration
{
    public static readonly CertificationStatus NotStarted = new(0, "NotStarted");
    public static readonly CertificationStatus InProgress = new(1, "InProgress");
    public static readonly CertificationStatus Issued = new(2, "Issued");
    public static readonly CertificationStatus Expired = new(3, "Expired");
    public static readonly CertificationStatus Revoked = new(4, "Revoked");
    public static readonly CertificationStatus Suspended = new(5, "Suspended");

    private CertificationStatus(int value, string name) : base(value, name) { }
}
