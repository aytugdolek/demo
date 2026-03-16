# Technical Standards

## Technology Stack

- Runtime: `.NET 10`
- Language: `C#` with the latest available compiler features enabled
- CLI UI: `Spectre.Console`
- Database: `SQL Server Developer Edition 17.0`
- Database server: `localhost\DEMO`
- Database name: `DemoDb`
- Authentication: `Windows`
- Database migration tool: `FluentMigrator`
- Testing framework: `xUnit`

## Language and SDK Rules

- Target framework should be `net10.0`.
- C# should use the latest available version. If preview language features are required by the chosen SDK, set `LangVersion` to `preview` in the project or shared build props.
- Centralize shared SDK and compiler settings where possible.

## Architecture Rules

The solution must follow Clean Architecture.

Recommended project boundaries:

- `src/Core`: entities, value objects, domain rules, and business abstractions
- `src/Application`: use cases, DTOs, interfaces, orchestration, validation
- `src/Infrastructure`: Socrata API client, SQL access, migrations, repositories, configuration
- `src/Cli`: Spectre.Console entry point, command parsing, paging workflow, terminal rendering
- `tests/*`: unit and integration test projects

Dependency direction:

- `Cli` depends on `Application`
- `Infrastructure` depends on `Application` and `Core`
- `Application` depends on `Core`
- `Core` depends on nothing external

## CLI Implementation Standards

Based on current Spectre.Console guidance, the CLI should rely on:

- `Table` for paged record rendering
- markup for status and error messaging
- prompt or command abstractions for command-line behavior
- optional live display features only when they improve usability without complicating interaction

CLI behavior rules:

- No parameter means interactive paging mode.
- `--download` means download-only mode.
- Paging interactions must be explicit and deterministic.
- Output should favor stable column sets over auto-expanding verbose output.

Suggested default columns:

- `entityid`
- `transactionid`
- `name`
- `historydes`
- `receiveddate`
- `effectivedate`

Long text fields such as `comment` should be truncated or displayed in a secondary detail view if necessary.

## Data Access Standards

Use `Microsoft.Data.SqlClient` for SQL Server connectivity.

Baseline development connection string intent:

```text
Server=localhost\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;
```

Rules:

- Use Windows Authentication.
- Keep the connection string outside hard-coded business logic.
- Prefer configuration binding and environment-aware settings.
- Do not couple API DTOs directly to persistence entities.

## Migration Standards

Based on FluentMigrator guidance, migrations should:

- use the SQL Server runner
- scan a dedicated migrations assembly or namespace
- run during controlled startup paths, not from arbitrary scattered code
- be versioned and forward-only unless a downgrade path is explicitly required

Recommended responsibility split:

- migration definitions live in Infrastructure
- migration execution is triggered by an application startup workflow or explicit maintenance command

## Testing Standards

Use xUnit and TDD with the standard loop:

- Red: write a failing test first
- Green: implement the smallest passing change
- Refactor: improve structure while keeping tests green

Testing rules:

- Core and Application logic must have unit tests.
- API client behavior should be isolated behind interfaces and tested with fakes or mocked handlers.
- Paging logic should be tested independently from the console renderer.
- Database integration tests should be separated from fast unit tests.
- Live external API calls must not be required for the unit test suite.

## Best Practices and Patterns

- Prefer small, composable use cases over large service classes.
- Keep side effects in Infrastructure.
- Model external API contracts explicitly.
- Use cancellation tokens for network and database operations.
- Use resilient HTTP handling with timeouts and clear error messages.
- Keep CLI commands thin and delegate business logic to Application services.
- Add structured logging where operational diagnosis matters.

## Documentation Standards

For implementation and design questions, use these sources first:

- Context7 for library-specific documentation and examples
- Microsoft Docs and MCP-backed Microsoft documentation tools for .NET, C#, SQL Server, and client-library guidance
- Official Socrata dataset documentation for endpoint behavior and field definitions

Avoid relying on unverified blog posts when official documentation is available.

## API Integration Guidance

Primary remote API:

- Documentation: `https://dev.socrata.com/foundry/data.colorado.gov/casm-dbbj`
- Query endpoint: `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`

Operational guidance:

- Include the app token in requests.
- Treat the Socrata dataset schema as an external contract that can evolve.
- Isolate API mapping code from UI code.
- Favor a typed client over ad hoc string manipulation.

## Initial Delivery Scope

Phase 1 should deliver:

- root CLI command with no parameters
- interactive paging over 20 rows per page
- `>` next-page navigation
- `<` previous-page navigation
- `Q` quit
- `--download` command path
- SQL Server connectivity setup
- FluentMigrator baseline migration pipeline
- xUnit test project scaffolding