using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.DTOs.Organizations;
using AssureCloud.Application.DTOs.Programs;
using AssureCloud.Application.DTOs.Assessments;
using AssureCloud.Application.DTOs.Audits;
using AssureCloud.Application.DTOs.Certifications;
using AssureCloud.Application.DTOs.Reports;
using AssureCloud.Application.DTOs.Users;
using AssureCloud.Application.DTOs.Common;
using Microsoft.Extensions.Logging;

namespace AssureCloud.Mvc.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    // Organizations
    public async Task<PagedResult<OrganizationDto>?> GetOrganizationsAsync(GetOrganizationsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<OrganizationDto>>($"api/v1/organizations{queryString}", ct);
    }

    public async Task<OrganizationDto?> GetOrganizationAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<OrganizationDto>($"api/v1/organizations/{id}", ct);
    }

    public async Task<OrganizationDto?> CreateOrganizationAsync(CreateOrganizationCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateOrganizationCommand, OrganizationDto>("api/v1/organizations", command, ct);
    }

    public async Task<OrganizationDto?> UpdateOrganizationAsync(Guid id, UpdateOrganizationCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateOrganizationCommand, OrganizationDto>($"api/v1/organizations/{id}", command, ct);
    }

    public async Task DeleteOrganizationAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/organizations/{id}", ct);
    }

    // Programs
    public async Task<PagedResult<ProgramDto>?> GetProgramsAsync(GetProgramsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<ProgramDto>>($"api/v1/programs{queryString}", ct);
    }

    public async Task<ProgramDto?> GetProgramAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<ProgramDto>($"api/v1/programs/{id}", ct);
    }

    public async Task<ProgramDto?> CreateProgramAsync(CreateProgramCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateProgramCommand, ProgramDto>("api/v1/programs", command, ct);
    }

    public async Task<ProgramDto?> UpdateProgramAsync(Guid id, UpdateProgramCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateProgramCommand, ProgramDto>($"api/v1/programs/{id}", command, ct);
    }

    public async Task DeleteProgramAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/programs/{id}", ct);
    }

    // Assessments
    public async Task<PagedResult<AssessmentDto>?> GetAssessmentsAsync(GetAssessmentsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<AssessmentDto>>($"api/v1/assessments{queryString}", ct);
    }

    public async Task<AssessmentDto?> GetAssessmentAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<AssessmentDto>($"api/v1/assessments/{id}", ct);
    }

    public async Task<AssessmentDto?> CreateAssessmentAsync(CreateAssessmentCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateAssessmentCommand, AssessmentDto>("api/v1/assessments", command, ct);
    }

    public async Task<AssessmentDto?> UpdateAssessmentAsync(Guid id, UpdateAssessmentCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateAssessmentCommand, AssessmentDto>($"api/v1/assessments/{id}", command, ct);
    }

    public async Task DeleteAssessmentAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/assessments/{id}", ct);
    }

    // Audits
    public async Task<PagedResult<AuditDto>?> GetAuditsAsync(GetAuditsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<AuditDto>>($"api/v1/audits{queryString}", ct);
    }

    public async Task<AuditDto?> GetAuditAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<AuditDto>($"api/v1/audits/{id}", ct);
    }

    public async Task<AuditDto?> CreateAuditAsync(CreateAuditCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateAuditCommand, AuditDto>("api/v1/audits", command, ct);
    }

    public async Task<AuditDto?> UpdateAuditAsync(Guid id, UpdateAuditCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateAuditCommand, AuditDto>($"api/v1/audits/{id}", command, ct);
    }

    public async Task DeleteAuditAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/audits/{id}", ct);
    }

    // Certifications
    public async Task<PagedResult<CertificationDto>?> GetCertificationsAsync(GetCertificationsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<CertificationDto>>($"api/v1/certifications{queryString}", ct);
    }

    public async Task<CertificationDto?> GetCertificationAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<CertificationDto>($"api/v1/certifications/{id}", ct);
    }

    public async Task<CertificationDto?> CreateCertificationAsync(CreateCertificationCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateCertificationCommand, CertificationDto>("api/v1/certifications", command, ct);
    }

    public async Task<CertificationDto?> UpdateCertificationAsync(Guid id, UpdateCertificationCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateCertificationCommand, CertificationDto>($"api/v1/certifications/{id}", command, ct);
    }

    public async Task DeleteCertificationAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/certifications/{id}", ct);
    }

    // Reports
    public async Task<PagedResult<ReportDto>?> GetReportsAsync(GetReportsQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<ReportDto>>($"api/v1/reports{queryString}", ct);
    }

    public async Task<ReportDto?> GetReportAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<ReportDto>($"api/v1/reports/{id}", ct);
    }

    public async Task<ReportDto?> CreateReportAsync(CreateReportCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateReportCommand, ReportDto>("api/v1/reports", command, ct);
    }

    public async Task<ReportDto?> UpdateReportAsync(Guid id, UpdateReportCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateReportCommand, ReportDto>($"api/v1/reports/{id}", command, ct);
    }

    public async Task DeleteReportAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/reports/{id}", ct);
    }

    // Users
    public async Task<PagedResult<UserDto>?> GetUsersAsync(GetUsersQuery query, CancellationToken ct = default)
    {
        var queryString = BuildQueryString(query);
        return await GetAsync<PagedResult<UserDto>>($"api/v1/users{queryString}", ct);
    }

    public async Task<UserDto?> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<UserDto>($"api/v1/users/{id}", ct);
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        return await PostAsync<CreateUserCommand, UserDto>("api/v1/users", command, ct);
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserCommand command, CancellationToken ct = default)
    {
        return await PutAsync<UpdateUserCommand, UserDto>($"api/v1/users/{id}", command, ct);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"api/v1/users/{id}", ct);
    }

    private async Task<T?> GetAsync<T>(string url, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync(url, ct);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, ct);
            }

            _logger.LogWarning("API call failed: {Url} - {StatusCode}", url, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling API: {Url}", url);
            throw;
        }
    }

    private async Task<TResult?> PostAsync<TRequest, TResult>(string url, TRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, request, _jsonOptions, ct);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions, ct);
            }

            _logger.LogWarning("API call failed: {Url} - {StatusCode}", url, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling API: {Url}", url);
            throw;
        }
    }

    private async Task<TResult?> PutAsync<TRequest, TResult>(string url, TRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(url, request, _jsonOptions, ct);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions, ct);
            }

            _logger.LogWarning("API call failed: {Url} - {StatusCode}", url, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling API: {Url}", url);
            throw;
        }
    }

    private async Task DeleteAsync(string url, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API call failed: {Url} - {StatusCode}", url, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling API: {Url}", url);
            throw;
        }
    }

    private static string BuildQueryString(object query)
    {
        var properties = query.GetType().GetProperties();
        var parts = new List<string>();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(query);
            if (value != null)
            {
                if (value is IEnumerable<object> enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        parts.Add($"{prop.Name}={Uri.EscapeDataString(item.ToString()!)}");
                    }
                }
                else
                {
                    parts.Add($"{prop.Name}={Uri.EscapeDataString(value.ToString()!)}");
                }
            }
        }

        return parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty;
    }
}