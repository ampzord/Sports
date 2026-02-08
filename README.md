# Sports

A full-stack sports management application built with **.NET 10**, **Vue 3**, and **[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)** for orchestration.

Manage leagues, teams, players, and matches through a REST API and a Vue SPA -- all wired together and launched with a single `dotnet run`.

---

## Architecture

```
Sports.AppHost (Aspire Orchestrator)
|-- Sports.Api                    REST API (.NET 10, FastEndpoints, MediatR, EF Core)
|-- Sports.MatchSimulationWorker  Background worker (consumes RabbitMQ messages)
|-- SportsUI                      Vue 3 SPA (Vite, Tailwind CSS)
|-- SQL Server                    Database (auto-migrated on startup)
|-- RabbitMQ                      Message broker
|-- Grafana + Loki                Logging and observability
```

---

## Projects

| Project | Description |
|---|---|
| **Sports.AppHost** | .NET Aspire host -- orchestrates all services, databases, and containers |
| **Sports.Api** | REST API with full CRUD for leagues, teams, players, and matches |
| **Sports.MatchSimulationWorker** | Worker service that consumes RabbitMQ messages and simulates match pass statistics |
| **Sports.Shared** | Shared entities, enums, MediatR behaviors, and configuration constants |
| **Sports.ServiceDefaults** | Aspire service defaults (OpenTelemetry, health checks, resilience) |
| **SportsUI** | Vue 3 frontend with Tailwind CSS |
| **Sports.Api.UnitTests** | Unit tests -- domain model constraints against a real SQL Server (Testcontainers) |
| **Sports.Api.IntegrationTests** | Integration tests -- full HTTP endpoint testing via `WebApplicationFactory` |
| **Sports.Worker.IntegrationTests** | Integration tests for the match simulation worker |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (v20.19+ or v22.12+)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server, RabbitMQ, Grafana, Loki)

### Clone and Run

```bash
git clone https://github.com/ampzord/Sports.git
cd Sports

cd SportsUI
npm install
cd ..

dotnet run --project Sports.AppHost
```

That single command starts everything:

- **SQL Server** on port `14330` (password configured in `Sports.AppHost/appsettings.json`)
- **RabbitMQ** with management UI
- **Grafana** + **Loki** for log aggregation
- **Sports.Api** with Swagger UI
- **Sports.MatchSimulationWorker**
- **SportsUI** Vite dev server

The **Aspire Dashboard** opens automatically in your browser, showing all resources with their URLs and health status.

### Where to Find Things

| Resource | Where to find it |
|---|---|
| Aspire Dashboard | Opens automatically on launch (`https://localhost:17298`) |
| Swagger UI | API resource URL + `/swagger` |
| Grafana | Grafana resource URL (admin / admin) |
| Vue Frontend | `sports-ui` resource URL in the Aspire dashboard |

---

## Backend

### API (`Sports.Api`)

Built with a **vertical slice / feature folder** structure. Each feature (e.g., `AddTeam`, `GetTeamById`) lives in its own folder with its endpoint, handler, command, request, and validator. Shared concerns like mappers and response DTOs live in the `_Shared` folder:

```
Features/
  Teams/
    _Shared/
      TeamMapper.cs            Single Mapperly mapper for all team operations
      Responses/
        TeamResponse.cs
    AddTeam/
      AddTeamEndpoint.cs       FastEndpoints endpoint
      AddTeamHandler.cs        MediatR handler
      AddTeamCommand.cs
      AddTeamRequest.cs
      AddTeamValidator.cs
    GetTeamById/
    GetTeams/
    UpdateTeam/
    DeleteTeam/
```

### API Endpoints

All endpoints follow a flat hierarchy. List endpoints support optional query parameters for filtering.

**Leagues**

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/leagues` | List all leagues |
| `GET` | `/api/leagues/{id}` | Get a league by ID |
| `POST` | `/api/leagues` | Create a league |
| `PUT` | `/api/leagues/{id}` | Update a league |
| `DELETE` | `/api/leagues/{id}` | Delete a league |

**Teams**

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/teams?leagueId={id}` | List all teams (optionally filter by league) |
| `GET` | `/api/teams/{id}` | Get a team by ID |
| `POST` | `/api/teams` | Create a team |
| `PUT` | `/api/teams/{id}` | Update a team |
| `DELETE` | `/api/teams/{id}` | Delete a team |

**Players**

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/players?teamId={id}` | List all players (optionally filter by team) |
| `GET` | `/api/players/{id}` | Get a player by ID |
| `POST` | `/api/players` | Create a player |
| `PUT` | `/api/players/{id}` | Update a player |
| `DELETE` | `/api/players/{id}` | Delete a player |

**Matches**

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/matches?leagueId={id}` | List all matches (optionally filter by league) |
| `GET` | `/api/matches/{id}` | Get a match by ID |
| `POST` | `/api/matches` | Create a match |
| `PUT` | `/api/matches/{id}` | Update a match |
| `DELETE` | `/api/matches/{id}` | Delete a match |
| `POST` | `/api/matches/simulate` | Trigger match simulation via RabbitMQ |

### Domain Model

| Entity | Key Fields |
|---|---|
| **League** | `Id`, `Name` (unique, max 100 chars) |
| **Team** | `Id`, `Name` (unique), `LeagueId` (FK, restrict delete) |
| **Player** | `Id`, `Name` (unique), `Position` (enum stored as string), `TeamId` (FK) |
| **Match** | `Id`, `HomeTeamId`, `AwayTeamId` (FK), `TotalPasses` (nullable) |

Relationships enforce referential integrity -- you cannot delete a league that has teams, or a team that has players or matches.

### Key Libraries

- **[FastEndpoints](https://fast-endpoints.com/)** -- Minimal API endpoint definitions with built-in validation and Swagger support
- **[MediatR](https://github.com/jbogard/MediatR)** -- CQRS-style command/query separation with pipeline behaviors
- **[ErrorOr](https://github.com/amantinband/error-or)** -- Discriminated union result type (no exceptions for expected errors)
- **[Mapperly](https://mapperly.riok.app/)** -- Source-generated object mapping (zero reflection, zero allocation)
- **[EF Core](https://learn.microsoft.com/en-us/ef/core/)** -- ORM with SQL Server, code-first migrations, and seed data
- **[Serilog](https://serilog.net/)** -- Structured logging to console and Grafana Loki
- **[FluentValidation](https://docs.fluentvalidation.net/)** -- Request validation

### Worker (`Sports.MatchSimulationWorker`)

A `BackgroundService` that listens on a RabbitMQ queue. When triggered from the API's `/api/matches/simulate` endpoint, it:

1. Picks up unprocessed matches in batches of 500
2. Uses `UPDLOCK, ROWLOCK, READPAST` SQL hints for safe concurrent processing
3. Assigns a random `TotalPasses` value (100-1000) to each match
4. Supports multiple concurrent workers without double-processing

### Seed Data

The database is seeded on startup with:
- 2 leagues (Premier League, La Liga)
- 8 teams (4 per league)
- 88 players (full 4-3-3 lineup of 11 per team)
- 100 matches

---

## Frontend (`SportsUI`)

A **Vue 3** single-page application.

- **Vue Router** -- Client-side routing for all CRUD views
- **Tailwind CSS** -- Utility-first styling
- **Axios** -- HTTP client (proxied through Vite to the API)
- **Vue Toastification** -- Toast notifications

### Routes

| Route | View |
|---|---|
| `/home` | Dashboard |
| `/leagues` | League list |
| `/leagues/create` | Create league |
| `/leagues/:id` | League detail |
| `/leagues/:id/edit` | Edit league |
| `/teams` | Team list |
| `/teams/create` | Create team |
| `/teams/:id` | Team detail |
| `/teams/:id/edit` | Edit team |
| `/players` | Player list |
| `/players/create` | Create player |
| `/players/:id` | Player detail |
| `/players/:id/edit` | Edit player |
| `/matches` | Match list |
| `/matches/create` | Create match |
| `/matches/:id` | Match detail |
| `/matches/:id/edit` | Edit match |

The Vite dev server proxies all `/api` requests to the backend, so no CORS configuration is needed.

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

Tests use **[Testcontainers](https://dotnet.testcontainers.org/)** to spin up a real SQL Server instance -- no mocking of the database layer. Integration tests use **[FluentAssertions](https://fluentassertions.com/)** and **[FluentAssertions.Web](https://github.com/adrianiftode/FluentAssertions.Web)** for expressive HTTP assertions like `response.Should().Be200Ok()`.

---

## Tech Stack

### Backend

| Category | Technology |
|---|---|
| Runtime | .NET 10 / C# 14 |
| API Framework | FastEndpoints |
| Mediator | MediatR |
| ORM | EF Core (SQL Server) |
| Mapping | Mapperly (source-gen) |
| Validation | FluentValidation |
| Error Handling | ErrorOr |
| Logging | Serilog, Grafana Loki |
| Messaging | RabbitMQ |
| Orchestration | .NET Aspire |

### Frontend

| Category | Technology |
|---|---|
| Framework | Vue 3 (Composition API) |
| Build Tool | Vite 7 |
| Styling | Tailwind CSS 4 |
| HTTP Client | Axios |
| Routing | Vue Router 4 |

### Testing

| Category | Technology |
|---|---|
| Framework | xUnit |
| Assertions | FluentAssertions, FluentAssertions.Web |
| HTTP Testing | Microsoft.AspNetCore.Mvc.Testing |
| Containers | Testcontainers (SQL Server) |
| Mocking | Moq |

---

## Future Improvements

- **GUIDs for IDs** -- Replace auto-increment `int` IDs with GUIDs to avoid enumeration attacks and simplify distributed scenarios.
- **Pagination** -- Add limit/offset or cursor-based pagination to all list endpoints.
- **Authentication** -- Add an auth layer (e.g., JWT, OAuth) to protect write endpoints.
- **OpenTelemetry Collector** -- Replace direct Loki/Grafana integration with a proper OpenTelemetry Collector for vendor-neutral telemetry export.

---

## Licensing Notes

Some libraries used in this project have changed their licensing model in recent versions:

- **[FluentValidation](https://docs.fluentvalidation.net/)** -- Version 12+ requires a commercial license for commercial use. This project uses v12. See the [FluentValidation license page](https://docs.fluentvalidation.net/en/latest/licensing.html) for details.
- **[MediatR](https://github.com/jbogard/MediatR)** -- Version 13+ requires a commercial license for commercial use. This project uses v14. See the [MediatR license page](https://www.jbogard.com/mediatr-licensing/) for details.
