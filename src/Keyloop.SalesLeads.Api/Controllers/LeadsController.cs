using Keyloop.SalesLeads.Api.DTOs;
using Keyloop.SalesLeads.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keyloop.SalesLeads.Api.Controllers;

[ApiController]
[Route("api/leads")]
public class LeadsController : ControllerBase
{
    private readonly ISalesLeadService _service;

    public LeadsController(ISalesLeadService service) => _service = service;

    /// <summary>Returns the incoming sales lead inbox.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<LeadResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LeadResponse>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(cancellationToken));

    /// <summary>Returns lead details and its activities, oldest activity first.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LeadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeadResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var lead = await _service.GetByIdAsync(id, cancellationToken);
        return lead is null ? NotFound() : Ok(lead);
    }

    /// <summary>Creates a sales lead.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LeadResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LeadResponse>> Create(CreateLeadRequest request, CancellationToken cancellationToken)
    {
        var lead = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = lead.Id }, lead);
    }

    /// <summary>Persists a follow-up activity for a lead.</summary>
    [HttpPost("{id:int}/activities")]
    [ProducesResponseType(typeof(LeadActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeadActivityResponse>> AddActivity(int id, LogActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _service.AddActivityAsync(id, request, cancellationToken);
        if (activity is null) return NotFound();
        return StatusCode(StatusCodes.Status201Created, activity);
    }
}
