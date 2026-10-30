namespace Keyloop.SalesLeads.Api.DTOs;

public record LeadActivityResponse(int Id, string ActivityType, string Notes, DateTime CreatedAtUtc);

public record LeadResponse(
    int Id,
    string CustomerName,
    string Email,
    string Phone,
    string VehicleInterest,
    string Status,
    DateTime CreatedAtUtc,
    IReadOnlyList<LeadActivityResponse> Activities);
