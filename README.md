# Keyloop Sales Lead Management API

A compact backend implementation for **Scenario C: The Sales Lead Management Tool** from the Keyloop technical assessment. It provides a lead inbox, lead details with a chronological activity log, and persistent follow-up activity logging.

## Technology choices

- **.NET 8 / ASP.NET Core Web API** — familiar REST API framework for a C# backend.
- **Entity Framework Core + SQLite** — relational persistence with zero separate database server setup. The `sales-leads.db` file is created in the application's working directory.
- **Swagger/OpenAPI** — interactive API exploration and demonstration.
- **xUnit + EF Core InMemory provider** — fast tests for service-layer business behavior.
- **Built-in `ILogger`** — structured application logs with lead/activity identifiers.

## Requirements

- .NET 8 SDK
- An IDE such as Visual Studio 2022 or VS Code (optional)

## Run locally

From this repository's root:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/Keyloop.SalesLeads.Api
```

Open the Swagger URL printed in the console (usually `https://localhost:xxxx/swagger` or `http://localhost:xxxx/swagger`). If HTTPS development certificates cause local issues, use the HTTP URL printed by the app.

The SQLite database is created automatically at startup. To reset demo data, stop the app and delete `sales-leads.db` from the application's working directory.

## API contract

| Method | Route | Purpose | Success response |
|---|---|---|---|
| `GET` | `/api/leads` | List incoming leads (inbox) | `200 OK` |
| `POST` | `/api/leads` | Create a lead | `201 Created` |
| `GET` | `/api/leads/{id}` | Get lead details and activities, oldest first | `200 OK` or `404 Not Found` |
| `POST` | `/api/leads/{id}/activities` | Add a follow-up activity | `201 Created`, `400`, or `404` |

### Example: create a lead

```bash
curl -X POST "http://localhost:5000/api/leads" \
  -H "Content-Type: application/json" \
  -d '{"customerName":"Ananya Rao","email":"ananya@example.com","phone":"9876543210","vehicleInterest":"Sedan X"}'
```

### Example: list leads

```bash
curl "http://localhost:5000/api/leads"
```

### Example: add a follow-up activity

Replace `1` with the ID returned when creating a lead.

```bash
curl -X POST "http://localhost:5000/api/leads/1/activities" \
  -H "Content-Type: application/json" \
  -d '{"activityType":"Call","notes":"Discussed test-drive availability"}'
```

### Example: inspect details and history

```bash
curl "http://localhost:5000/api/leads/1"
```

The details response includes activities ordered oldest first. Request validation is provided by data annotations and `[ApiController]` automatic 400 responses. Missing lead IDs return 404.

## Architecture and assumptions

See [`docs/system-design.md`](docs/system-design.md) for the architecture diagram, responsibilities, data flow, assumptions, reliability and observability choices.

- A lead represents one incoming customer enquiry.
- Activity timestamps are stored in UTC.
- The API returns the inbox without loading activity collections; the details endpoint loads the activity history.
- Authentication/authorization, pagination, rate limiting, and production migrations are out of scope for this small assessment and should be added before production use.
- `EnsureCreated()` is used to keep local setup simple. A production service should use EF Core migrations and a controlled deployment process.

## Tests

The test suite checks input trimming/default status, the missing-lead behavior for activity creation, and oldest-first activity ordering. Tests use an in-memory EF Core provider; the running API uses SQLite for persistent storage. Run `dotnet test` before submission and add integration tests against SQLite if time permits.

## AI Collaboration Narrative (complete honestly before submitting)

This assessment explicitly asks candidates to use GenAI and explain how they verified its output. Use this section as a draft and edit it to reflect what you actually did:

1. **Direction:** I used an AI assistant to break the scenario into its acceptance criteria, propose a minimal API/data model, and scaffold repetitive DTO, controller, and test code. I kept the implementation focused on the required lead inbox, details/history, and activity logging flows.
2. **Review:** I reviewed the generated code against each requirement, checked entity relationships and HTTP status codes, and inspected validation, cancellation-token propagation, UTC timestamps, and activity ordering.
3. **Verification:** I ran `dotnet restore`, `dotnet build`, and `dotnet test` locally; I exercised the endpoints in Swagger/cURL and checked that a newly logged activity appeared on a subsequent details request. **Only leave this sentence if you actually complete those checks.** Record any failures found and how you fixed them.
4. **Ownership and limitations:** I can explain the request flow from controller to service to EF Core, why SQLite is used for easy persistent local setup, and why the tests use an in-memory provider. I would replace `EnsureCreated()` with migrations and add authentication, pagination, and database-backed integration tests before production deployment.

Do not claim tests passed until you have run them on your machine.

## Before sharing the repository

- [ ] Run restore, build, and tests successfully.
- [ ] Run the API and try all four routes in Swagger.
- [ ] Review the code so you can explain it without reading the README.
- [ ] Update the AI narrative to accurately describe your own review and verification.
- [ ] Add your own GitHub repository URL to the email only after pushing the project.
