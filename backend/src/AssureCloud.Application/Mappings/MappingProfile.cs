using AutoMapper;
using AssureCloud.Application.DTOs;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Organization mappings
        CreateMap<Organization, OrganizationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.LocationsCount, opt => opt.MapFrom(src => src.Locations.Count))
            .ForMember(dest => dest.SuppliersCount, opt => opt.MapFrom(src => src.Suppliers.Count))
            .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.Users.Count));

        CreateMap<Organization, OrganizationListDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.LocationsCount, opt => opt.MapFrom(src => src.Locations.Count))
            .ForMember(dest => dest.SuppliersCount, opt => opt.MapFrom(src => src.Suppliers.Count));

        CreateMap<OrganizationLocation, OrganizationLocationDto>();

        CreateMap<Supplier, SupplierDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Program mappings
        CreateMap<Program, ProgramDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StandardsCount, opt => opt.MapFrom(src => src.Standards.Count))
            .ForMember(dest => dest.AssessmentsCount, opt => opt.MapFrom(src => src.Assessments.Count));

        CreateMap<Program, ProgramListDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StandardsCount, opt => opt.MapFrom(src => src.Standards.Count));

        CreateMap<Standard, StandardDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.RequirementsCount, opt => opt.MapFrom(src => src.Requirements.Count));

        CreateMap<Requirement, RequirementDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CriteriaCount, opt => opt.MapFrom(src => src.Criteria.Count));

        CreateMap<Criterion, CriterionDto>()
            .ForMember(dest => dest.ScoringMethod, opt => opt.MapFrom(src => src.ScoringMethod.ToString()));

        CreateMap<Control, ControlDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Assessment mappings
        CreateMap<Assessment, AssessmentDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ResponsesCount, opt => opt.MapFrom(src => src.Responses.Count))
            .ForMember(dest => dest.EvidenceCount, opt => opt.MapFrom(src => src.Evidence.Count))
            .ForMember(dest => dest.FindingsCount, opt => opt.MapFrom(src => src.Findings.Count))
            .ForMember(dest => dest.OpenFindingsCount, opt => opt.MapFrom(src => src.Findings.Count(f => f.IsOpen)));

        CreateMap<Assessment, AssessmentListDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<AssessmentResponse, AssessmentResponseDto>();

        CreateMap<Evidence, EvidenceDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Finding, FindingDto>()
            .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<AssessmentAssignment, AssessmentAssignmentDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Assessment, AssessmentSummaryDto>();

        // Audit mappings
        CreateMap<Audit, AuditDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.FindingsCount, opt => opt.MapFrom(src => src.Findings.Count))
            .ForMember(dest => dest.OpenFindingsCount, opt => opt.MapFrom(src => src.Findings.Count(f => f.IsOpen)))
            .ForMember(dest => dest.CorrectiveActionsCount, opt => opt.MapFrom(src => src.CorrectiveActions.Count))
            .ForMember(dest => dest.OverdueCorrectiveActionsCount, opt => opt.MapFrom(src => src.CorrectiveActions.Count(ca => ca.IsOverdue)));

        CreateMap<Audit, AuditListDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.FindingsCount, opt => opt.MapFrom(src => src.Findings.Count));

        CreateMap<AuditFinding, AuditFindingDto>()
            .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CorrectiveAction, CorrectiveActionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<AuditAssignment, AuditAssignmentDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Audit, AuditSummaryDto>();

        // Certification mappings
        CreateMap<Certification, CertificationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DecisionsCount, opt => opt.MapFrom(src => src.Decisions.Count))
            .ForMember(dest => dest.ScopesCount, opt => opt.MapFrom(src => src.Scopes.Count));

        CreateMap<Certification, CertificationListDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.IsExpiringSoon, opt => opt.MapFrom(src => src.IsExpiringSoon));

        CreateMap<CertificationDecision, CertificationDecisionDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<CertificationScope, CertificationScopeDto>();

        CreateMap<Certification, CertificationSummaryDto>();

        // Report mappings
        CreateMap<Report, ReportDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Report, ReportListDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Report, ReportSummaryDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}