namespace Keyloop.SalesLeads.Api.Models;

public class LeadActivity
{
    public int Id { get; set; }
    public int SalesLeadId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public SalesLead? SalesLead { get; set; }
}
