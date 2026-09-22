namespace AssureCloud.Workers;

public class WorkersOptions
{
    public const string SectionName = "Workers";

    public bool EnableReportGenerationWorker { get; set; } = true;
    public bool EnableCertificationExpiryWorker { get; set; } = true;
    public bool EnableAssessmentReminderWorker { get; set; } = true;
    public bool EnableAuditReminderWorker { get; set; } = true;
    public bool EnableCorrectiveActionWorker { get; set; } = true;
    public bool EnableDataRetentionWorker { get; set; } = true;

    public string ReportGenerationSchedule { get; set; } = "0 */30 * * * *";
    public string CertificationExpirySchedule { get; set; } = "0 0 9 * * *";
    public string AssessmentReminderSchedule { get; set; } = "0 0 8 * * *";
    public string AuditReminderSchedule { get; set; } = "0 0 8 * * *";
    public string CorrectiveActionSchedule { get; set; } = "0 0 9 * * *";
    public string DataRetentionSchedule { get; set; } = "0 0 2 * * 0";
}