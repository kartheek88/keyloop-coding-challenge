using Keyloop.SalesLeads.Api.Data;
using Keyloop.SalesLeads.Api.DTOs;
using Keyloop.SalesLeads.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Keyloop.SalesLeads.Api.Services;

public class SalesLeadService : ISalesLeadService
{
    private readonly SalesLeadDbContext _db;
    private readonly ILogger<SalesLeadService> _logger;

    public SalesLeadService(SalesLeadDbContext db, ILogger<SalesLeadService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyList<LeadResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var leads = await _db.SalesLeads.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
        return leads.Select(MapLeadWithoutActivities).ToList();
    }

    public async Task<LeadResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var lead = await _db.SalesLeads.AsNoTracking()
            .Include(x => x.Activities)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return lead is null ? null : MapLead(lead);
    }

    public async Task<LeadResponse> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken)
    {
        var lead = new SalesLead
        {
            CustomerName = request.CustomerName.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            VehicleInterest = request.VehicleInterest.Trim(),
            Status = "New",
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.SalesLeads.Add(lead);
        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created sales lead {LeadId}", lead.Id);
        return MapLead(lead);
    }

    public async Task<LeadActivityResponse?> AddActivityAsync(int leadId, LogActivityRequest request, CancellationToken cancellationToken)
    {
        var leadExists = await _db.SalesLeads.AnyAsync(x => x.Id == leadId, cancellationToken);
        if (!leadExists) return null;

        var activity = new LeadActivity
        {
            SalesLeadId = leadId,
            ActivityType = request.ActivityType.Trim(),
            Notes = request.Notes.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
        _db.LeadActivities.Add(activity);
        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Added activity {ActivityId} to lead {LeadId}", activity.Id, leadId);
        return new LeadActivityResponse(activity.Id, activity.ActivityType, activity.Notes, activity.CreatedAtUtc);
    }

    private static LeadResponse MapLeadWithoutActivities(SalesLead lead) =>
        new(lead.Id, lead.CustomerName, lead.Email, lead.Phone, lead.VehicleInterest,
            lead.Status, lead.CreatedAtUtc, Array.Empty<LeadActivityResponse>());

    private static LeadResponse MapLead(SalesLead lead)
    {
        var activities = lead.Activities
            .OrderBy(x => x.CreatedAtUtc)
            .ThenBy(x => x.Id)
            .Select(x => new LeadActivityResponse(x.Id, x.ActivityType, x.Notes, x.CreatedAtUtc))
            .ToList();
        return new LeadResponse(lead.Id, lead.CustomerName, lead.Email, lead.Phone,
            lead.VehicleInterest, lead.Status, lead.CreatedAtUtc, activities);
    }
}
