namespace AssureCloud.Messaging;

public class MessagingOptions
{
    public const string SectionName = "Messaging";

    public string ConnectionString { get; set; } = string.Empty;
    public string OrganizationTopicName { get; set; } = "organizations";
    public string ProgramTopicName { get; set; } = "programs";
    public string AssessmentTopicName { get; set; } = "assessments";
    public string AuditTopicName { get; set; } = "audits";
    public string CertificationTopicName { get; set; } = "certifications";
    public string ReportTopicName { get; set; } = "reports";
    public string UserTopicName { get; set; } = "users";
}