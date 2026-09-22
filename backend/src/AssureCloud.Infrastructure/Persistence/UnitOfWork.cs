using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AssureCloudDbContext _context;
    private bool _disposed;

    public IRepository<Organization> Organizations { get; }
    public IRepository<OrganizationLocation> OrganizationLocations { get; }
    public IRepository<Supplier> Suppliers { get; }
    public IRepository<User> Users { get; }
    public IRepository<Program> Programs { get; }
    public IRepository<Standard> Standards { get; }
    public IRepository<Requirement> Requirements { get; }
    public IRepository<Criterion> Criteria { get; }
    public IRepository<Control> Controls { get; }
    public IRepository<Assessment> Assessments { get; }
    public IRepository<AssessmentResponse> AssessmentResponses { get; }
    public IRepository<Evidence> Evidence { get; }
    public IRepository<Finding> Findings { get; }
    public IRepository<AssessmentAssignment> AssessmentAssignments { get; }
    public IRepository<Audit> Audits { get; }
    public IRepository<AuditFinding> AuditFindings { get; }
    public IRepository<CorrectiveAction> CorrectiveActions { get; }
    public IRepository<AuditAssignment> AuditAssignments { get; }
    public IRepository<Certification> Certifications { get; }
    public IRepository<CertificationDecision> CertificationDecisions { get; }
    public IRepository<CertificationScope> CertificationScopes { get; }
    public IRepository<Report> Reports { get; }

    public UnitOfWork(
        AssureCloudDbContext context,
        OrganizationRepository organizationRepository,
        OrganizationLocationRepository organizationLocationRepository,
        SupplierRepository supplierRepository,
        UserRepository userRepository,
        ProgramRepository programRepository,
        StandardRepository standardRepository,
        RequirementRepository requirementRepository,
        CriterionRepository criterionRepository,
        ControlRepository controlRepository,
        AssessmentRepository assessmentRepository,
        AssessmentResponseRepository assessmentResponseRepository,
        EvidenceRepository evidenceRepository,
        FindingRepository findingRepository,
        AssessmentAssignmentRepository assessmentAssignmentRepository,
        AuditRepository auditRepository,
        AuditFindingRepository auditFindingRepository,
        CorrectiveActionRepository correctiveActionRepository,
        AuditAssignmentRepository auditAssignmentRepository,
        CertificationRepository certificationRepository,
        CertificationDecisionRepository certificationDecisionRepository,
        CertificationScopeRepository certificationScopeRepository,
        ReportRepository reportRepository)
    {
        _context = context;
        Organizations = organizationRepository;
        OrganizationLocations = organizationLocationRepository;
        Suppliers = supplierRepository;
        Users = userRepository;
        Programs = programRepository;
        Standards = standardRepository;
        Requirements = requirementRepository;
        Criteria = criterionRepository;
        Controls = controlRepository;
        Assessments = assessmentRepository;
        AssessmentResponses = assessmentResponseRepository;
        Evidence = evidenceRepository;
        Findings = findingRepository;
        AssessmentAssignments = assessmentAssignmentRepository;
        Audits = auditRepository;
        AuditFindings = auditFindingRepository;
        CorrectiveActions = correctiveActionRepository;
        AuditAssignments = auditAssignmentRepository;
        Certifications = certificationRepository;
        CertificationDecisions = certificationDecisionRepository;
        CertificationScopes = certificationScopeRepository;
        Reports = reportRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}