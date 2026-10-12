# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

.NET 10 solution (`ParentsApi.slnx`), run from the repo root.

- Build: `dotnet build ParentsApi.slnx`
- Run API: `dotnet run --project src/Web.Api` (Swagger UI is enabled in Development)
- Test all: `dotnet test`
- Single test: `dotnet test tests/ArchitectureTests --filter "FullyQualifiedName~<TestName>"`
- Database: `docker compose up postgres` (Postgres 17, db `clean-architecture`, user/password `postgres`, port 5432). Connection string key is `ConnectionStrings:Database`.
- Full stack in Docker: `docker compose up` (API on 5000/5001).
- Migrations live in `src/Infrastructure/Database/Migrations`

Build is strict: `Directory.Build.props` enables `TreatWarningsAsErrors`, `AnalysisMode=All`, `EnforceCodeStyleInBuild` and SonarAnalyzer for all projects, so analyzer or style violations (see `.editorconfig`) fail the build. NuGet versions are managed centrally in `Directory.Packages.props`; add versions there, not in csproj files.

## Architecture

Clean Architecture with CQRS, split into projects under `src/`:

- `SharedKernel`: `Result`/`Result<T>`, `Error`/`ValidationError`, `Entity` (collects domain events), `IDomainEvent`, `IDateTimeProvider`.
- `Domain`: entities (`User`, `TodoItem`), their domain events and static `*Errors` classes.
- `Application`: use cases organised by feature, then operation (`Todos/Create`, `Users/Login`, ...). Each folder holds a Command/Query, its handler, and optionally a FluentValidation validator and response DTO. Handlers use `IApplicationDbContext` directly (no repository layer).
- `Infrastructure`: EF Core/Npgsql (`ApplicationDbContext`, snake_case naming, per-entity `*Configuration`), JWT auth, password hashing, permission-based authorization, domain event dispatcher.
- `Web.Api`: minimal API endpoints, middleware, exception handler.

`tests/ArchitectureTests` uses NetArchTest to enforce layer dependency rules (`Layers/LayerTests.cs`). Check these when adding project references or cross-layer usings.

### Cross-cutting wiring (needs several files to understand)

- **Handler registration is by convention.** `Application/DependencyInjection.cs` uses Scrutor to scan for `ICommandHandler<>`, `ICommandHandler<,>`, `IQueryHandler<,>` and `IDomainEventHandler<>` implementations (internal classes included). Then it wraps them with decorators: `ValidationDecorator` (commands only) and `LoggingDecorator` (all). New handlers need no manual registration. Validators are also auto-discovered.
- **Errors via `Result`, not exceptions.** Handlers return `Result`/`Result<T>`. Endpoints convert with `result.Match(Results.Ok, CustomResults.Problem)` (`Web.Api/Extensions/ResultExtensions.cs`). Validation failures are returned as `ValidationError` by the decorator. `GlobalExceptionHandler` covers unexpected exceptions.
- **Endpoints are auto-discovered.** Implement `IEndpoint` (`Web.Api/Endpoints/<Feature>/<Operation>.cs`, internal sealed class with a nested `Request`). `AddEndpoints(assembly)` and `MapEndpoints()` register them. Tag with `Tags.*` and call `.RequireAuthorization()`, or use `HasPermission` for permission-based checks (`Infrastructure/Authorization`).
- **Domain events are dispatched after save.** `ApplicationDbContext.SaveChangesAsync` calls `base.SaveChangesAsync`, then collects events from tracked `Entity` instances and dispatches them through `DomainEventsDispatcher` (new scope per event, reflection-based handler lookup). Handlers run in a separate transaction after commit, so they can fail without rolling back the save.
- **Middleware order in `Program.cs`** matters: request-context logging (Serilog), exception handler, authentication, then authorization.

## Adding a feature (checklist)

1. Domain: entity changes + errors in `Domain/<Entity>/<Entity>Errors.cs` (codes like `Todos.NotFound`). Raise domain events via `Raise(...)` inside the entity, not in handlers.
2. Application: `Application/<Feature>/<Operation>/` with `<Op>Command`/`<Op>Query` (sealed record), `<Op>CommandHandler` (internal sealed), optional `<Op>CommandValidator` and response DTO.
3. Infrastructure: if schema changes, update `*Configuration` and add a migration (see below).
4. Web.Api: `Endpoints/<Feature>/<Operation>.cs` implementing `IEndpoint`, tagged with `Tags.<Feature>`, with auth applied.
5. No DI registration needed (Scrutor + endpoint discovery). If something isn't picked up, check it implements the right interface.


## Migrations
- Add: `dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Web.Api --output-dir Database/Migrations`
- Applied automatically on startup; never run `dotnet ef database update`. Never edit committed migrations. Review for unintended drops before finishing.

## Conventions
- Pass `CancellationToken` through all async calls.
- Queries: `AsNoTracking()` + `Select` into the DTO.
- Time via `IDateTimeProvider`; current user via `<IUserContext>`, never from the request.
- Expected failures return `Result.Failure(<Entity>Errors.X)`; errors live in `Domain/<Entity>/<Entity>Errors.cs`.
- Raise domain events inside entities. Event handlers run after commit, so they must be idempotent.
- No repository layer. No `#pragma`/editorconfig suppressions; fix the code or ask.

## Don't

- Don't add a repository layer or generic services; handlers use `IApplicationDbContext`.
- Don't put package versions in csproj files (use `Directory.Packages.props`).
- Don't silence analyzer warnings with `#pragma` or `.editorconfig` changes; fix the code. If a rule seems genuinely wrong, ask first.
- Don't reference Infrastructure from Application or Domain (enforced by `LayerTests`).
- Don't commit secrets; JWT keys etc. go in user-secrets / environment variables.

## Testing

- Only architecture tests exist today (`tests/ArchitectureTests`). They verify layer rules, not behaviour.
- <If you add unit/integration tests: say where they go, naming, and e.g. Testcontainers usage.>

## Done means
`dotnet build` with zero warnings, `dotnet test` passes, and a migration was added if the schema changed. Only architecture tests exist; they don't verify behaviour.

## Git
- Conventional Commits; scopes: <todos, users, auth, infra, api>. Commit only when asked, after build + tests pass.
- Always ask before `git push` or rewriting pushed history.