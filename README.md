# FieldOps

Operations platform for small field-service businesses (HVAC, plumbing, electrical, cleaning). FieldOps manages customers, jobs, scheduling, quotes and invoices, and uses AI agents to turn customer requests into scheduled, quoted jobs.

## Stack

- **API:** ASP.NET Core (.NET 10), EF Core, PostgreSQL + pgvector
- **Web:** React + TypeScript (Vite)
- **AI:** Microsoft.Extensions.AI with tool calling
- **Jobs:** Hangfire
- **Tests:** xUnit
- **CI:** GitHub Actions

## Project layout

```
src/
  FieldOps.Domain/          Entities and business rules, no dependencies
  FieldOps.Application/     Use cases, validation, interfaces
  FieldOps.Infrastructure/  EF Core, external services, AI clients
  FieldOps.Api/             HTTP endpoints
tests/
  FieldOps.UnitTests/
web/                        React front end
```

## Getting started

Requirements: .NET 10 SDK, Node 22+, Docker.

```bash
docker compose up -d
dotnet run --project src/FieldOps.Api
cd web && npm install && npm run dev
```

The API health check is at `/health`, and the web app runs at http://localhost:5173.

## Roadmap

| Phase | Deliverable |
| --- | --- |
| 1. Foundation | Auth and roles, multi-tenant data model, seed data, CI |
| 2. Core operations | Customers, jobs, price book, quotes, invoices |
| 3. Scheduling | Calendar board, technician availability, slot suggestions |
| 4. AI agents | Intake agent, quote builder, equipment manual assistant |
| 5. Customer side | Notifications, online quote approval, payment status |
| 6. Reporting and hardening | Dashboards, performance, security review, deployment |

## Data and privacy

Development uses only synthetic data. No real customer data is stored in this repository.
