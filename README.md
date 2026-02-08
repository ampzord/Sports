# ? Sports

A full-stack sports management application built with **.NET 10**, **Vue 3**, and **[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)** for orchestration.

Manage leagues, teams, players, and matches through a REST API and a Vue SPA — all wired together and launched with a single `dotnet run`.

---

## Architecture

```
Sports.AppHost (Aspire Orchestrator)
??? Sports.Api              — REST API (.NET 10, FastEndpoints, MediatR, EF Core)
??? Sports.MatchSimulationWorker — Background worker (consumes RabbitMQ messages)
??? SportsUI                — Vue 3 SPA (Vite, Tailwind CSS)
??? SQL Server              — Database (auto-migrated on startup)
??? RabbitMQ                — Message broker
??? Grafana + Loki          — Logging & observability
```

---

## Projects

| Project | Description |
|---|---|
| **Sports.AppHost** | .NET Aspire host — orchestrates all services, databases, and containers |
| **Sports.Api** | REST API with full CRUD for leagues, teams, players, and matches |
| **Sports.MatchSimulationWorker** | Worker service that consumes RabbitMQ messages and simulates match pass statistics |
| **Sports.Shared** | Shared entities, enums, MediatR behaviors, and configuration constants |
| **Sports.ServiceDefaults** | Aspire service defaults (OpenTelemetry, health checks, resilience) |
| **SportsUI** | Vue 3 frontend with Tailwind CSS |
| **Sports.Api.UnitTests** | Unit tests — domain model constraints against a real SQL Server (Testcontainers) |
| **Sports.Api.IntegrationTests** | Integration tests — full HTTP endpoint testing via `WebApplicationFactory` |
| **Sports.Worker.IntegrationTests** | Integration tests for the match simulation worker |

---

## Backend

### API (`Sports.Api`)

Built with a **vertical slice / feature folder** structure. Each feature (e.g., `AddTeam`, `GetTeamById`) lives in its own folder with:

```
Features/
??? Teams/
    ??? _Shared/
    ?   ??? TeamMapper.cs          ? Single Mapperly mapper for all team operations
    ?   ??? Responses/
    ?       ??? TeamResponse.cs
    ??? AddTeam/
    ?   ??? AddTeamEndpoint.cs     ? FastEndpoints endpoint
    ?   ??? AddTeamHandler.cs      ? MediatR handler
    ?   ??? AddTeamCommand.cs
    ?   ??? AddTeamRequest.cs
    ?   ??? AddTeamValidator.cs
    ??? GetTeamById/
    ??? GetTeams/
    ??? UpdateTeam/
    ??? DeleteTeam/
```

**Key libraries:**

- **[FastEndpoints](https://fast-endpoints.com/)** — Minimal API endpoint definitions with built-in validation and Swagger support
- **[MediatR](https://github.com/jbogard/MediatR)** — CQRS-style command/query separation with pipeline behaviors
- **[ErrorOr](https://github.com/amantinband/error-or)** — Discriminated union result type (no exceptions for expected errors)
- **[Mapperly](https://mapperly.riok.app/)** — Source-generated object mapping (zero reflection, zero allocation)
- **[EF Core](https://learn.microsoft.com/en-us/ef/core/)** — ORM with SQL Server, code-first migrations, and seed data
- **[Serilog](https://serilog.net/)** — Structured logging to console and Grafana Loki
- **[FluentValidation](https://docs.fluentvalidation.net/)** — Request validation

### Domain Model

| Entity | Key Fields |
|---|---|
| **League** | `Id`, `Name` (unique, max 100 chars) |
| **Team** | `Id`, `Name` (unique), `LeagueId` (FK ? League, restrict delete) |
| **Player** | `Id`, `Name` (unique), `Position` (enum stored as string), `TeamId` (FK ? Team) |
| **Match** | `Id`, `HomeTeamId`, `AwayTeamId` (FK ? Team), `TotalPasses` (nullable) |

Relationships enforce referential integrity — you cannot delete a league that has teams, or a team that has players/matches.

### Worker (`Sports.MatchSimulationWorker`)

A `BackgroundService` that listens on a RabbitMQ queue. When triggered from the API's `/api/matches/simulate` endpoint, it:

1. Picks up unprocessed matches in batches of 500
2. Uses `UPDLOCK, ROWLOCK, READPAST` SQL hints for safe concurrent processing
3. Assigns a random `TotalPasses` value (100–1000) to each match
4. Supports multiple concurrent workers without double-processing

### Seed Data

The database is seeded on startup with:
- 2 leagues (Premier League, La Liga)
- 8 teams (4 per league)
- 88 players (full 4-3-3 lineup of 11 per team)
- 100 matches

---

## Frontend (`SportsUI`)

A **Vue 3** single-page application with:

- **Vue Router** — Client-side routing for all CRUD views
- **Tailwind CSS** — Utility-first styling
- **Axios** — HTTP client (proxied through Vite to the API)
- **Vue Toastification** — Toast notifications

### Pages

| Route | View |
|---|---|
| `/home` | Dashboard / home page |
| `/leagues` | List, create, edit, and view leagues |
| `/teams` | List, create, edit, and view teams |
| `/players` | List, create, edit, and view players |
| `/matches` | List, create, edit, and view matches |

The Vite dev server proxies all `/api` requests to the backend, so no CORS configuration is needed.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (v20.19+ or v22.12+)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server, RabbitMQ, Grafana, Loki)

### Run with Aspire

```bash
# 1. Clone the repo
git clone https://github.com/ampzord/Sports.git
cd Sports

# 2. Install frontend dependencies
cd SportsUI
npm install
cd ..

# 3. Start everything via Aspire
dotnet run --project Sports.AppHost
```

This single command starts:
- **SQL Server** on port `14330`
- **RabbitMQ** with management UI
- **Grafana** + **Loki** for log aggregation
- **Sports.Api** — REST API with Swagger UI
- **Sports.MatchSimulationWorker** — Background worker
- **SportsUI** — Vue dev server (Vite)

The **Aspire Dashboard** opens automatically in your browser, showing all resources and their endpoints.

### Explore

| Resource | Where to find it |
|---|---|
| Aspire Dashboard | Opens automatically (or `https://localhost:17298`) |
| Swagger UI | API resource URL + `/swagger` |
| Grafana | Grafana resource URL (admin/admin) |
| Vue Frontend | `sports-ui` resource URL in the dashboard |

---

## Running Tests

```bash
# Unit tests (requires Docker for Testcontainers)
dotnet test Sports.Api.UnitTests

# API integration tests
dotnet test Sports.Api.IntegrationTests

# Worker integration tests
dotnet test Sports.Worker.IntegrationTests

# All tests
dotnet test
```

Tests use **[Testcontainers](https://dotnet.testcontainers.org/)** to spin up a real SQL Server instance — no mocking of the database layer. Integration tests use **[FluentAssertions](https://fluentassertions.com/)** and **[FluentAssertions.Web](https://github.com/adrianiftode/FluentAssertions.Web)** for expressive HTTP assertions like `response.Should().Be200Ok()`.

---

## Tech Stack

### Backend
| | |
|---|---|
| Runtime | .NET 10 / C# 14 |
| API Framework | FastEndpoints |
| Mediator | MediatR |
| ORM | EF Core (SQL Server) |
| Mapping | Mapperly (source-gen) |
| Validation | FluentValidation |
| Error Handling | ErrorOr |
| Logging | Serilog ? Grafana Loki |
| Messaging | RabbitMQ |
| Orchestration | .NET Aspire |

### Frontend
| | |
|---|---|
| Framework | Vue 3 (Composition API) |
| Build Tool | Vite 7 |
| Styling | Tailwind CSS 4 |
| HTTP Client | Axios |
| Routing | Vue Router 4 |

### Testing
| | |
|---|---|
| Framework | xUnit |
| Assertions | FluentAssertions + FluentAssertions.Web |
| HTTP Testing | Microsoft.AspNetCore.Mvc.Testing |
| Containers | Testcontainers (SQL Server) |
| Mocking | Moq |
