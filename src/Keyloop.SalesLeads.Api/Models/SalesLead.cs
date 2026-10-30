namespace Keyloop.SalesLeads.Api.Models;

public class SalesLead
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehicleInterest { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public List<LeadActivity> Activities { get; set; } = new();
}
