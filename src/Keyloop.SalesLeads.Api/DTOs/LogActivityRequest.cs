using System.ComponentModel.DataAnnotations;

namespace Keyloop.SalesLeads.Api.DTOs;

public class LogActivityRequest
{
    [Required, StringLength(60, MinimumLength = 2)]
    public string ActivityType { get; set; } = string.Empty;

    [Required, StringLength(1000, MinimumLength = 2)]
    public string Notes { get; set; } = string.Empty;
}
