using System.ComponentModel.DataAnnotations;

namespace Keyloop.SalesLeads.Api.DTOs;

public class CreateLeadRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string VehicleInterest { get; set; } = string.Empty;
}
