# Colorado Business Entity Transaction History

A .NET 10 solution that demonstrates two ways to work with the Colorado business entity transaction history dataset:

- interactive paging in the CLI
- non-interactive download of the raw remote response to a local file

The project is organized into application, infrastructure, CLI, and test layers so the download and paging workflows stay independently testable.

## Solution Structure

- `src/Application`: application workflows, abstractions, and configuration validation
- `src/Cli`: command-line entry point and user-facing command behavior
- `src/Core`: core domain and architecture support
- `src/Infrastructure`: remote API access and file persistence
- `tests/Contract`: CLI behavior and routing coverage
- `tests/Integration`: infrastructure integration coverage
- `tests/Unit`: application and core unit tests

## Prerequisites

- .NET 10 SDK
- A Socrata app token for live API calls

## Configuration

The CLI reads configuration from `src/Cli/appsettings.json` and environment variables.

Common settings:

- `ConnectionStrings__DemoDb`: connection string used by the demo configuration validator
- `Socrata__AppToken`: required for live API requests
- `Socrata__BaseUrl`: optional override for the Socrata endpoint
- `Socrata__MockResponsePath`: optional local JSON file path for test or demo runs without live calls

## Run The CLI

From the repository root:

```powershell
dotnet restore
dotnet build
dotnet run --project src/Cli -- --help
```

### Validate Configuration

```powershell
dotnet run --project src/Cli -- --validate-config
```

### Interactive Paging

```powershell
dotnet run --project src/Cli
```

Run without arguments to start the paging workflow. Use `>` for next page, `<` for previous page, and `Q` to quit.

### Download Workflow

```powershell
dotnet run --project src/Cli -- --download
```

Successful runs save the remote response to:

```text
downloads/casm-dbbj-query.json
```

The download command:

- stays out of the paging flow
- fails if the target file already exists
- validates that the downloaded payload is not empty and is valid JSON
- cleans up stale in-progress temp files from prior interrupted runs before starting again

## Run Tests

```powershell
dotnet test
```

## Dataset

The live data source used by this demo is the Colorado business entity transaction history dataset:

- `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`

## Notes

- The repository ignores local `downloads/` output, logs, and other machine-specific artifacts.
- For predictable local demos, point `Socrata__MockResponsePath` at a saved JSON response file.