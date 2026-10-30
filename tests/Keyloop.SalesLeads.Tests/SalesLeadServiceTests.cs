using Keyloop.SalesLeads.Api.Data;
using Keyloop.SalesLeads.Api.DTOs;
using Keyloop.SalesLeads.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Keyloop.SalesLeads.Tests;

public class SalesLeadServiceTests
{
    private static SalesLeadDbContext CreateDb(string name)
    {
        var options = new DbContextOptionsBuilder<SalesLeadDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
        return new SalesLeadDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_TrimsInputAndSetsNewStatus()
    {
        using var db = CreateDb(nameof(CreateAsync_TrimsInputAndSetsNewStatus));
        var service = new SalesLeadService(db, NullLogger<SalesLeadService>.Instance);
        var result = await service.CreateAsync(new CreateLeadRequest
        {
            CustomerName = "  Ananya Rao ", Email = " ananya@example.com ",
            Phone = " 9876543210 ", VehicleInterest = " Sedan X "
        }, CancellationToken.None);

        Assert.Equal("Ananya Rao", result.CustomerName);
        Assert.Equal("ananya@example.com", result.Email);
        Assert.Equal("New", result.Status);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task AddActivityAsync_ReturnsNullWhenLeadDoesNotExist()
    {
        using var db = CreateDb(nameof(AddActivityAsync_ReturnsNullWhenLeadDoesNotExist));
        var service = new SalesLeadService(db, NullLogger<SalesLeadService>.Instance);

        var result = await service.AddActivityAsync(999, new LogActivityRequest
        {
            ActivityType = "Call", Notes = "Spoke with customer"
        }, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsActivitiesInChronologicalOrder()
    {
        using var db = CreateDb(nameof(GetByIdAsync_ReturnsActivitiesInChronologicalOrder));
        var service = new SalesLeadService(db, NullLogger<SalesLeadService>.Instance);
        var lead = await service.CreateAsync(new CreateLeadRequest
        {
            CustomerName = "Ananya Rao", Email = "ananya@example.com",
            Phone = "9876543210", VehicleInterest = "Sedan X"
        }, CancellationToken.None);

        await service.AddActivityAsync(lead.Id, new LogActivityRequest
        {
            ActivityType = "Call", Notes = "First call"
        }, CancellationToken.None);
        await Task.Delay(5);
        await service.AddActivityAsync(lead.Id, new LogActivityRequest
        {
            ActivityType = "Email", Notes = "Sent brochure"
        }, CancellationToken.None);

        var result = await service.GetByIdAsync(lead.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Activities.Count);
        Assert.Equal("First call", result.Activities[0].Notes);
        Assert.Equal("Sent brochure", result.Activities[1].Notes);
    }
}
