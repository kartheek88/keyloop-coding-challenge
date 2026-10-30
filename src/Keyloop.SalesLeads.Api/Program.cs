using Keyloop.SalesLeads.Api.Data;
using Keyloop.SalesLeads.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SalesLeadDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SalesLeads")
        ?? "Data Source=sales-leads.db"));
builder.Services.AddScoped<ISalesLeadService, SalesLeadService>();

var app = builder.Build();

// For this small assessment, create the SQLite schema at startup. In production,
// use reviewed EF Core migrations as part of the deployment process.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalesLeadDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
