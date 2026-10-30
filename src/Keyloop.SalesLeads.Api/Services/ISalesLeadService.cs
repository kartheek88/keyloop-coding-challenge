using Keyloop.SalesLeads.Api.DTOs;

namespace Keyloop.SalesLeads.Api.Services;

public interface ISalesLeadService
{
    Task<IReadOnlyList<LeadResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<LeadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<LeadResponse> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken);
    Task<LeadActivityResponse?> AddActivityAsync(int leadId, LogActivityRequest request, CancellationToken cancellationToken);
}
