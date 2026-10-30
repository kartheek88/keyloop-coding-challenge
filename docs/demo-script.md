# 5–7 minute video walkthrough script

Use this as a guide, not a script to read word for word. Speak naturally and show the actual application. Do not claim a check passed unless you ran it.

## 0:00–0:30 — Introduction

"Hi, I’m Karthikeya. For the Keyloop technical assessment, I chose Scenario C, the Sales Lead Management Tool. I implemented the backend in ASP.NET Core Web API with Entity Framework Core and SQLite. The main flows are listing incoming leads, viewing lead details with follow-up history, and recording a new activity."

## 0:30–1:45 — Design

Open `docs/system-design.md` and explain the diagram:

"The controller handles HTTP requests and response codes. The service contains the use-case logic, and EF Core handles persistence through the DbContext. SQLite keeps the demo self-contained and persistent across restarts. Swagger acts as the simple client because the assessment lets me implement the backend and stub the frontend."

## 1:45–4:15 — Live API demo

1. Open Swagger.
2. `POST /api/leads` with a sample lead and show the `201` response and returned ID.
3. `GET /api/leads` and show the new lead in the inbox.
4. `POST /api/leads/{id}/activities` with `activityType: "Call"` and notes such as `Discussed test-drive availability`.
5. `GET /api/leads/{id}` and show the persisted activity in the history.
6. If prepared, try an invalid request and a nonexistent ID to show validation and `404` behavior.

## 4:15–5:30 — Code and tests

Show `LeadsController.cs`, `SalesLeadService.cs`, `SalesLeadDbContext.cs`, and the test file. Explain that controller/service separation keeps HTTP concerns out of business logic, and tests cover trimming/default status, missing leads, and oldest-first history. Show the actual `dotnet test` result only after running it.

## 5:30–6:45 — AI collaboration and trade-offs

"I used GenAI to help break the requirements into a minimal data model, endpoints, and test cases. I reviewed the generated code against the acceptance criteria, inspected validation and persistence behavior, and refined anything that did not match the requirements. I verified the solution by [describe only the checks you actually ran]. The main trade-off is SQLite and automatic schema creation for easy local setup; a production version would use migrations, authentication, pagination, and database-backed integration tests."

## 6:45–7:00 — Closing

"That completes the lead inbox, lead details and persisted activity logging flows. Thank you for reviewing my submission."

## Recording checklist

- Close personal tabs and notifications.
- Make sure the API is running and Swagger loads before recording.
- Use sample/fake customer data, not real personal data.
- Keep the screen readable and the recording between 5 and 10 minutes.
- Upload the video somewhere accessible to the reviewer and test the link in a private/incognito window.
